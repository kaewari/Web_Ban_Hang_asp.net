using SNShop.Models;
using System;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        [OutputCache(Duration = 900, Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult List_Users(string error)
        {
            var p = db.Users.Select(s => s).ToList();
            ViewData["loi"] = error;
            return View(p);
        }
        public ActionResult Create_Users(User user)
        {           
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            ViewData["VT"] = new SelectList(db.Roles, "Id", "Name");
            return View(user);
        }
        [HttpPost]
        public ActionResult Create_Users(FormCollection formCollection, User user, UserRole userRole, Employee employee, Customer customer)
        {

            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            ViewData["VT"] = new SelectList(db.Roles, "Id", "Name");
            if (string.IsNullOrEmpty(formCollection["Truename"]) || string.IsNullOrWhiteSpace(formCollection["Truename"]))
            {
                ViewData["Loi"] = "Bạn vui lòng nhập họ tên";
            }
            else
            {
                if (string.IsNullOrEmpty(formCollection["Username"]) || string.IsNullOrWhiteSpace(formCollection["Username"]))
                {
                    ViewData["Loi"] = "Bạn vui lòng nhập username";
                }
                else
                {
                    if (string.IsNullOrEmpty(formCollection["Email"]) || string.IsNullOrWhiteSpace(formCollection["Email"]))
                    {
                        ViewData["Loi"] = "Bạn vui lòng nhập email";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(formCollection["PasswordHash"]) || string.IsNullOrWhiteSpace(formCollection["PasswordHash"]))
                        {
                            ViewData["Loi"] = "Bạn vui lòng nhập password";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(formCollection["PhoneNumber"]) || string.IsNullOrWhiteSpace(formCollection["PhoneNumber"]))
                            {
                                ViewData["loi"] = "Bạn chưa nhập số điện thoại";
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(formCollection["Address"]) || string.IsNullOrWhiteSpace(formCollection["Address"]))
                                {
                                    ViewData["loi"] = "Bạn chưa nhập địa chỉ";
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(formCollection["PR"]) || string.IsNullOrWhiteSpace(formCollection["PR"]))
                                    {
                                        ViewData["loi"] = "Bạn chưa chọn tỉnh/thành phố";
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(formCollection["DT"]) || string.IsNullOrWhiteSpace(formCollection["DT"]))
                                        {
                                            ViewData["loi"] = "Bạn chưa chọn quận/huyện";
                                        }
                                        else
                                        {
                                            try
                                            {
                                                using (TransactionScope tranScope = new TransactionScope())
                                                {
                                                    try
                                                    {
                                                        user.ID_Card = long.Parse(formCollection["ID_Card"]);
                                                        user.Email = formCollection["Email"];
                                                        user.Username = formCollection["Username"];
                                                        user.Truename = formCollection["Truename"];
                                                        user.PasswordHash = Common.Encode.GetMD5(formCollection["PasswordHash"]);
                                                        user.PhoneNumber = formCollection["PhoneNumber"];
                                                        user.DistrictID = int.Parse(formCollection["DT"]);
                                                        user.ProvinceID = int.Parse(formCollection["PR"]);
                                                        user.ModifiedDate = DateTime.Now;
                                                        db.Users.InsertOnSubmit(user);
                                                        db.SubmitChanges();
                                                        //var findLastId = db.Users.Any(user);
                                                        userRole.UserId = user.Id;
                                                        userRole.RoleId = int.Parse(formCollection["VT"].ToString());
                                                        db.UserRoles.InsertOnSubmit(userRole);
                                                        if (int.Parse(formCollection["VT"].ToString()) != 2)
                                                        {
                                                            employee.UserID = user.Id;
                                                            db.Employees.InsertOnSubmit(employee);
                                                        }
                                                        else
                                                        {
                                                            customer.UserID = user.Id;
                                                            db.Customers.InsertOnSubmit(customer);
                                                        }
                                                        db.SubmitChanges();
                                                        tranScope.Complete();
                                                        return RedirectToAction("List_Users");
                                                    }
                                                    catch
                                                    {
                                                        tranScope.Dispose();
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ViewData["loi"] = ex.Message;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return View(user);
        }
        public ActionResult Details_User(int id)
        {
            var p = db.Users.FirstOrDefault(s => s.Id == id);
            return View(p);
        }
        public ActionResult Edit_User_Image(int id)
        {
            try
            {
                var p = db.Users.FirstOrDefault(s => s.Id == id);
                p.Image = null;
                p.ModifiedDate = DateTime.Now;
                UpdateModel(p);
                db.SubmitChanges();
            }
            catch { }
            return RedirectToAction("Edit_Users_Image", new { id = id });
        }
        public ActionResult Delete_User(int id)
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    var p = db.Users.FirstOrDefault(s => s.Id == id);
                    db.Users.DeleteOnSubmit(p);
                    var roleId = db.UserRoles.FirstOrDefault(s => s.UserId == id);
                    if (roleId != null)
                    {
                        if (roleId.RoleId == 2)
                        {
                            var c = db.Customers.FirstOrDefault(s => s.UserID == id);
                            db.Customers.DeleteOnSubmit(c);
                        }
                        else
                        {
                            var e = db.Employees.FirstOrDefault(s => s.UserID == id);
                            db.Employees.DeleteOnSubmit(e);
                        }

                        var r = db.UserRoles.FirstOrDefault(s => s.UserId == id);
                        db.UserRoles.DeleteOnSubmit(r);
                    }
                    db.SubmitChanges();
                    tranScope.Complete();
                    return RedirectToAction("List_Users", new { error = "Xóa user thành công." });
                }
                catch { }
            }
            return RedirectToAction("List_Users", new { error = "Xóa user thất bại" });
        }
    }
}