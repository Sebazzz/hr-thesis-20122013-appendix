namespace Eey.Cms.Areas.Admin.Controllers {
    using System.Web.Mvc;

    using Eey.Cms.Data.Entities;
    using Eey.Cms.Data.Repositories;

    public class PageController : ControllerBase {
        private readonly ICmsPageRepository cmsPageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PageController(ICmsPageRepository cmsPageRepository) {
            this.cmsPageRepository = cmsPageRepository;
        }


        public ActionResult Index() {
            return View(this.cmsPageRepository.GetAll());
        }

        public ActionResult Create() {
            this.InitializeEditView(null);

            return View(new CmsPage());
        }

        [HttpPost]
        public ActionResult Create(CmsPage page) {
            if (!ModelState.IsValid) {
                this.InitializeEditView(page);

                return View(page);
            }

            this.cmsPageRepository.Add(page);
            this.cmsPageRepository.SaveChanges();

            return RedirectToAction("Edit", new { id = page.Id });
        }

        public ActionResult Edit(int id) {
            CmsPage page = this.cmsPageRepository.GetById(id);

            if (page == null) {
                return View("NotFound");
            }

            this.InitializeEditView(page);

            return View(page);
        }

        [HttpPost]
        public ActionResult Edit(int id, CmsPage page) {
            if (!ModelState.IsValid) {
                this.InitializeEditView(page);

                return View(page);
            }

            this.cmsPageRepository.SaveChanges();

            return RedirectToAction("Edit", new { id = page.Id });
        }

        private void InitializeEditView(CmsPage page) {
            this.ViewBag.ParentPages = new SelectList(
                this.cmsPageRepository.GetAll(), "Id", "Title", page != null && page.Parent != null ? (object)page.Parent.Id : null);
        }
    }
}