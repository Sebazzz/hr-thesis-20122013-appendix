namespace Prototype.NancyFX.Pages {
    using System.Collections.Generic;

    using Nancy;

    using Prototype.Common;
    using Prototype.NancyFX.Models;

    public class ReportModule : NancyModule {
        private readonly PersonRepository repository;

        public ReportModule(PersonRepository repository) : base ("/report") {
            this.repository = repository;
            this.Get["/"] = _ => this.ShowReport();
        }

        private dynamic ShowReport() {
            // set-up view model
            IEnumerable<Person> persons = this.repository.GetAll();

            ReportViewModel vm = new ReportViewModel(persons);

            return View["Index", vm];
        }
    }
}