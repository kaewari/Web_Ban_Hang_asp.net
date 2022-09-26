using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SNShop.Models;
using SNShop.DAO;
using SNShop.Common;
using Facebook;
using System.Configuration;
using System.Web.Security;
using System.Data.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNShop.Areas.Admin.Models;
using System.IO;
using System.Data.Entity.Core.Metadata.Edm;

namespace SNShop.Controllers
{
    public class AccountController : MyBaseController
    {     
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult LoadProvince()
        {
            var xElements = from i in db.Provinces select i;
            var list = new List<Province>();
            Province province = null;
            foreach (var item in xElements)
            {
                province = new Province
                {
                    Id = item.Id,
                    Name = item.Name
                };
                list.Add(province);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public JsonResult LoadDistrict(int provinceID)
        {
            var xElement = db.Districts.Where(m => m.ProvinceID == provinceID).ToList();
            var list = new List<District>();
            District district = null;
            foreach (var item in xElement)
            {
                district = new District
                {
                    Id = item.Id,
                    Name = item.Name
                };
                list.Add(district);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            var userSession = new UserLogin();
            var user = new User();
            var customer = new Customer();
            var userRole = new UserRole();         
            UserDao userDao = new UserDao();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email,picture");
                string email = me.email;
                string firstname = me.first_name;
                string lastname = me.last_name;
                var picture = me.picture.data.url;
                user.Email = email;
                user.Username = firstname + " " + lastname;
                user.Image = picture;
                user.Facebook = true;                            
                var resultCheck = userDao.CheckEmail(email);
                if (!resultCheck)
                {                   
                    db.Users.InsertOnSubmit(user);
                    db.SubmitChanges();
                    customer.UserID = user.Id;
                    userRole.UserId = user.Id;
                    userRole.RoleId = userDao.GetRoleByRoleName("Users").Id;
                    db.UserRoles.InsertOnSubmit(userRole);
                    db.Customers.InsertOnSubmit(customer);
                    db.SubmitChanges();
                }
                var u = userDao.GetUserByEmail(user.Email);
                userSession.UserName = user.Username;
                userSession.UserID = u.Id;
                userSession.Email = user.Email;
                userSession.Picture = picture;
                Session.Add(Constants.USER_SESSION, userSession);
                Session.Add("UserID", userSession.UserID);
                Session.Add("UserName", userSession.UserName);
                Session.Add("Email", userSession.Email);
                Session.Add("Image", userSession.Picture);
                FormsAuthentication.SetAuthCookie(email, false);
            }
            return Redirect("/");
        }
        public ImageModel Single_Product_Image(ImageModel imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.File.FileName);
            string extension = Path.GetExtension(imageModel.File.FileName);
            if (extension != ".jpg" && extension != ".png")
                return null;
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.Path = "~/Images/Users/" + fileName;
            var ServerSavePath = Path.Combine(Server.MapPath("~/Images/Users/"), fileName);
            //Save file to server folder  
            imageModel.File.SaveAs(ServerSavePath);
            //assigning file uploaded status to ViewBag for showing message to user.  
            ViewBag.UploadStatus = "Thêm thành công.";
            return imageModel;
        }
        public ActionResult Register(RegisterModel registerModel)
        {
            ViewBag.Message = "Trang đăng ký tài khoản.";
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            if (registerModel.ProvinceID.Equals(null))
            {
                registerModel.ProvinceID = -1;
                registerModel.DistrictID = -1;
            }
            return View(registerModel);
        }       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel registerModel, UserDao userDao, FormCollection formCollection)
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            if (ModelState.IsValid)
            {
                UserRole ur = new UserRole();
                Customer c = new Customer();
                User u = new User();
                var check = userDao.CheckEmail(registerModel.Email);
                if (!check)
                {
                    u.Truename = registerModel.Truename;
                    u.Email = registerModel.Email;
                    u.PasswordHash = Encode.GetMD5(registerModel.Password);
                    u.Address = registerModel.Address;
                    u.PhoneNumber = registerModel.PhoneNumber;
                    u.Username = registerModel.Username;
                    u.Facebook = false;
                    if (string.IsNullOrEmpty(formCollection["PR"]) || string.IsNullOrWhiteSpace(formCollection["PR"]))
                    {
                        ViewData["cityNull"] = "Bạn chưa chọn tỉnh/thành phố";
                        return View(registerModel);
                    }
                    else
                        u.ProvinceID = int.Parse(formCollection["PR"]);
                    if (string.IsNullOrEmpty(formCollection["DT"]) || string.IsNullOrWhiteSpace(formCollection["DT"]))
                    {
                        ViewData["districtNull"] = "Bạn chưa chọn quận/huyện";
                        return View(registerModel);
                    }
                    else
                        u.DistrictID = int.Parse(formCollection["DT"]);
                    db.Users.InsertOnSubmit(u);
                    db.SubmitChanges();
                    c.UserID = u.Id;
                    db.Customers.InsertOnSubmit(c);
                    db.SubmitChanges();
                    ur.RoleId = userDao.GetRoleByRoleName("Users").Id;
                    ur.UserId = c.UserID;
                    db.UserRoles.InsertOnSubmit(ur);
                    db.SubmitChanges();
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(registerModel);
        }
        
        public ActionResult Login()
        {
            return View();
        }   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel, UserDao userDao)
        {
            if (ModelState.IsValid)
            { 
                var result = userDao.CheckUser(loginModel.Password, loginModel.Email);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                }
                else if (result == 1)
                {
                    var user = userDao.GetUserByEmail(loginModel.Email);
                    var userSession = new UserLogin
                    {
                        UserName = user.Username,
                        UserID = user.Id,
                        Email = user.Email,
                        Picture = user.Image,
                    };
                    Session.Add(Constants.USER_SESSION, userSession);
                    Session.Add("UserID", userSession.UserID);
                    Session.Add("UserName", userSession.UserName);
                    Session.Add("Email", userSession.Email);
                    Session.Add("Picture", userSession.Picture);
                    return Redirect("/");
                }
            }
            return View(loginModel);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session["Picture"] = null;
            Session.Remove(HttpContext.Session.SessionID);
            return Redirect("/");
        }
        public ActionResult ShowProfile(int id)
        {
            EditModel editModel = new EditModel();
            User result = db.Users.SingleOrDefault(s => s.Id == id);
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            editModel.ID = result.Id;
            editModel.Truename = result.Truename;
            editModel.Address = result.Address;
            editModel.PhoneNumber = result.PhoneNumber;
            editModel.Username = result.Username;
            editModel.Email = result.Email;
            editModel.Image = result.Image;
            editModel.Facebook = result.Facebook.Value;
            if (result.ProvinceID.Equals(null))
            {
                editModel.ProvinceID = 0;
                editModel.DistrictID = 0;
            }
            else
            {
                editModel.ProvinceID = int.Parse(result.ProvinceID.ToString());
                editModel.DistrictID = int.Parse(result.DistrictID.ToString());
            }

            return View(editModel);
        }
        [HttpPost]
        public ActionResult ShowProfile(EditModel editModel, FormCollection formCollection, int id)
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(s => s.Id == id);//lay user cần sửa trog db
                if (!editModel.Truename.Equals(null))
                    user.Truename = editModel.Truename;
                if (!editModel.Username.Equals(null))
                    user.Username = editModel.Username;
                if (editModel.Email.Equals(null))
                    user.Email = editModel.Email;
                if (string.IsNullOrEmpty(formCollection["PR"]) || string.IsNullOrWhiteSpace(formCollection["PR"]))
                {
                    ViewData["cityNull"] = "Bạn chưa chọn tỉnh/thành phố";
                    return View(editModel);
                }
                else
                    user.ProvinceID = int.Parse(formCollection["PR"]);
                if (string.IsNullOrEmpty(formCollection["DT"]) || string.IsNullOrWhiteSpace(formCollection["DT"]))
                {
                    ViewData["districtNull"] = "Bạn chưa chọn quận/huyện";
                    return View(editModel);
                }               
                else
                    user.DistrictID = int.Parse(formCollection["DT"]);
                UpdateModel(user);
                db.SubmitChanges();
                return RedirectToAction("ShowProfile", "Account", new { id = id });
            }
            return View(editModel);
        }
        public ActionResult Edit_Users_Image(ImageModel imageModel, User p, int id)
        {
            try
            {
                ImageModel image = Single_Product_Image(imageModel);
                p = db.Users.FirstOrDefault(s => s.Id == id);
                p.Image = image.Path.Remove(0, 1);
                p.ModifiedDate = DateTime.Now;
                UpdateModel(p);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                ViewData["loi"] = ex.Message;
            }
            return RedirectToAction("ShowProfile", "Account", new {id = id});
        }
        public ActionResult ChangePassword(int id)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            var p = db.Users.SingleOrDefault(s => s.Id == id).Id;
            ChangePassword changePassword = new ChangePassword();
            changePassword.ID = id;
            if (session == null)
                return RedirectToAction("Index", "Home");

            return View(changePassword);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword, int id)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(s => s.Id == id);//lay user cần sửa trog db
                if (!user.PasswordHash.Equals(Encode.GetMD5(changePassword.OldPassword)))
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                else
                {
                    user.PasswordHash = Encode.GetMD5(changePassword.NewPassword);
                    UpdateModel(user);
                    db.SubmitChanges();
                    return RedirectToAction("Logout");
                }
            }
            return View(changePassword);
        }
    }
}