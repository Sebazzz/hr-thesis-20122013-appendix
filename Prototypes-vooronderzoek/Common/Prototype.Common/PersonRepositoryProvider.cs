namespace Prototype.Common {
    using System;

    public static class PersonRepositoryProvider {
        private static readonly PersonRepository Instance;

        static PersonRepositoryProvider() {
            Instance = new PersonRepository();

            // seed the repository somewhat
            Instance.Add(new Person("Sebastiaan Dammann", 22, new DateTime(1991, 07, 30)));

            Instance.Add(new Person("Aldo Quispel", 1, DateTime.UtcNow.AddYears(-36)) { RegistrationDate = DateTime.UtcNow.AddHours(-302) });

            Instance.Add(new Person("Michel van Wijk", 1, DateTime.UtcNow.AddYears(-39)) { RegistrationDate = DateTime.UtcNow.AddHours(-32) });

            Instance.Add(new Person("Patrick Themotus", 1, DateTime.UtcNow.AddYears(-41)) { RegistrationDate = DateTime.UtcNow.AddHours(-94) });
        }

        public static PersonRepository GetInstance() {
            return Instance;
        }
    }
}