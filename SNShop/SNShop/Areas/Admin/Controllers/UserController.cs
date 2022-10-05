﻿using SNShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult List_Users(string error)
        {
            var p = db.Users.Select(s => s).ToList();
            ViewData["loi"] = error;
            return View(p);
        }
        public ActionResult Create_Users()
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create_Users(FormCollection formCollection, User user)
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
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
                                                user.Email = formCollection["Email"];
                                                user.Username = formCollection["Username"];
                                                user.Truename = formCollection["Truename"];
                                                user.PasswordHash = formCollection["PasswordHash"];
                                                user.PhoneNumber = formCollection["PhoneNumber"];
                                                user.DistrictID = int.Parse(formCollection["DT"]);
                                                user.ProvinceID = int.Parse(formCollection["PR"]);
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
                                    }
                                }
                            }                         
                        }
                        
                    }
                    
                }
                
            }           
            return View(user);
        }
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "id")]
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