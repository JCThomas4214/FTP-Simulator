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
            ViewBag.Message = "User Management Page";

            return View();
        }

        public ActionResult Content()
        {
            ViewBag.Message = "Content Management Page";

            return View();
        }

        public ActionResult Settings()
        {
            ViewBag.Message = "Settings Page.";

            return View();
        }
    }
}
