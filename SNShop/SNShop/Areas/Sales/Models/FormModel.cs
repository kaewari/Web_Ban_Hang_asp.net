using SNShop.Models;
using System.Linq;

namespace SNShop.Areas.Sales.Models
{
    public class FormModel
    {
        private SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public int productID { get; set; }
        public string name { get; set; }
        public decimal? unitPrice { get; set; }
        public decimal? quantity { get; set; }
        public decimal? total { get { return unitPrice * quantity; } }
        public FormModel(int productID)
        {
            this.productID = productID;
            Product p = db.Products.Single(n => n.Id == productID);
            name = p.Name;
            unitPrice = (decimal)p.Price;
            quantity = 1;
        }
    }
}