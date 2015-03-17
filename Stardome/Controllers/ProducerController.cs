using Stardome.DomainObjects;
using Stardome.Repositories;
using Stardome.Services.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;


namespace Stardome.Controllers
{
    
    
    public class ProducerController : Controller
    {
        //
        // GET: /Producer/
       

        private IUserAuthCredentialService userAuthCredentialService;
        private IRoleService roleService;
        public ActionResult Index()
        {
            userAuthCredentialService = new UserAuthCredentialService(new UserAuthCredentialRepository(new StardomeEntitiesCS()));
            roleService = new RoleService(new RoleRepository(new StardomeEntitiesCS()));

            if (ModelState.IsValid && WebSecurity.IsAuthenticated)
            {

                int roleId = userAuthCredentialService.GetByUsername(WebSecurity.CurrentUserName).Role.Id;
                if (roleId != 2)
                    return (new AccountController()).RedirectToLocal(roleId);
                else
                {
                    ViewBag.showAdminMenu = false;
                    return View();
                }
            }
            else
            {
                ViewBag.showAdminMenu = false;
                return View();
            }

        }

     
        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files, string selectPath)
        {
            List<string> results = new List<string>();
            var allowedExtensions = new[] { ".doc", ".mp3", ".txt", ".jpeg" };
            var uploadedFiles = 0;
            var existingFiles = 0;
            var incorrectFiles = 0;
            foreach (var file in files) {
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
                            string filePath = selectPath;
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
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                }}
                //else Testing without Else
                //{
                   // ViewBag.Message = "You have not specified any files.";
                //}
            if (existingFiles > 0)
            {
                if (existingFiles == 1)
                    //ViewBag.Existing = "1 file already exists. Please rename it.";
                    results.Add(String.Format("1 file already exists. Please rename it."));
                else
                    //ViewBag.Existing = existingFiles + " files already exist. Please rename them.";
                    results.Add(String.Format(existingFiles + " files already exist. Please rename them."));
                                     
            }

            if (incorrectFiles > 0)
            {
                if (incorrectFiles == 1)
                    //ViewBag.Incorrect = " 1 file is of the wrong type.";
                    results.Add(String.Format(" 1 file is of the wrong type."));
                else
                    //ViewBag.Incorrect = incorrectFiles + " files are of the wrong type.";
                    results.Add(String.Format(incorrectFiles + " files are of the wrong type."));
            }

            if (uploadedFiles == 1)
                //ViewBag.Uploaded = " 1 file uploaded successfully.";
            results.Add(String.Format(" 1 file uploaded successfully."));
            else
                //ViewBag.Uploaded = uploadedFiles + " files uploaded successfully.";
                results.Add(String.Format(uploadedFiles + " files uploaded successfully."));
            //string[] resultArray = results.ToArray();
            //ViewBag.Results = resultArray;
             
            //return View();
            ViewBag.Message = "Uploading to " + selectPath;
            return View(results);
        }

        public ActionResult Upload()
        {
            List<string> results = new List<string>();
            ViewBag.showAdminMenu = false;
            return View(results);



        }

      }
}