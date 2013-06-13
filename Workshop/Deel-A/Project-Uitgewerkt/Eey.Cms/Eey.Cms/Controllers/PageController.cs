namespace Eey.Cms.Controllers {
    using System.Web.Mvc;

    using Eey.Cms.Data.Entities;
    using Eey.Cms.Data.Repositories;

    public class PageController : ControllerBase {
        private readonly ICmsPageRepository cmsPageRepository;

        public PageController(ICmsPageRepository cmsPageRepository) {
            this.cmsPageRepository = cmsPageRepository;
        }

        public ActionResult Index(int id) {
            // get page
            CmsPage currentPage = this.cmsPageRepository.GetById(id);

            if (currentPage == null) {
                return View("NotFound");
            }

            return View(currentPage);
        }
    }
}