using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Persons
{
    using Prototype.Common;

    public class DeleteController
    {
        private readonly PersonRepository personRepository;

        public DeleteController(PersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public DeleteViewModel Delete(DeleteInputModel input) {
            return new DeleteViewModel();
        }
    }
}