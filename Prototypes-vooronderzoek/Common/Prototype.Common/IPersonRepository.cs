namespace Prototype.Common {
    using System.Collections.Generic;

    public interface IPersonRepository {
        void Add(Person p);

        void Remove(Person p);

        void RemoveById(long identifier);

        Person GetById(long identifier);

        IEnumerable<Person> GetAll();

        void Update(Person p);

        void SaveChanges();
    }
}