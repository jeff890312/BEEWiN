using BEEWiN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers.User
{
    public class UserController : Controller
    {
        BEEWiNEntities db = new BEEWiNEntities();
        
        public ActionResult Index()
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            string email = (Session["Member"] as Member).Email;
            var member = db.Member.Where(m => m.Email == email).FirstOrDefault();
            // 總資產
            var total = (member.Wallet.Current_Accounts + member.Wallet.Staking_Accounts + member.Wallet.Reward_Accounts + member.Wallet.Invite_Accounts);
            TempData["Total"] = total.ToString("N2");
            TempData["TotalTWD"] = (total*28).ToString("N2");
            //
            return View("index", "_LayoutUser", member);
        }
        public ActionResult CoinOrders()
        {
            string email = (Session["Member"] as Member).Email;
            var coinorders = db.Coin_Order.Where(m => m.Email == email).OrderByDescending(m => m.Buy_Date).ToList();
            ViewBag.Coinorders = coinorders;
            return View("coinorders", "_LayoutUser");
        }
    }
}