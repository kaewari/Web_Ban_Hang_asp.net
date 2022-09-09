using SNShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNShop.Models
{
    public class CartModel
    {
        private SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public long UnitPrice { get; set; }
        public long Quantity { get; set; }
        public decimal? Total { get { return UnitPrice * Quantity; } }
        public CartModel(int productID)
        {            
            ProductID = productID;
            Product p = db.Products.Single(n => n.Id == productID);
            ProductName = p.Name;
            UnitPrice = (long)p.Price;
            Quantity = 1;
        }
    }
}