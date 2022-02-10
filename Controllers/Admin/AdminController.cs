using BEEWiN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEWiN.Controllers.Admin
{
    public class AdminController : Controller
    {
        BEEWiNEntities db = new BEEWiNEntities();
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            return View("Index", "_LayoutAdmin");
        }
        public ActionResult CoinOrderList(string searchString, int? status = 0)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            var order = from m in db.Coin_Order
                        orderby m.Buy_Date descending
                        select m;
            if (status == 1)
            {
                order = (IOrderedQueryable<Coin_Order>)order.Where(m => m.Order_Status == "未付款");
            }
            else if (status == 2)
            {
                order = from m in db.Coin_Order
                        where m.Order_Status == "進行中"
                        orderby m.Buy_Date descending
                        select m;
            }
            else if (status == 3)
            {
                order = from m in db.Coin_Order
                        where m.Order_Status == "取消"
                        orderby m.Buy_Date descending
                        select m;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                order = (IOrderedQueryable<Coin_Order>)order.Where(x => x.Email.Contains(searchString));               
            }
            ViewBag.SelectOrderlist = order;
            return View("coinorderlist", "_LayoutAdmin");
        }
        public ActionResult StakingOrderList(string searchString, int? status = 0)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            var order = from m in db.Staking_Order
                        orderby m.Staking_Date descending
                        select m;
            if (status == 1)
            {
                order = (IOrderedQueryable<Staking_Order>)order.Where(m => m.Order_Status == "未付款");
            }
            else if (status == 2)
            {
                order = from m in db.Staking_Order
                        where m.Order_Status == "進行中"
                        orderby m.Staking_Date descending
                        select m;
            }
            else if (status == 3)
            {
                order = from m in db.Staking_Order
                        where m.Order_Status == "取消"
                        orderby m.Staking_Date descending
                        select m;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                order = (IOrderedQueryable<Staking_Order>)order.Where(x => x.Email.Contains(searchString));
            }
            ViewBag.SelectOrderlist = order;
            return View("stakingorderlist", "_LayoutAdmin");
        }
        public ActionResult MemberList(string searchString, int? status = 0)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            var members = from m in db.Member
                        orderby m.Register_Date descending
                        select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                members = (IOrderedQueryable<Member>)members.Where(x => x.Email.Contains(searchString));
            }
            ViewBag.SelectMemberlist = members;
            return View("memberlist", "_LayoutAdmin");
        }
        public ActionResult PlanList(string searchString, int? status = 0)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            var order = from m in db.Plan
                        orderby m.Open_Date descending
                        select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                order = (IOrderedQueryable<Plan>)order.Where(x => x.PId.Contains(searchString));
            }
            ViewBag.Selectlist = order;
            return View("planlist", "_LayoutAdmin");
        }
        public ActionResult PlanEdit(string PId)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            Plan plan = db.Plan.Find(PId);
            if (PId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View("planedit", "_LayoutAdmin",plan);
        }
        [HttpPost]
        public ActionResult PlanEdit([Bind(Include = "PId,PType,PName,Rate,Risk,Tips,Open_Date,Close_Date,Offering_Day,PPrice,Capital_Size,Capital_Object,Plan_Status")] Plan plan)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("permissiondenied", "error");
            }
            if (ModelState.IsValid)
            {
                db.Entry(plan).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["EditOK"] = "ok";
                return View("planedit", "_LayoutAdmin");
            }
            return View("planedit", "_LayoutAdmin");
        }
    }
}