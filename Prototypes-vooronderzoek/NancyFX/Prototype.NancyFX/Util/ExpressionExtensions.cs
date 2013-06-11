namespace Prototype.NancyFX.Util {
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    /// <summary>
    ///   Static helper class that provides extensions for <see cref="Expression{TDelegate}" /> objects
    /// </summary>
    public static class ExpressionExtensions {
        /// <summary>
        ///   Gets the property called using the specified property lambda expression.
        /// </summary>
        /// <typeparam name="TSource"> The type of the source. </typeparam>
        /// <typeparam name="TProperty"> The type of the property. </typeparam>
        /// <param name="propertyLambda"> The property lambda. </param>
        /// <param name="checkIfPropertyForCurrentType"> Check if the property selected in the <paramref name="propertyLambda" /> is a property of the <typeparamref
        ///    name="TSource" /> </param>
        /// <returns> </returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda, bool checkIfPropertyForCurrentType = false) {
            Type type = typeof(TSource);

            // check if it's a member expression (should be)
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null) {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda));
            }

            // check if it's a property
            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null) {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda));
            }

            if (checkIfPropertyForCurrentType) {
                if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType)) {
                    throw new ArgumentException(string.Format("Expression '{0}' refers to a property that is not from type {1}.", propertyLambda, type));
                }
            }

            return propInfo;
        }

        /// <summary>
        ///   Gets a path in the form of PropertyName.PropertyName2 from a expression like (e => e.PropertyName.PropertyName2)
        /// </summary>
        /// <typeparam name="TSource"> </typeparam>
        /// <typeparam name="TProperty"> </typeparam>
        /// <param name="propertyLambda"> </param>
        /// <returns> </returns>
        public static string GetPropertyPath<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda) {
            // we need to walk the stack of properties called backwards to get the property path
            // A expression like e => e.A.B.C.D will contain chained expressions starting from D

            // check if it's a member expression (should be)
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null) {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda));
            }

            Stack<PropertyInfo> calledProperties = new Stack<PropertyInfo>();

            while (member != null) {
                // check if it's a property (current member)
                PropertyInfo propInfo = member.Member as PropertyInfo;
                if (propInfo == null) {
                    throw new ArgumentException(string.Format("Expression '{0}' refers to or via a field, not a property.", propertyLambda));
                }

                calledProperties.Push(propInfo);

                member = member.Expression as MemberExpression;
            }

            const char seperator = '.';
            StringBuilder propertyPathBuilder = new StringBuilder();
            while (calledProperties.Count > 0) {
                PropertyInfo currentProperty = calledProperties.Pop();

                propertyPathBuilder.Append(currentProperty.Name);
                if (calledProperties.Count > 0) {
                    propertyPathBuilder.Append(seperator);
                }
            }

            return propertyPathBuilder.ToString();
        }
    }
}