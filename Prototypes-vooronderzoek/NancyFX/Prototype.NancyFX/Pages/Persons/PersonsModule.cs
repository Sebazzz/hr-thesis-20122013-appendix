namespace Prototype.NancyFX.Pages.Persons {
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;

    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Responses;

    using Prototype.Common;
    using Prototype.NancyFX.Models;

    /// <summary>
    /// Module for person related CRUD
    /// </summary>
    public sealed class PersonsModule : NancyModule {
        private readonly PersonRepository repository;

        public PersonsModule(PersonRepository repository) : base("/persons") {
            this.repository = repository;

            // set-up routes
            Get["/"] = this.ShowOverviewList;

            Get["/add"] = this.ShowAddForm;
            Put["/add"] = this.ExecuteAdd;
            Post["/add"] = this.ExecuteAdd;

            Get["/edit/{id}"] = this.ShowEditForm;
            Post["/edit/{id}"] = this.ExecuteEdit;

            Get["/delete/{id}"] = this.ShowDeleteConfirmation;
            Post["/delete/{id}"] = this.ExecuteDelete;
            Delete["/delete/{id}"] = this.ExecuteDelete;
        }

        private dynamic ExecuteDelete(dynamic args)
        {
            // get id
            long id = default(long);
            if (args.id == null || !Int64.TryParse(args.id, NumberStyles.None, CultureInfo.InvariantCulture, out id))
            {
                return new RedirectResponse("/persons/", RedirectResponse.RedirectType.Permanent);
            }

            this.repository.RemoveById(id);

            return new RedirectResponse("/persons/", RedirectResponse.RedirectType.Permanent);
        }

        private dynamic ShowDeleteConfirmation(dynamic args) {
            // get id
            long id = default(long);
            if (args.id == null || !Int64.TryParse(args.id, NumberStyles.None, CultureInfo.InvariantCulture, out id)) {
                return new RedirectResponse("/persons/", RedirectResponse.RedirectType.Permanent);
            }

            // set-up vm
            Person context = this.repository.GetById(id);
            if (context == null) {
                return new RedirectResponse("/persons/", RedirectResponse.RedirectType.Permanent);
            }

            return View["DeleteConfirm", context];
        }

        private dynamic ShowEditForm(dynamic args)
        {
            // get id
            long id = default(long);
            if (args.id == null || !Int64.TryParse(args.id, NumberStyles.None, CultureInfo.InvariantCulture, out id))
            {
                return new RedirectResponse("/persons/", RedirectResponse.RedirectType.Permanent);
            }

            // set-up vm
            Person context = this.repository.GetById(id);
            if (context == null)
            {
                return new RedirectResponse("/persons/", RedirectResponse.RedirectType.Permanent);
            }

            this.ViewBag.IsModalOpened = this.Context.Request.Query.isModalOpened ?? false;

            return View["EditForm", context];
        }

        private dynamic ExecuteEdit(dynamic args)
        {
            // get id
            long id = default(long);
            if (args.id == null || !Int64.TryParse(args.id, NumberStyles.None, CultureInfo.InvariantCulture, out id))
            {
                return new RedirectResponse("/persons/", RedirectResponse.RedirectType.Permanent);
            }

            // set-up vm
            Person context = this.repository.GetById(id);
            if (context == null)
            {
                return new RedirectResponse("/persons/", RedirectResponse.RedirectType.Permanent);
            }

            // bind and validate
            this.BindToAndValidate(context, "UniqueId", "RegistrationDate");

            if (!this.ModelValidationResult.IsValid) {
                return View["ValidationErrors", this.ModelValidationResult];
            }

            // update repo and redirect
            this.repository.Update(context);

            this.ViewBag.IsModalOpened = this.Context.Request.Query.isModalOpened ?? false;

            return new RedirectResponse("/persons/");
        }

        private dynamic ExecuteAdd(dynamic args)
        {
            Person newPerson = new Person();

            // bind and validate
            this.BindToAndValidate(newPerson, "UniqueId", "RegistrationDate");

            if (!this.ModelValidationResult.IsValid)
            {
                return View["ValidationErrors", this.ModelValidationResult];
            }

            // update repo and redirect
            this.repository.Add(newPerson);

            this.ViewBag.IsModalOpened = this.Context.Request.Query.isModalOpened ?? false;

            return new RedirectResponse("/persons/");
        }

        private dynamic ShowAddForm(dynamic args)
        {
            Person p = new Person();

            this.ViewBag.IsModalOpened = this.Context.Request.Query.isModalOpened ?? false;


            return this.View["CreateForm", p];
        }

        private dynamic ShowOverviewList(dynamic args) {
            SortArgument sortArg = this.Bind<SortArgument>();

            // get all persons, execute sorting
            IEnumerable<Person> persons;
            if (sortArg == null || String.IsNullOrEmpty(sortArg.Column)) {
                persons = this.repository.GetAll();
            } else {
                string sortString =
                    sortArg.Column + " " +
                    (sortArg.SortOrder != SortOrder.Descending ? "ASC" : "DESC");

                persons = this.repository.GetAll(sortString);
            }

            // view model
            if (sortArg == null) {
                sortArg = new SortArgument();
            }

            PersonListViewModel vm = new PersonListViewModel(persons, sortArg.Reverse());

            return View["OverviewList", vm];
        }
    }
}