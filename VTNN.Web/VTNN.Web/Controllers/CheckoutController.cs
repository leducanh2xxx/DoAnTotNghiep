using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTNN.DataAccess.Data;

namespace VTNN.Web.Controllers
{
    public class CheckoutController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        // GET: Checkout
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
    }
}