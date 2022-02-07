using BEEWiN.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BEEWiN.Controllers
{
    public class AccountController : Controller
    {
        BEEWiNEntities db = new BEEWiNEntities();
        Mail mail = new Mail();
        Encryption encryption = new Encryption();
        Notify notify = new Notify();

        //=======================================================
        // 登入
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            //SHA加密
            string shapwd = encryption.SHA256(password);
            //=======================================================
            // 管理員登入
            var admin = db.Admin.Where(m => m.Email == email && m.Password == shapwd).FirstOrDefault();
            if (admin != null)
            {
                Session["Admin"] = admin;
                return RedirectToAction("index", "admin");
            }
            //=======================================================
            // 會員登入
            var member = db.Member.Where(m => m.Email == email && m.Password == shapwd).FirstOrDefault();
            if (member != null)
            {
                Session["Member"] = member;
                Session["MemberName"] = member.Name;
            }
            else
            {
                TempData["LoginFail"] = "on";
                TempData["LoginEmail"] = email;
                return RedirectToAction("index", "home");
            }
            return RedirectToAction("index", "home");
        }
        //=======================================================
        // 註冊
        [HttpPost]
        public ActionResult Register(Member member)
        {
            //=======================================================
            // 會員註冊
            var check_member = db.Member.Where(m => m.Email == member.Email).FirstOrDefault();
            if (check_member == null)
            {
                //=======================================================
                // 信箱驗證
                Session["VerifyCode"] = mail.VerifyCode(member.Email);
                //=======================================================
                // 暫存會員資料
                TempData["Member"] = member;
                return RedirectToAction("verify", "account");
            }
            else
            {
                //=======================================================
                // 註冊失敗
                TempData["RegisterFail"] = "on";
                return RedirectToAction("Index", "Home");
            }
        }
        //=======================================================
        // 會員信箱認證
        public ActionResult Verify()
        {
            return View("Verify", "_LayoutFront");
        }
        [HttpPost]
        public ActionResult Verify(string verifycode)
        {
            if (verifycode == Session["VerifyCode"].ToString())
            {
                var member = (TempData["Member"] as Member);
                //=======================================================
                // 紀錄會員
                member.Password = encryption.SHA256(member.Password); // SHA256加密
                member.Register_Date = DateTime.Now;
                member.Member_Type = "未驗證會員";
                db.Member.Add(member);
                //=======================================================
                // 新增錢包
                Wallet wallet = new Wallet();
                wallet.Email = member.Email;
                db.Wallet.Add(wallet);
                db.SaveChanges();
                //=======================================================
                // LineNotify
                string br = "\r\n";
                string sent_msg = br + "【新會員加入】" + br +
                                  "[會員名稱]: " + member.Name + br +
                                  "[會員帳號]: " + member.Email + br;
                notify.SendMessage(sent_msg);
                //=======================================================           
                Session.Remove("VerifyCode");
                return RedirectToAction("welcome", "account");
            }
            TempData["Verify"] = "fall";
            return View("verify", "_LayoutFront");
        }
        public ActionResult Welcome()
        {
            return View("welcome", "_LayoutAdmin");
        }
        //=======================================================
        // 登出
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        //=======================================================
        // 忘記密碼
        public ActionResult ForgotPassword()
        {
            return View("forgotpassword", "_LayoutAdmin");
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var Member = db.Member.Where(m => m.Email == email).FirstOrDefault();
            if (Member != null)
            {
                Mail mail = new Mail();
                Encryption encryption = new Encryption();
                //=======================================================
                // 自動產生亂數密碼
                var autopassword = Membership.GeneratePassword(8, 1); //至少3符號字元
                autopassword = Regex.Replace(autopassword, @"[^a-zA-Z0-9]", m => "7");
                //=======================================================
                // 發送郵件               
                mail.MailMessage(email, "BEEWiN 臨時密碼", autopassword);
                //=======================================================
                // SHA256加密                     
                Member.Password = encryption.SHA256(autopassword);
                //=======================================================     
                db.Entry(Member).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View("forgotpassword", "_LayoutAdmin");
        }
    }
}