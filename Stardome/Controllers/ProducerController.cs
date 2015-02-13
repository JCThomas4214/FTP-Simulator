using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stardome.Controllers
{
    public class ProducerController : Controller
    {
        //
        // GET: /Producer/

        public ActionResult Index()
        {
            ViewBag.showAdminMenu = false;
            return View();
        }

    }
}
