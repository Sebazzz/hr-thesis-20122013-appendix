using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Persons
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using FubuMVC.Core.Continuations;

    using Prototype.Common;
    using Prototype.Common.Resources;

    public class EditViewModel : PersonModifyBase
    {
        public bool IsModalOpened { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EditViewModel(Person currentPerson, bool isModalOpened)
                : base(currentPerson) {
            this.IsModalOpened = isModalOpened;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EditViewModel(FubuContinuation redirectTo)
                : base(redirectTo) {
        }
    }

    public abstract class PersonModifyBase : IRedirectable {
        public FubuContinuation RedirectTo { get; set; }

        [Display(ResourceType = typeof(PersonStrings), Name = "UniqueId_Name", ShortName = "UniqueId_Name")]
        [ApplicationGenerated]
        public long UniqueId { get; set; }

        [Display(ResourceType = typeof(PersonStrings), Name = "FullName_Name", ShortName = "FullName_Name")]
        public string FullName { get; set; }

        public int HouseNumber { get; set; }

        public DateTime BirthDate { get; set; }

        [ApplicationGenerated]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected PersonModifyBase(FubuContinuation redirectTo) {
            this.RedirectTo = redirectTo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected PersonModifyBase() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected PersonModifyBase(Person currentPerson) {
            Mapper.Map(currentPerson, this);
        }
    }
}