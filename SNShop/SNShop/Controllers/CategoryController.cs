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
        public ActionResult ProductOfCategory(int? page, string sortOrder, int Id)
        {
            var sanPham = db.Products.ToList();
            sanPham = db.Products.Where(v => v.CategoryID == Id).ToList();
            Category category = db.Categories.Where(s => s.Id == Id).FirstOrDefault();
            ViewBag.Title = category.Name;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortByName = string.IsNullOrEmpty(sortOrder) ? "ten_desc" : "";
            ViewBag.SortByPrice = (sortOrder == "dongia_desc" ? "dongia" : "dongia_desc");
            ViewBag.SortByDate = (sortOrder == "ngay_desc" ? "ngay" : "ngay_desc");
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
            ViewBag.Category = db.Categories.Select(s => s).ToList();

            return View(sanPham.ToList().ToPagedList(page.Value, pageSize));
        }
    }
}