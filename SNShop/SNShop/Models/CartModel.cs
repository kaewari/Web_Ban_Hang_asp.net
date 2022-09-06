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
        public decimal? UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal? Total { get { return UnitPrice * Quantity; } }
        public int STT = 0;
        public CartModel(int productID)
        {            
            ProductID = productID;
            Product p = db.Products.Single(n => n.Id == productID);
            ProductName = p.Name;
            UnitPrice = p.Price;
            Quantity = 1;
        }
    }
}