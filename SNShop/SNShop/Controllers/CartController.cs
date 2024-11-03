using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SNShop.Models;
using TransactionScope = System.Transactions.TransactionScope;
using SNShop.Common;
using SNShop.DAO;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Dynamic;
using System.Text;

namespace SNShop.Controllers
{
    public class CartController : MyBaseController
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        private decimal? total_quantity = 0;
        private decimal? total_amount = 0;
        public ActionResult ListCart(string msg = null)
        {
            List<CartModel> carts = GetListCarts();
            carts.RemoveAll(s => s.Quantity <= 0 || s.UnitsInStock <= 0);
            Session[Constants.CART_SESSION] = carts;
            ViewBag.Countproduct = Count();
            ViewBag.Total = Total();
            if (msg != null)
                ViewData["msg"] = msg;          
            return View(carts);
        }
        public List<CartModel> GetListCarts()//lay ds gio hang
        {
            List<CartModel> carts = Session[Constants.CART_SESSION] as List<CartModel>;
            if (carts == null)
            {
                carts = new List<CartModel>();
                Session[Constants.CART_SESSION] = carts;
            }
            return carts;
        }
        public ActionResult TrackCart()
        {
            if (Session["UserID"] != null)
            {
                List<TrackCartModel> trackCartModels = new List<TrackCartModel>(10);
                dynamic dynamicModel = new ExpandoObject();
                int Id = int.Parse(Session["UserID"].ToString());
                var listOrder = db.Orders.Where(s => s.CustomerID == Id).OrderByDescending(s => s.ModifiedDate);
                dynamicModel.ListOrderDetail = db.OrderDetails.Where(s => listOrder.Select(a => a.Id).Contains(s.OrderId)).OrderByDescending(s => s.ModifiedDate);
                dynamicModel.Date = db.Orders.Where(s => s.CustomerID == int.Parse(Session["UserID"].ToString()))
                    .OrderByDescending(s => s.ModifiedDate)
                    .Select(s => s.ModifiedDate)
                    .ToArray();
                return View(dynamicModel);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult AddCart(int id, string name, decimal? price, decimal? stock)
        {
            List<CartModel> carts = GetListCarts();//lay DSGH
            var checkExits = carts.FirstOrDefault(x => x.ProductID == id);
            var getStock = db.Products.FirstOrDefault(s => s.Id == id).UnitsInStock;
            if (getStock != stock)
                stock = getStock;
            if (checkExits == null)
            {
                checkExits = new CartModel(id);
                checkExits.UnitsInStock = stock;
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
        public decimal? Count()
        {
            List<CartModel> carts = Session[Constants.CART_SESSION] as List<CartModel>;
            if (carts.Any())
            {
                total_quantity = carts.Sum(s => s.Quantity);
            }
            return total_quantity;
        }
        public decimal? Total()
        {
            List<CartModel> carts = Session[Constants.CART_SESSION] as List<CartModel>;
            if (carts.Any())
            {
                total_amount = carts.Sum(s => s.Total);
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
                            Quantity = decimal.Parse(item.Quantity.ToString()),
                            UnitPrice = item.UnitPrice,
                            ModifiedDate = DateTime.Now,
                        };
                        db.OrderDetails.InsertOnSubmit(d);
                    }
                    db.SubmitChanges();
                    tranScope.Complete();
                }
                catch
                {
                    tranScope.Dispose();
                }
            }
            UpdateProduct();
        }
        public void UpdateProduct()
        {
            List<CartModel> carts = GetListCarts();
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    foreach (var item in carts)
                    {
                        var p = db.Products.SingleOrDefault(s => s.Id == item.ProductID);
                        p.UnitsInStock -= (int)item.Quantity;
                        p.UnitsOnOrder += (int)item.Quantity;
                        db.SubmitChanges();
                    }
                    tranScope.Complete();
                    carts.Clear();
                    Session[Constants.CART_SESSION] = carts;
                }
                catch
                {
                    tranScope.Dispose();
                }
            }
        }
        public object cart_stat(List<CartModel> carts)
        {
            if (carts.Any())
            {
                foreach (var c in carts)
                {
                    total_quantity += c.Quantity;
                    total_amount += c.Total;
                }
            }
            ViewBag.Countproduct = total_quantity;
            ViewBag.Total = total_amount;
            return new
            {
                total_quantity = total_quantity,
                total_amount = total_amount,
            };
        }
        public object cart_item(CartModel carts)
        {
            decimal? quantity = 0;
            decimal? amount = 0;
            if (carts != null)
            {
                quantity = carts.Quantity;
                amount = carts.Total;
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
        public JsonResult UpdateCart(int id, decimal? quantity)
        {
            List<CartModel> carts = GetListCarts();
            string error = null;
            if (carts.Any())
            {
                var checkExits = carts.FirstOrDefault(x => x.ProductID == id);
                if (checkExits != null)
                {
                    if (quantity > checkExits.UnitsInStock)
                    {
                        error = "Vượt quá số lượng còn trong kho.";
                        return Json(new
                        {
                            success = false,
                            error = error
                        });
                    }
                    else
                    {
                        checkExits.Quantity = quantity;
                        Session[Constants.CART_SESSION] = carts;
                        return Json(new
                        {
                            cartsData = cart_stat(carts),
                            cartItem = cart_item(checkExits),
                            success = true,
                        });
                    }
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
            if (carts.Any())
            {
                var checkExits = carts.FirstOrDefault(x => x.ProductID == id);
                if (checkExits != null)
                {
                    carts.Remove(checkExits);
                    Session[Constants.CART_SESSION] = carts;
                    return Json(new
                    {
                        status = 200,
                        cartsData = cart_stat(carts),
                    });
                }
            }
            return Json(new
            {
                status = 400,
                error = "Khong co san pham tuong ung de xoa!",
            });
        }
        public JsonResult DeleteAllCart()
        {
            List<CartModel> carts = GetListCarts();
            if (carts.Any())
            {
                carts.Clear();
                return Json(new
                {
                    status = 200,
                    cartsData = cart_stat(carts),
                });
            }
            return Json(new
            {
                status = 400,
                cartsData = cart_stat(carts),
            });
        }
        public ActionResult OrderForm()
        {
            object check = new object();
            lock(check)
            {
                var listCart = GetListCarts();
                if (listCart.Any())
                {
                    foreach (var item in listCart)
                    {
                        if (item.Quantity > db.Products.FirstOrDefault(s => s.Id == item.ProductID).UnitsInStock)
                        {
                            return RedirectToAction("ListCart", "Cart", new { msg = "Vượt quá số lượng hiện tại!!!" });
                        }
                        if (item.Quantity <= 0)
                        {
                            return RedirectToAction("ListCart", "Cart", new { msg = "Số lượng phải từ 1 trở lên!!!" });
                        }
                    }
                    if (Session["UserID"] != null && Session["Roles"].ToString() == "Users")
                    {
                        OrderFormModel orderFormModel = new OrderFormModel();
                        ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
                        ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
                        return View(orderFormModel);
                    }
                    return RedirectToAction("Login", "Account");
                }
            }
            return Redirect("/");
        }
        [HttpPost]
        public async Task<ActionResult> OrderForm(OrderFormModel orderForm, FormCollection formCollection)
        {
            
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            if (ModelState.IsValid)
            {
                try
                {
                    var user = db.Users.SingleOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));
                    orderForm.ID = formCollection["ID"];
                    orderForm.Truename = formCollection["Truename"];
                    orderForm.ProvinceID = int.Parse(formCollection["PR"]);
                    orderForm.DistrictID = int.Parse(formCollection["DT"]);
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
                    var getProducts = db.OrderDetails.Where(s => s.OrderId == getOrderID);
                    var body =
                        "<h1>Chào bạn,</h1>" +
                        "<h2>Cảm ơn bạn đã đặt hàng. Đây là thông tin đặt hàng của bạn.</h2>" +
                        "<h2>Họ tên: {0}</h2>" +
                        "<h2>CMND: {1}</h2>" +
                        "<h2>Số điện thoại: {2}</h2>" +
                        "<h2>Địa chỉ: {3}</h2>" +
                        "<h2>Ghi chú: {4}</h2>" +
                        "<h2>Tỉnh thành: {5}</h2>" +
                        "<h2>Quận huyện: {6}</h2>" +
                        "<h2>Ngày đặt hàng: {7}</h2>" +
                        "{8}" +
                        "<h2>Tổng tiền: {9}đ</h2>" +
                        "<h3>SNShop</h3>";
                    StringBuilder stringBuilder = new StringBuilder(10);
                    foreach (var item in getProducts)
                    {
                        string productsName = "<h2>Tên sản phẩm: " + item.Product.Name + ";" +
                                                "<span> Số lượng: " + item.Quantity + "</span>;" +
                                                "<span> Giá: " + string.Format("{0:#,0}đ", item.UnitPrice * item.Quantity) + "</span>" +
                                                "</h2>";
                        stringBuilder.Append(productsName);
                    }
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(orderForm.Email));
                    message.From = new MailAddress("1951052171son@ou.edu.vn");
                    message.Subject = "Thông tin đơn đặt hàng";
                    message.Body = string.Format(body, orderForm.Truename, orderForm.ID,
                                    orderForm.PhoneNumber, orderForm.Address,
                                    orderForm.Note, getProvinceName,
                                    getDistrictName, OrderDate, stringBuilder.ToString(), String.Format("{0:#,0}", total));
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "1951052171son@ou.edu.vn",
                            Password = "Caygame10800@"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        
                        if (user != null)
                        {
                            await smtp.SendMailAsync(message);
                            Session[Constants.CART_SESSION] = null;
                            return RedirectToAction("Success");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email chưa đăng ký!!!");
                            return View(orderForm);
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Failure");
                }
            }
            return View(orderForm);
        }
    }
}