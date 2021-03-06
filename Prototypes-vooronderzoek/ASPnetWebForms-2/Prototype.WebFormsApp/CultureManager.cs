﻿namespace Prototype.WebFormsApp {
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;

    public static class CultureManager {
        private static readonly CultureInfo[] Cultures;

        static CultureManager() {
            Cultures = new[] { new CultureInfo("en-US", false), new CultureInfo("nl-NL", false) };
        }

        public static void OnRequestStart(HttpContext currentContext) {
            string sessionCulture = currentContext.Session["Culture"] as String;
            if (sessionCulture != null) {
                SetCulture(Cultures.First(c => c.Name == sessionCulture));
            } else {
                // detect language
                string twoLetterName = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

                CultureInfo c = Cultures.FirstOrDefault(culture => culture.TwoLetterISOLanguageName == twoLetterName) ?? Cultures[0];

                SetCulture(c);
                currentContext.Session["Culture"] = c.Name;
            }
        }

        public static void ToggleCulture(HttpContext currentContext) {
            string sessionCulture = currentContext.Session["Culture"] as String;

            CultureInfo newCulture = Cultures.First(c => c.Name != sessionCulture);
            SetCulture(newCulture);
            currentContext.Session["Culture"] = newCulture.Name;
        }

        public static CultureInfo GetOtherCulture(HttpContext currentContext) {
            string sessionCulture = currentContext.Session["Culture"] as String;
            return (Cultures.First(c => c.Name != sessionCulture));
        }

        private static void SetCulture(CultureInfo culture) {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}