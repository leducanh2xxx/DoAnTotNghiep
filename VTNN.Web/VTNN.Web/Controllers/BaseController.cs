using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTNN.DataAccess.Data;

namespace VTNN.Web.Controllers
{
    public class BaseController : ProtectController
    {
        ApplicationDBContext db = new ApplicationDBContext();
        [NotAuthorize]
        public PartialViewResult _Header()
        {
            return PartialView(user);
        }
        [NotAuthorize]
        public PartialViewResult _MenuPC()
        {
            var categories = db.Categories.ToList();
            return PartialView(categories);
        }
        [NotAuthorize]
        public PartialViewResult _MenuMobile()
        {
            ViewBag.User = user.FullName;
            var categories = db.Categories.ToList();
            return PartialView(categories);
        }
        [NotAuthorize]
        public PartialViewResult _BreadcrumbLevelOne(string id)
        {
            var category = db.Categories.Where(c => c.CategoryId.ToString() == id).SingleOrDefault();

            return PartialView(category);
        }
        [NotAuthorize]
        public PartialViewResult _BreadcrumbLevelTwo(string id)
        {
            var product = db.Products.Where(c => c.ProductId.ToString() == id).SingleOrDefault();

            return PartialView(product);
        }
        public PartialViewResult _ListOrder()
        {
            var order = db.Orders.Join(db.Users, o => o.UserId, u => u.UserId, (o, u) => new
            {
                order = o,
                user = u
            }).Where(u => u.user.UserId == user.UserId)
            .Select(o => new
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
            }).OrderByDescending(o => o.created_at).ToList();

            List<CustomOrder> list = new List<CustomOrder>();
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

            return PartialView(list);
        }
    }
}