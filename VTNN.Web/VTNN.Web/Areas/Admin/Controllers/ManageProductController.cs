using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VTNN.DataAccess.Data;

namespace VTNN.Web.Areas.Admin.Controllers
{
    public class ManageProductController : ProtectAdminController
    {
        ApplicationDBContext db = new ApplicationDBContext();

        // GET: Admin/ManageProduct
        public ActionResult Index(string searchString)
        {
            var products = db.Products.Select(p => p);
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }

            return View(products.OrderByDescending(p => p.ProductId).ToList());
        }

        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/ManageProduct/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.Image = "";
                    var f = Request.Files["product_image"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/product/images/" + FileName);
                        f.SaveAs(UploadPath);
                        product.Image = FileName;
                    }
                    db.Products.Add(product);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", product.CategoryId);
                ViewBag.Error = "Dữ liệu không hợp lệ!";
                return View(product);
            }
        }

        // GET: Admin/ManageProduct/Update
        public ActionResult Update(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", product.CategoryId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f = Request.Files["product_image"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/product/images/" + FileName);
                        f.SaveAs(UploadPath);
                        product.Image = FileName;
                    }
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ!";
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", product.CategoryId);
                return View(product);
            }


        }
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            Product product = db.Products.Find(id);
            try
            {
                db.Products.Remove(product);
                db.SaveChanges();
                bool success = true;

                return Json(success, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                bool success = false;
                return Json(success, JsonRequestBehavior.AllowGet);
            }

        }
    }
}