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
        public ActionResult ProductOfCategory(int? page, int? pageSize, int Id)
        {
            if (page == null)
                page = 1;
            if (pageSize == null)
                pageSize = 5;
            ViewBag.PageSize = pageSize;
            List<Product> products = db.Products.Where(s => s.CategoryID == Id).ToList();
            Category category = db.Categories.Where(s => s.Id == Id).FirstOrDefault();
            ViewBag.Title = category.Name;
            return View(products.ToPagedList((int)page, (int)pageSize));
        }
    }
}