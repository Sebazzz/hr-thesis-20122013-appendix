namespace Prototype.Common.GenericAnnotations {
    using System;
    using System.ComponentModel.DataAnnotations;

    using Prototype.Common.Resources;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GenericRequiredAttribute : RequiredAttribute {
        public GenericRequiredAttribute() {
            this.ErrorMessageResourceName = "Generic_Required";
            this.ErrorMessageResourceType = typeof(PersonStrings);
        }
    }
}