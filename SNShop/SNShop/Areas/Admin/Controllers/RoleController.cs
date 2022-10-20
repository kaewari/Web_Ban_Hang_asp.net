using SNShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/Role
        [OutputCache(Duration = 900, Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult List_Roles(string error)
        {
            ViewData["loi"] = error;
            return View(db.Roles.ToList());
        }
        public ActionResult Create_Roles()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create_Roles(FormCollection formCollection, Role role)
        {
            if (string.IsNullOrEmpty(formCollection["Name"]) || string.IsNullOrWhiteSpace(formCollection["Name"]))
                ViewData["loi"] = "Bạn phải nhập tên role.";
            else
            {
                try
                {
                    var lastRoleId = db.Roles.OrderByDescending(s => s.Id).FirstOrDefault().Id;
                    role.Id = lastRoleId + 1;
                    role.Name = formCollection["Name"];
                    db.Roles.InsertOnSubmit(role);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    ViewData["loi"] = ex.Message;
                }
            }
            return View(role);
        }
        public ActionResult Edit_Roles(int id)
        {
            return View(db.Roles.Where(s=>s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit_Roles(FormCollection formCollection, Role role, int id)
        {
            var p = db.Roles.Where(s => s.Id == id).FirstOrDefault();
            if (p != null)
            {
                if (string.IsNullOrEmpty(formCollection["Name"]) || string.IsNullOrWhiteSpace(formCollection["Name"]))
                    ViewData["loi"] = "Bạn phải nhập tên role.";
                else
                {
                    role.Name = formCollection["Name"];
                    db.Roles.InsertOnSubmit(role);
                    db.SubmitChanges();
                }
            }
            return View(role);
        }
        public ActionResult Details_Roles(int id)
        {
            return View(db.Roles.Where(s => s.Id == id).FirstOrDefault());
        }
        public ActionResult Delete_Roles(int id)
        {
            string error = null;
            try
            {
                db.Roles.DeleteOnSubmit(db.Roles.FirstOrDefault(s => s.Id == id));
                db.SubmitChanges();
            }
            catch
            {
                error = "Bạn không thể xóa role này. Hãy thử xóa hết tất cả user thuộc role này và thử lại.";
            }
            return RedirectToAction("List_Roles", new {error = error });
        }
    }
}