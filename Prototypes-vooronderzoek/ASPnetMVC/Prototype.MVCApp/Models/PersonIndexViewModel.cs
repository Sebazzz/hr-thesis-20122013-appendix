using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.MVCApp.Models
{
    using System.Data.SqlClient;

    using Prototype.Common;

    public class PersonIndexViewModel {

        public IEnumerable<Person> Persons;

        public string SortColumn;

        public SortOrder SortOrder;
    }
}