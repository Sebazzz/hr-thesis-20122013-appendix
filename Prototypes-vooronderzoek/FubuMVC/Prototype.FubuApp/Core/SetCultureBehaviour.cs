using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Core
{
    using System.Globalization;
    using System.Threading;

    using FubuMVC.Core;
    using FubuMVC.Core.Behaviors;
    using FubuMVC.Core.Runtime;

    public class SetCultureBehavior : BasicBehavior {
        private static readonly CultureInfo[] Cultures;

        static SetCultureBehavior()
        {
            Cultures = new[] { new CultureInfo("en-US", false), new CultureInfo("nl-NL", false) };
        }

        private readonly ISessionState session;
        private readonly IFubuRequest fubuRequest;

        public SetCultureBehavior(ISessionState session, IFubuRequest fubuRequest): base(PartialBehavior.Executes) {
            this.session = session;
            this.fubuRequest = fubuRequest;
        }

        protected override DoNext performInvoke() {
            // get culture from query string
            {
                SelectedCulture requestedCulture = this.fubuRequest.Get<SelectedCulture>();

                if (requestedCulture != null && !String.IsNullOrEmpty(requestedCulture.LetterCultureName)) {
                    CultureInfo c = Cultures.FirstOrDefault(cc => cc.TwoLetterISOLanguageName == requestedCulture.LetterCultureName);
                    CultureInfo nc = Cultures.FirstOrDefault(cc => cc.TwoLetterISOLanguageName != requestedCulture.LetterCultureName);

                    if (c != null) {
                        this.session.Set(new SelectedCulture(c));
                        this.session.Set(new OtherCulture(nc));
                    }
                }
            }

            // set culture from session
            SelectedCulture culture = session.Get<SelectedCulture>();
            if (culture != null) {
                CultureInfo cultureInstance = new CultureInfo(culture.LetterCultureName);

                Thread.CurrentThread.CurrentUICulture = cultureInstance;
                Thread.CurrentThread.CurrentCulture = cultureInstance;
            } else {
                // detect culture 
                string twoLetterName = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

                CultureInfo c = Cultures.FirstOrDefault(cc => cc.TwoLetterISOLanguageName == twoLetterName) ?? Cultures[0];
                CultureInfo nc = Cultures.FirstOrDefault(cc => cc.TwoLetterISOLanguageName != twoLetterName);

                Thread.CurrentThread.CurrentUICulture = c;
                Thread.CurrentThread.CurrentCulture = c;

                this.session.Set(new OtherCulture(nc));
                this.session.Set(new SelectedCulture(c));
            }

            return DoNext.Continue;
        }
    }

    public class OtherCulture : Culture {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public OtherCulture(CultureInfo info)
                : base(info) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public OtherCulture() {
        }
    }

    public class SelectedCulture : Culture {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SelectedCulture(CultureInfo info)
                : base(info) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SelectedCulture() {
        }
    }

    public class Culture
    {
        [QueryString]
        public string LetterCultureName { get; set; }

        public string DisplayName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Culture() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Culture(CultureInfo info)
        {
            this.LetterCultureName = info.TwoLetterISOLanguageName;
            this.DisplayName = info.DisplayName;
        }
    }
}