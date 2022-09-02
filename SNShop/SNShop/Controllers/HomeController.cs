using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SNShop.Models;
using PagedList;
namespace SNShop.Controllers
{
    public class HomeController : MyBaseController
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public ActionResult Index(int? page, int? pageSize, string SearchString = "")
        {
            if (page == null)
                page = 1;
            if (pageSize == null)
                pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Category = db.Categories.Select(s => s).ToList();
            var sanPham = from s in db.Products
                          select s;
            if (SearchString != "")
            {
                sanPham = sanPham.Where(x => x.Name.ToUpper().Contains(SearchString.ToUpper()));
            }
            return View(sanPham.ToList().ToPagedList((int)page, (int)pageSize));
        }
        public ActionResult Detail(int id)
        {
            Product p = db.Products.FirstOrDefault(s => s.Id == id);
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
        public ActionResult AddImage(Product_Image product_Image)
        {

            db.Product_Images.InsertOnSubmit(product_Image);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}