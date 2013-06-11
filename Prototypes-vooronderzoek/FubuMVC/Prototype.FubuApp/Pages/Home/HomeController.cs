using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.FubuApp.Pages.Home
{
    public class HomeController
    {
        public HomeViewModel Index(HomeInputModel input) {
            return new HomeViewModel();    
        }
    }
}