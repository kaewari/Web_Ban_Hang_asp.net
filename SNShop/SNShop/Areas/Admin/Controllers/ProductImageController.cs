using SNShop.Areas.Admin.Models;
using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace SNShop.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/ProductImage
        public ActionResult List_Product_Image(string error)
        {
            var p = db.ProductImages.ToList();
            ViewData["loi"] = error;
            return View(p);
        }
        public List<string> Product_Image(HttpPostedFileBase[] Files)
        {
            //string fileName = Path.GetFileNameWithoutExtension(imageModel.File.FileName);
            //string extension = Path.GetExtension(imageModel.File.FileName);
            //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //imageModel.Path = "~/Images/Products/" + fileName;
            //fileName = Path.Combine(Server.MapPath("~/Images/Products/"), fileName);
            //imageModel.File.SaveAs(fileName);
            List<string> pathList = new List<string>();
            foreach(HttpPostedFileBase item in Files)
            {
                if (item != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                    string extension = Path.GetExtension(item.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    var path = "~/Images/Products/" + fileName;
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Images/Products/") + fileName);
                    //Save file to server folder  
                    item.SaveAs(ServerSavePath);
                    pathList.Add(path);
                    //assigning file uploaded status to ViewBag for showing message to user.  
                    ViewBag.UploadStatus = Files.Count().ToString() + " files uploaded successfully.";
                }
            }
            return pathList;
        }
        public ActionResult Create_Product_Image()
        {
            ViewData["SP"] = new SelectList(db.Products, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create_Product_Image(HttpPostedFileBase[] Files, FormCollection formCollection, ProductImage productImage)
        {
            try
            {
                ViewData["SP"] = new SelectList(db.Products, "Id", "Name");
                List<string> image = Product_Image(Files);
                for (int i = 0; i < image.Count(); i++)
                {
                    productImage = new ProductImage();
                    productImage.ProductID = int.Parse(formCollection["SP"]);
                    productImage.Thumbnail_Photo = image[i].Remove(0, 1);
                    productImage.ModifiedDate = DateTime.Now;
                    db.ProductImages.InsertOnSubmit(productImage);
                    db.SubmitChanges();
                }
            }
            catch
            {
                ViewData["loi"] = "Bạn vui lòng chọn ảnh.";
            }
            return View(productImage);
        }
        public ActionResult Details_Product_Image(int id)
        {
            var p = db.ProductImages.FirstOrDefault(s => s.Id == id);
            return View(p);
        }
        public ActionResult Edit_Product_Image(int id)
        {
            var p = db.ProductImages.FirstOrDefault(s => s.Id == id);
            ViewData["SP"] = new SelectList(db.Products, "Id", "Name");
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit_Product_Image(FormCollection formCollection, int id)
        {
            var p = db.ProductImages.FirstOrDefault(s => s.Id == id);
            p.ProductID = int.Parse(formCollection["SP"]);
            p.Thumbnail_Photo = formCollection["Thumbnail_Photo"];
            p.ModifiedDate = DateTime.Now;
            return View(p);
        }
        public ActionResult Delete_Product_Image(int id)
        {
            try
            {
                ProductImage productImage = db.ProductImages.FirstOrDefault(s => s.Id == id);
                db.ProductImages.DeleteOnSubmit(productImage);
                db.SubmitChanges();
            }
            catch
            {
                ViewData["loi"] = "Bạn không thể xóa ảnh này.";
            }
            return RedirectToAction("List_Product_Image", new { error = ViewData["loi"] });
        }
    }
}