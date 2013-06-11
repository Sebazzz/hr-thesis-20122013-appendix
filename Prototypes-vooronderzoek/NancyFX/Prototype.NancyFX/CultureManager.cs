namespace Prototype.NancyFX {
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;

    using Nancy;
    using Nancy.Responses;
    using Nancy.Session;

    public class CultureManager {
        private static readonly CultureInfo[] Cultures;

        static CultureManager()
        {
            Cultures = new[] { new CultureInfo("en-US", false), new CultureInfo("nl-NL", false) };
        }

        /// <summary>
        /// Handles changing and setting culture info
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Response HandleCulture(NancyContext context) {
            ISession session = context.Request.Session;

            // enable change of cultures
            if (context.Request.Url.Query.Contains("ChangeCulture"))
            {
                session["Culture"] = Cultures.First(c => c.Name != (string)session["Culture"]).Name;

                return new RedirectResponse(context.Request.Path);
            }

            // set or initialize current culture
            string sessionCulture = session["Culture"] as String;
            if (sessionCulture != null)
            {
                SetCulture(Cultures.First(c => c.Name == sessionCulture));
            }
            else
            {
                // detect language
                string twoLetterName = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

                CultureInfo c = Cultures.FirstOrDefault(culture => culture.TwoLetterISOLanguageName == twoLetterName) ?? Cultures[0];

                SetCulture(c);
                session["Culture"] = c.Name;
            }

            // initialize view bag
            context.ViewBag.OtherCultureName = GetOtherCulture().DisplayName;

            return null;
        }

        private static CultureInfo GetOtherCulture()
        {
            return (Cultures.First(c => c.Name != Thread.CurrentThread.CurrentUICulture.Name));
        }

        private static void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}