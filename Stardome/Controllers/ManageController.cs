using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stardome.DomainObjects;
using Stardome.Models;
using System.IO.Compression;
using Ionic.Zip;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Stardome.Controllers
{
    public class ManageController : Controller
    {
        private readonly AdminController adminController;
        //
        // GET: /Manage/
        public ManageController(AdminController anAdminController)
        {
            adminController = anAdminController;
        }
        public ManageController()
        {
            adminController = new AdminController();
        }
        
        public ActionResult Actions()
        {
            List<string> dummy = new List<string>();
            ContentModel model = new ContentModel
            {
                RootPath = adminController.GetMainPath(adminController.GetUserRoleId(User.Identity.Name)),
                RoleId = adminController.GetUserRoleId(User.Identity.Name),
                List = dummy
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            adminController.GetValue(SiteSettings.Content);

            return View(model);
        }

        [HttpPost]
        public ActionResult Actions(IEnumerable<HttpPostedFileBase> files, string lastSelected)
        {
            List<string> results = new List<string>();
            string[] allowedExtensions = new[] { ".mp3", ".txt", ".jpeg" };
            int uploadedFiles = 0;
            int existingFiles = 0;
            int incorrectFiles = 0;
            if (files != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        if (!allowedExtensions.Contains(extension))
                        {
                            // Files with an extension that we don't allow, won't be uploaded
                            ++incorrectFiles;
                        }
                        else
                        {
                            try
                            {
                                // Upload files to the directory lastSelected. If the folder doesn't exist, it creates it.
                                string filePath = lastSelected;
                                bool exists = Directory.Exists(Path.GetFullPath(filePath));
                                if (!exists)
                                {
                                    Directory.CreateDirectory(Path.GetFullPath(filePath));
                                }
                                string path = Path.Combine(Path.GetFullPath(filePath),
                                    Path.GetFileName(file.FileName));
                                if (!System.IO.File.Exists(path))
                                {
                                    // Updloaded file to directory
                                    file.SaveAs(path);
                                    ++uploadedFiles;
                                }
                                else
                                {
                                    // This file name already exists in current directory
                                    ++existingFiles;
                                }

                            }
                            catch (Exception ex)
                            {
                                ViewBag.Message = "Could not upload file(s)";
                            }
                        }
                    }
                }
            }
            
            if (existingFiles > 0)
            {
                results.Add(existingFiles == 1
                    ? "1 file already exists. Please rename it."
                    : String.Format("{0} files already exist. Please rename them.", existingFiles));
            }

            if (incorrectFiles > 0)
            {
                results.Add(incorrectFiles == 1
                    ? "1 file is of the wrong type."
                    : String.Format("{0} files are of the wrong type.", incorrectFiles));
            }
            if (uploadedFiles > 0)
            {
                results.Add(uploadedFiles == 1
                    ? "1 file uploaded successfully."
                    : String.Format("{0} files uploaded successfully.", uploadedFiles));
                ViewBag.Message = "Uploaded to " + lastSelected;
            }
            

            ContentModel model = new ContentModel
            {
                RootPath = adminController.GetMainPath(adminController.GetUserRoleId(User.Identity.Name)),
                RoleId = adminController.GetUserRoleId(User.Identity.Name),
                List = results
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            adminController.GetValue(SiteSettings.Content);

            return View(model);
        }

        public ActionResult ByUser()
        {
            string users = new JavaScriptSerializer().Serialize(adminController.GetUsers().Data);
            users=users.Remove(0,users.IndexOf('['));
            users = users.Remove(users.IndexOf(']')+1, (users.Length - users.IndexOf(']'))-1);
            ContentModel model = new ContentModel
            {
                RootPath = adminController.GetMainPath(adminController.GetUserRoleId(User.Identity.Name)),
                RoleId = adminController.GetUserRoleId(User.Identity.Name),

                UserList = (new JavaScriptSerializer()).Deserialize<List<User>>(users)
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            adminController.GetValue(SiteSettings.Content);

            return View(model);
        }


        public ActionResult GetFolderPermissionsForUser(int UserId)
        {

            FolderUserAccessModel model = new FolderUserAccessModel
            {
               
            };
            //ViewBag.showAdminMenu = model.User.RoleId == (int)Enums.Roles.Admin;
            //adminController.GetValue(SiteSettings.Content);

            return View(model);
        }

        public void UpdateFolderPermissions(int UserId, List<String> SelectedFolders)
        {

            
        }

        public ActionResult ByFolder()
        {
            ContentModel model = new ContentModel
            {
                RootPath = adminController.GetMainPath(adminController.GetUserRoleId(User.Identity.Name)),
                RoleId = adminController.GetUserRoleId(User.Identity.Name)
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            adminController.GetValue(SiteSettings.Content);

            return View(model);
        }

        [HttpPost]
       public ActionResult DeleteFile(string filePath)
        {
            System.IO.File.Delete(Server.MapPath("~") + filePath);
            return null;
        }

    }
}
