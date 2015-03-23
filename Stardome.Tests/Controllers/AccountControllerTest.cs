using System;
using System.Collections.ObjectModel;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Controllers;
using System.Web.Mvc;
using Moq;
using Stardome.DomainObjects;
using Stardome.Services.Domain;
using Stardome.Services.Application;
using Stardome.Models;

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
        readonly Mock<IAuthenticationProvider> aMockAuthenticationProvider = new Mock<IAuthenticationProvider>();

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
            controller = new AccountController(aMockUserAuthCredentialService.Object, aMockSiteSettingsService.Object, aMockUserInformationService.Object, aMockAuthenticationProvider.Object);

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
                        Email = "email1@email.com",
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

        [TestMethod]
        public void Manage_ChangePasswordSuccess()
        {

            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredential1);
            aMockAuthenticationProvider.Setup(AP => AP.IsAuthenticated()).Returns(true);
            ViewResult result = controller.Manage(Enums.ManageMessageId.ChangePasswordSuccess) as ViewResult;
            Boolean isTrue = Equals(result.ViewBag.StatusMessage, "Your password has been changed.");
            isTrue = isTrue && result.ViewBag.showAdminMenu;
            isTrue = isTrue && Equals(result.ViewBag.Name, "Jane Doe");
            isTrue = isTrue && Equals(result.ViewBag.Email, "email1@email.com");
            isTrue = isTrue && Equals(result.ViewBag.Role, "Admin");

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void Manage_SetPasswordSuccess()
        {

            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredential1);
            aMockAuthenticationProvider.Setup(AP => AP.IsAuthenticated()).Returns(true);
            ViewResult result = controller.Manage(Enums.ManageMessageId.SetPasswordSuccess) as ViewResult;
            Boolean isTrue = Equals(result.ViewBag.StatusMessage, "Your password has been set.");

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void Manage_NullUserInfo()
        {
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredentialNoUserInfo);
            aMockAuthenticationProvider.Setup(AP => AP.IsAuthenticated()).Returns(true);
            ViewResult result = controller.Manage(Enums.ManageMessageId.ChangePasswordSuccess) as ViewResult;
            Boolean isTrue = !result.ViewBag.showAdminMenu;
            isTrue = isTrue && String.IsNullOrEmpty(result.ViewBag.Name);
            isTrue = isTrue && String.IsNullOrEmpty(result.ViewBag.Email);
            isTrue = isTrue && Equals(result.ViewBag.Role, "User");

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
         public void LoginTest()  //POST Method
         {
            LoginModel lm =new LoginModel();
            lm.UserName = "username";
            lm.Password = null;
            lm.RememberMe = false;
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredential1);
            aMockAuthenticationProvider.Setup(AP => AP.Login("username",null,false)).Returns(true);
            var result =(RedirectToRouteResult) controller.Login(lm, "") ;
            Assert.AreEqual("Users", result.RouteValues["action"]);
         }

        [TestMethod]
        public void InavlidLoginTest()  //POST Method
        {
            LoginModel lm = new LoginModel();
            lm.UserName = "username";
            lm.Password = null;
            lm.RememberMe = false;
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredential1);
            aMockAuthenticationProvider.Setup(AP => AP.Login("username1", null, false)).Returns(true);
            var result = (ViewResult)controller.Login(lm, "");
            Assert.AreEqual(false,result.ViewData.ModelState.IsValid);
        }
 
        [TestMethod]
        public void LogOffTest() //POST Method
        { 
            RedirectToRouteResult result = (RedirectToRouteResult)controller.LogOff();
            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Manage() //POST Method
        {
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredential1);
            aMockAuthenticationProvider.Setup(AP => AP.Login("username", null, false)).Returns(true);
            aMockAuthenticationProvider.Setup(AP => AP.IsAuthenticated()).Returns(true);
            var result = (ViewResult)controller.Manage(Enums.ManageMessageId.ChangePasswordSuccess);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void ManageNotAuthenticated() //POST Method
        {
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredential1);
            aMockAuthenticationProvider.Setup(AP => AP.Login("username", null, false)).Returns(true);
            aMockAuthenticationProvider.Setup(AP => AP.IsAuthenticated()).Returns(false);
            var result = (RedirectToRouteResult)controller.Manage(Enums.ManageMessageId.ChangePasswordSuccess);
            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

        //[TestMethod]
        //public void LostPassword() //POST Method
        //{
        //    aMockAuthenticationProvider.Setup(AP => AP.GeneratePasswordResetToken("username1", 15)).Returns("token");
        //    aMockUserAuthCredentialService.Setup(aService => aService.GetByEmail("email1@email.com")).Returns(userAuthCredential1);
        //    LostPasswordModel lpm = new LostPasswordModel();
        //    lpm.Email = "email1@email.com";
        //    var result = (ViewResult)controller.LostPassword(lpm);
        //    Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        //}

        [TestMethod]
        public void ResetPassword() //POST Method
        {
            aMockAuthenticationProvider.Setup(AP => AP.ResetPassword("token",null)).Returns(true);
            ResetPasswordModel rpm = new ResetPasswordModel();
            rpm.ReturnToken = "token";
            rpm.Password = null;
            var result = (ViewResult)controller.ResetPassword(rpm);
            Assert.AreEqual("Successfully Changed", result.ViewBag.Message);
        }

        [TestMethod]
        public void ResetPasswordWrongToken() //POST Method
        {
            aMockAuthenticationProvider.Setup(AP => AP.ResetPassword("token", null)).Returns(true);
            ResetPasswordModel rpm = new ResetPasswordModel();
            rpm.ReturnToken = "token1";
            rpm.Password = null;
            var result = (ViewResult)controller.ResetPassword(rpm);
            Assert.AreEqual("Something went horribly wrong!", result.ViewBag.Message);
        }

        //[TestMethod]
        //public void Register()
        //{
        //    User userAdded = new User
        //    {
        //        Id = 1,
        //        EmailAddress = "email",
        //        FirstName = "fname",
        //        LastName = "lname",
        //        RoleId = (int) Enums.Roles.Admin,
        //        Username = "username"
        //    };
        //    // Don't have to mock CreateUserAndAccount because need to return null
        //    aMockAuthenticationProvider.Setup(aService => aService.GetUserId(userAdded.Username)).Returns(1);
        //    aMockAuthenticationProvider.Setup(aService => aService.GeneratePasswordResetToken(userAdded.Username, It.IsAny<int>())).Returns("token");
        //    ActionResult result = controller.Register(userAdded);
        //}
    }
}
