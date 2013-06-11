using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prototype.WebFormsApp.PersonalData
{
    using Prototype.Common;

    public partial class RemoveConfirm : System.Web.UI.Page {
        protected PersonRepository Repository;
        protected Person Entity;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CultureManager.OnRequestStart(this.Context);

            // get ID
            long id;
            if (String.IsNullOrEmpty(Request.QueryString["id"]) || !Int64.TryParse(Request.QueryString["id"], out id)) {
                Response.RedirectPermanent(ResolveClientUrl("~/PersonalData/ListPage.aspx"));
                return;
            }

            // get obj
            this.Repository = PersonRepositoryProvider.GetInstance();
            this.Entity = this.Repository.GetById(id);

            if (this.Entity == null) {
                Response.RedirectPermanent(ResolveClientUrl("~/PersonalData/ListPage.aspx"));
                return;
            }

            // set title
            this.Page.Title = String.Format(GetLocalResourceObject("Page.Title").ToString(), this.Entity.FullName);
        }

        protected void OnRemoveConfirmButtonClick(object sender, EventArgs e) {
            this.Repository.RemoveById(this.Entity.UniqueId);

            Response.Redirect(ResolveClientUrl("~/PersonalData/ListPage.aspx"));
        }
    }
}