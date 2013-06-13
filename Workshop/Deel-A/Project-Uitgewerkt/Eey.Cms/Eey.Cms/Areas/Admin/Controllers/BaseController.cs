using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eey.Cms.Areas.Admin.Controllers {
    using System.Web.Mvc;

    [Authorize(Users = "Admin")]
    public abstract class ControllerBase : Cms.Controllers.ControllerBase {
    }
}