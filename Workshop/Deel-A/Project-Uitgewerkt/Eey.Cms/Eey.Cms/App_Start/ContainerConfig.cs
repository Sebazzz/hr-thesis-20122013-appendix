using Eey.Cms.App_Start;

using WebActivator;

[assembly: PreApplicationStartMethod(typeof(ContainerConfig), "RegisterDependencies")]

namespace Eey.Cms.App_Start {
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    using Eey.Cms.Data;
    using Eey.Cms.Data.Repositories;

    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;

    public static class ContainerConfig {
        public static void RegisterDependencies() {
            Lifestyle uowLifestyle = new WebRequestLifestyle(true);

            // Create the container as usual.
            var container = new Container();

            // Register types
            container.Register(DatabaseContextFactory.CreateDatabaseContext, uowLifestyle);
            container.RegisterManyForOpenGeneric(typeof(IRepository<>), typeof(Repository<>).Assembly);

            container.BatchRegisterRepositories(typeof(Repository<>).Assembly, uowLifestyle);
            container.BatchRegisterServices(typeof(Repository<>).Assembly, uowLifestyle);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcAttributeFilterProvider();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

               /// <summary>
        /// Registers <c>XxxRepository</c> types as service implementation if they implement the <c>IXxxRepository</c> interface.
        /// </summary>
        /// <remarks>
        /// Execute convention based type registration as per http://simpleinjector.codeplex.com/wikipage?title=Advanced-scenarios&referringTitle=Documentation
        /// </remarks>
        /// <param name="container"></param>
        /// <param name="targetAssembly"></param>
        /// <param name="scope"> </param>
        private static void BatchRegisterRepositories(this Container container, Assembly targetAssembly, Lifestyle scope) {
            const string conventionEndString = "Repository";

            Func<string, bool> predicate = typeName =>
            {
                // Does 'User' exist in the assembly?

                string entityTypeName = typeName.Substring(0, typeName.Length - conventionEndString.Length); // User
                return targetAssembly.GetExportedTypes().Any(t => t.Name == entityTypeName);
            };

            BatchRegisterObjects(container, targetAssembly, scope, conventionEndString, predicate);
        }

        /// <summary>
        /// Registers <c>XxxService</c> types as service implementation if they implement the <c>IXxxService</c> interface.
        /// </summary>
        /// <remarks>
        /// Execute convention based type registration as per http://simpleinjector.codeplex.com/wikipage?title=Advanced-scenarios&referringTitle=Documentation
        /// </remarks>
        /// <param name="container"></param>
        /// <param name="targetAssembly"></param>
        /// <param name="scope"> </param>
        private static void BatchRegisterServices(this Container container, Assembly targetAssembly, Lifestyle scope) {
            const string conventionEndString = "Service";

            Func<string, bool> predicate = _ => true;
            BatchRegisterObjects(container, targetAssembly, scope, conventionEndString, predicate );
        }

        private static void BatchRegisterObjects(Container container, Assembly targetAssembly, Lifestyle scope, string conventionEndString, Func<string, bool> predicate) {
            Debug.WriteLine("{0}Registering '{1}' objects in assembly '{2}'", Environment.NewLine, conventionEndString, targetAssembly.GetName().Name);

            var registration = from type in targetAssembly.GetExportedTypes()
                               where type.IsClass && !type.IsAbstract

                               let typeName = type.Name // UserRepository
                               where typeName != null && typeName.EndsWith(conventionEndString) // [Repository]

                               where predicate(typeName) 

                               let interfaceTypeName = type.Namespace + "." + "I" + type.Name // IUserRepository
                               let interfaceType = targetAssembly.GetExportedTypes().FirstOrDefault(t => t.FullName == interfaceTypeName)
                               where interfaceType != null

                               select new {
                                   ServiceType = interfaceType,
                                   ImplementationType = type,
                               };

            // convention based
            foreach (var reg in registration) {
                Debug.WriteLine("    Registering service: {0} with implementation {1}", reg.ServiceType.Name, reg.ImplementationType.Name);

                container.Register(reg.ServiceType, reg.ImplementationType, scope);
            }
        }
    }
}