using BEEWiN.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers.User
{
    public class UserController : Controller
    {
        BEEWiNEntities db = new BEEWiNEntities();
        Encryption encryption = new Encryption();
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
            TempData["TotalTWD"] = (total * 28).ToString("N2");
            //
            return View("index", "_LayoutUser", member);
        }
        public ActionResult CoinOrders()
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            string email = (Session["Member"] as Member).Email;
            var coinorders = db.Coin_Order.Where(m => m.Email == email).OrderByDescending(m => m.Buy_Date).ToList();
            ViewBag.Coinorders = coinorders;
            return View("coinorders", "_LayoutUser");
        }
        public ActionResult StakingOrders()
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            string email = (Session["Member"] as Member).Email;
            var stakingorders = db.Staking_Order.Where(m => m.Email == email).OrderByDescending(m => m.Staking_Date).ToList();
            ViewBag.Stakingorders = stakingorders;
            return View("stakingorders", "_LayoutUser");
        }
        //=======================================================
        // 用戶修改資料
        public ActionResult Settings()
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            string email = (Session["Member"] as Member).Email;
            var member = db.Member.Where(m => m.Email == email).FirstOrDefault();
            return View("settings", "_LayoutUser", member);
        }
        [HttpPost]
        public ActionResult Settings(Member M)
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            else
            {
                db.Entry(M).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ModifyProfileOK"] = "ok";
                return RedirectToAction("settings", "user", "_LayoutUser");
            }
        }
        //=======================================================
        // 用戶修改密碼
        public ActionResult ResetPassword()
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            string email = (Session["Member"] as Member).Email;
            var member = db.Member.Where(m => m.Email == email).FirstOrDefault();
            return View("resetpassword", "_LayoutUser", member);
        }

        [HttpPost]
        public ActionResult ResetPassword(string newpassword)
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            string email = (Session["Member"] as Member).Email;
            var member = db.Member.Where(m => m.Email == email).FirstOrDefault();
            member.Password = encryption.SHA256(newpassword);
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
            TempData["ModifyProfileOK"] = "ok";
            return RedirectToAction("settings", "user", "_LayoutUser");
        }
    }
}