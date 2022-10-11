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
    public class BannerController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server)]
        // GET: Admin/Banner
        public ActionResult List_Banners()
        {
            var p = db.Banners.ToList();
            return View(p);
        }
        public ImageModel Single_Product_Image(ImageModel imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.File.FileName);
            string extension = Path.GetExtension(imageModel.File.FileName);
            if (extension != ".jpg" && extension != ".png")
                return null;
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.Path = "~/Images/Products/" + fileName;
            var ServerSavePath = Path.Combine(Server.MapPath("~/Images/Products/"), fileName);
            //Save file to server folder  
            imageModel.File.SaveAs(ServerSavePath);
            //assigning file uploaded status to ViewBag for showing message to user.  
            ViewBag.UploadStatus = "Thêm thành công.";
            return imageModel;
        }
        public List<string> Multiple_Product_Image(HttpPostedFileBase[] Files)
        {
            List<string> pathList = new List<string>();
            try
            {
                foreach (HttpPostedFileBase item in Files)
                {
                    if (item != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                        string extension = Path.GetExtension(item.FileName);
                        if (extension != ".jpg" && extension != ".png")
                            return null;
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = "~/Images/Banners/" + fileName;
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Images/Banners/") + fileName);
                        //Save file to server folder  
                        item.SaveAs(ServerSavePath);
                        pathList.Add(path);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = Files.Count().ToString() + " ảnh đã được thêm thành công.";
                    }
                }
            }
            catch
            {
                return null;
            }

            return pathList;
        }
        public ActionResult Create_Banner_Image()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create_Banner_Image(HttpPostedFileBase[] Files, Banner p)
        {
            try
            {
                List<string> image = Multiple_Product_Image(Files);
                for (int i = 0; i < image.Count(); i++)
                {
                    p = new Banner();
                    p.Path = image[i].Remove(0, 1);
                    p.EmployeeID = db.Employees.SingleOrDefault(s => s.UserID == int.Parse(Session["UserID"].ToString())).Id;
                    p.ModifiedDate = DateTime.Now;
                    db.Banners.InsertOnSubmit(p);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                ViewData["loi"] = ex.Message;
            }
            return View();
        }
        public ActionResult Edit_Banner_Image(int id)
        {
            var p = db.Banners.FirstOrDefault(s => s.Id == id);
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit_Banner_Image(ImageModel imageModel, Banner p, int id)
        {
            try
            {
                ImageModel image = Single_Product_Image(imageModel);
                p = db.Banners.FirstOrDefault(s => s.Id == id);
                p.Path = image.Path.Remove(0, 1);
                p.EmployeeID = db.Employees.SingleOrDefault(s => s.UserID == int.Parse(Session["UserID"].ToString())).Id;
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
        [HttpPost]
        public ActionResult Delete_Banner_Image(int id)
        {
            string error = "Bạn không thể xóa ảnh này.";
            string success = "Xóa ảnh thành công";
            try
            {
                Banner banner = db.Banners.SingleOrDefault(s => s.Id == id);
                db.Banners.DeleteOnSubmit(banner);
                db.SubmitChanges();
                return Json(new
                {
                    status = 200,
                    success = success
                });
            }
            catch
            {
            }
            return Json(new
            {
                status = 400,
                error = error
            });
        }
    }
}