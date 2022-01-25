using BEEWiN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers.Front
{
    public class BWCController : Controller
    {
        BEEWiNEntities db = new BEEWiNEntities();
        //代幣申購
        public ActionResult Beewincoin()
        {
            var bwc = db.Coin.Where(m => m.Symbol == "BWC").FirstOrDefault();
            return View("beewincoin", "_LayoutFront",bwc);
        }
        //社群挖礦
        public ActionResult Beewinmining()
        {
            return View("beewinmining", "_LayoutFront");
        }
    }
}