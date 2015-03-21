using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stardome.DomainObjects;
using Stardome.Models;

namespace Stardome.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/
        public ActionResult Actions(String username = null)
        {
            AdminController adminController = new AdminController();
            ContentModel model = new ContentModel
            {
                RootPath = adminController.GetMainPath(adminController.GetUserRoleId(User.Identity.Name)),
                RoleId = adminController.GetUserRoleId(User.Identity.Name)
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            adminController.GetValue(Headers.Content);

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
            ManageController manageController = new ManageController();

            foreach (HttpPostedFileBase file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!allowedExtensions.Contains(extension))
                    {
                        //ViewBag.Message = "Incorrect file type";
                        //Files with an extension that we don't allow, won't be uploaded
                        ++incorrectFiles;
                    }
                    else try
                        {   //Upload files to the folder TestUploads. If the folder doesn't exist, it creates it.
                            string filePath = lastSelected;
                            bool exists = System.IO.Directory.Exists(Server.MapPath(filePath));
                            if (!exists) { System.IO.Directory.CreateDirectory(Server.MapPath(filePath)); }
                            string path = Path.Combine(Server.MapPath(filePath),
                                                       Path.GetFileName(file.FileName));
                            if (!System.IO.File.Exists(path))
                            {
                                file.SaveAs(path);
                                //ViewBag.Message = "File uploaded successfully";
                                ++uploadedFiles;
                            }
                            else
                            {
                                //ViewBag.Message = "At least 1 file already exists. Please rename it to upload it.";
                                ++existingFiles;
                            }

                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "Could not upload file(s)";
                        }
                }
            }
            if (existingFiles > 0)
            {
                if (existingFiles == 1)
                    results.Add(String.Format("1 file already exists. Please rename it."));
                else
                    results.Add(String.Format(existingFiles + " files already exist. Please rename them."));

            }

            if (incorrectFiles > 0)
            {
                if (incorrectFiles == 1)
                    results.Add(String.Format(" 1 file is of the wrong type."));
                else
                    results.Add(String.Format(incorrectFiles + " files are of the wrong type."));
            }

            if (uploadedFiles == 1)
                results.Add(String.Format(" 1 file uploaded successfully."));
            else
                results.Add(String.Format(uploadedFiles + " files uploaded successfully."));

            ViewBag.Message = "Uploading to " + lastSelected;

            AdminController adminController = new AdminController();
            ContentModel model = new ContentModel
            {
                RootPath = adminController.GetMainPath(adminController.GetUserRoleId(User.Identity.Name)),
                RoleId = adminController.GetUserRoleId(User.Identity.Name),
                List = results
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            adminController.GetValue(Headers.Content);

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
