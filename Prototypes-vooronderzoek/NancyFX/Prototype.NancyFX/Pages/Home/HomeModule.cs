namespace Prototype.NancyFX.Pages {
    using Nancy;

    public class HomeModule : NancyModule {
        public HomeModule() {
            this.Get["/"] = _ => this.View["Home"];
        }
    }
}