using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers.Front
{
    public class AboutUsController : Controller
    {
        //團隊成員
        public ActionResult Team()
        {
            return View("Team", "_LayoutFront");
        }
    }
}