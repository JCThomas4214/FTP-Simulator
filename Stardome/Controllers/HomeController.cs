using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stardome.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Users()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Content()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Settings()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
