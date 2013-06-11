namespace Prototype.NancyFX.Models {
    using System.Data.SqlClient;

    public class SortArgument {
        public SortOrder SortOrder { get; set; }

        public string Column { get; set; }

        public SortArgument Reverse() {
            SortArgument clone = new SortArgument();
            clone.Column = this.Column;

            if (this.SortOrder != SortOrder.Unspecified) {
                if (this.SortOrder == SortOrder.Ascending) {
                    clone.SortOrder = SortOrder.Descending;
                } else {
                    clone.SortOrder = SortOrder.Ascending;
                }
            }

            return clone;
        }
    }


}