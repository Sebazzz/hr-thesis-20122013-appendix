namespace Prototype.FubuApp.Pages.Persons {
    using System.Data.SqlClient;

    public class IndexInputModel {
        public SortOrder SortOrder { get; set; }

        public string SortColumn { get; set; }
    }
}