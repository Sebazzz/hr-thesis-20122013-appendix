using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.MVCApp
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    /// <summary>
    ///   Static helper class that contains extensions for the <see cref = "IQueryable{T}" /> interface
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Orders the specified <paramref name="queryable"/> by the specified property. 
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="Queryable.OrderBy{TSource,TKey}(System.Linq.IQueryable{TSource},System.Linq.Expressions.Expression{System.Func{TSource,TKey}})"/> internally.
        /// Both simple ("PropertyName") and complex ("PropertyName.OtherProperty") property access is supported. 
        /// </remarks>
        /// <example>
        /// With <typeparamref name="T"/> being a type containing date time:
        /// Simple property access:  <paramref name="propertyAccessExpr"/> should be "PropertyName"
        /// Complex property access:  <paramref name="propertyAccessExpr"/> should be "PropertyName.Seconds"
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="propertyAccessExpr">The property access expr.</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyAccessExpr)
                where T : new()
        {
            Type queryableType = typeof(Queryable);
            MethodInfo orderByMethod = queryableType.GetMethods(BindingFlags.Public |
                                                                BindingFlags.Static)
                    .Single(mi => mi.Name == "OrderBy" && mi.GetParameters().Length == 2);

            return ExecuteLambdaWithMethod(queryable, orderByMethod, propertyAccessExpr);
        }

        /// <summary>
        /// Orders the specified <paramref name="queryable"/> by the specified property (descending)
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="Queryable.OrderByDescending{TSource,TKey}(System.Linq.IQueryable{TSource},System.Linq.Expressions.Expression{System.Func{TSource,TKey}})"/> internally.
        /// Both simple ("PropertyName") and complex ("PropertyName.OtherProperty") property access is supported. 
        /// </remarks>
        /// <example>
        /// With <typeparamref name="T"/> being a type containing date time:
        /// Simple property access:  <paramref name="propertyAccessExpr"/> should be "PropertyName"
        /// Complex property access:  <paramref name="propertyAccessExpr"/> should be "PropertyName.Seconds"
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="propertyAccessExpr">The property access expr.</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> queryable,
                                                          string propertyAccessExpr) where T : new()
        {
            Type queryableType = typeof(Queryable);
            MethodInfo orderByMethod = queryableType.GetMethods(BindingFlags.Public |
                                                                BindingFlags.Static)
                    .Single(mi => mi.Name == "OrderByDescending" && mi.GetParameters().Length == 2);

            return ExecuteLambdaWithMethod(queryable, orderByMethod, propertyAccessExpr);
        }

        private static IQueryable<T> ExecuteLambdaWithMethod<T>(IQueryable<T> queryable,
                                                                 MethodInfo methodToExecute,
                                                                 string propertyAccessExpr)
        {
            if (propertyAccessExpr == null)
            {
                throw new ArgumentNullException("propertyAccessExpr");
            }

            string[] propAccessParts = propertyAccessExpr.Split('.');

            Type currentType = typeof(T);
            ParameterExpression paramExpr = Expression.Parameter(currentType, "entity");
            Expression chainParam = paramExpr;
            PropertyInfo prop = null;

            foreach (string columnName in propAccessParts)
            {
                prop = currentType.GetProperty(columnName,
                                               BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (prop == null)
                {
                    throw new ArgumentException(String.Format("No property '{0}' found on type '{1}'", columnName, currentType.FullName), "propertyAccessExpr");
                }

                MemberExpression propertyExpr = Expression.Property(chainParam, currentType, columnName);

                chainParam = propertyExpr;

                currentType = prop.PropertyType;
            }

            Contract.Assume(chainParam != null);
            LambdaExpression lambdaExpression = Expression.Lambda(chainParam, paramExpr);

            if (methodToExecute != null)
            {
                methodToExecute = methodToExecute.MakeGenericMethod(typeof(T), prop.PropertyType);
                return
                        (IQueryable<T>)
                        methodToExecute.Invoke(null, new object[] { queryable, lambdaExpression });
            }

            return queryable;
        }
    }

    /// <summary>
    /// Static class with domain specific view helpers
    /// </summary>
    public static class ViewHelpers
    {
        public static IHtmlString MetaAcceptLanguage<T>(this HtmlHelper<T> html)
        {
            var acceptLanguage = HttpUtility.HtmlAttributeEncode(Thread.CurrentThread.CurrentUICulture.ToString());
            return new HtmlString(String.Format(@"<meta name=""accept-language"" content=""{0}"">", acceptLanguage));
        }

        /// <summary>
        /// Creates an action link with the specified jQuery icon image (see http://jqueryui.com/themeroller)
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="iconClass"></param>
        /// <param name="linkText"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString IconActionLink(this HtmlHelper htmlHelper, string iconClass, string linkText, string action, string controller = null, object routeValues = null, object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder linkTag = new TagBuilder("a");
            linkTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            linkTag.MergeAttribute("href", urlHelper.Action(action, controller, routeValues));

            if (String.IsNullOrEmpty(linkText))
            {
                linkTag.InnerHtml = " ";
                linkTag.AddCssClass("ui-icon");
                linkTag.AddCssClass(iconClass);
            }
            else
            {
                TagBuilder innerSpanTag = new TagBuilder("span");
                innerSpanTag.InnerHtml = "&nbsp;";
                innerSpanTag.AddCssClass("ui-icon");
                innerSpanTag.AddCssClass(iconClass);

                linkTag.InnerHtml = innerSpanTag + HttpUtility.HtmlEncode(linkText);
                linkTag.AddCssClass("withIcon");
            }

            return MvcHtmlString.Create(linkTag.ToString());
        }

        public static MvcHtmlString MailToLink(this HtmlHelper htmlHelper, string emailAddress, string linkText = null, bool useIcon = true, object htmlAttributes = null)
        {
            TagBuilder linkTag = new TagBuilder("a");
            linkTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            linkTag.MergeAttribute("href", String.Concat("mailto:", HttpUtility.UrlEncode(emailAddress)));

            string innerText = HttpUtility.HtmlEncode(linkText ?? emailAddress);
            string extraInnerText = String.Empty;
            if (useIcon)
            {
                TagBuilder innerSpanTag = new TagBuilder("span");
                innerSpanTag.InnerHtml = "&nbsp;";
                innerSpanTag.AddCssClass("ui-icon");
                innerSpanTag.AddCssClass("ui-icon-mail-closed");

                extraInnerText = innerSpanTag.ToString();
            }

            linkTag.InnerHtml = extraInnerText + innerText;
            linkTag.AddCssClass("withIcon");

            return MvcHtmlString.Create(linkTag.ToString());
        }

        /// <summary>
        /// Renders the link to the current page with query parameters added to it
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="queryParams">The query params to add to the current request.</param>
        /// <returns></returns>
        public static MvcHtmlString CurrentLinkWithParameters(this HtmlHelper htmlHelper, string linkText, object queryParams)
        {
            return CurrentLinkWithParameters(htmlHelper, linkText, queryParams, null);
        }

        /// <summary>
        /// Renders the link to the current page with query parameters added to it
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="queryParams">The query params to add to the current request.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CurrentLinkWithParameters(this HtmlHelper htmlHelper, string linkText, object queryParams, object htmlAttributes)
        {
            RouteData currentRouteData = htmlHelper.ViewContext.RouteData;

            string routeName = "Default";

            RouteValueDictionary dict = new RouteValueDictionary();
            RouteValueDictionary originalRouteValues = currentRouteData.DataTokens;
            RouteValueDictionary queryParamsDict = HtmlHelper.AnonymousObjectToHtmlAttributes(queryParams);

            // copy original route values to new dictionary
            foreach (string key in originalRouteValues.Keys)
            {
                dict[key] = originalRouteValues[key];
            }

            // copy extra query params to new dictionary
            foreach (string key in queryParamsDict.Keys)
            {
                dict[key] = queryParamsDict[key].ToString();
            }

            // remove default route values - these are not needed
            dict.Remove("area");
            dict.Remove("namespaces");
            dict.Remove("namespacefallback");

            return htmlHelper.RouteLink(linkText, routeName, dict, htmlAttributes);
        }

        /// <summary>
        /// Creates a URL helper from the specified html helper
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static UrlHelper CreateUrlHelper(this HtmlHelper htmlHelper)
        {
            return new UrlHelper(htmlHelper.ViewContext.RequestContext);
        }

        /// <summary>
        /// Gets a html link with the specified parameters
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public static string CurrentLinkWithParameters(this UrlHelper urlHelper, object queryParams)
        {
            RouteData currentRouteData = urlHelper.RequestContext.RouteData;

            string routeName = "Default";

            RouteValueDictionary dict = new RouteValueDictionary();
            RouteValueDictionary originalRouteValues = currentRouteData.DataTokens;
            RouteValueDictionary queryParamsDict = HtmlHelper.AnonymousObjectToHtmlAttributes(queryParams);

            // copy original route values to new dictionary
            foreach (string key in originalRouteValues.Keys)
            {
                dict[key] = originalRouteValues[key];
            }

            // copy extra query params to new dictionary
            foreach (string key in queryParamsDict.Keys)
            {
                dict[key] = queryParamsDict[key].ToString();
            }

            // remove default route values - these are not needed
            dict.Remove("area");
            dict.Remove("namespaces");
            dict.Remove("usenamespacefallback");

            return urlHelper.RouteUrl(routeName, dict);
        }

        public static MvcHtmlString SortableColumnHeader<TEntity, TProperty>(
                    this HtmlHelper htmlHelper,
                    Expression<Func<TEntity, TProperty>> columnToDisplay,
                    string customDisplayName,
                    string sortedColumnName,
                    SortOrder? currentSortOrder)
        {

            return SortableColumnHeader(htmlHelper, columnToDisplay, customDisplayName, sortedColumnName, currentSortOrder, null);
        }

        public static MvcHtmlString SortableColumnHeader<TEntity, TProperty>(
                    this HtmlHelper htmlHelper,
                    Expression<Func<TEntity, TProperty>> columnToDisplay,
                    string sortedColumnName, SortOrder? currentSortOrder)
        {

            return SortableColumnHeader(htmlHelper, columnToDisplay, sortedColumnName, currentSortOrder, null);
        }

        public static MvcHtmlString SortableColumnHeader<TEntity, TProperty>(
                    this HtmlHelper htmlHelper,
                    Expression<Func<TEntity, TProperty>> columnToDisplay,
                    string sortedColumnName, SortOrder? currentSortOrder,
                    object htmlAttributes)
        {

            return SortableColumnHeader(htmlHelper, columnToDisplay, null, sortedColumnName, currentSortOrder, htmlAttributes);
        }

        public static MvcHtmlString SortableColumnHeader<TEntity, TProperty>(
                    this HtmlHelper htmlHelper,
                    Expression<Func<TEntity, TProperty>> columnToDisplay,
                    string customDisplayName,
                    string sortedColumnName,
                    SortOrder? currentSortOrder,
                    object htmlAttributes)
        {
            // html attributes
            IDictionary<string, object> htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            // get property name
            PropertyInfo columnToDisplayPropInfo = columnToDisplay.GetPropertyInfo();
            string propertyName = columnToDisplay.GetPropertyPath();
            string displayName = customDisplayName;

            // ... check for DisplayAttributes
            if (String.IsNullOrEmpty(displayName))
            {
                DisplayAttribute displayAttribute = columnToDisplayPropInfo
                                   .GetCustomAttributes(typeof(DisplayAttribute), true)
                                   .OfType<DisplayAttribute>()
                                   .FirstOrDefault(da => da.GetShortName() != null);
                if (displayAttribute == null)
                {
                    displayName = propertyName;
                }
                else
                {
                    displayName = displayAttribute.GetShortName();
                }
            }

            // display name is either custom set name, ShortName, Name or property name

            bool isSortedColumn = String.Compare(sortedColumnName, propertyName, true) == 0;

            // generate sorting parameter
            currentSortOrder = currentSortOrder.GetValueOrDefault(SortOrder.Unspecified);
            SortOrder newSortOrder;
            if (isSortedColumn)
            {
                if (currentSortOrder == SortOrder.Unspecified ||
                    currentSortOrder == SortOrder.Descending)
                {
                    newSortOrder = SortOrder.Ascending;
                }
                else
                {
                    newSortOrder = SortOrder.Descending;
                }
            }
            else
            {
                newSortOrder = currentSortOrder.Value;
            }

            // create anchor link
            TagBuilder tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttributes(htmlAttributesDict);

            string sortSpecifier = String.Empty;
            if (isSortedColumn)
            {
                // this column is sorted, add sort specifier
                TagBuilder sortSpecifierTag = new TagBuilder("span");
                sortSpecifierTag.InnerHtml = "&nbsp;";
                sortSpecifierTag.AddCssClass("ui-icon");
                sortSpecifierTag.AddCssClass(currentSortOrder == SortOrder.Ascending ? "ui-icon-triangle-1-s" : "ui-icon-triangle-1-n");
                sortSpecifier = sortSpecifierTag.ToString(TagRenderMode.Normal);

                tagBuilder.AddCssClass("withIcon");
            }

            tagBuilder.InnerHtml = sortSpecifier + HttpUtility.HtmlEncode(displayName);
            tagBuilder.MergeAttribute("href", CurrentLinkWithParameters(htmlHelper.CreateUrlHelper(), new { sortOrder = newSortOrder, sortColumn = propertyName }));

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ColumnHeader<TEntity, TProperty>(
                this HtmlHelper htmlHelper,
                Expression<Func<TEntity, TProperty>> columnToDisplay)
        {
            return ColumnHeader(htmlHelper, columnToDisplay, null);
        }

        public static MvcHtmlString ColumnHeader<TEntity, TProperty>(
                    this HtmlHelper htmlHelper,
                    Expression<Func<TEntity, TProperty>> columnToDisplay,
                    string customDisplayName)
        {
            // get property name
            PropertyInfo columnToDisplayPropInfo = columnToDisplay.GetPropertyInfo();
            string propertyName = columnToDisplay.GetPropertyPath();
            string displayName = customDisplayName;
            string longName = String.Empty;

            // ... check for DisplayAttributes
            DisplayAttribute displayAttribute = columnToDisplayPropInfo
                                .GetCustomAttributes(typeof(DisplayAttribute), true)
                                .OfType<DisplayAttribute>()
                                .FirstOrDefault(da => da.GetShortName() != null);
            if (displayAttribute == null)
            {
                displayName = propertyName;
            }
            else
            {
                string name = displayAttribute.GetName();
                string shortName = displayAttribute.GetShortName();

                if (String.IsNullOrEmpty(displayName))
                {
                    if (shortName == name)
                    {
                        longName = displayName;
                    }
                    else
                    {
                        longName = name;
                    }
                }
                else
                {
                    longName = displayName;
                }

                displayName = shortName;
            }

            if (!String.IsNullOrEmpty(longName))
            {
                TagBuilder spanBuilder = new TagBuilder("span");
                spanBuilder.SetInnerText(displayName);
                spanBuilder.MergeAttribute("title", longName);
                return MvcHtmlString.Create(spanBuilder.ToString());
            }

            // display name is either custom set name, ShortName, Name or property name
            return MvcHtmlString.Create(HttpUtility.HtmlEncode(displayName));
        }

        /// <summary>
        /// Returns the value of the <see cref="DisplayAttribute"/> <see cref="DisplayAttribute.Description"/> for the specified <paramref name="propertyToDisplay"/>
        /// </summary>
        public static string DescriptionFor<TEntity, TProperty>(this HtmlHelper<TEntity> htmlHelper, Expression<Func<TEntity, TProperty>> propertyToDisplay)
        {
            PropertyInfo selectedProperty = propertyToDisplay.GetPropertyInfo();

            DisplayAttribute displayAttribute = selectedProperty
                                .GetCustomAttributes(typeof(DisplayAttribute), true)
                                .OfType<DisplayAttribute>().Single();

            if (displayAttribute == null)
            {
                return String.Empty;
            }

            return displayAttribute.GetDescription() ?? String.Empty;
        }

        /// <summary>
        /// Returns the value of the <see cref="DisplayAttribute"/> <see cref="DisplayAttribute.Name"/> for the specified <paramref name="propertyToDisplay"/>
        /// </summary>
        public static string NameFor<TEntity, TProperty>(this HtmlHelper<TEntity> htmlHelper, Expression<Func<TEntity, TProperty>> propertyToDisplay)
        {
            PropertyInfo selectedProperty = propertyToDisplay.GetPropertyInfo();

            DisplayAttribute displayAttribute = selectedProperty
                                .GetCustomAttributes(typeof(DisplayAttribute), true)
                                .OfType<DisplayAttribute>().Single();

            if (displayAttribute == null)
            {
                return String.Empty;
            }

            return displayAttribute.GetName() ?? String.Empty;
        }

        /// <summary>
        /// Returns the value of the <see cref="DisplayAttribute"/> <see cref="DisplayAttribute.Name"/> for the specified <paramref name="propertyToDisplay"/>
        /// </summary>
        public static string Name<TEntity>(this HtmlHelper htmlHelper, Expression<Func<TEntity, object>> propertyToDisplay)
        {
            PropertyInfo selectedProperty = propertyToDisplay.GetPropertyInfo();

            DisplayAttribute displayAttribute = selectedProperty
                                .GetCustomAttributes(typeof(DisplayAttribute), true)
                                .OfType<DisplayAttribute>().Single();

            if (displayAttribute == null)
            {
                return String.Empty;
            }

            return displayAttribute.GetName() ?? String.Empty;
        }
    }

    /// <summary>
    /// Static helper class that provides extensions for <see cref="Expression{TDelegate}"/> objects
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets the property called using the specified property lambda expression.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="checkIfPropertyForCurrentType">Check if the property selected in the <paramref name="propertyLambda"/> is a property of the <typeparamref name="TSource"/></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda, bool checkIfPropertyForCurrentType = false)
        {
            Type type = typeof(TSource);

            // check if it's a member expression (should be)
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda));
            }

            // check if it's a property
            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda));
            }

            if (checkIfPropertyForCurrentType)
            {
                if (type != propInfo.ReflectedType &&
                    !type.IsSubclassOf(propInfo.ReflectedType))
                {
                    throw new ArgumentException(string.Format(
                                                              "Expression '{0}' refers to a property that is not from type {1}.",
                                                              propertyLambda,
                                                              type));
                }
            }

            return propInfo;
        }

        /// <summary>
        /// Gets a path in the form of PropertyName.PropertyName2 from a expression like (e => e.PropertyName.PropertyName2)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyLambda"></param>
        /// <returns></returns>
        public static string GetPropertyPath<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
        {
            // we need to walk the stack of properties called backwards to get the property path
            // A expression like e => e.A.B.C.D will contain chained expressions starting from D

            // check if it's a member expression (should be)
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda));
            }

            Stack<PropertyInfo> calledProperties = new Stack<PropertyInfo>();

            while (member != null)
            {
                // check if it's a property (current member)
                PropertyInfo propInfo = member.Member as PropertyInfo;
                if (propInfo == null)
                {
                    throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to or via a field, not a property.",
                        propertyLambda));
                }

                calledProperties.Push(propInfo);

                member = member.Expression as MemberExpression;
            }

            const char seperator = '.';
            StringBuilder propertyPathBuilder = new StringBuilder();
            while (calledProperties.Count > 0)
            {
                PropertyInfo currentProperty = calledProperties.Pop();

                propertyPathBuilder.Append(currentProperty.Name);
                if (calledProperties.Count > 0)
                {
                    propertyPathBuilder.Append(seperator);
                }
            }

            return propertyPathBuilder.ToString();
        }
    }
}