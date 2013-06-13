using System;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Eey.Cms.Filters {
    using Eey.Cms.Data;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute {
        private static SimpleMembershipInitializer Initializer;
        private static object InitializerLock = new object();
        private static bool IsInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref Initializer, ref IsInitialized, ref InitializerLock);
        }

        // ReSharper disable ClassNeverInstantiated.Local
        private sealed class SimpleMembershipInitializer {
            public SimpleMembershipInitializer() {
                try {
                    using (var context = new DatabaseContext()) {
                        if (!context.Database.Exists()) {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "Id", "UserName", autoCreateTables: true);
                    
                    if (!WebSecurity.UserExists("Admin")) {
                        WebSecurity.CreateUserAndAccount("Admin", "welkom01");
                    }
                } catch (Exception ex) {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
        // ReSharper restore ClassNeverInstantiated.Local
    }
}
