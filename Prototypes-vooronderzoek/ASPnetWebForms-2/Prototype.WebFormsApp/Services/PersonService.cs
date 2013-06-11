using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.WebFormsApp.Services
{
    using Prototype.Common;

    public sealed class PersonService
    {
        private readonly HttpRequest currentRequest;
        private readonly PersonRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PersonService(PersonRepository repository, HttpRequest currentRequest)
        {
            this.repository = repository;
            this.currentRequest = currentRequest;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public IEnumerable<Person> GetPerson(long id)
        {
            //// get ID
            //long id = 0;

            if (String.IsNullOrEmpty(this.currentRequest.QueryString["id"]))
            {
                return new[] { GetNewPerson() };
            }

            if (!Int64.TryParse(this.currentRequest.QueryString["id"], out id))
            {
                return new[] { this.GetNewPerson() };
            }

            // get obj
            return new[] { this.repository.GetById(id) };
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public void InsertPerson(Person person)
        {
            //Person person = new Person(0, fullName, houseNumber, birthDate, DateTime.UtcNow);
            
            this.repository.Add(person);

            this.repository.SaveChanges();
        }


        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
        public void UpdatePerson(Person person)
        {
            //Person person = new Person(uniqueId, fullName, houseNumber, birthDate, DateTime.UtcNow);

            this.repository.Update(person);

            this.repository.SaveChanges();
        }

        private Person GetNewPerson()
        {
            return new Person() { BirthDate = DateTime.UtcNow };
        }
    }
}