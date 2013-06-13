namespace Eey.Cms.Controllers {
    using System.Web.Mvc;

    using Eey.Cms.Filters;
    using Eey.Cms.Models;

    [InjectLayoutViewModel]
    public class ControllerBase : Controller {
        protected LayoutViewModel ViewModel {
            get {
                return (LayoutViewModel)HttpContext.Items[InjectLayoutViewModelAttribute.ItemContextKey];
            }
        }
    }
}