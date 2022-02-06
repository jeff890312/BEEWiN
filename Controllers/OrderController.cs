using BEEWiN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers
{
    public class OrderController : Controller
    {
        BEEWiNEntities db = new BEEWiNEntities();


        //=======================================================
        // 管理員操作

        // 審核訂單
        public ActionResult CheckCoinOrder(string orderguid)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            //=======================================================
            // 付款審核
            var coinorder = db.Coin_Order.Where(m => m.Order_Guid == orderguid).FirstOrDefault();
            coinorder.Order_Status = "通過";
            db.SaveChanges();
            //=======================================================
            // 代幣總值增加          
            coinorder.Coin.Capital_Size = db.Coin_Order.Where(m => m.Order_Status == "通過").Sum(m => m.Total);
            //=======================================================
            // 會員錢包增加             
            coinorder.Member.Wallet.Current_Accounts += coinorder.Total;
            db.SaveChanges();
            //=======================================================
            // LineNotify
            string newLine = "\r\n";
            Notify notify = new Notify();
            string sent_msg = newLine + "【付款審核成功】" + newLine + newLine +
                              "[管理員]:" + Session["Admin"] + newLine +
                              "[訂單序號]: " + coinorder.Order_Guid + newLine +
                              "[顧客帳號]: " + coinorder.Email + newLine +
                              "[商品類別]: " + coinorder.Symbol + newLine +
                              "[訂單總額]: " + coinorder.Total + "USDT" + newLine;
            notify.SendMessage(sent_msg);
            return RedirectToAction("coinorderlist", "admin");
        }

        // 刪除訂單
        public ActionResult DeleteCoinOrder(String orderguid)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            var coinorder = db.Coin_Order.Where(m => m.Order_Guid == orderguid).FirstOrDefault();
            db.Coin_Order.Remove(coinorder);
            db.SaveChanges();
            return RedirectToAction("coinorderlist", "admin");
        }

        //=======================================================
        // 用戶下單

        // Staking 購買代幣
        [HttpPost]
        public ActionResult AddOrder(string symbol, int amount)
        {
            string Email = (Session["Member"] as Member).Email;
            string guid = Guid.NewGuid().ToString();

            var member = db.Member.Where(m => m.Email == Email).FirstOrDefault();
            var coin = db.Coin.Where(m => m.Symbol == symbol).FirstOrDefault();
            Coin_Order coinOrder = new Coin_Order();
            coinOrder.Order_Guid = guid;
            coinOrder.Email = Email;
            coinOrder.Symbol = symbol;
            coinOrder.Buy_Date = DateTime.Now;
            coinOrder.Buy_Price = coin.Price;
            coinOrder.Buy_Amount = amount;
            coinOrder.Total = amount * coin.Price;
            coinOrder.Order_Status = "未付款";
            db.Coin_Order.Add(coinOrder);
            db.SaveChanges();

            //=======================================================
            // LineNotify
            string newLine = "\r\n";
            Notify notify = new Notify();
            string sent_msg = newLine + "【新訂單通知】" + newLine + newLine +
                              "[訂單序號]: " + guid + newLine +
                              "[顧客帳號]: " + Email + newLine +
                              "[商品類別]: " + coin.Symbol + newLine +
                              "[訂單總額]: " + coinOrder.Total + "USDT";
            notify.SendMessage(sent_msg);
            //=======================================================

            return RedirectToAction("index", "home");
        }

        // Staking 質押訂單
        [HttpPost]
        public ActionResult AddStakingOrder(string sid, int amount)
        {
            string Email = (Session["Member"] as Member).Email;
            string guid = Guid.NewGuid().ToString();

            var member = db.Member.Where(m => m.Email == Email).FirstOrDefault();
            var staking = db.Staking.Where(m => m.SId == sid).FirstOrDefault();
            //=======================================================
            // 帳戶餘額檢查 (防盜措施)
            if (member.Wallet.Current_Accounts < amount)
            {
                return Content("餘額不足，請重新檢查帳戶。");
            }
            //=======================================================
            // 添加訂單
            Staking_Order staking_Order = new Staking_Order();
            staking_Order.Order_Guid = guid;
            staking_Order.SId = staking.SId;
            staking_Order.Email = Email;
            staking_Order.Symbol = staking.Symbol;
            staking_Order.ARP = staking.ARP;
            staking_Order.Amount = amount;
            staking_Order.Return_Amount = amount * staking.ARP / 100;
            staking_Order.Staking_Date = DateTime.Today.AddDays(1);
            staking_Order.Expire_Date = staking_Order.Staking_Date.AddDays(365);
            staking_Order.Order_Status = "質押中";
            
            db.Staking_Order.Add(staking_Order);
            db.SaveChanges();
            //=======================================================
            // LineNotify
            string newLine = "\r\n";
            Notify notify = new Notify();
            string sent_msg = newLine + "【質押通知】" + newLine + newLine +
                              "[訂單序號]: " + guid + newLine +
                              "[顧客帳號]: " + Email + newLine +
                              "[質押名稱]: " + staking.SName + newLine +
                              "[質押總額]: " + amount + "BWC";
            notify.SendMessage(sent_msg);
            //=======================================================
            // 質押總額統計          
            staking.Capital_Size = db.Staking_Order.Where(m => m.Order_Status == "質押中" || m.Order_Status == "已結算").Sum(m => m.Amount);
            //=======================================================
            // 會員資產帳戶轉移             
            member.Wallet.Current_Accounts -= staking_Order.Amount;
            member.Wallet.Staking_Accounts += staking_Order.Amount;
            //=======================================================
            db.SaveChanges();
            return RedirectToAction("staking", "plan");
        }
    }
}