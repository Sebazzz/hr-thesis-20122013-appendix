using System;
using System.Web.UI.WebControls;

namespace Prototype.WebFormsApp.PersonalData
{
    public partial class ListPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CultureManager.OnRequestStart(this.Context);
        }

        protected void OnCreateDataSourceObject(object sender, ObjectDataSourceEventArgs e) {
            e.ObjectInstance = Common.PersonRepositoryProvider.GetInstance();
        }

    }
}