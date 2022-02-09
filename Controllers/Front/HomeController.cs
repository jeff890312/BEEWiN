using BEEWiN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers
{
    public class HomeController : Controller
    {
        BEEWiNEntities db = new BEEWiNEntities();
        public ActionResult Index()
        {
            var bwc = db.Coin.Where(m => m.Symbol == "BWC").FirstOrDefault();
            TempData["Capital_Size"] = bwc.Capital_Size.ToString("N0");
            return View("Index","_LayoutFront");
        }
    }
}