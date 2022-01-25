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

        public ActionResult PayOrder(string symbol, int amount)
        {
            var coin = db.Coin.Where(m => m.Symbol == symbol).FirstOrDefault();
            TempData["Symbol"] = symbol;
            TempData["Amount"] = amount;
            TempData["Price"] = coin.Price;
            TempData["Total"] = amount * coin.Price;
            return View("PayOrder", "_LayoutFront");
        }

        //Admin
        // When admin check the coin order form customer
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
    }
}