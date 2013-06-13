namespace Eey.Cms.Areas.Admin.Controllers {
    using System.Web.Mvc;

    public class HomeController : ControllerBase {
        public ActionResult Index() {
            return this.View();
        }
    }
}