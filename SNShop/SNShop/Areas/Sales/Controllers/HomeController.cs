using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using CsvHelper;
using SNShop.Areas.Sales.Common;
using SNShop.Areas.Sales.Models;
using SNShop.DAO;
using SNShop.Models;
namespace SNShop.Areas.Sales.Controllers
{
    public class HomeController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Sales/Home
        private decimal? total_quantity = 0;
        private decimal? total_amount = 0;
        public ActionResult Index()
        {
            if (Session["UserID"] != null || Session["Roles"].ToString() == "Members")
            {
                var pairs = db.Products.ToList();
                ViewBag.Data = pairs;
                var listProductsForm = GetListForm();
                var card_id = GetCardID();
                dynamic dynamicModel = new ExpandoObject();
                dynamicModel.ListProductForm = listProductsForm;
                dynamicModel.Card_ID = card_id;
                return View(dynamicModel);
            }
            else
            {
                return RedirectToAction("EmployeeLogin", "Account");
            }
        }
        public ID_Card GetCardID()
        {
            ID_Card iD_Card = Session[Constants.CUSTOMER_FORM_ID_CARD_SESSION] as ID_Card;

            if (iD_Card == null)
            {
                iD_Card = new ID_Card();
                Session[Constants.CUSTOMER_FORM_ID_CARD_SESSION] = iD_Card;
            }
            return iD_Card;
        }
        public ActionResult ListForm()
        {
            List<FormModel> forms = GetListForm();
            return View(forms);
        }
        public JsonResult CheckCustomer(decimal id)
        {
            List<FormModel> forms = new List<FormModel>();
            var cus = db.Users.SingleOrDefault(s => s.ID_Card == id);
            if (cus != null)
            {
                ID_Card card_id = Session[Constants.CUSTOMER_FORM_ID_CARD_SESSION] as ID_Card;
                card_id.Id = id;
                Session[Constants.CUSTOMER_FORM_ID_CARD_SESSION] = card_id;
                return Json(new { status = 200, cus_id = cus.Id, card_id = card_id });
            }
            return Json(new {status = 400, msg = "Không có ID này. Vui lòng nhờ Admin tạo User mới" });
        }
        public JsonResult Create_Receipt(int? id)
        {
            List<FormModel> forms = GetListForm();
            if (forms.Any())
            {
                using (TransactionScope tranScope = new TransactionScope())
                {
                    try
                    {
                        ID_Card iD_Card = GetCardID();
                        var CustomnerName = db.Customers.SingleOrDefault(s => s.Id == id).User.Truename;
                        var filePath = "D:\\HoaDon.csv";
                        Order order = new Order();
                        UserDao userDao = new UserDao();
                        order.ModifiedDate = DateTime.Now;
                        order.CustomerID = (int)id;
                        db.Orders.InsertOnSubmit(order);
                        db.SubmitChanges();
                        foreach (var item in forms)
                        {
                            OrderDetail d = new OrderDetail
                            {
                                OrderId = order.Id,
                                ProductID = item.productID,
                                Quantity = decimal.Parse(item.quantity.ToString()),
                                UnitPrice = item.unitPrice,
                                ModifiedDate = DateTime.Now,
                            };
                            db.OrderDetails.InsertOnSubmit(d);
                        }
                        using (StreamWriter writer = new StreamWriter(new FileStream(filePath,
                        FileMode.Create, FileAccess.Write), Encoding.UTF8))
                        {
                            writer.WriteLine("{0},{1}", null, "=====HÓA ĐƠN=====");
                            writer.WriteLine("{0},{1}", "Ngày lập:", DateTime.Now.ToString("dd-MM-yyyy"));
                            writer.WriteLine("{0},{1}", "Customer Name:", CustomnerName);
                            writer.WriteLine("{0},{1},{2}", "Product Name", "Quantity", "Unit Price");
                            decimal sum = 0;
                            for (int i = 0; i < forms.Count(); i++)
                            {
                                writer.WriteLine("{0},{1},{2}", forms[i].name, forms[i].quantity, string.Format("{0:#.#}đ", forms[i].unitPrice));
                                sum += (decimal)forms[i].total;
                            }
                            writer.WriteLine("{0},{1}", "Total:", string.Format("{0:#.#}đ", sum));
                            writer.WriteLine("{0},{1}", "Employee Name:", Session["TrueName"]);
                            writer.Close();
                        }
                        db.SubmitChanges();
                        tranScope.Complete();
                        iD_Card = null;
                        id = null; 
                        Session[Constants.CUSTOMER_FORM_ID_CARD_SESSION] = iD_Card;
                    }
                    catch
                    {
                        tranScope.Dispose();
                    }
                }

                return Json(new
                {
                    status = 200
                });
            }
            return Json(new
            {
                status = 400
            });
        }
        public JsonResult UpdateProduct()
        {
            List<FormModel> forms = GetListForm();
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    foreach (var item in forms)
                    {
                        var p = db.Products.SingleOrDefault(s => s.Id == item.productID);
                        p.UnitsInStock -= (int)item.quantity;
                        p.UnitsOnOrder += (int)item.quantity;
                        UpdateModel(p);
                        db.SubmitChanges();
                    }
                    db.SubmitChanges();
                    tranScope.Complete();
                    forms.Clear();
                    Session[Constants.FORM_SESSION] = forms;
                }
                catch
                {
                    tranScope.Dispose();
                }
                return Json(new
                {
                    status = 200
                });
            }
        }
        [HttpPost]
        public JsonResult Delete_Input(int id)
        {
            List<FormModel> forms = GetListForm();
            string error = null;
            if (forms.Any())
            {
                var checkExits = forms.FirstOrDefault(x => x.productID == id);
                if (checkExits != null)
                {
                    forms.Remove(checkExits);
                    Session[Constants.FORM_SESSION] = forms;
                    return Json(new
                    {
                        status = 200,
                        cartsData = cart_stat(forms),
                    });
                }
            }
            return Json(new
            {
                status = 400,
                error = error,
            });
        }
        public JsonResult Delete_All_Input()
        {
            List<FormModel> forms = GetListForm();
            if (forms.Any())
            {
                forms.Clear();
                return Json(new
                {
                    status = 200,
                    cartsData = cart_stat(forms),
                });
            }
            return Json(new
            {
                status = 400,
                cartsData = cart_stat(forms),
            });
        }
        public JsonResult CheckProduct(int id, int quantity)
        {
            List<FormModel> forms = GetListForm();//lay DSGH
            FormModel checkExits = null;
            int countProduct = db.Products.OrderByDescending(s => s.Id).Select(s => s.Id).FirstOrDefault();
            if (id <= countProduct)
            {
                checkExits = forms.FirstOrDefault(x => x.productID == id);
            }
            else
            {
                return Json(new
                {
                    success = false,
                });
            }
            if (checkExits == null)
            {
                checkExits = new FormModel(id);
                checkExits.name = db.Products.FirstOrDefault(s => s.Id == id).Name;
                checkExits.unitPrice = db.Products.FirstOrDefault(s => s.Id == id).Price;
                checkExits.quantity = quantity;
                forms.Add(checkExits);
            }
            else
            {
                return Json(new
                {
                    success = true,
                    valid = false,
                    msg = "Sản phẩm đã tồn tại!!!",
                });
            }
            return Json(new
            {
                cartsData = cart_stat(forms),
                cartItem = cart_item(checkExits),
                name = checkExits.name,
                valid = true,
                success = true,
            });
        }
        public List<FormModel> GetListForm()
        {
            List<FormModel> form = Session[Constants.FORM_SESSION] as List<FormModel>;
            if (form == null)
            {
                form = new List<FormModel>();
                Session[Constants.FORM_SESSION] = form;
            }
            return form;
        }
        public object cart_stat(List<FormModel> forms)
        {
            if (forms.Any())
            {
                foreach (var c in forms)
                {
                    total_quantity += c.quantity;
                    total_amount += c.total;
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
        public object cart_item(FormModel form)
        {
            decimal? quantity = 0;
            decimal? amount = 0;
            if (form != null)
            {
                quantity = form.quantity;
                amount = form.total;
            }
            return new
            {
                quantity = quantity,
                amount = amount,
            };
        }
    }
}