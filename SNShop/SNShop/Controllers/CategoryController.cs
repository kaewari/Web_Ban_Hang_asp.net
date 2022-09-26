using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SNShop.Models;
using PagedList;
namespace SNShop.Controllers
{
    public class CategoryController : MyBaseController
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Category
        public ActionResult ProductOfCategory(int? page, string sortOrder, int id)
        {
            var sanPham = db.Products.ToList();
            sanPham = db.Products.Where(v => v.CategoryID == id).ToList();
            Category category = db.Categories.Where(s => s.Id == id).FirstOrDefault();
            ViewBag.Title = category.Name;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortByName = string.IsNullOrEmpty(sortOrder) ? "ten_desc" : "";
            ViewBag.SortByPrice = (sortOrder == "dongia_desc" ? "dongia" : "dongia_desc");
            ViewBag.SortByDate = (sortOrder == "ngay_desc" ? "ngay" : "ngay_desc");
            ViewBag.SubCategory = db.SubCategories.Where(s => s.CategoryID == id).ToList();
            switch (sortOrder)
            {
                case "ten_desc":
                    sanPham = db.Products.OrderByDescending(s => s.Name).ToList();
                    break;
                case "dongia_desc":
                    sanPham = db.Products.OrderByDescending(s => s.Price).ToList();
                    break;
                case "dongia":
                    sanPham = db.Products.OrderBy(s => s.Price).ToList();
                    break;
                case "ngay_desc":
                    sanPham = db.Products.OrderByDescending(s => s.ModifiedDate).ToList();
                    break;
                case "ngay":
                    sanPham = db.Products.OrderBy(s => s.ModifiedDate).ToList();
                    break;
                default:
                    sanPham = db.Products.OrderBy(s => s.Name).ToList();
                    break;
            }
            if (!page.HasValue)
            {
                page = 1;
            }
            int pageSize = 12;
            ViewBag.PageSize = pageSize;
            
            ViewBag.Category = db.Categories.ToList();
            ViewBag.ProductOfCategory = db.Products.Where(s => s.CategoryID == id).ToList();
            ViewBag.PresentImage = db.ProductImages.ToList();
            return View(sanPham.ToList().ToPagedList(page.Value, pageSize));
        }
        public ActionResult ProductOfSubCategory(int id)
        {
            var x = db.SubCategories.FirstOrDefault(a => a.Id == id).CategoryID;
            var categoryID = db.Categories.FirstOrDefault(s => s.Id == x).Id;
            var productID = db.Products.FirstOrDefault(s => s.CategoryID == categoryID).Id;
            ViewBag.ProductOfSubCategory = db.Products.Where(s => s.SubCategoryID == id).ToList();
            ViewBag.SubCategory = db.SubCategories.Where(s => s.CategoryID == categoryID).ToList();
            ViewBag.PresentImage = db.ProductImages.ToList();
            ViewBag.Category = db.Categories.ToList();
            return View();
        }
    }
}