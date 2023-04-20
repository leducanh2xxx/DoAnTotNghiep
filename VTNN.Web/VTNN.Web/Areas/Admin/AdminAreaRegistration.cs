using System.Web.Mvc;

namespace VTNN.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}",
                new { controller = "Manage", action = "Index" }
            );
            //GET: Admin
            context.MapRoute(
                "Manage",
                "Admin/{controller}",
                new { controller = "Manage", action = "Index" }
            );
            //GET: Admin/ManageCategory
            context.MapRoute(
                "ManageCategory",
                "Admin/{controller}/{action}",
                new { controller = "ManageCategory", action = "Index" }
            );

            //GET: Admin/ManageOrder
            context.MapRoute(
                "ManageOrder",
                "Admin/{controller}/{action}",
                new { controller = "ManageOrder", action = "Index" }
            );
            //GET: Admin/ManageProduct
            context.MapRoute(
                "ManageProduct",
                "Admin/{controller}/{action}",
                new { controller = "ManageProduct", action = "Index" }
            );
        }
    }
}