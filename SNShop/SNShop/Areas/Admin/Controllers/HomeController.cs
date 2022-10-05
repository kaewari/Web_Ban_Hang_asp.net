﻿using SNShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;
namespace SNShop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["UserID"] != null && Session["Roles"].ToString() == "Admin")
            {
                decimal? sumMonthly = 0;
                decimal? sumYearly = 0;
                try
                {
                    sumMonthly = db.OrderDetails
                        .Where(s => s.ModifiedDate.Year == DateTime.Now.Year
                                && s.ModifiedDate.Month == DateTime.Now.Month)
                        .Sum(s => s.Quantity * s.UnitPrice);
                    sumYearly = db.OrderDetails
                        .Where(s => s.ModifiedDate.Year == DateTime.Now.Year)
                        .Sum(s => s.Quantity * s.UnitPrice);
                }
                catch { }
                finally
                {
                    ViewBag.TotalRevenueMonthly = sumMonthly;
                    ViewBag.TotalRevenueYearly = sumYearly;
                }

                var revenueMonthly = db.OrderDetails
                    .Where(s => s.ModifiedDate.Year == DateTime.Now.Year)
                    .GroupBy(a => a.ModifiedDate.Month)
                    .Select(s => new
                    {
                        total = s.Sum(b => b.UnitPrice * b.Quantity),
                        monthOfYear = s.Select(a => a.ModifiedDate.Month).Distinct()
                    }).ToArray();
                var userCount = db.UserRoles
                    .GroupBy(a => a.RoleId)
                    .Select(s => new
                    {
                        key = s.Select(b=>b.Role.Name).Distinct(),
                        count = s.Count()
                    }).ToArray();
                var roleName = db.Roles.ToArray();
                ViewData["FieldsList"] = revenueMonthly;
                ViewData["UserList"] = userCount;
                ViewBag.Roles = roleName;
                return View();
            }
            return RedirectToAction("AdminLogin", "Account");
        }
        public ActionResult Components_Buttons()
        {
            return View();
        }
        public ActionResult Components_Cards()
        {
            return View();
        }
        public ActionResult Utilities_Colors()
        {
            return View();
        }
        public ActionResult Utilities_Borders()
        {
            return View();
        }
        public ActionResult Utilities_Animations()
        {
            return View();
        }
        public ActionResult Utilities_Others()
        {
            return View();
        }
        public ActionResult Pages_Login()
        {
            return View();
        }
        public ActionResult Pages_ForgotPassword()
        {
            return View();
        }
        public ActionResult Pages_Blank()
        {
            return View();
        }
        public ActionResult Charts()
        {
            return View();
        }
    }
}