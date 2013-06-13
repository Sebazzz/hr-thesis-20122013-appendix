using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eey.Cms.Data.Repositories {
    using Eey.Cms.Data.Entities;

    public interface ICmsPageRepository : IRepository<CmsPage> {
        IEnumerable<CmsPage> GetRootCmsPages();
    }

    public sealed class CmsPageRepository : Repository<CmsPage>, ICmsPageRepository {
        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public CmsPageRepository(DatabaseContext context)
            : base(context) {
        }

        public IEnumerable<CmsPage> GetRootCmsPages() {
            return this.Entities.Where(c => c.Parent == null);
        }
    }
}
