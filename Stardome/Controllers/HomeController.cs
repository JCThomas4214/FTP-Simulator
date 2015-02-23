using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Stardome.DomainObjects;
using Stardome.Models;
using Stardome.Repositories;
using Stardome.Services.Domain;
using System.Collections.Generic;

namespace Stardome.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserAuthCredentialService userAuthCredentialService;
        private readonly RoleService roleService;
        private readonly SiteSettingsService siteSettingsService;

        public HomeController()
        {
            userAuthCredentialService = new UserAuthCredentialService(new UserAuthCredentialRepository(new StardomeEntitiesCS()));
            roleService = new RoleService(new RoleRepository(new StardomeEntitiesCS()));
            siteSettingsService = new SiteSettingsService(new SiteSettingsRepository(new StardomeEntitiesCS()));
        }

        public ActionResult Users()
        {
            //var model = new UserManagement
            //{
            //    UserList = userAuthCredentialService.GetUserAuthCredentials().ToList(),
            //    Roles = roleService.GetRoles().ToList()
            //};
            ViewBag.showAdminMenu = true;
            ViewBag.Message = "User Management Page";

            return View();
        }

        public ActionResult Content()
        {
            ViewBag.showAdminMenu = true;
            ViewBag.Message = "Content Management Page";

            return View();
        }

        public ActionResult Settings()
        {
            ViewBag.UpdateMessage = "";
            var model = siteSettingsService.GetAll().ToList();
            ViewBag.showAdminMenu = true;
            ViewBag.Message = "Settings Page.";

            return View(model);
        }

        [HttpPost]
        public ActionResult Settings(List<Stardome.DomainObjects.SiteSetting> lstSiteSettings)
        {
            ViewBag.UpdateMessage = "";
            if (ModelState.IsValid)
            {
               ViewBag.UpdateMessage= siteSettingsService.UpdateSiteSettings(lstSiteSettings);
            }

            var model = siteSettingsService.GetAll().ToList();
            ViewBag.showAdminMenu = true;
            ViewBag.Message = "Settings Page.";
            return View(model);
        }

        [HttpPost]
        public JsonResult GetUsers()
        {
            IList<User> users = new List<User>();
            IList<UserAuthCredential> userAuthCredentials = userAuthCredentialService.GetUserAuthCredentials().ToList();
            
            foreach (UserAuthCredential credential in userAuthCredentials)
            {
                UserInformation userInformation = credential.UserInformations.First();
                users.Add(new User
                {
                    Id = credential.Id,
                    EmailAddress = userInformation.Email,
                    Name = userInformation.FirstName + " " + userInformation.LastName,
                    Role = credential.Role.Role1,
                    Username = credential.Username
                });
            }

            return Json(new { Result = "OK", Records = users, TotalRecordCount = users.Count });
        }
    }
}


