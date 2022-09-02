using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SNShop.Models;
using System.Transactions;
using TransactionScope = System.Transactions.TransactionScope;
using System.Globalization;

namespace SNShop.Controllers
{
    public class CartController : MyBaseController
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Cart
        public List<CartModel> GetListCarts()//lay ds gio hang
        {
            ViewBag.Count = 0;
            List<CartModel> carts = Session["Cart"] as List<CartModel>;
            if (carts == null)
            {
                carts = new List<CartModel>();
                Session["Cart"] = carts;
            }
            return carts;
        }
        public ActionResult AddCart(int id)
        {
            List<CartModel> carts = GetListCarts();//lay DSGH
            CartModel c = carts.Find(s => s.ProductID == id);
            if (c == null)
            {
                c = new CartModel(id);
                carts.Add(c);
            }
            else
            {
                c.Quantity++;
            }
            return RedirectToAction("ListCart");
        }
        public ActionResult Delete(int id)
        {
            List<CartModel> carts = GetListCarts();//lay DSGH
            CartModel c = carts.Find(s => s.ProductID == id);

            if (c != null)
            {
                carts.RemoveAll(s => s.ProductID == id);
                return RedirectToAction("ListCart");
            }
            if (carts.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ListCart");
        }
        private int Count()
        {
            int n = 0;
            List<CartModel> carts = Session["Cart"] as List<CartModel>;
            if (carts != null)
            {
                n = carts.Sum(s => s.Quantity);
            }
            return n;
        }

        private decimal? Total()
        {
            decimal? total = 0;
            List<CartModel> carts = Session["Cart"] as List<CartModel>;
            if (carts != null)
            {
                total = carts.Sum(s => s.Total);
            }
            return total;
        }
        public ActionResult OrderProduct(FormCollection collection)
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();
                    order.ModifiedDate = DateTime.Now;
                    order.CustomerID = int.Parse(Session["UserID"].ToString());
                    db.Orders.InsertOnSubmit(order);
                    db.SubmitChanges();
                    List<CartModel> carts = GetListCarts();
                    foreach (var item in carts)
                    {
                        OrderDetail d = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductID = item.ProductID,
                            Quantity = short.Parse(item.Quantity.ToString()),
                            UnitPrice = (int)item.UnitPrice,
                            ModifiedDate = DateTime.Now,
                        };

                        db.OrderDetails.InsertOnSubmit(d);
                    }
                    db.SubmitChanges();
                    tranScope.Complete();
                    Session["Cart"] = null;
                }
                catch
                {
                    tranScope.Dispose();
                    return Redirect("/");
                }
            }
            return Redirect("/");
        }
        public ActionResult ListCart()// hien thi gio hang
        {
            List<CartModel> carts = GetListCarts();//lay DSGH
            ViewBag.Countproduct = Count();
            ViewBag.Total = Total();
            return View(carts);
        }
    }
}