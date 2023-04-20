using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTNN.DataAccess.Data;

namespace VTNN.Web.Areas.Admin.Controllers
{
    public class ManageOrderController : ProtectAdminController
    {
        ApplicationDBContext db = new ApplicationDBContext();
        static List<CustomOrder> list;

        // GET: Admin/ManageOrder
        public ActionResult Index()
        {
            var order = db.Orders.Join(db.Users, o => o.UserId, u => u.UserId, (o, u) => new
            {
                order = o,
                user = u
            }).Select(o => new
            {
                order_id = o.order.OrderId,
                user_id = o.user.FullName,
                status = o.order.Status,
                created_at = o.order.Created_At
            }).Join(db.OrderDetails, x => x.order_id, od => od.OrderId, (x, od) => new
            {
                ele = x,
                order_detail = od
            }).GroupBy(x => new
            {
                order_id = x.ele.order_id,
                user_id = x.ele.user_id,
                status = x.ele.status,
                created_at = x.ele.created_at
            })
            .Select(e => new
            {
                order_id = e.Key.order_id,
                user_id = e.Key.user_id,
                status = e.Key.status,
                created_at = e.Key.created_at,
                amount = e.Sum(v => v.order_detail.Quantity * v.order_detail.Price)
            }).OrderByDescending(o => o.order_id).ToList();

            List<string> status = new List<string>
            {
                "Đã giao",  "Đang giao",  "Đã huỷ"
            };
            ViewBag.Status = status;
            list = new List<CustomOrder>();
            foreach (var item in order)
            {
                CustomOrder c = new CustomOrder();
                c.OrderId = item.order_id;
                c.UserId = item.user_id;
                if (item.status == 1)
                {
                    c.Status = "Đang giao";
                }
                else if (item.status == 2)
                {
                    c.Status = "Đã giao";
                }
                else if (item.status == 3)
                {
                    c.Status = "Đã huỷ";
                }
                c.Amount = decimal.Parse(item.amount.ToString());
                c.Created_At = DateTime.Parse(item.created_at.ToString());

                list.Add(c);
            }
            return View(list);
        }
        // GET: Admin/ManageOrder/id
        //public ActionResult Details()
        //{
        //    return View();
        //}

        public ActionResult Details(int id)
        {
            Order order = db.Orders.Find(id);
            User user = db.Users.Find(order.UserId);
            List<OrderDetail> productsInOrder = db.OrderDetails.Where(p => p.OrderId == id).ToList();
            List<Product> listProduct = new List<Product>();
            foreach (var item in productsInOrder)
            {
                Product pr = db.Products.Where(p => p.ProductId == item.ProductId).SingleOrDefault();
                listProduct.Add(pr);
            }
            ViewBag.User = user;
            ViewBag.Order = order;
            if (order.Status == 1)
            {
                ViewBag.Status = "Đang giao";
            }
            else if (order.Status == 2)
            {
                ViewBag.Status = "Đã giao";
            }
            else if (order.Status == 3)
            {
                ViewBag.Status = "Đã huỷ";
            }
            ViewBag.Products = productsInOrder;
            ViewBag.ProductInfo = listProduct;
            foreach (var item in list)
            {
                if (item.OrderId == id)
                {
                    ViewBag.Total = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", item.Amount) + " vnđ";
                }
            }

            return View();
        }

        [HttpPost]
        public JsonResult Update(int? id, FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int status = int.Parse(frm["status"]);

                    Order order = db.Orders.Find(id);
                    if (order == null)
                    {
                        return Json("NOT_FOUND", JsonRequestBehavior.AllowGet);
                    }

                    order.Status = status;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
                return Json("NOT_FOUND", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("NOT_FOUND", JsonRequestBehavior.AllowGet);
            }
        }
    }
}