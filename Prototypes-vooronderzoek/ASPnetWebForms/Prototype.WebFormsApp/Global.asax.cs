namespace Prototype.WebFormsApp {
    using System;
    using System.Web;

    public class Global : HttpApplication {
        private void Application_BeginRequest(object sender, EventArgs e) {
            if (this.Context.Session != null) {
                CultureManager.OnRequestStart(this.Context);
            }
        }

        private void Session_Start(object sender, EventArgs e)
        {
            CultureManager.OnRequestStart(this.Context);
        }
    }
}