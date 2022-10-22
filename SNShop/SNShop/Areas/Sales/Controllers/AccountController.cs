using SNShop.Areas.Sales.Common;
using SNShop.Areas.Sales.Models;
using SNShop.DAO;
using SNShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using EmailModel = SNShop.Areas.Sales.Models.EmailModel;
using Encode = SNShop.Areas.Sales.Common.Encode;
using ResetPasswordCodeModel = SNShop.Areas.Sales.Models.ResetPasswordCodeModel;
using ResetPasswordModel = SNShop.Areas.Sales.Models.ResetPasswordModel;

namespace SNShop.Areas.Sales.Controllers
{
    public class AccountController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public void ReloadSession(User user)
        {
            UserDao userDao = new UserDao();
            var userSession = new EmployeeLogin()
            {
                UserName = user.Username,
                TrueName = user.Truename,
                UserID = user.Id,
                Email = user.Email,
                Image = user.Image,
                Roles = userDao.GetRoleById(user.Id).Name,
            };
            Session.Add(Constants.USER_SESSION, userSession);
            Session.Add("TrueName", userSession.TrueName);
            Session.Add("UserID", userSession.UserID);
            Session.Add("UserName", userSession.UserName);
            Session.Add("Email", userSession.Email);
            Session.Add("Roles", userSession.Roles);
            Session.Add("Image", userSession.Image);
        }
        // GET: Admin/Account
        public ActionResult EmployeeLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeLogin(EmployeeLoginModel employeeLoginModel)
        {
            UserDao userDao = new UserDao();
            if (ModelState.IsValid)
            {
                var result = userDao.CheckSales(employeeLoginModel.Password, employeeLoginModel.Email);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Bạn không có quyền vào trang này.");
                }
                else if (result == 1)
                {
                    var user = db.Users.SingleOrDefault(s => s.Email == employeeLoginModel.Email);
                    ReloadSession(user);
                    user.Status = true;
                    UpdateModel(user);
                    db.SubmitChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(employeeLoginModel);
        }
        public ActionResult Logout()
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
            return RedirectToAction("EmployeeLogin", "Account");
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
            return View();
        }
        [HttpPost]
        public ActionResult ResetPasswordCode(ResetPasswordCodeModel resetPasswordCodeModel)
        {
            var code = db.ResetPasswordCodes.SingleOrDefault(s => s.Code == int.Parse(resetPasswordCodeModel.Code));
            if (code != null)
            {
                return RedirectToAction("ResetPassword", "Account", new { code = int.Parse(resetPasswordCodeModel.Code) });
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
            EditEmployeeModel editModel = new EditEmployeeModel();
            var session = (EmployeeLogin)Session[Constants.USER_SESSION];
            if (session == null)
                return RedirectToAction("Index", "Home");
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            var result = userDao.GetUserByEmail(Session["Email"].ToString());
            editModel.ID = result.Id;
            editModel.Address = result.Address;
            editModel.PhoneNumber = result.PhoneNumber;
            editModel.Username = result.Username;
            editModel.Email = result.Email;
            editModel.Image = result.Image;
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
        public ActionResult ShowProfile(EditEmployeeModel editModel, FormCollection formCollection)
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(s => s.Id == editModel.ID);
                user.Address = editModel.Address;
                user.PhoneNumber = editModel.PhoneNumber;
                user.Username = editModel.Username;
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
                ReloadSession(user);
                return RedirectToAction("ShowProfile", "Account");
            }
            return View(editModel);
        }
        [HttpPost]
        public ActionResult ChangeImage(ImageModel imageModel)
        {
            try
            {
                User user = db.Users.SingleOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));
                string fileName = Path.GetFileNameWithoutExtension(imageModel.File.FileName);
                string extension = Path.GetExtension(imageModel.File.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                imageModel.Path = "~/Images/Avatars/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Avatars/"), fileName);
                imageModel.File.SaveAs(fileName);
                user.Image = imageModel.Path.Remove(0, 1);
                UpdateModel(user);
                db.SubmitChanges();
                ReloadSession(user);
            }
            catch
            {
                ViewData["loi"] = "Bạn vui lòng chọn ảnh.";
            }
            return RedirectToAction("ShowProfile", "Account");
        }
        public ActionResult ChangeEmployeePassword()
        {
            ChangeEmployeePasswordModel changeEmployeePasswordModel = new ChangeEmployeePasswordModel();
            return View(changeEmployeePasswordModel);
        }
        [HttpPost]
        public ActionResult ChangeEmployeePassword(ChangeEmployeePasswordModel changeEmployeePassword)
        {
            if (!ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(s => s.Id == int.Parse(Session["UserID"].ToString()));
                if (!user.PasswordHash.Equals(Encode.GetMD5(changeEmployeePassword.OldPassword)))
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                else
                {
                    user.PasswordHash = Encode.GetMD5(changeEmployeePassword.ConfirmNewPassword);
                    UpdateModel(user);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("UpdatePasswordSuccess", "Account");
        }
        public ActionResult UpdatePasswordSuccess()
        {
            return View();
        }
    }
}