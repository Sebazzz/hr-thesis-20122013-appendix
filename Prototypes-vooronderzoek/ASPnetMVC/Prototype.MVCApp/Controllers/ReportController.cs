namespace Prototype.MVCApp.Controllers {
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Prototype.Common;

    public partial class ReportController : MasterController {
        private readonly PersonRepository personRepository;

        public ReportController() {
            this.personRepository = PersonRepositoryProvider.GetInstance();
        }

        public virtual ActionResult Index() {
            IEnumerable<Person> persons = this.personRepository.GetAll();

            return View(persons);
        }
    }
}