using System.Linq;

namespace SNShop.Models
{
    public class CartModel
    {
        private SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitsInStock { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Total { get { return UnitPrice * Quantity; } }
        public CartModel(int productID)
        {            
            ProductID = productID;
            Product p = db.Products.Single(n => n.Id == productID);
            ProductName = p.Name;
            UnitPrice = (long)p.Price;
            UnitsInStock = p.UnitsInStock;
            Quantity = 1;
        }
    }
}