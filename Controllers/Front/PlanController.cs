using BEEWiN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers.Front
{
    public class PlanController : Controller
    {
        BEEWiNEntities db = new BEEWiNEntities();
        public ActionResult Staking()
        {
            var bwc = db.Staking.Where(m => m.Symbol == "BWC").OrderBy(m => m.ARP);
            ViewBag.BWC = bwc;

            if(Session["Member"]!= null)
            {
                string email = (Session["Member"] as Member).Email;
                var member = db.Member.Where(m => m.Email == email).FirstOrDefault();
                TempData["Balance"] = member.Wallet.Current_Accounts;
                TempData["Current"] = member.Wallet.Current_Accounts.ToString("N0");
                TempData["CurrentTWD"] = (member.Wallet.Current_Accounts * 28).ToString("N0");
            }
            else
            {
                TempData["Current"] = 0;
                TempData["CurrentTWD"] = 0;
            }   

            return View("staking", "_LayoutFront");
        }
    }
}