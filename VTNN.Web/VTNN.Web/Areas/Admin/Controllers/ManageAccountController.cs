using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VTNN.DataAccess.Data;
using VTNN.Web.Commons;

namespace VTNN.Web.Areas.Admin.Controllers
{
    public class ManageAccountController : ProtectAdminController
    {
        ApplicationDBContext db = new ApplicationDBContext();
        // GET: Admin/Account
        public ActionResult Index()
        {
            var account = db.Users.OrderByDescending(a => a.UserId).ToList();
            return View(account);
        }
        public ActionResult Create()
        {
            List<int> roles = new List<int>
            {
                0,1
            };
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fullName = frm["FullName"];
                    string phone = frm["PhoneNumber"];
                    string email = frm["Email"];
                    string password = frm["Password"];
                    string address = frm["Address"];
                    string confirmPassword = frm["ConfirmPassword"];
                    string role = frm["Role"];
                    string isActive = frm["Is_Active"];
                    if (!password.Equals(confirmPassword))
                    {
                        ViewBag.Error = "Mật khẩu không khớp.";
                        return View();
                    }

                    var us = db.Users.Where(u => u.Email == email).SingleOrDefault();

                    if (us != null)
                    {
                        ViewBag.Error = "Vui lòng nhập địa chỉ email khác.";
                        return View();
                    }
                    User user = new User();
                    user.FullName = fullName;
                    user.PhoneNumber = phone;
                    user.Email = email;
                    user.Password = Helper.EncodePassword(password);
                    user.Address = address;
                    user.Role = role.Equals("Quản trị") ? 1 : 0;
                    user.Is_Active = isActive == null ? false : true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(user);
                    db.SaveChanges();
                    //TempData["Message"] = " Thêm tài khoản thành công.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ViewBag.Error = "Dữ liệu không hợp lệ!";
                return View();
            }
        }

        public ActionResult Update(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string email = frm["Email"];
                    string role = frm["Role"];
                    string is_active = frm["Is_Active"];
                    var user = db.Users.Where(u => u.Email == email).SingleOrDefault();

                    if (user == null)
                    {
                        ViewBag.Error = "tài khoản không tồn tại";
                        return View();
                    }
                    user.Role = role.Equals("Quản trị") ? 1 : 0;
                    user.Is_Active = is_active == null ? false : true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    //TempData["Message"] = " Cập nhật tài khoản thành công.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ!";
                return View();
            }


        }

        [HttpPost]
        public JsonResult Delete(int? id)
        {
            bool success = false;
            User currentUser = Session["admin"] as User;
            if (currentUser.UserId == id)
            {
                success = false;
                return Json(success, JsonRequestBehavior.AllowGet);
            }
            User user = db.Users.Find(id);
            try
            {
                db.Users.Remove(user);
                db.SaveChanges();
                success = true;

                return Json(success, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                success = false;
                return Json(success, JsonRequestBehavior.AllowGet);
            }

        }
    }
}