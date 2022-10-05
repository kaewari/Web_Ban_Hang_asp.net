using System.Net;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using SNShop.Models;
namespace SNShop.Controllers
{
    public class AddProvinceDistrictController : Controller
    {
        SNOnlineShopDataContext da = new SNOnlineShopDataContext();
        // GET: Province
        public ActionResult Index()
        {
            string path = "https://provinces.open-api.vn/api/?depth=2";
            object jsonObject = GetJson(path);
            ViewBag.data = jsonObject;

            for (int i = 1; i <= 63; i++)
            {
                Province p = new Province
                {
                    Id = i,
                    Name = ViewBag.data[i - 1].name,
                    Code = ViewBag.data[i - 1].code,
                };
                da.Provinces.InsertOnSubmit(p);

                da.SubmitChanges();
            }
            // GET: District
            int j = 1;
            for (int i = 1; i <= 63; i++)
            {
                foreach (var district in ViewBag.data[i - 1].districts)
                {

                    District d = new District
                    {
                        Id = j,
                        ProvinceID = i,
                        Name = district.name,
                        Code = district.code
                    };
                    j++;
                    da.Districts.InsertOnSubmit(d);
                    da.SubmitChanges();

                }
            }
            return View();
        }
        public object GetJson(string path)
        {
            using (WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 })
            {
                return JsonConvert.DeserializeObject<object>(
                    webClient.DownloadString(path)
                    );
            }
        }
    }
}