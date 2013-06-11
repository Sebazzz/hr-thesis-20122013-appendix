using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prototype.WebFormsApp
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack) {
                SetCultureMenuItemText();
            }
        }

        protected void OnChangeLanguageButtonClick(object sender, EventArgs e) {
            CultureManager.ToggleCulture(this.Context);

            this.Response.Redirect(this.Request.Path);
        }

        private void SetCultureMenuItemText() {
            this.LanguageToggleButton.Text = CultureManager.GetOtherCulture(this.Context).DisplayName;
        }
    }
}
