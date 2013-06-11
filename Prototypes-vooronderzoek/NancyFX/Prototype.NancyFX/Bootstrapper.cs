namespace Prototype.NancyFX {
    using System;
    using System.Collections.Generic;

    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Diagnostics;
    using Nancy.Elmah;
    using Nancy.TinyIoc;

    using Prototype.Common;

    public sealed class Bootstrapper : DefaultNancyBootstrapper {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines) {
            base.ApplicationStartup(container, pipelines);

            // configure error handling and diagnostics
            Elmahlogging.Enable(pipelines, "_diag/_elmah", new string[0], new[] { HttpStatusCode.NotFound, HttpStatusCode.InsufficientStorage, });

            StaticConfiguration.EnableRequestTracing = true;
            StaticConfiguration.DisableErrorTraces = false;

            // configure view conventions
            this.Conventions.ViewLocationConventions.Clear();
            this.Conventions.ViewLocationConventions.Insert(0, (viewName, model, context) =>
            {
                if (viewName.EndsWith("Master.cshtml")) {
                    if (viewName.StartsWith("/")) {
                        return viewName.Substring(1);
                    }

                    return viewName;
                }

                return String.Format("Pages/{0}/Views/{1}.cshtml", context.ModuleName, viewName);
            });

            // configure culture handling
            pipelines.BeforeRequest += CultureManager.HandleCulture;

            // session configuration
            Nancy.Session.CookieBasedSessions.Enable(pipelines);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context) {
            
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context) {
            // set-up dependencies
            container.Register((_, __) => PersonRepositoryProvider.GetInstance());
            
            base.ConfigureRequestContainer(container, context);
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration {
            get {
                return new DiagnosticsConfiguration() {
                                                              Password = "welkom01", 
                                                              SlidingTimeout = 60 * 60,
                                                              Path = "_diag/_nancy"
                                                      };
            }
        }

        protected override IEnumerable<Type> ViewEngines {
            get {
                return base.ViewEngines.NotOfType<Nancy.ViewEngines.SuperSimpleViewEngine.SuperSimpleViewEngineWrapper>();
            }
        }
    }
}