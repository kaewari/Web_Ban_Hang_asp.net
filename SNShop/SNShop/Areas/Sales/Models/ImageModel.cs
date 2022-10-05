using System.Web;

namespace SNShop.Areas.Sales.Models
{
    public class ImageModel
    {
        public string Path { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}