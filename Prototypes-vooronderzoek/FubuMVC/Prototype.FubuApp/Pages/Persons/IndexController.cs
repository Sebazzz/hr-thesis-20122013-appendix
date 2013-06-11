using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Persons
{
    using Prototype.Common;

    public class IndexController {
        private readonly PersonRepository personRepository;

        public IndexController(PersonRepository personRepository) {
            this.personRepository = personRepository;
        }

        public IndexViewModel Index(IndexInputModel input) {
            IndexViewModel vm = new IndexViewModel();

            vm.Persons = personRepository.GetAll();

            return vm;
        }
    }
}