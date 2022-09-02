using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNShop.Controllers
{
    public class MyBaseController : Controller
    {
        // GET: MyBase
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            List<CartModel> carts = Session["Cart"] as List<CartModel>;
            if (carts == null)
            {
                ViewBag.Countproduct = 0;
                ViewBag.Total = 0;
            }
            else
            {
                ViewBag.Countproduct = carts.Count();
                ViewBag.Total = carts.Sum(s => s.Total);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}