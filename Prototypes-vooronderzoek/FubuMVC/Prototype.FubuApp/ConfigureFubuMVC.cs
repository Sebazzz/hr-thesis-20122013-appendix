using FubuMVC.Core;

namespace Prototype.FubuApp
{
    using AutoMapper;

    using FubuMVC.Core.UI;
    using FubuMVC.Razor;

    using HtmlTags;

    using Prototype.Common;
    using Prototype.FubuApp.Core;
    using Prototype.FubuApp.Pages.Home;
    using Prototype.FubuApp.Pages.Persons;

    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC() {
            Actions.IncludeClassesSuffixedWithController();

            // register PersonRepository
            Services(sr => sr.AddService(PersonRepositoryProvider.GetInstance()));

            Policies.WrapWith<SetCultureBehavior>();

            Routes
                .HomeIs<HomeController>(x => x.Index(null))
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodSuffix("Html")
                .RootAtAssemblyNamespace();

            Import<RazorEngineRegistry>();
            Import<ApplicationGeneratedHtmlConvention>();

            // configure mapper
            Mapper.CreateMap<Person, PersonModifyBase>()
                  .ForMember(vm => vm.RedirectTo, opt => opt.Ignore());

            Mapper.CreateMap<PersonModifyBase, Person>()
                  .ForMember(p => p.UniqueId, opt => opt.Ignore())
                  .ForMember(p => p.RegistrationDate, opt => opt.Ignore())
                  .ForSourceMember(vm => vm.RedirectTo, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }

    public class ApplicationGeneratedHtmlConvention : HtmlConventionRegistry {
        public ApplicationGeneratedHtmlConvention() {
            this.Editors
                .If(x => x.Accessor.InnerProperty.GetCustomAttributes(typeof(ApplicationGeneratedAttribute), true).Length > 0)
                .BuildBy(r =>
                    new HtmlTag("span")
                        .Append(
                            new TextboxTag()
                                .Name(r.ElementId + "_RO")
                                .Id(r.ElementId + "_RO")
                                .Attr("readonly", "readonly")
                                .Value(r.StringValue()))
                        .Append(
                            new HiddenTag()
                                .Id(r.ElementId)
                                .Name(r.ElementId)
                                .Value(r.StringValue())));
        }
    }
}