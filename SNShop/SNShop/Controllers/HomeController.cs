using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SNShop.Models;
namespace SNShop.Controllers
{
    public class HomeController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public ActionResult Index()
        {
            List<Product> products = db.Products.Select(s => s).ToList();
            return View(products);
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
    }
}