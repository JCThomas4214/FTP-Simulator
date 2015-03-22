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

        // Admin/Users
        public ActionResult Users()
        {
            // Get RoleId to check in the view that only the admin has access
            MainModel model = new MainModel
            {
                RoleId = GetUserRoleId(User.Identity.Name)
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            GetValue(Headers.Users);
            return View(model);
        }

        // Admin/Content
        public ActionResult Content()
        {
            // Get RoleId and RootPath which is configurable based on the Role
            ContentModel model = new ContentModel
            {
                RootPath = GetMainPath(GetUserRoleId(User.Identity.Name)),
                RoleId = GetUserRoleId(User.Identity.Name)
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            GetValue(Headers.Content);
           
            return View(model);
        }

        // Admin/Settings
        public ActionResult Settings()
        {
            ViewBag.UpdateMessage = String.Empty;
            // Gets the Site Settings 
            SettingModel model = SettingsHelper();

            return View(model);
        }

        // Post: Admin/Settings
        // Updates the site settings
        [HttpPost]
        public ActionResult Settings(SettingModel settingModel)
        {
            ViewBag.UpdateMessage = String.Empty;
            if (ModelState.IsValid)
            {
                // Updates the settings and returns a success or failure message
                ViewBag.UpdateMessage = siteSettingsService.UpdateSiteSettings(settingModel.Settings);
            }

            // Grabs the siteSettings
            SettingModel model = SettingsHelper();
            return View(model);
        }

        // Gets all the users that are not InActive and contain userInformation 
        [HttpPost]
        public JsonResult GetUsers()
        {
            IList<User> users = new List<User>();
            IList<UserAuthCredential> userAuthCredentials = userAuthCredentialService.GetUserAuthCredentials().ToList();

            // Produces the User objects that will get inserted in the json
            foreach (UserAuthCredential credential in userAuthCredentials)
            {
                UserInformation userInformation = credential.UserInformations.FirstOrDefault();
                if (userInformation != null && credential.Role.Id != (int)Enums.Roles.InActive)
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

        // By Deleting the User it makes the User InActive so that the user is still stored in the database
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

        // Update User by allowing to edit the Firstname, Lastname, Email, and Role
        [HttpPost]
        public JsonResult UpdateUser(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Access was not granted." });
                }
                UserAuthCredential oldUser = userAuthCredentialService.GetById(user.Id);
                if (oldUser == null || oldUser.UserInformations.FirstOrDefault() == null)
                {
                    throw new Exception();
                }

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

        // Gets all the Roles that are found in the database and converts to json object
        [HttpPost]
        public JsonResult GetRoles()
        {
            var roles = roleService.GetRoles().Select(aRole => new { DisplayText = aRole.Role1, Value = aRole.Id});
            return Json(new { Result = "OK", Options = roles });
        }

        // Gets the Role id based on the username
        public int GetUserRoleId(string username)
        {
            int roleId = userAuthCredentialService.GetByUsername(username).RoleId;
            return roleId;
        }

        // Gets the Path based on the Site Setting value
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

        // Updates only the Role Id for the specific UserAuthCredential
        private void UpdateUserRole(User user)
        {
            UserAuthCredential oldUser = userAuthCredentialService.GetById(user.Id);
            oldUser.RoleId = user.RoleId;

            userAuthCredentialService.UpdateAUser(oldUser);
        }

        // Gets the value of the Site Setting parameter
        public void GetValue(String header)
        {
            ViewBag.Message = siteSettingsService.FindSiteSetting(header).Value;
        }

        // Orders the SiteSetting by category and list each SiteSetting
        private SettingModel SettingsHelper()
        {
            List<SiteSetting> list = siteSettingsService.GetAll().OrderBy(aSettings => aSettings.Category).ToList();
            
            SettingModel model = new SettingModel
            {
                RoleId = GetUserRoleId(User.Identity.Name),
                Settings = list
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            GetValue(Headers.Settings);
            return model;
        }
    }
}


