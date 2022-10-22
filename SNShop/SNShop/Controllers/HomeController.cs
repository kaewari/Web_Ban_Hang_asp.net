    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SNShop.Models;
using PagedList;
using System.Dynamic;

namespace SNShop.Controllers
{
    public class HomeController : MyBaseController
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public ActionResult Index(int? page, string SearchString, string sortOrder)
        {
            var sanPham = new List<Product>();
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
            ViewBag.CurrentFilter = SearchString;
            try
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    sanPham = db.Products.Where(s => s.Name.Contains(SearchString)).ToList();
                }
            }
            catch (Exception) { }
            sanPham.OrderByDescending(v => v.Id);
            int pageSize = 12;
            ViewBag.PageSize = pageSize;
            ViewBag.Category = db.Categories.ToList();
            ViewBag.Check = 0;
            ViewBag.PresentImage = db.ProductImages.ToList();
            ViewBag.Banner = db.Banners.ToList();
            return View(sanPham.ToList().ToPagedList(page.Value, pageSize));
        }
        public ActionResult Detail(int id)
        {
            dynamic dynamicModel = new ExpandoObject();
            dynamicModel.ProductImages = db.Products.FirstOrDefault(s => s.Id == id).ProductImages.Where(s => s.ProductID == id);
            dynamicModel.Products = db.Products.FirstOrDefault(s => s.Id == id);
            return View(dynamicModel);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Comment_Rule()
        {
            return View();
        }
        [Route("404")]
        public ActionResult Pages_404()
        {
            Response.StatusCode = 404;
            return View();
        }
        [Route("500")]
        public ActionResult Pages_500()
        {
            Response.StatusCode = 500;
            return View();
        }
        public ActionResult OrdersReturn()
        {
            return View();
        }
    }
}