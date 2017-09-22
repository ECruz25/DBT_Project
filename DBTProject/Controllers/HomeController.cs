using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBTProject.Controllers
{
    public class HomeController : Controller 
    {
        public ActionResult Index()
        {
            Session["ViewBag"] = null;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public User GetUser()
        {
            return Session["User"] as User;
        }
    }
}