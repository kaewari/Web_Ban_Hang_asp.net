using SNShop.Common;
using SNShop.DAO;
using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SNShop.Areas.Admin.Models;
using SNShop.Areas.Admin.Common;
using Constants = SNShop.Areas.Admin.Common.Constants;
using Encode = SNShop.Areas.Admin.Common.Encode;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Web.UI.WebControls;
using ChangeAdminPasswordModel = SNShop.Areas.Admin.Models.ChangeAdminPasswordModel;
using System.Security.Principal;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SNShop.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin(LoginAdminModel loginAdminModel)
        {
            UserDao userDao = new UserDao();
            if (ModelState.IsValid)
            {
                var result = userDao.CheckUser(loginAdminModel.Password, loginAdminModel.Email);
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
                    var user = userDao.GetUserByEmail(loginAdminModel.Email);
                    var userSession = new AdminLogin()
                    {
                        UserName = user.Username,
                        UserID = user.Id,
                        Email = user.Email,
                        Roles = userDao.GetRoleById(user.Id).Name,
                    };
                    Session.Add(Constants.USER_SESSION, userSession);
                    Session.Add("UserID", userSession.UserID);
                    Session.Add("UserName", userSession.UserName);
                    Session.Add("Email", userSession.Email);
                    Session.Add("Roles", userSession.Roles);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginAdminModel);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session.Remove(HttpContext.Session.SessionID);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(EmailModel emailModel)
        {
            User user = db.Users.SingleOrDefault(s => s.Email == emailModel.Email);
            List<ResetPasswordCode> resetPasswordCodeList = db.ResetPasswordCodes.ToList();
            ResetPasswordCode resetPasswordCode = new ResetPasswordCode();
            Random random = new Random();
            if (ModelState.IsValid)
            {
                var body = "<p>Chào bạn,</p>" +
                            "<p>Bạn vui lòng click vào nút Xác thực email dưới đây để xác minh địa chỉ email của bạn.</p>" +
                            "<h2>Code: {0}</h2>" +
                            "<h3>SNShop</h3>";
                int code = random.Next(100000, 999999);
                while (resetPasswordCodeList.Count(s=>s.Code == code) > 0)
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
                        Password = "caygame1080@"
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
            return View();
        }
        [HttpPost]
        public ActionResult ResetPasswordCode(ResetPasswordCodeModel resetPasswordCodeModel)
        {
            var code = db.ResetPasswordCodes.SingleOrDefault(s=>s.Code == int.Parse(resetPasswordCodeModel.Code));
            if (code != null)
            {
                return RedirectToAction("ResetPassword", "Account", new {code = int.Parse(resetPasswordCodeModel.Code) });
            }
            ModelState.AddModelError("", "Mã bảo mật không đúng");
            return View(resetPasswordCodeModel);
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel resetPasswordModel, int code)
        {
            ResetPasswordCode resetPasswordCode = db.ResetPasswordCodes.SingleOrDefault(s => s.Code == code);
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
        public ActionResult ShowProfile()
        {
            UserDao userDao = new UserDao();
            EditAdminModel editModel = new EditAdminModel();
            var session = (AdminLogin)Session[Constants.USER_SESSION];
            if (session == null)
                return RedirectToAction("Index", "Home");
            var result = userDao.GetUserByEmail(Session["Email"].ToString());
            editModel.ID = result.Id;
            editModel.Address = result.Address;
            editModel.PhoneNumber = result.PhoneNumber;
            editModel.Username = result.Username;
            editModel.Email = result.Email;
            editModel.Image = result.Image;
            editModel.ProvinceID = result.ProvinceID.ToString();
            editModel.DistrictID = result.DistrictID.ToString();
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
        public ActionResult ShowProfile(EditAdminModel editModel)
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
        [HttpPost]
        public ActionResult ChangeImage(ImageModel imageModel)
        {
            User user = db.Users.SingleOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));
            string fileName = Path.GetFileNameWithoutExtension(imageModel.File.FileName);
            string extension = Path.GetExtension(imageModel.File.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.Path = "~/Images/Avartars" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/Avartars"), fileName);
            imageModel.File.SaveAs(fileName);
            user.Image = imageModel.Path.Remove(0,1);
            UpdateModel(user);
            db.SubmitChanges();
            return RedirectToAction("ShowProfile", "Home");
        }
        public ActionResult ChangeAdminPassword()
        {
            var session = (AdminLogin)Session[Constants.USER_SESSION];
            if (session == null)
                return RedirectToAction("LoginAdmin", "Account");
            return View();
        }
        [HttpPost]
        public ActionResult ChangeAdminPassword(ChangeAdminPasswordModel changeAdminPassword)
        {
            User user = db.Users.FirstOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));
            if (!user.PasswordHash.Equals(Encode.GetMD5(changeAdminPassword.OldPassword)))
                ModelState.AddModelError("", "Mật khẩu không đúng.");
            else
            {
                user.PasswordHash = Encode.GetMD5(changeAdminPassword.ConfirmNewPassword);
                UpdateModel(user);
                db.SubmitChanges();
                Logout();
            }
            return RedirectToAction("UpdatePasswordSuccess", "Account");
        }
        public ActionResult UpdatePasswordSuccess()
        {
            return View();
        }
    }
}