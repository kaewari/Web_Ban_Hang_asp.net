using Microsoft.Ajax.Utilities;
using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public ActionResult List_Users(string error)
        {
            var p = db.Users.Select(s => s).ToList();
            ViewData["loi"] = error;
            return View(p);
        }
        public ActionResult Create_Users()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create_Users(FormCollection formCollection, User user)
        {
            if (string.IsNullOrEmpty(formCollection["Email"]) || string.IsNullOrWhiteSpace(formCollection["Email"]))
            {
                ViewData["Loi"] = "Bạn vui lòng nhập email";
            }
            if (string.IsNullOrEmpty(formCollection["Username"]) || string.IsNullOrWhiteSpace(formCollection["Username"]))
            {
                ViewData["Loi"] = "Bạn vui lòng nhập username";
            }
            if (string.IsNullOrEmpty(formCollection["Truename"]) || string.IsNullOrWhiteSpace(formCollection["Truename"]))
            {
                ViewData["Loi"] = "Bạn vui lòng nhập họ tên";
            }
            else
            {
                try
                {
                    user.Email = formCollection["Email"];
                    user.Username = formCollection["Username"];
                    user.Truename = formCollection["Truename"];
                    user.ModifiedDate = DateTime.Now;
                    db.Users.InsertOnSubmit(user);
                    db.SubmitChanges();
                    return RedirectToAction("List_Users");
                }
                catch (Exception ex)
                {
                    ViewData["loi"] = ex.Message;
                }
            }
            return View(user);
        }
        public ActionResult Details_Users(int id)
        {
            var p = db.Users.FirstOrDefault(s => s.Id == id);
            return View(p);
        }
        public ActionResult Delete_User_Image(int id)
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
    }
}