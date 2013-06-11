namespace Prototype.Common.GenericAnnotations {
    using System;
    using System.ComponentModel.DataAnnotations;

    using Prototype.Common.Resources;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GenericRangeAttribute : RangeAttribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.RangeAttribute" /> class by using the specified minimum and maximum values and the specific type.
        /// </summary>
        /// <param name="type"> Specifies the type of the object to test. </param>
        /// <param name="minimum"> Specifies the minimum value allowed for the data field value. </param>
        /// <param name="maximum"> Specifies the maximum value allowed for the data field value. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" />
        ///   is null.</exception>
        public GenericRangeAttribute(Type type, string minimum, string maximum)
                : base(type, minimum, maximum) {
            this.SetResources();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.RangeAttribute" /> class by using the specified minimum and maximum values.
        /// </summary>
        /// <param name="minimum"> Specifies the minimum value allowed for the data field value. </param>
        /// <param name="maximum"> Specifies the maximum value allowed for the data field value. </param>
        public GenericRangeAttribute(double minimum, double maximum)
                : base(minimum, maximum) {
            this.SetResources();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.RangeAttribute" /> class by using the specified minimum and maximum values.
        /// </summary>
        /// <param name="minimum"> Specifies the minimum value allowed for the data field value. </param>
        /// <param name="maximum"> Specifies the maximum value allowed for the data field value. </param>
        public GenericRangeAttribute(int minimum, int maximum)
                : base(minimum, maximum) {
            this.SetResources();
        }

        private void SetResources() {
            this.ErrorMessageResourceType = typeof(PersonStrings);
            this.ErrorMessageResourceName = "Generic_Range";
        }
    }
}