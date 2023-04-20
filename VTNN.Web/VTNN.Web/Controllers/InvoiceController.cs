using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using VTNN.DataAccess.Data;

namespace VTNN.Web.Controllers
{
    public class InvoiceController : ProtectController
    {
        ApplicationDBContext db = new ApplicationDBContext();
        // GET: Invoice
        public ActionResult Index(int id)
        {
            var order = db.Orders.Where(o => o.OrderId == id && o.UserId == user.UserId).SingleOrDefault();
            if (order == null)
            {
                return Redirect("/NotFound/Index");
            }
            var quantity = db.OrderDetails.Select(od => od)
                .Where(od => od.OrderId == id)
                .Sum(od => od.Quantity);
            var price = db.OrderDetails.Select(od => od)
                .Where(od => od.OrderId == id)
                .Sum(od => od.Quantity * od.Price);
            var detail = db.OrderDetails.Where(od => od.OrderId == id).ToList();

            ViewBag.CreatedAt = order.Created_At;
            ViewBag.Address = user.Address;
            ViewBag.OrderId = id;
            ViewBag.Quantity = quantity;
            ViewBag.Price = price;

            return View(detail);
        }
        [HttpPost]
        public ActionResult Index(FormCollection frm)
        {
            string ids = frm["ids"];
            if (user.Address == null || user.PhoneNumber == null ||
                user.FullName == null || user.Email == null)
            {
                TempData["ErrorMessage"] = "Vui lòng cập nhật đầy đủ thông tin trước khi thanh toán!";
                return RedirectToAction("Index", "Profile");
            }

            string[] listId = ids.Split(',');
            List<int> idsInt = new List<int>();
            for (int i = 0; i < listId.Length; i++)
            {
                idsInt.Add(int.Parse(listId[i]));
            }

            Dictionary<int, int> dic = new Dictionary<int, int>();

            for (int i = 0; i < idsInt.Count; i++)
            {
                int value = 1;
                if (dic.ContainsKey(idsInt[i]))
                {
                    value = dic[idsInt[i]];
                    dic[idsInt[i]] = value + 1;
                }
                else
                {
                    dic.Add(idsInt[i], value);
                }

            }

            var products = db.Products.Where(p => idsInt.Contains(p.ProductId)).ToList();
            int uid = user.UserId;

            DbContextTransaction transaction = db.Database.BeginTransaction();

            try
            {
                Order order = new Order();

                order.UserId = uid;
                order.Status = 1;
                db.Orders.Add(order);

                for (int i = 0; i < idsInt.Count; i++)
                {
                    if (dic.ContainsKey(idsInt[i]))
                    {
                        OrderDetail od = new OrderDetail();
                        int value = dic[idsInt[i]];
                        od.OrderId = order.OrderId;
                        od.ProductId = idsInt[i];
                        od.Quantity = value;
                        Product product = products.Find(p => p.ProductId == idsInt[i]);

                        if (product.Amount < value)
                        {
                            transaction.Rollback();
                            TempData["ErrorMessage"] = "Số lượng sản phẩm " + product.ProductName + " không hợp lệ";
                            return RedirectToAction("Index", "Checkout");
                        }

                        product.Amount = product.Amount - value;
                        od.Price = product.Price;
                        db.OrderDetails.Add(od);
                        dic.Remove(idsInt[i]);
                    }
                }

                db.SaveChanges();

                transaction.Commit();
                return RedirectToAction("Index", "Invoice", new { id = order.OrderId });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["ErrorMessage"] = "Lỗi khi thanh toán " + ex.Message;
                return RedirectToAction("Index", "Checkout");
            }
        }
    }
}