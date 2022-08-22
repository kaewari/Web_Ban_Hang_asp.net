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

namespace SNShop.Controllers
{
    public class AccountController : Controller
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
            var userSession = new Common.UserLogin();
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
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string firstname = me.first_name;
                string lastname = me.last_name;
                user.Email = email;
                user.Username = firstname + " " + lastname;             
                userRole.UserId = user.Id;
                userRole.RoleId = userDao.GetRole("Customer").Id;
                customer.Id = user.Id;
                var resultCheck = userDao.CheckEmail(email);
                if (!resultCheck)
                {                   
                    db.Users.InsertOnSubmit(user);
                    db.SubmitChanges();

                }
                userSession.UserName = user.Username;
                userSession.UserID = user.Id;
                userSession.Email = user.Email;
                Session.Add(Constants.USER_SESSION, userSession);
                Session.Add("UserID", userSession.UserID);
                Session.Add("UserName", userSession.UserName);
                Session.Add("Email", userSession.Email);
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
                    ur.RoleId = userDao.GetRole("Customer").Id;
                    ur.UserId = c.UserID;
                    db.UserRoles.InsertOnSubmit(ur);
                    db.SubmitChanges();
                    return RedirectToAction("Index","Home");
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
            { var result = userDao.CheckUser(loginModel.Password, loginModel.Email);
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
                    var user = userDao.GetUserByName(loginModel.Email);
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
            Session.RemoveAll();
            return Redirect("/");
        }
        [HttpGet]
        public ActionResult ShowProfile()
        {
            UserDao userDao = new UserDao();
            var session = (SNShop.Common.UserLogin)Session[SNShop.Common.Constants.USER_SESSION];
            if (session == null)
                return RedirectToAction("Index", "Home");
            var result = userDao.GetUserByName(Session["Email"].ToString());
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowProfile(User u)
        {
            
            return View();
        }
    }
}