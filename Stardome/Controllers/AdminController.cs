using System.Linq;
using System.Web.Mvc;
using Stardome.DomainObjects;
using Stardome.Models;
using Stardome.Repositories;
using Stardome.Services.Domain;
using System.Collections.Generic;

namespace Stardome.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserAuthCredentialService userAuthCredentialService;
        private readonly IRoleService roleService;
        private readonly SiteSettingsService siteSettingsService;

        public AdminController()
        {
            userAuthCredentialService = new UserAuthCredentialService(new UserAuthCredentialRepository(new StardomeEntitiesCS()));
            roleService = new RoleService(new RoleRepository(new StardomeEntitiesCS()));
            siteSettingsService = new SiteSettingsService(new SiteSettingsRepository(new StardomeEntitiesCS()));
        }

        //public HomeController(IUserAuthCredentialService a)
        //{
        //    userAuthCredentialService = new UserAuthCredentialService(new UserAuthCredentialRepository(new StardomeEntitiesCS()));
        //    roleService = new RoleService(new RoleRepository(new StardomeEntitiesCS()));
        //    siteSettingsService = new SiteSettingsService(new SiteSettingsRepository(new StardomeEntitiesCS()));
        //}
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
                ViewBag.UpdateMessage = siteSettingsService.UpdateSiteSettings(lstSiteSettings);
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

        /*[HttpPost]
        public JsonResult DeleteUser(int userId)
        {

        }
        */
        /*
         *     [HttpPost]
    public JsonResult DeleteStudent(int studentId)
    {
        try
        {
            _repository.StudentRepository.DeleteStudent(studentId);
            return Json(new { Result = "OK" });
        }
        catch (Exception ex)
        {
            return Json(new { Result = "ERROR", Message = ex.Message });
        }
    }
         * */
    }
}


