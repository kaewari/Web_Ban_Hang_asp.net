using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SNShop.Models;
using PagedList;

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
            int pageSize = 21;
            ViewBag.PageSize = pageSize;
            ViewBag.Category = db.Categories.ToList();
            ViewBag.Check = 0;
            ViewBag.PresentImage = db.ProductImages.ToList();
            return View(sanPham.ToList().ToPagedList(page.Value, pageSize));
        }
        public ActionResult Detail(int id)
        {
            Product p = db.Products.FirstOrDefault(s => s.Id == id);
            ViewBag.AllImageOfProduct = p.ProductImages.Where(s => s.ProductID == id);
            return View(p);
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
        public ActionResult AddImage()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddImage(ProductImage product_Image)
        {

            db.ProductImages.InsertOnSubmit(product_Image);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [Route("404")]
        public ActionResult Pages_404()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View();
        }
        [Route("500")]
        public ActionResult Pages_500()
        {
            Response.StatusCode = 500;  //you may want to set this to 200
            return View();
        }
    }
}