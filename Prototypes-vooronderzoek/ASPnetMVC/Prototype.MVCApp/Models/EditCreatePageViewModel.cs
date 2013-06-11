using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.MVCApp.Models
{
    using Prototype.Common;

    public sealed class EditCreatePageViewModel
    {
        public Person Context { get; set; }

        public bool IsOpenedModal { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EditCreatePageViewModel() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EditCreatePageViewModel(Person context, bool isOpenedModal) {
            this.Context = context;
            this.IsOpenedModal = isOpenedModal;
        }
    }
}