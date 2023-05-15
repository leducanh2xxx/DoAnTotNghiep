using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTNN.DataAccess.Data;
using VTNN.Web.Commons;

namespace VTNN.Web.Controllers
{
    public class ProfileController : ProtectController
    {
        ApplicationDBContext db = new ApplicationDBContext();
        // GET: Profile
        public ActionResult Index()
        {
            User oldUser = Session["user"] as User;
            User newUser = db.Users.Where(us => us.Email.Equals(oldUser.Email)).FirstOrDefault();
            Session["user"] = newUser;
            var errMsg = TempData["ErrorMessage"] as string;
            ViewBag.Infor = errMsg;
            return View(newUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(FormCollection frm)
        {
            string fullname = frm["FullName"];
            string phone = frm["PhoneNumber"];
            string email = frm["Email"];
            string address = frm["Address"];

            User user = db.Users.Where(us => us.Email == email).SingleOrDefault();
            user.FullName = fullname;
            user.PhoneNumber = phone;
            user.Address = address;
            if (user != null)
            {
                db.Entry(user).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                ViewBag.Information = "Cập nhật thành công";
            }
            else
            {
                ViewBag.Information = "Có lỗi xảy ra khi cập nhật";
            }

            return View("Index", user);
        }

        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                User user = Session["user"] as User;
                string old_password = frm["old_password"];
                string new_password = frm["password"];
                string confirm_password = frm["confirm_password"];
                if (!Helper.EncodePassword(old_password).Equals(user.Password))
                {
                    ViewBag.Error = "Mật khẩu cũ không đúng!";
                    return View(frm);
                }
                User userEntity = db.Users.Where(us => us.Email == user.Email).SingleOrDefault();
                userEntity.Password = Helper.EncodePassword(new_password);
                db.Entry(userEntity).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                ViewBag.Information = "Đổi mật khẩu thành công";

                return View("Index", userEntity);

            }
            ViewBag.Error = "Chưa validate!";
            return View();
        }

        [NotAuthorize]
        public ActionResult Logout()
        {
            Session["user"] = null;
            return Redirect("/");
        }
        [NotAuthorize]
        public ActionResult Login()
        {
            if (Session["user"] == null)
            {
                return View();
            }
            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NotAuthorize]
        public ActionResult Login(FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                string email = frm["email"];
                string password = frm["password"];

                string currentPass = Helper.EncodePassword(password);
                var user = db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(currentPass)).SingleOrDefault();
                if (user != null && !user.Is_Active)
                {
                    ViewBag.error = "Tài khoản của bạn đã bị khoá";
                    return View();
                }
                else if (user != null && user.Is_Active)
                {
                    //add session
                    Session["user"] = user;
                    return Redirect("/");
                }
                else
                {
                    ViewBag.error = "Đăng nhập thất bại";
                    return View();
                }
            }
            ViewBag.error = "Đăng nhập thất bại";
            return View();
        }
        [NotAuthorize]
        public ActionResult Register()
        {
            if (Session["user"] == null)
            {
                return View();
            }
            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NotAuthorize]
        public ActionResult Register(FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fullName = frm["FullName"];
                    string email = frm["Email"];
                    string password = frm["Password"];
                    string confirmPassword = frm["ConfirmPassword"];
                    string phone = frm["PhoneNumber"];
                    string address = frm["Address"];

                    if (!password.Equals(confirmPassword))
                    {
                        ViewBag.Error = "Mật khẩu không khớp.";
                        return View();
                    }

                    var user = db.Users.Where(us => us.Email == email).SingleOrDefault();

                    if (user != null)
                    {
                        ViewBag.Error = "Vui lòng nhập địa chỉ email khác.";
                        return View();
                    }

                    User u = new User();
                    u.FullName = fullName;
                    u.Email = email;
                    u.Password = Helper.EncodePassword(password);
                    u.Address = address;
                    u.PhoneNumber = phone;
                    u.Role = 0;
                    u.Is_Active = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(u);
                    db.SaveChanges();
                }
                return Redirect("/Profile/Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi dữ liệu " + ex;
                return View();
            }
        }
    }
}