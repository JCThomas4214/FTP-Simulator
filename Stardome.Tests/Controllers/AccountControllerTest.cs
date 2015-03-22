using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Principal;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Controllers;
using System.Web.Mvc;
using Moq;
using Stardome.DomainObjects;
using Stardome.Services.Domain;

namespace Stardome.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private Mock<IUserAuthCredentialService> aMockUserAuthCredentialService;
        private Mock<ISiteSettingsService> aMockSiteSettingsService;
        private Mock<IUserInformationService> aMockUserInformationService;
        private Mock<ControllerContext> controllerContext;
        private Mock<IPrincipal> principal;

        private AccountController controller;

        private UserAuthCredential userAuthCredential1;
        private UserAuthCredential userAuthCredentialNoUserInfo;

        [TestInitialize]
        public void Init()
        {
            aMockUserAuthCredentialService = new Mock<IUserAuthCredentialService>();
            aMockSiteSettingsService = new Mock<ISiteSettingsService>();
            aMockUserInformationService = new Mock<IUserInformationService>();
            controllerContext = new Mock<ControllerContext>();
            principal = new Mock<IPrincipal>();
            controller = new AccountController(aMockUserAuthCredentialService.Object, aMockSiteSettingsService.Object, aMockUserInformationService.Object);
            principal.SetupGet(x => x.Identity.Name).Returns("username");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            controller.ControllerContext = controllerContext.Object;

            userAuthCredential1 = new UserAuthCredential
            {
                Id = 4,
                RoleId = (int)Enums.Roles.Admin,
                Role = new Role { Id = (int)Enums.Roles.Admin, Role1 = "Admin" },
                Username = "username1",
                UserInformations = new Collection<UserInformation>
                {
                    new UserInformation
                    {
                        Email = "email1",
                        FirstName = "Jane",
                        LastName = "Doe"
                    }
                }
            };
            
            userAuthCredentialNoUserInfo = new UserAuthCredential
            {
                Id = 3,
                RoleId = (int)Enums.Roles.User,
                Role = new Role { Id = (int)Enums.Roles.User, Role1 = "User" },
                Username = "username3"
            };
        }

        [TestMethod]
        public void RedirectToLocal_Admin()
        {
            var result = (RedirectToRouteResult)controller.RedirectToLocal((int)Enums.Roles.Admin);
            Assert.IsTrue(Equals("Users", result.RouteValues["action"]) && Equals("Admin", result.RouteValues["controller"]));
        }

        [TestMethod]
        public void RedirectToLocal_Producer()
        {
            var result = (RedirectToRouteResult)controller.RedirectToLocal((int)Enums.Roles.Producer);
            Assert.IsTrue(Equals("Actions", result.RouteValues["action"]) && Equals("Manage", result.RouteValues["controller"]));
        }

        [TestMethod]
        public void RedirectToLocal_User()
        {
            var result = (RedirectToRouteResult)controller.RedirectToLocal((int)Enums.Roles.User);
            Assert.IsTrue(Equals("Actions", result.RouteValues["action"]) && Equals("Manage", result.RouteValues["controller"]));
        }

        [TestMethod]
        public void RedirectToLocal_InActive()
        {
            var result = (RedirectToRouteResult)controller.RedirectToLocal((int)Enums.Roles.InActive);
            Assert.IsTrue(Equals("Login", result.RouteValues["action"]) && Equals("Account", result.RouteValues["controller"]));
        }

        //[TestMethod]
        //public void Register()
        //{
        //    // Does register add the userInfo and UserAuthCredential
        //    //User addedUser = new User
        //    //{
                
        //    //};
        //    //controller.Register(user);
        //    //var result = controller.Register();
        //    //Assert.IsInstanceOfType(result, typeof(ViewResult));
        //}

        [TestMethod]
        public void Manage_ChangePasswordSuccess()
        {

            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredential1);

            HttpContext.Current = new HttpContext( new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()) );
            // User is logged in
            HttpContext.Current.User = new GenericPrincipal( new GenericIdentity("username"), new string[0] );

            ViewResult result = controller.Manage(Enums.ManageMessageId.ChangePasswordSuccess) as ViewResult;
            
            // User is logged out
            HttpContext.Current.User = new GenericPrincipal( new GenericIdentity(String.Empty), new string[0] );
            Boolean isTrue = Equals(result.ViewBag.StatusMessage, "Your password has been changed.");
            isTrue = isTrue && result.ViewBag.showAdminMenu;
            isTrue = isTrue && Equals(result.ViewBag.Name, "Jane Doe");
            isTrue = isTrue && Equals(result.ViewBag.Email, "email1");
            isTrue = isTrue && Equals(result.ViewBag.Role, "Admin");

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void Manage_SetPasswordSuccess()
        {

            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredential1);

            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
            // User is logged in
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);

            ViewResult result = controller.Manage(Enums.ManageMessageId.SetPasswordSuccess) as ViewResult;

            // User is logged out
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            Boolean isTrue = Equals(result.ViewBag.StatusMessage, "Your password has been set.");

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void Manage_NullUserInfo()
        {

            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredentialNoUserInfo);

            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
            // User is logged in
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);

            ViewResult result = controller.Manage(Enums.ManageMessageId.ChangePasswordSuccess) as ViewResult;

            // User is logged out
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            Boolean isTrue = !result.ViewBag.showAdminMenu;
            isTrue = isTrue && String.IsNullOrEmpty(result.ViewBag.Name);
            isTrue = isTrue && String.IsNullOrEmpty(result.ViewBag.Email);
            isTrue = isTrue && Equals(result.ViewBag.Role, "User");

            Assert.IsTrue(isTrue);
        }

    }
}
