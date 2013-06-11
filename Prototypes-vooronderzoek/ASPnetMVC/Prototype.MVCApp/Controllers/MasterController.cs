namespace Prototype.MVCApp.Controllers {
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    ///   Represents the base controller for each controller in the application
    /// </summary>
    public abstract partial class MasterController : Controller
    {
        /// <summary>
        /// Initializes data that might not be available when the constructor is called.
        /// </summary>
        /// <param name="requestContext">The HTTP context and route data.</param>
        protected override void Initialize(RequestContext requestContext) {
            // set the culture
            CultureManager.OnRequestStart(requestContext.HttpContext);

            base.Initialize(requestContext);
        }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext) {
            // set culture data
            CultureInfo otherCulture = CultureManager.GetOtherCulture(this.HttpContext);
            this.ViewBag.OtherCultureName = otherCulture.DisplayName;
            this.ViewBag.OtherCultureCode = otherCulture.TwoLetterISOLanguageName;

            this.ViewBag.PageCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// Changes the culture and redirects to referrer
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public virtual ActionResult ChangeCulture(string lang)
        {
            CultureManager.ToggleCulture(this.HttpContext);

            Uri referrer = this.HttpContext.Request.UrlReferrer;
            if (referrer == null) {
                return RedirectToAction("Index");
            }

            return Redirect(referrer.ToString());
        }
    }
}