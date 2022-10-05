using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SNShop.Areas.Sales.Controllers
{
    public class OrderController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Sales/Orders
        public ActionResult List_Orders_Approved()
        {
            _ = new List<Order>(100);
            List<Order> orders = db.Orders.Where(s => s.EmployeeID != null && s.ModifiedDate == DateTime.Now).OrderByDescending(s=>s.ModifiedDate).ToList();
            return View(orders);
        }
    }
}