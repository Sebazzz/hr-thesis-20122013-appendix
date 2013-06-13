namespace Eey.Cms.Filters {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Eey.Cms.Data.Repositories;
    using Eey.Cms.Models;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InjectLayoutViewModelAttribute : ActionFilterAttribute {
        public const string ItemContextKey = "__LayoutPageViewModel";

        public ICmsPageRepository CmsPageRepository { get; set; }

        protected virtual LayoutViewModel CreateLayoutViewModel() {
            return new LayoutViewModel();
        }

        protected virtual void InitializeLayoutViewModel(LayoutViewModel layoutViewModel) {
            layoutViewModel.VisiteablePages = this.GetVisitablePages().ToArray();
        }

        private IEnumerable<VisiteablePage> GetVisitablePages() {
            return from page in this.CmsPageRepository.GetRootCmsPages().AsQueryable()
                   orderby page.Title
                   select new VisiteablePage { Id = page.Id, Name = page.Title };
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            LayoutViewModel viewModel = this.CreateLayoutViewModel();
            this.InitializeLayoutViewModel(viewModel);

            filterContext.HttpContext.Items[ItemContextKey] = viewModel;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            ViewResultBase viewResult = filterContext.Result as ViewResultBase;

            if (viewResult != null) {
                viewResult.ViewBag.LayoutViewModel = filterContext.HttpContext.Items[ItemContextKey];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.ActionFilterAttribute"/> class.
        /// </summary>
        public InjectLayoutViewModelAttribute() {
            this.Order = 0;
        }
    }
}