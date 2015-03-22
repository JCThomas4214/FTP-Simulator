using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Controllers;
using System.Web.Mvc;
using Moq;
using Stardome.DomainObjects;
using Stardome.Services.Domain;
using Stardome.Repositories;
using Stardome.Models;
using Stardome.Services.Application;

namespace Stardome.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        readonly Mock<IUserAuthCredentialService> aMockUserAuthCredentialService = new Mock<IUserAuthCredentialService>();
        readonly Mock<IRoleService> aMockRoleService = new Mock<IRoleService>();
        readonly Mock<IAuthenticationProvider> aMockAuthenticationProvider = new Mock<IAuthenticationProvider>();
        
        [TestInitialize]
        public void Init()
        {
            aMockAuthenticationProvider.Setup(AP => AP.IsAuthenticated()).Returns(true);
            aMockAuthenticationProvider.Setup(AP => AP.CurrentUserName()).Returns("username");
            aMockAuthenticationProvider.Setup(AP => AP.GetUserId("username")).Returns(1);
            
            
        }



        private static int id = 1;
        private static string username = "usn";
        private UserAuthCredential userAuthCredential = new UserAuthCredential { Id = id, Username = username };

        [TestMethod]
        public void RedirectToLocal()
        {
            var controller = new AccountController();
            var result = (RedirectToRouteResult)controller.RedirectToLocal(1);
            Assert.AreEqual("Users", result.RouteValues["action"]);

            result = (RedirectToRouteResult)controller.RedirectToLocal(3);
            Assert.AreEqual("Actions", result.RouteValues["action"]);

        }

        [TestMethod]
        public void RedirectToLocalRandomId()
        {
            var controller = new AccountController();
            var result = (RedirectToRouteResult)controller.RedirectToLocal(new Random().Next());
            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

        [TestMethod]
        public void LoginTest()
        {
            var controller = new AccountController(aMockUserAuthCredentialService.Object, aMockRoleService.Object, aMockAuthenticationProvider.Object);
            LoginModel lm =new LoginModel();
            lm.UserName = "username";
            lm.Password = "password";
            ViewResult result = (ViewResult)controller.Login(lm, "");
            Assert.IsNull(result.ViewBag.Message);
        }

        [TestMethod]
        public void LogOffTest()
        {
            var controller = new AccountController(aMockUserAuthCredentialService.Object, aMockRoleService.Object, aMockAuthenticationProvider.Object);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.LogOff();
            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

    }
}
