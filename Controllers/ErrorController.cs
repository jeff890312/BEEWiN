using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Permissiondenied()
        {
            return View("permissiondenied", "_LayoutAdmin");
        }
    }
}