using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eey.Cms.Models {
    public class LayoutViewModel {

        public IEnumerable<VisiteablePage> VisiteablePages { get; set; }
    }

    public class VisiteablePage {

        public string Name { get; set; }

        public int Id { get; set; }
    }
}