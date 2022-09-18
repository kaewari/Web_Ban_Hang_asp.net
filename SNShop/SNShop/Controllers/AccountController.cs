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

        public ActionResult Register()
        {
            ViewBag.Message = "Trang đăng ký tài khoản.";

            return View();
        }       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRole ur, Customer c, User u, RegisterModel registerModel, UserDao userDao)
        {
            if (ModelState.IsValid)
            {
                var check = userDao.CheckEmail(registerModel.Email);
                if (!check)
                {
                    u.PasswordHash = Encode.GetMD5(registerModel.Password);
                    u.Address = registerModel.Address;
                    u.PhoneNumber = registerModel.PhoneNumber;
                    u.Username = registerModel.Username;
                    u.Facebook = false;
                    if (!string.IsNullOrEmpty(registerModel.ProvinceID))
                    {
                        u.ProvinceID = int.Parse(registerModel.ProvinceID);
                    }
                    if (!string.IsNullOrEmpty(registerModel.DistrictID))
                    {
                        u.DistrictID = int.Parse(registerModel.DistrictID);
                    }
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
                    };
                    Session.Add(Constants.USER_SESSION, userSession);
                    Session.Add("UserID", userSession.UserID);
                    Session.Add("UserName", userSession.UserName);
                    Session.Add("Email", userSession.Email);
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
            Session.Remove(HttpContext.Session.SessionID);
            return Redirect("/");
        }
        public ActionResult ShowProfile()
        {
            UserDao userDao = new UserDao();
            EditModel editModel = new EditModel();
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
                return RedirectToAction("Index", "Home");
            var result = userDao.GetUserByEmail(Session["Email"].ToString());
            editModel.ID = result.Id;
            editModel.Address = result.Address;
            editModel.PhoneNumber = result.PhoneNumber;
            editModel.Username = result.Username;
            editModel.Email = result.Email;
            editModel.ProvinceID = result.ProvinceID.ToString();
            editModel.DistrictID = result.DistrictID.ToString();
            editModel.Facebook = result.Facebook.Value;
            if (!string.IsNullOrEmpty(result.ProvinceID.ToString()))
                ViewBag.ProvinceID = result.ProvinceID;
            else
                ViewBag.ProvinceID = 0;
            if (!string.IsNullOrEmpty(result.ProvinceID.ToString()))
                ViewBag.DistrictID = result.DistrictID;
            else
                ViewBag.DistrictID = 0;
            return View(editModel);
        }
        [HttpPost]
        public ActionResult ShowProfile(EditModel editModel)
        {
            User user = db.Users.FirstOrDefault(s => s.Id == editModel.ID);//lay user cần sửa trog db
            if (ModelState.IsValid)
            {
                user.Address = editModel.Address;
                user.PhoneNumber = editModel.PhoneNumber;
                user.Username = editModel.Username;
                user.Email = editModel.Email;
                if (!string.IsNullOrEmpty(editModel.ProvinceID))
                {
                    user.ProvinceID = int.Parse(editModel.ProvinceID);
                }
                if (!string.IsNullOrEmpty(editModel.DistrictID))
                {
                    user.DistrictID = int.Parse(editModel.DistrictID);
                }
                UpdateModel(user);
                db.SubmitChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(editModel);
        }
        public ActionResult ChangePassword()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangeAdminPasswordModel changePassword)
        {
            User user = db.Users.FirstOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));//lay user cần sửa trog db
            if (!user.PasswordHash.Equals(Encode.GetMD5(changePassword.OldPassword)))
                ModelState.AddModelError("", "Mật khẩu không đúng.");
            else
            {
                user.PasswordHash = Encode.GetMD5(changePassword.ConfirmNewPassword);
                UpdateModel(user);
                db.SubmitChanges();
                Logout();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}