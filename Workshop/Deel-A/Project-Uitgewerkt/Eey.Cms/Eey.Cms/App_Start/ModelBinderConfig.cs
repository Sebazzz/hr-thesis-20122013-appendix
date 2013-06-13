using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eey.Cms.App_Start {
    using System.Collections;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Globalization;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Eey.Cms.Data;

    public static class ModelBinderConfig {
        public static void Configure() {
            ModelBinderProviders.BinderProviders.Add(new EFModelBinderProvider());
        }
    }

    public class EFModelBinderProvider : IModelBinderProvider {
        /// <summary>
        /// Returns the model binder for the specified type.
        /// </summary>
        /// <returns>
        /// The model binder for the specified type.
        /// </returns>
        /// <param name="modelType">The type of the model.</param>
        public IModelBinder GetBinder(Type modelType) {
            return (IModelBinder) DependencyResolver.Current.GetService(typeof(EFModelBinder));
        }
    }

    /// <summary>
    /// Represents a model binder that creates an model from an ID that is taken in via values or routing
    /// and then uses NHibernate session to pick up an persistable entity. This can be used as primary model binder. See remarks.
    /// </summary>
    /// <remarks>
    /// When used in combination with the <see cref="SessionModelBinderProvider"/>, this class can be used as primary model binder for
    /// NHibernate based models
    /// </remarks>
    public class EFModelBinder : DefaultModelBinder {
        private readonly DatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.DefaultModelBinder"/> class.
        /// </summary>
        public EFModelBinder(DatabaseContext context) {
            this.context = context;
        }

        /// <summary>
        /// Creates the specified model type by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        /// A data object of the specified type.
        /// </returns>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param><param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param><param name="modelType">The type of the model object to return.</param>
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType) {
            // we can understand some collection interfaces, e.g. IList<>, IDictionary<,>
            if (modelType.IsGenericType) {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

            // create from - by convention - id
            object instance = this.CreateModelFromId(controllerContext, bindingContext, modelType);
            if (instance != null) {
                return instance;
            }

            // fallback to base implementation
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        /// The bound object.
        /// </returns>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param><param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param><exception cref="T:System.ArgumentNullException">The <paramref name="bindingContext "/>parameter is null.</exception>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            // try base implementation first
            object model = base.BindModel(controllerContext, bindingContext);

            // if the current model name is an id, use that. this will happen in for example: selection lists (<select>)
            if (model == null) {
                Type idType = typeof(int);

                // get the id, this will be the model name itself
                object id = GetIdFromContext(bindingContext, idType, null);

                if (!IsIdentityNullOrDefault(idType, id)) {
                    model = GetDatabasePersistedModel(bindingContext.ModelType, (int) id);
                }

                // remove any model error (which was set in BindSimpleModel)
                ModelState modelState;
                if (bindingContext.ModelState.TryGetValue(bindingContext.ModelName, out modelState)) {
                    modelState.Errors.Clear();
                }
            }

            return model;
        }

        /// <summary>
        /// Returns the value of a property using the specified controller context, binding context, property descriptor, and property binder.
        /// </summary>
        /// <returns>
        /// An object that represents the property value.
        /// </returns>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param><param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param><param name="propertyDescriptor">The descriptor for the property to access. The descriptor provides information such as the component type, property type, and property value. It also provides methods to get or set the property value.</param><param name="propertyBinder">An object that provides a way to bind the property.</param>
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder) {
            // check if the target is a list, the source is probably comma seperated
            Type propertyType = propertyDescriptor.PropertyType;
            bool isList = false;
            if (propertyType.IsGenericType) {
                // check if it is an IList<T>
                if (propertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IList<>))) {
                    isList = true;
                }
            }

            // create a list if necessary
            if (isList) {
                Type itemType = propertyType.GetGenericArguments()[0];
                DbSet metaDataForType = this.context.Set(itemType);

                if (metaDataForType != null) {
                    // create list or reuse existing
                    IList itemList = bindingContext.Model as IList;
                    if (itemList != null) {
                        itemList.Clear();
                    } else {
                        itemList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                    }

                    // split values by string and retrieve
                    ValueProviderResult splittableValues = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                    if (splittableValues == null || splittableValues.AttemptedValue == null) {
                        goto fallback;
                    }

                    string[] splittedValues = splittableValues.AttemptedValue.Split(',');

                    foreach (string splittedValue in splittedValues) {
                        string idString = splittedValue != null ? splittedValue.Trim() : null;

                        if (String.IsNullOrEmpty(idString)) {
                            continue;
                        }

                        // get ID
                        object id;
                        try {
                            id = Convert.ChangeType(idString, typeof(int), CultureInfo.InvariantCulture);
                        } catch (Exception) {
                            continue;
                        }

                        object model = GetDatabasePersistedModel(itemType, (int) id);
                        itemList.Add(model);
                    }

                    return itemList;
                }
            }

        fallback:
            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }



        private object CreateModelFromId(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType) {
            // get ID property
            PropertyInfo idProperty = this.GetPropertyFromModel(modelType, "Id");

            Type idType = idProperty.PropertyType;

            // get ID value from data
            object idValue = this.GetIdFromContext(bindingContext, idType, modelType, idProperty.Name);

            if (IsIdentityNullOrDefault(idType, idValue)) {
                // get id from route
                idValue = this.GetIdFromRoute(controllerContext, idType);
            }

            if (IsIdentityNullOrDefault(idType, idValue)) {
                return null;
            }

            // get the object from the session
            object entity = GetDatabasePersistedModel(modelType, (int) idValue);

            return entity;
        }

        private static bool IsIdentityNullOrDefault(Type idType, object id) {
            object defaultIdValue = GetDefaultIdTypeValue(idType);

            return ReferenceEquals(defaultIdValue, null) && ReferenceEquals(id, null) || ReferenceEquals(id, null) || Equals(defaultIdValue, id);
        }

        private static object GetDefaultIdTypeValue(Type idType) {
            return idType.IsValueType ? Activator.CreateInstance(idType) : null;
        }

        /// <summary>
        /// Returns a object from the database
        /// </summary>
        /// <returns></returns>
        protected virtual object GetDatabasePersistedModel(Type modelType, int idValue) {
            DbSet dbSet = this.context.Set(modelType);

            if (dbSet != null) {
                return dbSet.Find(idValue);
            }

            return null;
        }

        private PropertyInfo GetPropertyFromModel(Type modelType, string name) {
            PropertyInfo idProperty = modelType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

            return idProperty;
        }

        private ValueProviderResult GetPropertyValue(ModelBindingContext ctx, string name) {
            // compose key
            string key = name;
            if (!String.IsNullOrEmpty(ctx.ModelName) && !String.IsNullOrEmpty(name)) {
                key = ctx.ModelName + "." + key;
            } else if (String.IsNullOrEmpty(name)) {
                key = ctx.ModelName;
            }

            if (key == null) {
                return null;
            }

            var result = ctx.ValueProvider.GetValue(key);

            // fallback to empty, if we may
            if (result == null && ctx.FallbackToEmptyPrefix) {
                return ctx.ValueProvider.GetValue(name);
            }

            return result;
        }

        private object GetIdFromContext(ModelBindingContext ctx, Type idType, string key) {
            var id = this.GetPropertyValue(ctx, key);

            try {
                return id.ConvertTo(idType, CultureInfo.InvariantCulture);
            } catch {
                return null;
            }
        }

        private object GetIdFromContext(ModelBindingContext ctx, Type idType, Type modelType, string mostLikelyName = null) {
            object id = null;

            // try the most likely name first
            if (mostLikelyName != null) {
                id = this.GetIdFromContext(ctx, idType, mostLikelyName);
            }

            // "id"
            id = id ?? this.GetIdFromContext(ctx, idType, "id");

            // "Id"
            id = id ?? this.GetIdFromContext(ctx, idType, "Id");

            // "ProductId"
            id = id ?? this.GetIdFromContext(ctx, idType, modelType.Name + "Id");

            return id;
        }

        private object GetIdFromRoute(ControllerContext controllerContext, Type idType) {
            RouteData rd = controllerContext.RouteData;

            object id;
            if (rd.Values.TryGetValue("id", out id) && id != null) {
                try {
                    id = Convert.ChangeType(id, idType, CultureInfo.InvariantCulture);
                } catch {
                    id = null;
                }
            }

            return id;
        }
    }
}