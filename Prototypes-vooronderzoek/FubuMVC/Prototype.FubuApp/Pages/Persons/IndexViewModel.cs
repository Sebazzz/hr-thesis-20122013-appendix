namespace Prototype.FubuApp.Pages.Persons {
    using System.Collections.Generic;
    using System.Data.SqlClient;

    using Prototype.Common;

    public class IndexViewModel {

        public IEnumerable<Person> Persons;

        public SortOrder CurrentSortOrder;

        public string CurrentSortColumn;
    }
}