using SNShop.Areas.Admin.Models;
using SNShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SNShop.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/ProductImage
        [OutputCache(Duration = 900, Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult List_Product_Image(string error)
        {
            _ = new List<ProductImage>(1500);
            List<ProductImage> p = db.ProductImages.ToList();
            ViewData["loi"] = error;
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
                        var path = "~/Images/Products/" + fileName;
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Images/Products/") + fileName);
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
        public ActionResult Create_Product_Image(int id)
        {
            ViewBag.Images = db.ProductImages.Where(s => s.ProductID == id);
            ViewBag.ProductName = db.Products.FirstOrDefault(s => s.Id == id).Name;
            return View();
        }
        [HttpPost]
        public ActionResult Create_Product_Image(HttpPostedFileBase[] Files, ProductImage productImage, int id)
        {
            try
            {
                int p = db.ProductImages.Where(s => s.ProductID == id).Count();
                if (p > 4 || Files.Count() > 4 || (p + Files.Count() > 4))
                    ViewData["loi"] = "Bạn chỉ được thêm tối đa 4 ảnh. Hiện đang có " + p + " ảnh.";
                else
                {
                    List<string> image = Multiple_Product_Image(Files);
                    for (int i = 0; i < image.Count(); i++)
                    {
                        productImage = new ProductImage();
                        productImage.ProductID = id;
                        productImage.Thumbnail_Photo = image[i].Remove(0, 1);
                        productImage.ModifiedDate = DateTime.Now;
                        db.ProductImages.InsertOnSubmit(productImage);
                        db.SubmitChanges();
                    }
                }
                ViewBag.Images = db.ProductImages.Where(s => s.ProductID == id);
                ViewBag.ProductName = db.ProductImages.FirstOrDefault(s => s.ProductID == id).Product.Name;
            }
            catch {}
            return View(productImage);
        }
        public ActionResult Create_Product_Image_2()
        {
            ViewData["SP"] = new SelectList(db.Products, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create_Product_Image_2(HttpPostedFileBase[] Files, FormCollection formCollection, ProductImage productImage)
        {
            try
            {
                ViewData["SP"] = new SelectList(db.Products, "Id", "Name");

                int p = db.ProductImages.Where(s => s.ProductID == int.Parse(formCollection["SP"])).Count();
                if (p > 4 || Files.Count() > 4 || (p + Files.Count() > 4))
                    ViewData["loi"] = "Bạn chỉ được thêm tối đa 4 ảnh. Hiện đang có " + p + " ảnh.";
                else
                {
                    List<string> image = Multiple_Product_Image(Files);
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
            }
            catch { }
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
        public ActionResult Edit_Product_Image(ImageModel imageModel, FormCollection formCollection, ProductImage p, int id)
        {
            try
            {
                ViewData["SP"] = new SelectList(db.Products, "Id", "Name");
                ImageModel image = Single_Product_Image(imageModel);
                p = db.ProductImages.FirstOrDefault(s => s.Id == id);
                p.ProductID = int.Parse(formCollection["SP"]);
                p.Thumbnail_Photo = image.Path.Remove(0,1);
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
        public ActionResult Delete_Product_Image(int id)
        {
            string error = "Bạn không thể xóa ảnh này.";
            string success = "Xóa ảnh thành công";
            try
            {
                ProductImage productImage = db.ProductImages.FirstOrDefault(s => s.Id == id);
                db.ProductImages.DeleteOnSubmit(productImage);
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
            return Json(new {
                status = 400,
                error = error
            });
        }
        [HttpPost]
        public ActionResult Delete_All_Product_Image()
        {
            string success = "Xóa thành công tất cả ảnh.";
            string error = "Không thể xóa ảnh.";
            try
            {
                List<ProductImage> pImage = db.ProductImages.ToList();

                db.ProductImages.DeleteAllOnSubmit(pImage);
                db.SubmitChanges();

                return Json(new
                {
                    success = success,
                    status = 200
                });
            }
            catch
            {

                return Json(new
                {
                    error = error,
                    status = 400
                });
            }
        }
    }
}