namespace Prototype.NancyFX.Pages.Diagnostics {
    using Nancy;

    /// <summary>
    ///   Enables access to diagnostics
    /// </summary>
    public sealed class DiagnosticsModule : NancyModule {
        public DiagnosticsModule()
                : base("/_diag") {
            this.Get["/"] = _ => this.View["Index"];
        }
    }
}