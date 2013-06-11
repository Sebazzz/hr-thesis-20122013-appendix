using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prototype.WebFormsApp.PersonalData
{
    using System.Diagnostics;
    using System.Web.UI.HtmlControls;

    using Prototype.Common;
    using Prototype.WebFormsApp.Services;

    public partial class EditPage : Page {
        protected bool IsCurrentlyOpenedModal = false;

        protected PersonRepository Repository;
        protected Person CurrentEntity;
        protected bool IsEditingNew;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CultureManager.OnRequestStart(this.Context);

            // check if opened in dialog
            IsCurrentlyOpenedModal = Request.QueryString["inDialog"] == "true";

            if (IsCurrentlyOpenedModal) {
                this.MasterPageFile = "~/Modal.Master";
            }
        }


        protected void Page_Load(object sender, EventArgs e) {
            IsEditingNew = false;
            this.Repository = PersonRepositoryProvider.GetInstance();

            if (!Page.IsPostBack) {
                // get ID
                long id = 0;

                if (String.IsNullOrEmpty(Request.QueryString["id"])) {
                    IsEditingNew = true;
                }
                else {
                    if (!Int64.TryParse(Request.QueryString["id"], out id)) {
                        Response.RedirectPermanent(ResolveClientUrl("~/PersonalData/ListPage.aspx"));
                        return;
                    }
                }

                this.form.ChangeMode(IsEditingNew ? FormViewMode.Insert : FormViewMode.Edit);

                // get obj
                this.CurrentEntity = IsEditingNew ? new Person() {BirthDate = DateTime.UtcNow} : this.Repository.GetById(id);

                if (this.CurrentEntity == null)
                {
                    Response.RedirectPermanent(ResolveClientUrl("~/PersonalData/ListPage.aspx"));
                    return;
                }

                ViewState["CurrentEntity"] = this.CurrentEntity;
                ViewState["IsEditingNew"] = IsEditingNew;
                ViewState["IsCurrentlyOpenedModal"] = IsCurrentlyOpenedModal;
            } else {
                IsEditingNew = (bool)ViewState["IsEditingNew"];
                CurrentEntity = (Person)ViewState["CurrentEntity"];
                IsCurrentlyOpenedModal = (bool)ViewState["IsCurrentlyOpenedModal"];
            }

            // set title
            this.Page.Title = this.IsEditingNew ? this.GetLocalResourceObject("PageTitleNew").ToString() : 
                                String.Format(this.GetLocalResourceObject("PageTitleEdit").ToString(), this.CurrentEntity.FullName);
        }

        protected void OnFormViewDataBound(object sender, EventArgs e)
        {
            FormView currentFormView = (FormView)sender;
            Button saveButton = currentFormView.FindControl("SaveButton") as Button;
            HtmlAnchor cancelEditLink = currentFormView.FindControl("CancelEditLink") as HtmlAnchor;

            if (saveButton == null || cancelEditLink == null)
            {
                Debugger.Break();
                return;
            }

            saveButton.CommandName = this.IsEditingNew ? "Insert" : "Update";

            if (this.IsEditingNew)
            {
                cancelEditLink.HRef = "javascript:parent.cbEditFinished(true);";
            }
        }

        protected void OnFormViewInitialize(object sender, EventArgs e)
        {
            this.form.InsertItemTemplate = this.form.EditItemTemplate;
        }

        protected void OnServiceCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = new PersonService(PersonRepositoryProvider.GetInstance(), this.Request);
        }

        protected void OnItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            this.PerformSaveRedirect();
        }

        protected void OnItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            this.PerformSaveRedirect();
        }

        private void PerformSaveRedirect()
        {
            if (IsCurrentlyOpenedModal)
            {
                this.ClientScript.RegisterStartupScript(typeof(ScriptDescriptor), "KillModal", "javascript:parent.cbEditFinished(false);");
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/PersonalData/ListPage.aspx"));
            }
        }

        protected void OnItemInsert(object sender, FormViewInsertEventArgs e)
        {
            e.Cancel = !this.IsValid;
        }
    }
}