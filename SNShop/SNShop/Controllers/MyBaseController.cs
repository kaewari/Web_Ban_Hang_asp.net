using SNShop.Common;
using SNShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SNShop.Controllers
{
    public class MyBaseController : Controller
    {
        // GET: MyBase
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {          
            List<CartModel> carts = Session[Constants.CART_SESSION] as List<CartModel>;

            if (carts == null)
            {
                ViewBag.Countproduct = 0;
                ViewBag.Total = 0;
            }
            else
            {
                ViewBag.Countproduct = carts.Sum(s => s.Quantity);
                ViewBag.Total = (decimal)carts.Sum(s => s.Total);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}