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
using SNShop.Areas.Admin.Models;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;

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
        public void OrderProduct()
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
                            UnitPrice = item.UnitPrice,
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
                }
            }
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
            string error = null;
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
                else
                {
                    error = "Khong co san pham tuong ung de cap nhat!";
                }
            }          
            return Json(new
            {
                success = false,
                error = error
            });
        }
        [HttpPost]
        public JsonResult DeleteCart(int id)
        {
            List<CartModel> carts = GetListCarts();
            string error = null;
            if (carts != null)
            {
                var checkExits = carts.FirstOrDefault(x => x.ProductID == id);
                if (checkExits != null)
                {
                    carts.Remove(checkExits);
                    Session[Constants.CartSession] = carts;
                    return Json(new
                    {
                        status = 200,
                        cartsData = cart_stat(carts),
                    });
                }
                else
                {
                    error = "Khong co san pham tuong ung de xoa!";
                }
            }
            return Json(new
            {
                status = 400,
                error = error,
            });
        }
        public ActionResult OrderForm()
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> OrderForm(OrderForm orderForm, FormCollection formCollection)
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            if (ModelState.IsValid)
            {
                try
                {
                    var user = db.Users.SingleOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));
                    orderForm.Truename = formCollection["Truename"];
                    orderForm.ProvinceID = int.Parse(formCollection["PR"]);
                    orderForm.DistrictID = int.Parse(formCollection["DT"]);
                    orderForm.Email = formCollection["Email"];
                    orderForm.Note = formCollection["Note"];
                    orderForm.Address = formCollection["Address"];
                    orderForm.PhoneNumber = formCollection["PhoneNumber"];
                    var OrderDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                    OrderProduct();
                    var getProvinceName = db.Provinces.FirstOrDefault(s => s.Id == orderForm.ProvinceID).Name;
                    var getDistrictName = db.Districts.FirstOrDefault(s => s.Id == orderForm.DistrictID).Name;
                    var getCustomerID = db.Customers.FirstOrDefault(s => s.UserID == user.Id).Id;
                    var getOrderID = db.Orders.OrderByDescending(s => s.Id).Where(s => s.CustomerID == getCustomerID).FirstOrDefault().Id;
                    decimal? total = db.OrderDetails.Where(s => s.OrderId == getOrderID).Sum(s => s.Quantity * s.UnitPrice);
                    var body =
                        "<h1>Chào bạn,</h1>" +
                        "<h2>Cảm ơn bạn đã đặt hàng. Đây là thông tin đặt hàng của bạn.</h2>" +
                        "<h2>Họ tên: {0}</h2>" +
                        "<h2>Email: {1}</h2>" +
                        "<h2>Số điện thoại: {2}</h2>" +
                        "<h2>Địa chỉ: {3}</h2>" +
                        "<h2>Ghi chú: {4}</h2>" +
                        "<h2>Tỉnh thành: {5}</h2>" +
                        "<h2>Quận huyện: {6}</h2>" +
                        "<h2>Ngày đặt hàng: {7}</h2>" +
                        "<h2>Tổng tiền: {8}</h2>" +
                        "<h3>SNShop</h3>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(orderForm.Email));
                    message.From = new MailAddress("1951052171son@ou.edu.vn");
                    message.Subject = "Thông tin đơn đặt hàng";
                    message.Body = string.Format(body, orderForm.Truename, orderForm.Email,
                                    orderForm.PhoneNumber, orderForm.Address,
                                    orderForm.Note, getProvinceName,
                                    getDistrictName, OrderDate, total);
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {

                        var credential = new NetworkCredential
                        {
                            UserName = "1951052171son@ou.edu.vn",
                            Password = "caygame1080@"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        if (user != null)
                        {                         
                            await smtp.SendMailAsync(message);
                            return RedirectToAction("Success");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email chưa đăng ký!!!");
                            return View(orderForm);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewData["loi"] = ex.Message;
                    return RedirectToAction("Failure");
                }

            }
            return View(orderForm);
        }
    }
}