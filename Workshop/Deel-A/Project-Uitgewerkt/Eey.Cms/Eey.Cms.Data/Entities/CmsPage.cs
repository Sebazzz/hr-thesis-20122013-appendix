namespace Eey.Cms.Data.Entities {
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a CMS page
    /// </summary>
    public class CmsPage : IHasIdentifier {

        public int Id { get; set; }

        [StringLength(140, MinimumLength = 3)]
        [Display(Name = "Page Title")]
        public string Title { get; set; }

        [StringLength(4000, MinimumLength = 20)]
        [Display(Name = "Body text")]
        [DataType(DataType.MultilineText)]
        public virtual string Body { get; set; }

        public virtual ICollection<CmsPage> Children { get; set; }

        public virtual CmsPage Parent { get; set; }
    }
}
