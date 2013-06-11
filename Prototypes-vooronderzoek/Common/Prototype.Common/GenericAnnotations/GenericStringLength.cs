namespace Prototype.Common.GenericAnnotations {
    using System;
    using System.ComponentModel.DataAnnotations;

    using Prototype.Common.Resources;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GenericStringLengthAttribute : StringLengthAttribute {
        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.StringLengthAttribute" /> class by using a specified maximum length.
        /// </summary>
        /// <param name="maximumLength"> The maximum length of a string. </param>
        public GenericStringLengthAttribute(int maximumLength)
                : base(maximumLength) {
            this.ErrorMessageResourceName = "Generic_StringLength";
            this.ErrorMessageResourceType = typeof(PersonStrings);
        }
    }
}