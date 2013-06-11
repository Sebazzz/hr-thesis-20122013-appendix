using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Persons
{
    using FubuMVC.Core;

    public class EditInputModel
    {

        [QueryString]
        public bool IsModalOpened { get; set; }

        [QueryString]
        public long Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EditInputModel() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EditInputModel(long id) {
            this.Id = id;
        }
    }
}