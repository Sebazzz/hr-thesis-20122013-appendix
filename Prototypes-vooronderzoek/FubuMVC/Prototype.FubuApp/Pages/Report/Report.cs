using System;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Report
{
    using Prototype.Common;

    public class ReportController
    {
        private readonly PersonRepository personRepository;

        public ReportController(PersonRepository personRepository) {
            this.personRepository = personRepository;
        }

        public ReportViewModel Index(ReportInputModel input) {
            return new ReportViewModel(this.personRepository.GetAll());
        }
    }
}