using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/Order
        public ActionResult List_Orders_Not_Approved()
        {
            _ = new List<Order>(100);
            List<Order> orders = db.Orders.Where(s=>s.EmployeeID == null).ToList();
            return View(orders);
        }
        public ActionResult List_Orders_Approved()
        {
            _ = new List<Order>(100);
            List<Order> orders = db.Orders.Where(s => s.EmployeeID != null).ToList();
            return View(orders);
        }
        public JsonResult Delete_Orders(int id)
        {
            try
            {
                Order order = db.Orders.SingleOrDefault(s => s.Id == id);
                Employee employee = db.Employees.SingleOrDefault(s => s.UserID == int.Parse(Session["UserID"].ToString()));
                db.Orders.DeleteOnSubmit(order);
                db.SubmitChanges();
                ViewData["loi"] = "Xóa đơn đặt hàng thành công";
                return Json(new
                {
                    status = 200,
                });
            }
            catch
            {
                ViewData["loi"] = "Bạn không thể xóa đơn đặt hàng này.";
            }
            return Json(new
            {
                status = 400,
            });

        }
        public ActionResult Edit_Orders(Order order, int id)
        {
            Employee employee = db.Employees.SingleOrDefault(s => s.UserID == int.Parse(Session["UserID"].ToString()));
            order = db.Orders.SingleOrDefault(s => s.Id == id);
            order.EmployeeID = employee.Id;
            order.ModifiedDate = DateTime.Now;
            UpdateModel(order);
            db.SubmitChanges();
            return RedirectToAction("List_Orders_Not_Approved", "Order");
        }
        public ActionResult Details_Orders(int id)
        {
            Order order = db.Orders.FirstOrDefault(s => s.Id == id);   
            return View(order);
        }
    }
}