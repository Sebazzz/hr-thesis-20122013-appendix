using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Persons
{
    using AutoMapper;

    using FubuMVC.Core.Continuations;

    using Prototype.Common;

    public class EditController
    {
         private readonly PersonRepository personRepository;

         public EditController(PersonRepository personRepository)
         {
            this.personRepository = personRepository;
        }

        public EditViewModel Get_Edit_Id(EditInputModel editInputModel) {
            Person current = personRepository.GetById(editInputModel.Id);

            if (current == null) {
                return new EditViewModel(FubuContinuation.RedirectTo<IndexController>(x => x.Index(null)));
            }

            return new EditViewModel(current, editInputModel.IsModalOpened);
        }

        public EditViewModel Post_Edit(EditPersonInput input)
        {
            Person current = personRepository.GetById(input.UniqueId);

            if (current == null)
            {
                return new EditViewModel(FubuContinuation.RedirectTo<IndexController>(x => x.Index(null)));
            }

            Mapper.Map(input, current);

            return new EditViewModel(FubuContinuation.RedirectTo<IndexController>(x=>x.Index(null)));
        }

        
    }

    public class EditPersonInput : PersonModifyBase {
        
    }

    public class CreatePersonInput : PersonModifyBase
    {

    }
    
}