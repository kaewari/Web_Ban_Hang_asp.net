using SNShop.Areas.Admin.Models;
using SNShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class UserImageController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/CreateUserImage
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
        public ActionResult Create_User_Image(int id)
        {
            return View(db.Users.FirstOrDefault(s=>s.Id == id));
        }
        [HttpPost]
        public ActionResult Create_User_Image(ImageModel imageModel, User p, int id)
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
            return View(p);
        }
        public ActionResult Edit_Users_Image(int id)
        {
            var p = db.Users.FirstOrDefault(s => s.Id == id);
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
            if (string.IsNullOrEmpty(p.ProvinceID.ToString()))
                p.ProvinceID = 0;
            if (string.IsNullOrEmpty(p.ProvinceID.ToString()))
                p.DistrictID = 0;
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit_Users_Image(ImageModel imageModel, User p, int id)
        {
            ViewData["PR"] = new SelectList(db.Provinces, "Id", "Name");
            ViewData["DT"] = new SelectList(db.Districts, "Id", "Name");
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
            return View(p);
        }
        public ActionResult Edit_Users(FormCollection formCollection, int id)
        {
            try
            {
                var p = db.Users.FirstOrDefault(s => s.Id == id);
                p.Truename = formCollection["truename"];
                p.Email = formCollection["Email"];
                p.Username = formCollection["Username"];
                p.ProvinceID = int.Parse(formCollection["PR"]);
                p.DistrictID = int.Parse(formCollection["DT"]);
                p.ModifiedDate = DateTime.Now;
                UpdateModel(p);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                ViewData["loi"] = ex.Message;
            }
            return RedirectToAction("Edit_Users_Image", new { id = id });
        }
        public ActionResult Delete_User_Image(int id, string url)
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
            return RedirectToAction(url, new {id = id});
        }
    }
}