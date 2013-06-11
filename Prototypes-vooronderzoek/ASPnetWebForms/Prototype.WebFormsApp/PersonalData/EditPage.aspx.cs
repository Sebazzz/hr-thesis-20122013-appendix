using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prototype.WebFormsApp.PersonalData
{
    using System.Globalization;

    using Prototype.Common;

    public partial class EditPage : System.Web.UI.Page {
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
            if (IsEditingNew)
            {
                this.Page.Title = GetLocalResourceObject("PageTitleNew").ToString();
            } else {
                this.Page.Title = String.Format(GetLocalResourceObject("PageTitleEdit").ToString(), this.CurrentEntity.FullName);

                this.CancelEditLink.HRef = "javascript:parent.cbEditFinished(true);";
            }
            
            if (!this.IsPostBack) {
                // bind form
                this.UniqueIdTextBox.Text = this.CurrentEntity.UniqueId.ToString(CultureInfo.InvariantCulture);
                this.FullNameTextBox.Text = this.CurrentEntity.FullName;
                this.HouseNumberTextBox.Text = this.CurrentEntity.HouseNumber.ToString();
                this.BirthDateTextBox.Text = this.CurrentEntity.BirthDate.ToShortDateString();
                this.RegistrationDateTextBox.Text = this.CurrentEntity.BirthDate.ToLocalTime().ToShortDateString();
            }
        }

        protected void OnSaveEntityButtonClick(object sender, EventArgs e) {
            this.Validate();
            if (!IsValid) {
                return;
            }

            // bind entity
            this.CurrentEntity.FullName = this.FullNameTextBox.Text;
            this.CurrentEntity.HouseNumber = Convert.ToInt32(this.HouseNumberTextBox.Text);
            this.CurrentEntity.BirthDate = DateTime.Parse(this.BirthDateTextBox.Text);

            if (IsEditingNew) {
                this.Repository.Add(CurrentEntity);
            } else {
                this.Repository.Update(CurrentEntity);
            }
            this.Repository.SaveChanges();

            if (IsCurrentlyOpenedModal) {
                this.ClientScript.RegisterStartupScript(typeof(ScriptDescriptor), "KillModal", "javascript:parent.cbEditFinished(false);");
            } else {
                Response.Redirect(ResolveClientUrl("~/PersonalData/ListPage.aspx"));
            }
        }
    }
}