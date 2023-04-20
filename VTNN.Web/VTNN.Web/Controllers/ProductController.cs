using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTNN.DataAccess.Data;

namespace VTNN.Web.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        // GET: Product
        public ActionResult Detail(int id)
        {
            if (id == null)
            {
                return Redirect("/NotFound/Index");
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return Redirect("/NotFound/Index");
            }
            return View(product);
        }
        public ActionResult Search(string keyword, string order, decimal? fromPrice, decimal? toPrice, string category, int page = 1)
        {
            if (keyword == null)
            {
                keyword = "";
            }
            var products = db.Products.Where(p => p.ProductName.Contains(keyword)).OrderByDescending(p => p.ProductId);

            if (order != null)
                switch (order)
                {
                    case "desc":
                        products = products.OrderByDescending(p => p.Price);
                        ViewBag.order = "desc";
                        break;
                    case "asc":
                        products = products.OrderBy(p => p.Price);
                        ViewBag.order = "asc";
                        break;
                    default:
                        ViewBag.order = "default";
                        break;
                }

            if (fromPrice != null)
            {
                ViewBag.from = fromPrice;
                products = (IOrderedQueryable<Product>)products.Where(p => p.Price >= fromPrice);
            }

            if (toPrice != null)
            {
                ViewBag.to = toPrice;
                products = (IOrderedQueryable<Product>)products.Where(p => p.Price <= toPrice);
            }

            if (category != null)
            {
                string[] ids = category.Split(',');
                ViewBag.category = category;
                products = (IOrderedQueryable<Product>)products.Where(p => ids.Contains(p.CategoryId.ToString()));
            }

            ViewBag.keyword = keyword;
            ViewBag.Categories = db.Categories.OrderBy(c => c.CategoryId).ToList();
            return View(products.ToPagedList(page, 12));
        }
        public JsonResult Index(int id)
        {
            if (id == null)
            {
                return Json("NOT_FOUND", JsonRequestBehavior.AllowGet);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return Json("NOT_FOUND", JsonRequestBehavior.AllowGet);
            }
            return Json(
                new
                {
                    id = product.ProductId,
                    name = product.ProductName,
                    price = product.Price,
                    quantity = product.Amount,
                    image = product.Image
                },
                JsonRequestBehavior.AllowGet
                );
        }
        public JsonResult List(string ids)
        {
            if (ids == null)
            {
                return Json("NOT_FOUND", JsonRequestBehavior.AllowGet);
            }
            string[] listId = ids.Split(',');
            var product = db.Products.Where(p => listId.Contains(p.ProductId.ToString())).ToList();
            if (product == null)
            {
                return Json("NOT_FOUND", JsonRequestBehavior.AllowGet);
            }

            List<object> list = new List<object>();

            foreach (var item in product)
            {
                list.Add(new
                {
                    id = item.ProductId,
                    name = item.ProductName,
                    image = item.Image,
                    price = item.Price,
                    quantity = item.Amount
                });
            }

            return Json(
                list,
                JsonRequestBehavior.AllowGet
                );
        }
    }
}