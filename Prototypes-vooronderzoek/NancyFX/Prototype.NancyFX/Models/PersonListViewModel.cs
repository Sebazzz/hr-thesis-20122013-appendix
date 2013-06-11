namespace Prototype.NancyFX.Models {
    using System.Collections.Generic;

    using Prototype.Common;

    public class PersonListViewModel {
        public SortArgument SortState { get; set; }

        public IEnumerable<Person> Persons { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PersonListViewModel() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PersonListViewModel(IEnumerable<Person> persons, SortArgument sortState) {
            this.Persons = persons;
            this.SortState = sortState;
        }
    }
}