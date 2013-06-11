using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prototype.MVCApp.Controllers
{
    using System.Data.SqlClient;

    using Prototype.Common;
    using Prototype.MVCApp.Models;

    public partial class PersonController : MasterController
    {
        private readonly PersonRepository personRepository;

        public PersonController()
        {
            this.personRepository = PersonRepositoryProvider.GetInstance();
        }

        //
        // GET: /Person/
        public virtual ActionResult Index(bool? manualTable, string sortColumn, SortOrder? sortOrder) {
            IEnumerable<Person> personList = personRepository.GetAll().ToList();

            PersonIndexViewModel vm = new PersonIndexViewModel();
            vm.SortColumn = sortColumn;
            vm.SortOrder = sortOrder.GetValueOrDefault(SortOrder.Ascending);

            if (!String.IsNullOrEmpty(sortColumn)) {
                vm.Persons = vm.SortOrder == SortOrder.Descending ?
                    personList.AsQueryable().OrderByDescending(sortColumn).AsEnumerable() :
                    personList.AsQueryable().OrderBy(sortColumn).AsEnumerable();
            } else {
                vm.Persons = personList;
            }

            // select view
            string view = "Index";
            if (manualTable.GetValueOrDefault()) {
                view = "IndexManual";
            }

            return View(view, vm);
        }

        //
        // GET: /Person/Create
        public virtual ActionResult Create(bool isModalOpened = false)
        {
            return View(new EditCreatePageViewModel(new Person(), isModalOpened));
        }

        ////
        //// POST: /Person/Create
        //[HttpPost]
        //public virtual ActionResult Create(bool isModalOpened, FormCollection collection)
        //{
        //    Person p = new Person();
            
        //    try
        //    {
        //        UpdateModel(p, "Context", collection.ToValueProvider());

        //        if (!ModelState.IsValid) {
        //            return View(new EditCreatePageViewModel(p, isModalOpened));
        //        }

        //        this.personRepository.Add(p);
        //        this.personRepository.SaveChanges();

        //        if (isModalOpened) {
        //            return View(Views.CloseModal);
        //        }

        //        return RedirectToAction(MVC.Person.Index());
        //    }
        //    catch
        //    {
        //        return View(new EditCreatePageViewModel(p, isModalOpened));
        //    }
        //}

        //
        // POST: /Person/Create
        [HttpPost]
        public virtual ActionResult Create(Person p) {
            if (!ModelState.IsValid) {
                return View(p);
            }
            
            try {
                this.personRepository.Add(p);
                this.personRepository.SaveChanges();

                return RedirectToAction("Index");
            } catch (Exception ex) {
                ModelState.AddModelError("Error", ex.Message);

                return View(p);
            }
        }

        //
        // GET: /Person/Edit/5

        public virtual ActionResult Edit(long id, bool isModalOpened = false)
        {
            Person context = this.personRepository.GetById(id);

            if (context == null)
            {
                return RedirectToActionPermanent(MVC.Person.Index());
            }

            return View(new EditCreatePageViewModel(context, isModalOpened));
        }

        //
        // POST: /Person/Edit/5

        [HttpPost]
        public virtual ActionResult Edit(long id, bool isModalOpened, FormCollection collection)
        {
            Person context = this.personRepository.GetById(id);

            if (context == null)
            {
                return RedirectToActionPermanent(MVC.Person.Index());
            }

            try
            {
                UpdateModel(context, "Context");

                if (!ModelState.IsValid)
                {
                    return View(new EditCreatePageViewModel(context, isModalOpened));
                }

                this.personRepository.Update(context);
                this.personRepository.SaveChanges();

                if (isModalOpened)
                {
                    return View(Views.CloseModal);
                }

                return RedirectToAction(MVC.Person.Index());
            }
            catch
            {
                return View(new EditCreatePageViewModel(context, isModalOpened));
            }
        }

        //
        // GET: /Person/Delete/5
        public virtual ActionResult Delete(long id) {
            Person context = this.personRepository.GetById(id);

            if (context == null) {
                return RedirectToActionPermanent(MVC.Person.Index());
            }

            return View(context);
        }

        //
        // POST: /Person/Delete/5
        [HttpPost]
        public virtual ActionResult Delete(long id, FormCollection collection)
        {
            try
            {
                this.personRepository.RemoveById(id);
                this.personRepository.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                Person context = this.personRepository.GetById(id);

                if (context == null)
                {
                    return RedirectToActionPermanent(MVC.Person.Index());
                }

                return View(context);
            }
        }
    }
}
