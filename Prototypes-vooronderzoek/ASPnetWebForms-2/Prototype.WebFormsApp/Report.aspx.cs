namespace Prototype.WebFormsApp {
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Prototype.Common;

    public partial class Report : Page {
        protected void Page_Load(object sender, EventArgs e) {
        }

        protected void Page_PreInit(object sender, EventArgs e) {
            CultureManager.OnRequestStart(this.Context);
        }

        protected void OnCreateDataSourceObject(object sender, ObjectDataSourceEventArgs e) {
            e.ObjectInstance = PersonRepositoryProvider.GetInstance();
        }

        protected string GetDisplayAttributeValue(object item, string propertyName) {
            Type t = item.GetType();
            PropertyInfo property = t.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (property == null) {
                return SplitOnWord(propertyName);
            }

            // get display attr
            DisplayAttribute displayAttr = property.GetCustomAttributes(typeof(DisplayAttribute), true).OfType<DisplayAttribute>().FirstOrDefault();

            if (displayAttr == null) {
                return SplitOnWord(propertyName);
            }

            return displayAttr.GetName();
        }

        private string SplitOnWord(string input) {
            Regex r = new Regex("([A-Z]+[a-z]+)");
            string result = r.Replace(input, m => (m.Value.Length > 3 ? m.Value : m.Value.ToLower()) + " ");

            return result;
        }
    }
}