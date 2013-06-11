using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Persons
{
    using Prototype.Common;

    public class CreateController
    {
        private readonly PersonRepository personRepository;

        public CreateController(PersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public CreateViewModel Create(CreateInputModel input) {
            
            return new CreateViewModel(new Person(), false);
        }
    }
}