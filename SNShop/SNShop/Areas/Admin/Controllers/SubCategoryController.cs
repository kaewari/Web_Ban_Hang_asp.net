using SNShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class SubCategoryController : Controller
    {
        // GET: Admin/SubCategory
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/Category
        [OutputCache(Duration = 900, Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult List_SubCategories(string error)
        {
            var p = db.SubCategories.Select(s => s).ToList();
            ViewData["loi"] = error;
            return View(p);
        }
        public ActionResult Create_SubCategories()
        {
            ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create_SubCategories(FormCollection formCollection, SubCategory subCategory)
        {
            if (string.IsNullOrEmpty(formCollection["Name"]) || string.IsNullOrWhiteSpace(formCollection["Name"]))
            {
                ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
                ViewData["Loi"] = "Bạn vui lòng nhập tên danh mục";
            }
            else
            {
                subCategory.CategoryID = int.Parse(formCollection["LSP"]);
                subCategory.Name = formCollection["Name"];
                subCategory.ModifiedDate = DateTime.Now;
                db.SubCategories.InsertOnSubmit(subCategory);
                db.SubmitChanges();
                return RedirectToAction("List_SubCategories");
            }
            return View(subCategory);
        }
        public ActionResult Edit_SubCategories(int id)
        {
            var p = db.SubCategories.FirstOrDefault(s => s.Id == id);
            ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit_SubCategories(FormCollection formCollection, int id)
        {
            var p = db.SubCategories.FirstOrDefault(s => s.Id == id);
            if (string.IsNullOrEmpty(formCollection["Name"]) || string.IsNullOrWhiteSpace(formCollection["Name"]))
            {
                ViewData["LSP"] = new SelectList(db.Categories, "Id", "Name");
                ViewData["Loi"] = "Bạn vui lòng nhập tên danh mục";
            }
            else
            {
                p.CategoryID = int.Parse(formCollection["LSP"]);
                p.Name = formCollection["Name"];
                p.ModifiedDate = DateTime.Now;
                UpdateModel(p);
                db.SubmitChanges();
                return RedirectToAction("List_SubCategories");
            }
            return View(p);
        }
        public ActionResult Details_SubCategories(int id)
        {
            var p = db.SubCategories.Where(s => s.Id == id).FirstOrDefault();
            return View(p);
        }
        public ActionResult Delete_SubCategories(int id)
        {
            try
            {
                var p = db.SubCategories.Where(s => s.Id == id).FirstOrDefault();
                db.SubCategories.DeleteOnSubmit(p);
                db.SubmitChanges();
            }
            catch
            {
                ViewData["loi"] = "Bạn không thể xóa danh mục này.";
            }
            return RedirectToAction("List_SubCategories", new { error = ViewData["loi"] });
        }
    }
}