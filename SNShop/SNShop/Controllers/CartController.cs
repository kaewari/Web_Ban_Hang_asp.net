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
using SNShop.Common;
using System.Web.Script.Serialization;
using SNShop.DAO;

namespace SNShop.Controllers
{
    public class CartController : MyBaseController
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        private long total_quantity = 0;
        private long total_amount = 0;
        // GET: Cart
        public List<CartModel> GetListCarts()//lay ds gio hang
        {

            List<CartModel> carts = Session[Constants.CartSession] as List<CartModel>;
            if (carts == null)
            {
                carts = new List<CartModel>();
                Session[Constants.CartSession] = carts;
            }
            return carts;
        }
        [HttpPost]
        public JsonResult AddCart(int id, string name, long price)
        {
            List<CartModel> carts = GetListCarts();//lay DSGH
            var checkExits = carts.FirstOrDefault(x => x.ProductID == id);
            if (checkExits == null)
            {
                checkExits = new CartModel(id);
                checkExits.ProductName = name;
                checkExits.UnitPrice = price;
                carts.Add(checkExits);
            }
            else
            {
                checkExits.Quantity++;
            }
            return Json(new
            {
                cartsData = cart_stat(carts),
                cartItem = cart_item(checkExits),
                success = true,
            });
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
        public long Count()
        {
            List<CartModel> carts = Session[Constants.CartSession] as List<CartModel>;
            if (carts != null)
            {
                total_quantity = carts.Sum(s => s.Quantity);
            }
            return total_quantity;
        }
        public decimal? Total()
        {
            List<CartModel> carts = Session[Constants.CartSession] as List<CartModel>;
            if (carts != null)
            {
                total_amount = (long)carts.Sum(s => s.Total);
            }
            return total_amount;
        }
        public ActionResult OrderProduct()
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();
                    UserDao userDao = new UserDao();
                    order.ModifiedDate = DateTime.Now;
                    order.CustomerID = userDao.GetUserById(int.Parse(Session["UserID"].ToString()));
                    db.Orders.InsertOnSubmit(order);
                    db.SubmitChanges();
                    List<CartModel> carts = GetListCarts();
                    foreach (var item in carts)
                    {
                        OrderDetail d = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductID = item.ProductID,
                            Quantity = long.Parse(item.Quantity.ToString()),
                            UnitPrice = (long)item.UnitPrice,
                            ModifiedDate = DateTime.Now,
                        };

                        db.OrderDetails.InsertOnSubmit(d);
                    }
                    db.SubmitChanges();
                    tranScope.Complete();
                    Session[Constants.CartSession] = null;
                }
                catch
                {
                    tranScope.Dispose();
                    return RedirectToAction("Failure", "Cart");
                }
            }
            return RedirectToAction("Success", "Cart");
        }
        public ActionResult ListCart()// hien thi gio hang
        {
            List<CartModel> carts = GetListCarts();//lay DSGH
            ViewBag.Countproduct = Count();
            ViewBag.Total = Total();
            return View(carts);
        }
        public object cart_stat(List<CartModel> carts)
        {
            if (carts != null)
            {
                foreach(var c in carts)
                {
                    total_quantity += (long)c.Quantity;
                    total_amount += (long)c.Total;
                }
            }
            ViewBag.Countproduct = total_quantity;
            ViewBag.Total = total_amount;
            return new {
                total_quantity = total_quantity,
                total_amount = total_amount,
            };
        }
        public object cart_item(CartModel carts)
        {
            long quantity = 0;
            long amount = 0;
            if (carts != null)
            {
                quantity = (long)carts.Quantity;
                amount = (long)carts.Total;
            }
            return new
            {
                quantity = quantity,
                amount = amount,
            };
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Failure()
        {
            return View();
        }
        [HttpPost]
        public JsonResult UpdateCart(int id, long quantity)
        {
            List<CartModel> carts = GetListCarts();

            if (carts != null)
            {
                var checkExits = carts.FirstOrDefault(x => x.ProductID == id);
                if (checkExits != null)
                {
                    checkExits.Quantity = quantity;
                    return Json(new
                    {
                        cartsData = cart_stat(carts),
                        cartItem = cart_item(checkExits),
                        success = true,
                    });
                }
            }          
            return Json(new
            {
                success = false,
            });
        }
    }
}