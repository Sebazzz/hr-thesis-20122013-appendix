namespace Prototype.NancyFX.Views {
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Nancy.ViewEngines.Razor;

    using Prototype.NancyFX.Util;

    public static class ViewHelpers {
        /// <summary>
        ///   Returns the value of the <see cref="DisplayAttribute" /> <see cref="DisplayAttribute.Description" /> for the specified <paramref
        ///    name="propertyToDisplay" />
        /// </summary>
        public static string DescriptionFor<TEntity, TProperty>(this HtmlHelpers<TEntity> htmlHelper, Expression<Func<TEntity, TProperty>> propertyToDisplay) {
            PropertyInfo selectedProperty = propertyToDisplay.GetPropertyInfo();

            DisplayAttribute displayAttribute = selectedProperty.GetCustomAttributes(typeof(DisplayAttribute), true).OfType<DisplayAttribute>().FirstOrDefault();

            if (displayAttribute == null) {
                return String.Empty;
            }

            return displayAttribute.GetDescription() ?? String.Empty;
        }

        /// <summary>
        ///   Returns the value of the <see cref="DisplayAttribute" /> <see cref="DisplayAttribute.Name" /> for the specified <paramref
        ///    name="propertyToDisplay" />
        /// </summary>
        public static string NameFor<TViewModel, TEntity, TProperty>(this HtmlHelpers<TViewModel> htmlHelper, Expression<Func<TEntity, TProperty>> propertyToDisplay)
        {
            PropertyInfo selectedProperty = propertyToDisplay.GetPropertyInfo();

            DisplayAttribute displayAttribute = selectedProperty.GetCustomAttributes(typeof(DisplayAttribute), true).OfType<DisplayAttribute>().FirstOrDefault();

            if (displayAttribute == null) {
                return selectedProperty.Name;
            }

            return displayAttribute.GetName() ?? String.Empty;
        }

        /// <summary>
        ///   Returns the value of the <see cref="DisplayAttribute" /> <see cref="DisplayAttribute.Name" /> for the specified <paramref
        ///    name="propertyToDisplay" />
        /// </summary>
        public static string Name<TEntity, TProperty>(this HtmlHelpers<TEntity> htmlHelper, Expression<Func<TEntity, TProperty>> propertyToDisplay)
        {
            PropertyInfo selectedProperty = propertyToDisplay.GetPropertyInfo();

            DisplayAttribute displayAttribute = selectedProperty.GetCustomAttributes(typeof(DisplayAttribute), true).OfType<DisplayAttribute>().FirstOrDefault();

            if (displayAttribute == null) {
                return selectedProperty.Name;
            }

            return displayAttribute.GetName() ?? String.Empty;
        }


        public static string ToStringInvariant(this IFormattable formattable) {
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        }
    }
}