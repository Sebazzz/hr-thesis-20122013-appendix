using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Persons
{
    using FubuMVC.Core;
    using FubuMVC.Core.Continuations;

    public class DeleteInputModel : IRedirectable 
    {
        [QueryString]
        public long Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DeleteInputModel() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DeleteInputModel(long id) {
            this.Id = id;
        }

        public FubuContinuation RedirectTo { get; set; }
    }
}