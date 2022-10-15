using Microsoft.Ajax.Utilities;
using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace SNShop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/Product
        [OutputCache(Duration = 900, Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult List_Products(string error)
        {
            var p = db.Products.Select(s => s).ToList();
            ViewData["loi"] = error;
            return View(p);
        }
        public ActionResult Create_Product()
        {
            ViewData["NCC"] = new SelectList(db.Brands, "Id", "Name");
            ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create_Product(FormCollection form, Product product)
        {
            ViewData["NCC"] = new SelectList(db.Brands, "Id", "Name");
            ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            if (string.IsNullOrEmpty(form["Name"]) || string.IsNullOrWhiteSpace(form["Name"]))
            {
                ViewData["Loi"] = "Vui lòng nhập tên sản phẩm.";
            }
            else
            {
                try
                {
                    product.Name = form["Name"];
                    if (!string.IsNullOrEmpty(form["LSP"]) || !string.IsNullOrWhiteSpace(form["LSP"]))
                        product.CategoryID = int.Parse(form["LSP"]);
                    if (!string.IsNullOrEmpty(form["NCC"]) || !string.IsNullOrWhiteSpace(form["NCC"]))
                        product.BrandID = int.Parse(form["NCC"]);
                    if (!string.IsNullOrEmpty(form["Release"]) || !string.IsNullOrWhiteSpace(form["Release"]))
                    {
                        product.ModifiedDate = DateTime.Parse(form["Release"]);
                        ViewData["Loi"] = "Vui lòng chọn thời gian ra mắt.";
                    }                    
                    if (!string.IsNullOrEmpty(form["QuantityPerUnit"]) || !string.IsNullOrWhiteSpace(form["QuantityPerUnit"]))
                    {
                        product.QuantityPerUnit = form["QuantityPerUnit"];
                        ViewData["Loi"] = "Vui lòng nhập tổng số lượng.";
                    }
                    if (!string.IsNullOrEmpty(form["Price"]) || !string.IsNullOrWhiteSpace(form["Price"]))
                    {
                        product.Price = int.Parse(form["Price"]);
                        ViewData["Loi"] = "Vui lòng nhập giá sản phẩm.";
                    }
                    if (!string.IsNullOrEmpty(form["UnitsInStock"]) || !string.IsNullOrWhiteSpace(form["UnitsInStock"]))
                    {
                        product.UnitsInStock = int.Parse(form["UnitsInStock"]);
                        ViewData["Loi"] = "Vui lòng nhập số lượng trong kho.";
                    }
                    if (!string.IsNullOrEmpty(form["UnitsOnOrder"]) || !string.IsNullOrWhiteSpace(form["UnitsOnOrder"]))
                    {
                        product.UnitsOnOrder = int.Parse(form["UnitsOnOrder"]);
                        ViewData["Loi"] = "Vui lòng nhập số lượng trong đơn đặt hàng.";
                    }                      
                    if (!string.IsNullOrEmpty(form["Discontinued"]) || !string.IsNullOrWhiteSpace(form["Discontinued"]))
                        product.Discontinued = bool.Parse(form["Discontinued"]);
                    db.Products.InsertOnSubmit(product);
                    db.SubmitChanges();
                    return RedirectToAction("List_Products");
                }
                catch (Exception ex)
                {
                    ViewData["Loi"] = ex.Message;
                }
            }

            return View(product);
        }
        public ActionResult Details_Product(int id)
        {
            Product p = db.Products.FirstOrDefault(s => s.Id == id);
            return View(p);
        }
        public ActionResult Edit_Product(int id)
        {
            var p = db.Products.FirstOrDefault(s => s.Id == id);
            ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            ViewData["NCC"] = new SelectList(db.Brands, "Id", "Name");
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit_Product(FormCollection form, int id)
        {
            ViewData["NCC"] = new SelectList(db.Brands, "Id", "Name");
            ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            var p = db.Products.Where(s => s.Id == id).FirstOrDefault();
            if (string.IsNullOrEmpty(form["Name"]))
            {
                ViewData["Loi"] = "Vui lòng nhập tên sản phẩm.";
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(form["LSP"]) || !string.IsNullOrWhiteSpace(form["LSP"]))
                        p.CategoryID = int.Parse(form["LSP"]);
                    if (!string.IsNullOrEmpty(form["NCC"]) || !string.IsNullOrWhiteSpace(form["NCC"]))
                        p.BrandID = int.Parse(form["NCC"]);
                    p.Name = form["Name"];
                    if (!string.IsNullOrEmpty(form["CPU"]))
                        p.CPU = form["CPU"];
                    if (!string.IsNullOrEmpty(form["RAM"]))
                        p.RAM = form["RAM"];
                    if (!string.IsNullOrEmpty(form["Color"]))
                        p.Color = form["Color"];
                    if (!string.IsNullOrEmpty(form["Design"]))
                        p.Design = form["Design"];
                    if (!string.IsNullOrEmpty(form["PIN"]))
                        p.PIN = form["PIN"];
                    if (!string.IsNullOrEmpty(form["OS"]))
                        p.OS = form["OS"];
                    if (!string.IsNullOrEmpty(form["Screen"]))
                        p.Screen = form["Screen"];
                    if (!string.IsNullOrEmpty(form["Release"]) || !string.IsNullOrWhiteSpace(form["Release"]))
                    {
                        p.ModifiedDate = DateTime.Parse(form["Release"]);
                        ViewData["Loi"] = "Vui lòng chọn thời gian ra mắt.";
                    }
                    if (!string.IsNullOrEmpty(form["QuantityPerUnit"]))
                        p.VGA = form["VGA"];
                    if (!string.IsNullOrEmpty(form["Release"]) || !string.IsNullOrWhiteSpace(form["Release"]))
                    {
                        p.ModifiedDate = DateTime.Parse(form["Release"]);
                        ViewData["Loi"] = "Vui lòng chọn thời gian ra mắt.";
                    }
                    if (!string.IsNullOrEmpty(form["QuantityPerUnit"]) || !string.IsNullOrWhiteSpace(form["QuantityPerUnit"]))
                    {
                        p.QuantityPerUnit = form["QuantityPerUnit"];
                        ViewData["Loi"] = "Vui lòng nhập tổng số lượng.";
                    }
                    if (!string.IsNullOrEmpty(form["Price"]) || !string.IsNullOrWhiteSpace(form["Price"]))
                    {
                        p.Price = int.Parse(form["Price"]);
                        ViewData["Loi"] = "Vui lòng nhập giá sản phẩm.";
                    }
                    if (!string.IsNullOrEmpty(form["UnitsInStock"]) || !string.IsNullOrWhiteSpace(form["UnitsInStock"]))
                    {
                        p.UnitsInStock = int.Parse(form["UnitsInStock"]);
                        ViewData["Loi"] = "Vui lòng nhập số lượng trong kho.";
                    }
                    if (!string.IsNullOrEmpty(form["UnitsOnOrder"]) || !string.IsNullOrWhiteSpace(form["UnitsOnOrder"]))
                    {
                        p.UnitsOnOrder = int.Parse(form["UnitsOnOrder"]);
                        ViewData["Loi"] = "Vui lòng nhập số lượng trong đơn đặt hàng.";
                    }
                    if (!string.IsNullOrEmpty(form["Discontinued"]) || !string.IsNullOrWhiteSpace(form["Discontinued"]))
                        p.Discontinued = bool.Parse(form["Discontinued"]);
                    p.ModifiedDate = DateTime.Now;
                    UpdateModel(p);
                    db.SubmitChanges();
                    return RedirectToAction("List_Products");
                }
                catch (Exception ex)
                {
                    ViewData["loi"] = ex.Message;
                }
            }
            return View(p);
        }
        [HttpPost]
        public ActionResult Delete_Product(int id)
        {
            string error = null;
            try
            {
                var p = db.Products.FirstOrDefault(s => s.Id == id);
                db.Products.DeleteOnSubmit(p);
                db.SubmitChanges();
                error = "Xóa thành công sản phẩm " + id;
                return Json(new
                {
                    success = error,
                    status = 200
                });
            }
            catch
            {
                error = "Bạn không thể xóa sản phẩm " + id;
            }
            return Json(new
            {
                error = error,
                status = 400
            });
        }
        [HttpPost]
        public ActionResult Delete_All_Product()
        {
            bool check = false;
            List<int> listSuccess = new List<int>();
            List<int> listFail = new List<int>();
            List<ProductImage> pImage = db.ProductImages.DistinctBy(s=>s.ProductID).ToList();
            List<Product> p = db.Products.ToList();
            int totalProduct = db.Products.Count();
            if (pImage.Count() == 0)
            {
                db.Products.DeleteAllOnSubmit(p);
                db.SubmitChanges();
            }
            else
            {
                foreach (var item in p)
                {
                    foreach (var itemj in pImage)
                    {                        
                        if (item.Id == itemj.ProductID)
                        {
                            check = true;
                            listFail.Add(item.Id);
                            break;
                        }
                    }
                    if (check == false)
                    {
                        db.Products.DeleteOnSubmit(item);
                        db.SubmitChanges();
                        listSuccess.Add(item.Id);
                    }
                    check = false;
                }
            }
            string success = "Xóa thành công tất cả sản phẩm.";
            return Json(new
            {
                totalProduct = totalProduct,
                countSuccess = listSuccess,
                countFail = listFail,
                success = success,
                status = 200
            });
        }
    }
}
