namespace Prototype.FubuApp.Pages.Report {
    using System.Collections.Generic;
    using System.Linq;

    using Prototype.Common;

    public class ReportViewModel {

        public List<Person> Persons { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ReportViewModel() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ReportViewModel(IEnumerable<Person> persons) {
            this.Persons = persons.ToList();
        }
    }
}