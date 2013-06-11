namespace Prototype.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public sealed class PersonRepository : IPersonRepository {
        private readonly List<Person> persons;

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public PersonRepository() {
            this.persons = new List<Person>();
        }

        public void Add(Person p) {
            if (p == null) {
                throw new ArgumentNullException("p");
            }

            this.persons.Add(p);
        }

        public void Remove(Person p) {
            if (p == null) {
                throw new ArgumentNullException("p");
            }

            this.persons.Remove(p);
        }

        public void RemoveById(long identifier) {
            this.persons.RemoveAll(p => p.UniqueId == identifier);
        }

        public Person GetById(long identifier) {
            return this.persons.FirstOrDefault(p => p.UniqueId == identifier);
        }

        public IEnumerable<Person> GetAll() {
            return GetAll(null);
        }

        public IEnumerable<Person> GetAll(string sortBy) {
            IEnumerable<Person> all = this.persons;

            if (!String.IsNullOrEmpty(sortBy)) {
                if (sortBy.StartsWith("FullName"))
                {
                    return sortBy.EndsWith("DESC") ? all.OrderByDescending(p => p.FullName) : all.OrderBy(p => p.FullName);
                }

                if (sortBy.StartsWith("UniqueId"))
                {
                    return sortBy.EndsWith("DESC") ? all.OrderByDescending(p => p.UniqueId) : all.OrderBy(p => p.UniqueId);
                }
            }

            return all;
        }

        public void Update(Person p) {
            this.RemoveById(p.UniqueId);
            this.Add(p);
        }

        public void SaveChanges() {
            Thread.Sleep(250); // simulate slow DB
        }
    }
}