using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastException = Server.GetLastError();
            //log by reading lastException.Message

            string actionName = Request.Path;
            Error error = new Error();
            ErrorsController ec = new ErrorsController();
            error.errorValue = ec.LogError(User.Identity.Name, lastException, "", actionName);
            db.Errors.Add(error);
            db.SaveChanges();
            Server.ClearError();
            Response.Redirect("\\Error.html");
        }

    }
}
