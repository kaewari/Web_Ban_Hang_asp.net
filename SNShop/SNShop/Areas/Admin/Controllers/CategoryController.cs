using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNShop.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        // GET: Admin/Category
        public ActionResult List_Categories(string error)
        {
            var p = db.Categories.Select(s => s).ToList();
            ViewData["loi"] = error;
            return View(p);
        }
        public ActionResult Create_Categories()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create_Categories(FormCollection formCollection, Category category)
        {
            if (string.IsNullOrEmpty(formCollection["Name"]) || string.IsNullOrWhiteSpace(formCollection["Name"]))
            {
                ViewData["Loi"] = "Bạn vui lòng nhập tên danh mục";
            }
            else
            {
                try
                {
                    category.Name = formCollection["Name"];
                    category.ModifiedDate = DateTime.Now;
                    db.Categories.InsertOnSubmit(category);
                    db.SubmitChanges();
                    return RedirectToAction("List_Categories");
                }
                catch (Exception ex)
                {
                    ViewData["loi"] = ex.Message;
                }
            }
            return View(category);
        }
        public ActionResult Edit_Categories(int id)
        {
            var p = db.Categories.FirstOrDefault(s => s.Id == id);
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit_Categories(FormCollection formCollection, int id)
        {
            var p = db.Categories.FirstOrDefault(s => s.Id == id);
            if (string.IsNullOrEmpty(formCollection["Name"]) || !string.IsNullOrWhiteSpace(formCollection["Name"]))
            {              
                ViewData["loi"] = "Bạn vui lòng nhập tên danh mục.";
            }
            else
            {
                try
                {
                    p.Name = formCollection["Name"];
                    p.ModifiedDate = DateTime.Now;
                    UpdateModel(p);
                    db.SubmitChanges();
                    return RedirectToAction("List_Categories");
                }
                catch (Exception ex)
                {
                    ViewData["loi"] = ex.Message;
                }
            }          
            return View(p);
        }
        public ActionResult Details_Categories(int id)
        {
            var p = db.Categories.FirstOrDefault(s => s.Id == id);
            return View(p);
        }
        public ActionResult Delete_Categories(int id)
        {
            string error = null;
            try
            {
                var p = db.Categories.FirstOrDefault(s => s.Id == id);
                db.Categories.DeleteOnSubmit(p);
                db.SubmitChanges();
            }
            catch
            {
                error = "Bạn không thể xóa danh mục này. Hãy thử xóa tất cả danh mục con, sản phẩm của danh mục này và thử lại.";
            }
            return RedirectToAction("List_Categories", new {error = error});
        }
    }
}