namespace Prototype.FubuApp.Pages.Persons {
    using FubuMVC.Core.Continuations;

    using Prototype.Common;

    public class CreateViewModel : PersonModifyBase {
        public bool IsModalOpened { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public CreateViewModel(Person currentPerson, bool isModalOpened)
                : base(currentPerson) {
            this.IsModalOpened = isModalOpened;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public CreateViewModel(FubuContinuation redirectTo)
                : base(redirectTo) {
        }
    }
}