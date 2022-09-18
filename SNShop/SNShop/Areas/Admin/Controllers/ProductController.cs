using SNShop.Areas.Admin.Models;
using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/Product
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
            if (string.IsNullOrEmpty(form["Name"]))
            {
                ViewData["Loi"] = "Vui lòng nhập tên sản phẩm.";
                ViewData["NCC"] = new SelectList(db.Brands, "Id", "Name");
                ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            }
            else
            {
                product.Name = form["Name"];
                product.CategoryID = int.Parse(form["LSP"]);
                product.BrandID = int.Parse(form["NCC"]);
                product.ModifiedDate = DateTime.Parse(form["Release"]); ;
                db.Products.InsertOnSubmit(product);
                db.SubmitChanges();
                return RedirectToAction("List_Products");
            }
            if (!string.IsNullOrEmpty(form["QuantityPerUnit"]))
                product.QuantityPerUnit = int.Parse(form["QuantityPerUnit"]);
            if (!string.IsNullOrEmpty(form["Price"]))
                product.Price = int.Parse(form["Price"]);
            if (!string.IsNullOrEmpty(form["UnitsInStock"]))
                product.UnitsInStock = int.Parse(form["UnitsInStock"]);
            if (!string.IsNullOrEmpty(form["UnitsOnOrder"]))
                product.UnitsOnOrder = int.Parse(form["UnitsOnOrder"]);
            if (!string.IsNullOrEmpty(form["Discontinued"]))
                product.Discontinued = bool.Parse(form["Discontinued"]);
            return View(product);
        }
        public ActionResult Details_Product(int id)
        {
            Product p = db.Products.FirstOrDefault(s => s.Id == id);
            if (p.UnitsInStock == null || p.UnitsOnOrder == null || p.QuantityPerUnit == null || p.Discontinued == null)
            {
                p.UnitsInStock = 0;
                p.UnitsOnOrder = 0;
                p.QuantityPerUnit = 0;
                p.Discontinued = false;
            }
            return View(p);
        }
        public ActionResult Edit_Product(int id)
        {
            var p = db.Products.Where(s => s.Id == id).FirstOrDefault();
            ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            ViewData["NCC"] = new SelectList(db.Brands, "Id", "Name");
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit_Product(FormCollection form, int id)
        {
            var formats = new[]
            {
                "dd/MM/yyyy",
                "dd/MM/yyyy HH:mm",
                "dd/MM/yyyy HH:mm tt",
                "dd/MM/yyyy HH:mm:ss",
                "dd/MM/yyyy HH:mm:ss tt",
            };
            var p = db.Products.Where(s => s.Id == id).FirstOrDefault();
            if (string.IsNullOrEmpty(form["Name"]))
            {
                ViewData["Loi"] = "Vui lòng nhập tên sản phẩm.";
                ViewData["NCC"] = new SelectList(db.Brands, "Id", "Name");
                ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            }
            else
            {
                p.BrandID = int.Parse(form["NCC"]);
                p.CategoryID = int.Parse(form["LSP"]);
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
                if (!string.IsNullOrEmpty(form["Release"]))
                    p.Release = DateTime.Parse(form["Release"]);
                if (!string.IsNullOrEmpty(form["QuantityPerUnit"]))
                    p.VGA = form["VGA"];
                if (!string.IsNullOrEmpty(form["QuantityPerUnit"]))
                    p.QuantityPerUnit = int.Parse(form["QuantityPerUnit"]);
                if (!string.IsNullOrEmpty(form["Price"]))
                    p.Price = (int?)decimal.Parse(form["Price"]);
                if (!string.IsNullOrEmpty(form["UnitsInStock"]))
                    p.UnitsInStock = int.Parse(form["UnitsInStock"]);
                if (!string.IsNullOrEmpty(form["UnitsOnOrder"]))
                    p.UnitsOnOrder = int.Parse(form["UnitsOnOrder"]);
                if (!string.IsNullOrEmpty(form["Discontinued"]))
                    p.Discontinued = bool.Parse(form["Discontinued"]);
                p.ModifiedDate = DateTime.Now;
                UpdateModel(p);
                db.SubmitChanges();
                return RedirectToAction("List_Products");
            }
            if (!string.IsNullOrEmpty(form["Release"]))
                p.Release = DateTime.Now;
            if (!string.IsNullOrEmpty(form["QuantityPerUnit"]))
                p.QuantityPerUnit = int.Parse(form["QuantityPerUnit"]);
            if (!string.IsNullOrEmpty(form["Price"]))
                p.Price = int.Parse(form["Price"]);
            if (!string.IsNullOrEmpty(form["UnitsInStock"]))
                p.UnitsInStock = int.Parse(form["UnitsInStock"]);
            if (!string.IsNullOrEmpty(form["UnitsOnOrder"]))
                p.UnitsOnOrder = int.Parse(form["UnitsOnOrder"]);
            if (!string.IsNullOrEmpty(form["Discontinued"]))
                p.Discontinued = bool.Parse(form["Discontinued"]);
            return View(p);
        }

        public ActionResult Delete_Product(int id)
        {
            try
            {
                var p = db.Products.FirstOrDefault(s => s.Id == id);
                db.Products.DeleteOnSubmit(p);
                db.SubmitChanges();
            }
            catch
            {
                ViewData["loi"] = "Bạn không thể xóa sản phẩm này.";
            }
            return RedirectToAction("List_Products", new {error = ViewData["loi"] });
        }
        
    }
}
