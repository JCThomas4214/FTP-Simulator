using Stardome.DomainObjects;
using Stardome.Repositories;
using Stardome.Services.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Stardome.Controllers
{
    [DataContract]
    internal class Files
    {
        [DataMember]
        internal string name;

        [DataMember]
        internal int size;

        [DataMember]
        internal string url;

        [DataMember]
        internal string thumbnailUrl;

        [DataMember]
        internal string deleteUrl;

        [DataMember]
        internal string deleteType;

        [DataMember]
        internal string error;
    }
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
        public Tuple<ActionResult,DataContractJsonSerializer> Upload(IEnumerable<HttpPostedFileBase> files)
        {
            Files f = new Files();
            f.name = "picture1.jpg";
            f.size = 902604;
            f.url = @"http:\/\/example.org\/files\/picture1.jpg";
            f.thumbnailUrl = @"http:\/\/example.org\/files\/thumbnail\/picture1.jpg";
            f.deleteUrl = @"http:\/\/example.org\/files\/picture1.jpg";
            f.deleteType = "DELETE";
            f.error = "Filetype not allowed";

            Files g = new Files();
            g.name = "picture2.jpg";
            g.size = 841946;
            g.url = @"http:\/\/example.org\/files\/picture2.jpg";
            g.thumbnailUrl = @"http:\/\/example.org\/files\/thumbnail\/picture2.jpg";
            g.deleteUrl = @"http:\/\/example.org\/files\/picture2.jpg";
            g.deleteType = "DELETE";
            g.error = "Filetype not allowed";

            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Files));

            var allowedExtensions = new[] { ".doc", ".mp3", ".txt", ".jpeg" };

            foreach (var file in files)
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
            Tuple<ActionResult, DataContractJsonSerializer> fileTuple = new Tuple<ActionResult, DataContractJsonSerializer>(View(), ser);
            return fileTuple;
        }

        public ActionResult Upload()
        {
            ViewBag.showAdminMenu = false;
            return View();



        }

       /* public DataContractJsonSerializer Uploader()
        {
            Files f = new Files();
            f.name = "picture1.jpg";
            f.size = 902604;
            f.url = @"http:\/\/example.org\/files\/picture1.jpg";
            f.thumbnailUrl = @"http:\/\/example.org\/files\/thumbnail\/picture1.jpg";
            f.deleteUrl = @"http:\/\/example.org\/files\/picture1.jpg";
            f.deleteType = "DELETE";
            f.error = "Filetype not allowed";

            Files g = new Files();
            g.name = "picture2.jpg";
            g.size = 841946;
            g.url = @"http:\/\/example.org\/files\/picture2.jpg";
            g.thumbnailUrl = @"http:\/\/example.org\/files\/thumbnail\/picture2.jpg";
            g.deleteUrl = @"http:\/\/example.org\/files\/picture2.jpg";
            g.deleteType = "DELETE";
            g.error = "Filetype not allowed";

            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Files));

            return ser;
        }*/



    }
}