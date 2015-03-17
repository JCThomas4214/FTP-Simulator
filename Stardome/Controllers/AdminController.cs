using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
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
        private readonly ISiteSettingsService siteSettingsService;
        private readonly IRoleService roleService;

        public AdminController()
        {
            userAuthCredentialService = new UserAuthCredentialService(new UserAuthCredentialRepository(new StardomeEntitiesCS()));
            siteSettingsService = new SiteSettingsService(new SiteSettingsRepository(new StardomeEntitiesCS()));
            roleService = new RoleService(new RoleRepository(new StardomeEntitiesCS()));
        }

        public AdminController(IUserAuthCredentialService aUserAuthCredentialService, ISiteSettingsService aSiteSettingsService, IRoleService aRoleService)
        {
            userAuthCredentialService = aUserAuthCredentialService;
            siteSettingsService = aSiteSettingsService;
            roleService = aRoleService;
        }

        public ActionResult Users()
        {
            MainModel model = new MainModel
            {
                RoleId = GetUserId()
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            GetValue(Headers.Users);
            return View(model);
        }

        public ActionResult Content()
        {
            ContentModel model = new ContentModel
            {
                RootPath = GetMainPath(GetUserId()),
                RoleId = GetUserId()
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            GetValue(Headers.Content);
           
            return View(model);
        }

        public ActionResult Settings()
        {
            ViewBag.UpdateMessage = String.Empty;
            List<SiteSetting> list = siteSettingsService.GetAll().ToList();
            List<string> categories = list.Select(aSetting => aSetting.Category).Distinct().ToList();
            List<Tuple<string, string, string, int>> data = list.OrderBy(aSetting => aSetting.Name).Select(aSetting => new Tuple<string, string, string, int>(aSetting.Category, aSetting.Name, aSetting.Value, aSetting.Id)).ToList();
            
            SettingModel model = new SettingModel{
                RoleId = GetUserId(),
                Categories = categories,
                Settings = data
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            GetValue(Headers.Settings);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Settings(List<SiteSetting> lstSiteSettings)
        {
            ViewBag.UpdateMessage = "";
            if (ModelState.IsValid)
            {
                ViewBag.UpdateMessage = siteSettingsService.UpdateSiteSettings(lstSiteSettings);
            }

            var model = siteSettingsService.GetAll().ToList();
            GetValue(Headers.Settings);
            return View(model);
        }

        [HttpPost]
        public JsonResult GetUsers()
        {
            IList<User> users = new List<User>();
            IList<UserAuthCredential> userAuthCredentials = userAuthCredentialService.GetUserAuthCredentials().ToList();

            foreach (UserAuthCredential credential in userAuthCredentials)
            {
                UserInformation userInformation = credential.UserInformations.FirstOrDefault();
                if (userInformation != null && credential.Role.Role1 != Enums.Roles.InActive.ToString())
                {
                    users.Add(new User
                    {
                        Id = credential.Id,
                        EmailAddress = userInformation.Email,
                        FirstName = userInformation.FirstName,
                        LastName = userInformation.LastName,
                        RoleId = credential.Role.Id,
                        Username = credential.Username
                    });
                }
            }
            return Json(new { Result = "OK", Records = users, TotalRecordCount = users.Count });
        }

        [HttpPost]
        public JsonResult DeleteUser(User user)
        {
            try
            {
                user.RoleId = (int)Enums.Roles.InActive;
                UpdateUserRole(user);
                return Json(new { Result = "OK" });
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Unable to delete user" });
            }
        }

        [HttpPost]
        public JsonResult UpdateUser(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                UserAuthCredential oldUser = userAuthCredentialService.GetById(user.Id);
                oldUser.Username = user.Username;
                oldUser.UserInformations.First().FirstName = user.FirstName;
                oldUser.UserInformations.First().LastName = user.LastName;
                oldUser.UserInformations.First().Email = user.EmailAddress;
                oldUser.RoleId = user.RoleId;

                userAuthCredentialService.UpdateAUser(oldUser);
                return Json(new { Result = "OK" });
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Unable to update user" });
            }
        }

        [HttpPost]
        public JsonResult GetRoles()
        {
            var roles = roleService.GetRoles().Select(aRole => new { DisplayText = aRole.Role1, Value = aRole.Id});
            return Json(new { Result = "OK", Options = roles });
        }

        private int GetUserId()
        {
            int userId = userAuthCredentialService.GetByUsername(User.Identity.Name).RoleId;
            return userId;
        }

        public string GetMainPath(int roleId)
        {
            string path = String.Empty;
            switch (roleId)
            {
                case (int) Enums.Roles.Admin:
                    path = siteSettingsService.GetFilePath();
                    break;
                case (int) Enums.Roles.Producer:
                    // Falls through
                case (int) Enums.Roles.User:
                    path = siteSettingsService.GetFilePath();
                    break;
            }
            if (path.IsNullOrWhiteSpace())
            {
                return null;
            }
            path = path.Replace("\\", "/");
            if (path.EndsWith("/"))
            {
                return path;
            }
            return path + "/";
        }

        private void UpdateUserRole(User user)
        {
            UserAuthCredential oldUser = userAuthCredentialService.GetById(user.Id);
            oldUser.RoleId = user.RoleId;

            userAuthCredentialService.UpdateAUser(oldUser);
        }

        private void GetValue(String header)
        {
            ViewBag.Message = siteSettingsService.FindSiteSetting(header).Value;
        }
    }
}


