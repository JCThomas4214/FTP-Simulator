using System;
using System.Linq;
using System.Reflection;
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
            userAuthCredentialService =
                new UserAuthCredentialService(new UserAuthCredentialRepository(new StardomeEntitiesCS()));
            siteSettingsService = new SiteSettingsService(new SiteSettingsRepository(new StardomeEntitiesCS()));
            roleService = new RoleService(new RoleRepository(new StardomeEntitiesCS()));

        }

        public AdminController(IUserAuthCredentialService aUserAuthCredentialService,
            ISiteSettingsService aSiteSettingsService, IRoleService aRoleService)
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
                //RolesList = GetRoles()
            };
            ViewBag.showAdminMenu = model.RoleId == (int) Enums.Roles.Admin;
            ViewBag.Message = GetValue(SiteSettings.Users);
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
            ViewBag.showAdminMenu = model.RoleId == (int) Enums.Roles.Admin;
            ViewBag.Message = GetValue(SiteSettings.Content);

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
        public JsonResult GetActiveUsers(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            IList<User> users = GetUsersHelper(jtSorting);
            IEnumerable<User> activeUsers = users.Where(x => x.RoleId != (int) Enums.Roles.InActive);

            users = GetUsersSortSize(activeUsers, jtSorting);
            return Json(new {Result = "OK", Records = users, TotalRecordCount = users.Count});
        }

        // Gets all the users that are not InActive and contain userInformation 
        [HttpPost]
        public JsonResult GetAllUsers(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            IList<User> users = GetUsersHelper(jtSorting);
            users = GetUsersSortSize(users, jtSorting);
            return Json(new {Result = "OK", Records = users, TotalRecordCount = users.Count});
        }

        private List<User> GetUsersSortSize(IEnumerable<User> users, string jtSorting = null)
        {
            if (jtSorting != null && !jtSorting.Contains("RoleId"))
            {
                string[] param = jtSorting.Split(' ');
                
                PropertyInfo property = typeof(User).GetProperty(param[0]);

                if (param.Length == 2 && param[1].Equals("DESC"))
                {
                    users = users.OrderByDescending(x => property.GetValue(x, null));
                }
                else
                {
                    users = users.OrderBy(x => property.GetValue(x, null));
                }
                
            }
            
            return users.ToList();
        }

        private IList<User> GetUsersHelper(string jtSorting)
        {
            IList<User> users = new List<User>();
            IList<UserAuthCredential> userAuthCredentials;
            if (jtSorting != null && jtSorting.Contains("RoleId"))
            {
                if (jtSorting.Contains("DESC"))
                {
                    userAuthCredentials =
                        userAuthCredentialService.GetUserAuthCredentials()
                            .OrderByDescending(aUser => aUser.Role.Role1)
                            .ToList();
                }
                else
                {
                    userAuthCredentials =
                        userAuthCredentialService.GetUserAuthCredentials()
                            .OrderBy(aUser => aUser.Role.Role1)
                            .ToList();
                }
            }
            else
            {
                userAuthCredentials = userAuthCredentialService.GetUserAuthCredentials().ToList();
            }
            // Produces the User objects that will get inserted in the json
            foreach (UserAuthCredential credential in userAuthCredentials)
            {
                UserInformation userInformation = credential.UserInformations.FirstOrDefault();
                if (userInformation != null)
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
            return users;
        }

        // By Deleting the User it makes the User InActive so that the user is still stored in the database
        [HttpPost]
        public JsonResult DeleteUser(User user)
        {
            try
            {
                user.RoleId = (int) Enums.Roles.InActive;
                UpdateUserRole(user);
                return Json(new {Result = "OK"});
            }
            catch (Exception)
            {
                return Json(new {Result = "ERROR", Message = "Unable to delete user"});
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
                    return Json(new {Result = "ERROR", Message = "Access was not granted."});
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
                return Json(new {Result = "OK"});
            }
            catch (Exception)
            {
                return Json(new {Result = "ERROR", Message = "Unable to update user"});
            }
        }

        // Gets all the Roles that are found in the database and converts to json object
        [HttpPost]
        public JsonResult GetRoles()
        {
            var roles = roleService.GetRoles().Select(aRole => new {DisplayText = aRole.Role1, Value = aRole.Id});
            return Json(new {Result = "OK", Options = roles});
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
        public String GetValue(String header)
        {
            return siteSettingsService.FindSiteSetting(header).Value;
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
            ViewBag.showAdminMenu = model.RoleId == (int) Enums.Roles.Admin;
            ViewBag.Message = GetValue(SiteSettings.Settings);
            return model;
        }
    }
}