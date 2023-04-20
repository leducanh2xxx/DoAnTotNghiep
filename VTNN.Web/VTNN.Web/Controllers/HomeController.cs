using PagedList;
using System.Linq;
using System.Web.Mvc;
using VTNN.DataAccess.Data;

namespace VTNN.Web.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index(int? page)
        {
            var products = db.Products.OrderByDescending(p => p.ProductId);

            int pageNumber = (page ?? 1);


            return View(products.ToPagedList(pageNumber, 10));
        }
    }
}