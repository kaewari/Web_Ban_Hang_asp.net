﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SNShop.Models;
using SNShop.DAO;
using SNShop.Common;
using Facebook;
using System.Configuration;
using System.Web.Security;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using SNShop.Areas.Admin.Models;
using EmailModel = SNShop.Models.EmailModel;
using ResetPasswordModel = SNShop.Models.ResetPasswordModel;
using ResetPasswordCodeModel = SNShop.Models.ResetPasswordCodeModel;

namespace SNShop.Controllers
{
    public class AccountController : MyBaseController
    {     
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();

        public void ReloadSession(User user)
        {
            UserDao userDao = new UserDao();
            var userSession = new UserLogin()
            {
                UserName = user.Username,
                UserID = user.Id,
                Email = user.Email,
                Image = user.Image,
                Roles = userDao.GetRoleById(user.Id).Name,
            };
            Session.Add(Constants.USER_SESSION, userSession);
            Session.Add("UserID", userSession.UserID);
            Session.Add("UserName", userSession.UserName);
            Session.Add("Email", userSession.Email);
            Session.Add("Roles", userSession.Roles);
            Session.Add("Image", userSession.Image);
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
            var user = new User();
            var customer = new Customer();
            var userRole = new UserRole();         
            UserDao userDao = new UserDao();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code,
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
                var resultCheck = userDao.CheckEmail(email);
                user.Email = email;
                user.Username = firstname + " " + lastname;
                user.Truename = user.Username;
                user.Image = picture;
                user.Facebook = true;
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
                user.Id = userDao.GetUserByEmail(email).Id;
                ReloadSession(user);
                FormsAuthentication.SetAuthCookie(email, false);
            }
            return RedirectToAction("ListCart", "Cart");
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
        public ActionResult Register(RegisterModel registerModel, UserDao userDao, FormCollection formCollection, string url)
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
                    u.ID_Card = registerModel.ID;
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
                    if(url != null)
                        return Redirect(url);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewData["loi"] = "Email đã tồn tại!!!";
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
        public ActionResult Login(UserLoginModel loginModel, UserDao userDao, string url)
        {
            if (ModelState.IsValid)
            { 
                var result = userDao.CheckCustomer(loginModel.Password, loginModel.Email);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                }
                else if (result == 2)
                {
                    ModelState.AddModelError("", "Tài khoản không được phép truy cập.");                 
                }
                else if (result == 1)
                {
                    var user = db.Users.SingleOrDefault(s => s.Email == loginModel.Email);
                    ReloadSession(user);
                    user.Status = true;
                    UpdateModel(user);
                    db.SubmitChanges();
                    if (url == null)
                        return Redirect("/");
                    return Redirect(url);
                }
            }
            return View(loginModel);
        }
        public ActionResult Logout(string url)
        {
            if (Session["UserID"] != null)
            {
                var user = db.Users.SingleOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));
                user.Status = false;
                UpdateModel(user);
                db.SubmitChanges();
            }
            Session.Clear();
            Session.Remove(HttpContext.Session.SessionID);
            if (url != null)
                return Redirect(url);
            return RedirectToAction("Login", "Account");
        }
        public ActionResult ShowProfile(string msg)
        {
            if (Session["UserID"] != null)
            {
                ViewData["msg"] = msg;
                EditUserModel editModel = new EditUserModel();
                User result = db.Users.SingleOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));
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
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult ShowProfile(EditUserModel editModel, FormCollection formCollection)
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            string msg = null;
            try
            {
                UserDao userDao = new UserDao();
                if (ModelState.IsValid && Session["UserID"] != null)
                {
                    User user = db.Users.FirstOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));//lay user cần sửa trog db
                    if (!editModel.Truename.Equals(null))
                        user.Truename = editModel.Truename;
                    if (userDao.CheckUsername(editModel.Username, user.Id) == false)
                        user.Username = editModel.Username;
                    else
                    {
                        ViewData["duplicateUsername"] = "Username đã tồn tại";
                        return View(editModel);
                    }
                    if (userDao.CheckEmail(editModel.Email, user.Id) == false)
                    {
                        user.Email = editModel.Email;
                    }
                    else
                    {
                        ViewData["duplicateEmail"] = "Email đã tồn tại";
                        return View(editModel);
                    }
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
                    Session.Clear();
                    ReloadSession(user);
                    msg = "Cập nhật thành công";
                }
                else
                {
                    return View(editModel);
                }
            }
            catch
            {
                return View(editModel);
            }
            return RedirectToAction("ShowProfile", "Account", new { msg = msg });
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
                ReloadSession(p);
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
            ChangePasswordModel changePassword = new ChangePasswordModel();
            changePassword.ID = id;
            if (session == null)
                return RedirectToAction("Index", "Home");
            return View(changePassword);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel changePassword, int id)
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
        public ActionResult ForgotPassword()
        {
            EmailModel emailModel = new EmailModel();
            return View(emailModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(EmailModel emailModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["Email"]) && !string.IsNullOrWhiteSpace(formCollection["Email"]))
            {
                User user = db.Users.SingleOrDefault(s => s.Email == emailModel.Email);
                List<ResetPasswordCode> resetPasswordCodeList = db.ResetPasswordCodes.ToList();
                ResetPasswordCode resetPasswordCode = new ResetPasswordCode();
                Random random = new Random();
                var body = "<p>Chào bạn,</p>" +
                            "<p>Bạn vui lòng click vào nút Xác thực email dưới đây để xác minh địa chỉ email của bạn.</p>" +
                            "<h2>Code: {0}</h2>" +
                            "<h3>SNShop</h3>";
                int code = random.Next(100000, 999999);
                while (resetPasswordCodeList.Count(s => s.Code == code) > 0)
                {
                    code = random.Next(100000, 999999);
                }
                var message = new MailMessage();
                message.To.Add(new MailAddress(emailModel.Email));
                message.From = new MailAddress("1951052171son@ou.edu.vn");
                message.Subject = "Đặt lại mật khẩu";
                message.Body = string.Format(body, code.ToString());
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
                        var check = resetPasswordCodeList.Where(s => s.UserID == user.Id);
                        if (check != null)
                            db.ResetPasswordCodes.DeleteAllOnSubmit(check);
                        resetPasswordCode.UserID = user.Id;
                        resetPasswordCode.Code = code;
                        resetPasswordCode.ModifiedDate = DateTime.Now;
                        db.ResetPasswordCodes.InsertOnSubmit(resetPasswordCode);
                        db.SubmitChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email chưa đăng ký!!!");
                        return View(emailModel);
                    }
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("ResetPasswordCode", "Account");
                }
            }
            return View(emailModel);
        }
        public ActionResult ResetPasswordCode()
        {
            ResetPasswordCodeModel resetPasswordCodeModel = new ResetPasswordCodeModel();
            return View(resetPasswordCodeModel);
        }
        [HttpPost]
        public ActionResult ResetPasswordCode(ResetPasswordCodeModel resetPasswordCodeModel)
        {
            if (ModelState.IsValid)
            {
                var code = db.ResetPasswordCodes.SingleOrDefault(s => s.Code == int.Parse(resetPasswordCodeModel.Code));
                if (code != null)
                {
                    return RedirectToAction("ResetPassword", "Account", new { code = int.Parse(resetPasswordCodeModel.Code) });
                }
                ModelState.AddModelError("", "Mã bảo mật không đúng");
            }
            return View(resetPasswordCodeModel);
        }
        public ActionResult ResetPassword()
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
            return View(resetPasswordModel);
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel resetPasswordModel, int code)
        {
            ResetPasswordCode resetPasswordCode = db.ResetPasswordCodes.SingleOrDefault(s => s.Code == code);
            if (ModelState.IsValid)
            {
                if (resetPasswordCode != null)
                {
                    resetPasswordCode.User.PasswordHash = Encode.GetMD5(resetPasswordModel.ConfirmPassword);
                    UpdateModel(resetPasswordCode.User);
                    db.ResetPasswordCodes.DeleteOnSubmit(resetPasswordCode);
                    db.SubmitChanges();
                    return RedirectToAction("UpdatePasswordSuccess", "Account");
                }
                return RedirectToAction("ForgotPassword", "Account");
            }
            return View(resetPasswordModel);
        }
        public ActionResult UpdatePasswordSuccess()
        {
            return View();
        }
    }
}