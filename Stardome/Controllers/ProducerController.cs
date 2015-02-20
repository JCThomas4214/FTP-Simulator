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
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var allowedExtensions = new[] { ".doc", ".mp3", ".txt", ".jpeg" };


            if (file != null && file.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    ViewBag.Message = "Incorrect file type";
                }
                
                
                else try
                {
                    string filePath = "~/TestUploads";
                    bool exists = System.IO.Directory.Exists(Server.MapPath(filePath));
                    if (!exists) { System.IO.Directory.CreateDirectory(Server.MapPath(filePath)); }
                    string path = Path.Combine(Server.MapPath(filePath),
                                               Path.GetFileName(file.FileName));
                    if (!System.IO.File.Exists(path))
                    {
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";
                    }
                    else
                    {
                        ViewBag.Message = "A file with that name already exists";
                    }
                    
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }
        
        public ActionResult Upload()
        {
            ViewBag.showAdminMenu = false;
            return View();
        }



    }
}