using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stardome.DomainObjects;
using Stardome.Models;
using System.Web.Script.Serialization;
using Stardome.Services.Domain;
using Stardome.Repositories;
using WebMatrix.WebData;

namespace Stardome.Controllers
{
    public class ManageController : Controller
    {
        private readonly AdminController adminController;
        private readonly IFolderService folderService;
        private readonly IAccessService accessService;
        //
        // GET: /Manage/
        public ManageController(AdminController anAdminController, IFolderService aFolderService, IAccessService anAccessService)
        {
            adminController = anAdminController;
            folderService = aFolderService;
            accessService = anAccessService;
        }
        public ManageController()
        {
            adminController = new AdminController();
            folderService = new FolderService(new FolderRepository(new StardomeEntitiesCS()));
            accessService = new AccessService(new AccessRepository(new StardomeEntitiesCS()));
        }
        
        public ActionResult Actions()
        {
            ContentModel model = new ContentModel
            {
                RootPath = adminController.GetMainPath(adminController.GetUserRoleId(User.Identity.Name)),
                RoleId = adminController.GetUserRoleId(User.Identity.Name),
                List = new List<string>()
            };
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            ViewBag.Message = adminController.GetValue(SiteSettings.Content);
           
            return View(model);
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            string strUploadStatus=string.Empty;
            List<UploadFilesResult> uploadList = new List<UploadFilesResult>();
            List<string> results = new List<string>();
            string[] allowedExtensions = new[] { ".mp3", ".mp4" };
            int uploadedFiles = 0;
            int existingFiles = 0;
            int incorrectFiles = 0;

            string lastSelectedFolder = Request.Form[0];
            lastSelectedFolder = lastSelectedFolder.Replace("\"", "").Replace("\\", "/");
            if (Request.Files.Count>0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
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
                            HttpPostedFileBase hpf = file as HttpPostedFileBase;
                            if (hpf.ContentLength == 0)
                                continue;
                            try
                            {
                                string filePath = lastSelectedFolder;
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
                                strUploadStatus = "Could not upload file(s)";
                            }

                            uploadList.Add(new UploadFilesResult()
                            {
                                Name = hpf.FileName,
                                Length = hpf.ContentLength,
                                Type = hpf.ContentType
                            }); 
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
                strUploadStatus = "Uploaded to " + lastSelectedFolder;

            }
            
            return Json(new { Result = "OK", UploadStatus = strUploadStatus });
        }

        public ActionResult ByUser()
        {
            ViewBag.message = "Manage Content Permissions";
            ViewBag.StatusMessage = string.Empty;
            string users = new JavaScriptSerializer().Serialize(adminController.GetActiveUsers(0,100).Data);
            users=users.Remove(0,users.IndexOf('['));
            users = users.Remove(users.IndexOf(']')+1, (users.Length - users.IndexOf(']'))-1);
            List<string> dummy = new List<string>();
            ContentModel model = new ContentModel
            {
                RootPath = adminController.GetMainPath(adminController.GetUserRoleId(User.Identity.Name)),
                RoleId = adminController.GetUserRoleId(User.Identity.Name),

                UserList = (new JavaScriptSerializer()).Deserialize<List<User>>(users),
                List = dummy
            };
            
            ViewBag.showAdminMenu = model.RoleId == (int)Enums.Roles.Admin;
            ViewBag.Message = adminController.GetValue(SiteSettings.ContentByUser);

            return View(model);
        }

        public JsonResult GetFolderPermissionsForUser(int UserId)
        {
            List<Access> accesses = accessService.GetAccessByUserId(UserId);
            List<string> folderIds =new List<string>();
            List<string> folderNames = new List<string>();
            foreach (Access access in accesses)
            {
                string folderId = System.Web.HttpUtility.JavaScriptStringEncode(access.Folder.Path); // access.Folder.Path.Replace(@"\", @"\\");
                folderIds.Add(access.Folder.Path);
                folderNames.Add((access.Folder.Name));
            }

            return Json(new { Result = "OK", folderIds = folderIds, folderNames=folderNames, TotalRecordCount = accesses.Count });

            
        }

        public JsonResult UpdateFolderPermissions(int UserId, List<String> SelectedFolders, List<string> SelectedFolderNames)
        {

            string errMsg = string.Empty;
            List<Access> accesses = accessService.GetAccessByUserId(UserId);

            foreach (Access access in accesses)
            {
                if ((SelectedFolders == null) || (!(SelectedFolderNames.Exists(a => a.Equals(access.Folder.Name)))))
                {
                    errMsg = accessService.DeleteAccess(access);
                }
            }

            if (SelectedFolders != null)
            {
                foreach (string selectedFolder in SelectedFolderNames)
                {

                    if (accessService.GetAccessByFolderName(selectedFolder, UserId) == null)
                    {
                        Access a = new Access();
                        a.UserId = UserId;
                        a.FolderId = folderService.GetFolderByFolderName(selectedFolder).Id;
                        a.DateGiven = DateTime.Now;
                        errMsg = accessService.AddAccess(a);
                    }

                }
            }
            if (errMsg == string.Empty)
                errMsg = "Updated Permissions Successfully";


            return Json(new { resultMessage = errMsg }); ;
            
        }

        [HttpPost]
       public ActionResult DeleteFile(string filePath)
        {
            
            System.IO.File.Delete(Server.MapPath("~") + filePath);
            return null;
        }

        [HttpPost]
        public ActionResult CreateFolder(string Path, string Name)
        {
            string folderPath = Path.Replace(Server.MapPath("~"),"") + Name;
            System.IO.Directory.CreateDirectory(Path+ Name);
            Folder f = new Folder();          

            f.Name = Name;
            f.Path = folderPath;
            f.CreatedBy = WebSecurity.CurrentUserId;
            f.CreatedOn = DateTime.Now;
            folderService.AddFolder(f);

            return null;
        }

        [HttpPost]
        public ActionResult DeleteFolder(string Path, string Name)
        {
            string folderPath = Path.Replace(Server.MapPath("~"), "");
            folderPath=folderPath.Remove(folderPath.Length - 1, 1);
            System.IO.Directory.Delete(Path);
            Folder f = folderService.GetFolderByFolderPath(folderPath);              
            folderService.DeleteFolder(f);

            return null;
        }

        public void GrantPermissionToFolder(string folderId, string folderName, List<string> selectedUsers)
        {
            List<Access> accesses;
            // Delete all the subfolder entries
            foreach (string userId in selectedUsers)
            {
                accesses = accessService.GetAccessByUserId(Convert.ToInt32(userId));
                foreach (Access access in accesses)
                { 
                    folderId=folderId.Replace(@"\\", @"\");
                    if (access.Folder.Path.StartsWith(folderId) && access.Folder.Path != folderId)
                    {
                        accessService.DeleteAccess(access);
                    }
                }
            }
            
            // Delete Access if user is unchecked
            accesses = folderService.GetFolderByFolderName(folderName).Accesses.ToList();
            foreach (Access access in accesses)
            {
                if (!(selectedUsers.Exists(a => a.Equals(access.UserId.ToString()))))
                {
                   accessService.DeleteAccess(accessService.GetById(access.Id));
                }
            }
            
            // Adding permissions to selected users
            foreach (string userId in selectedUsers)
            {
                int fId = folderService.GetFolderByFolderName(folderName).Id;
                List<Access> access = accessService.GetAccessByUserId(Convert.ToInt32(userId));
                if  (!(access.Exists(a => a.FolderId == fId)))
                {
                    Access a = new Access();
                    a.UserId = Convert.ToInt32(userId);
                    a.FolderId = fId;
                    a.DateGiven = DateTime.Now;
                    accessService.AddAccess(a);
                }
            }


        }

        public JsonResult GetUserPermissionsForFolder(string folderName)
        {
            List<Access> accesses = folderService.GetFolderByFolderName(folderName).Accesses.ToList();
            List<string> selectedUsers = new List<string>();
           foreach (Access access in accesses)
            {
                selectedUsers.Add(access.UserId.ToString());
                
            }

           return Json(new { Result = "OK", selectedUsers = selectedUsers, TotalRecordCount = accesses.Count });

        }
    }
}
