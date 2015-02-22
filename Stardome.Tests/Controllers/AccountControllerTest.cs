using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Controllers;
using System.Web.Mvc;
using Moq;
using Stardome.DomainObjects;
using Stardome.Services.Domain;
using Stardome.Repositories;

namespace Stardome.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        readonly Mock<IUserAuthCredentialService> aMockUserAuthCredentialService = new Mock<IUserAuthCredentialService>();
        readonly Mock<IRoleService> aMockRoleService = new Mock<IRoleService>();

        private static int id = 1;
        private static string username = "usn";
        private UserAuthCredential userAuthCredential = new UserAuthCredential { Id = id, Username = username };

        [TestMethod]
        public void RedirectToLocal()
        {
            var controller = new AccountController();
            var result = (RedirectToRouteResult)controller.RedirectToLocal(1);
            Assert.AreEqual("Users", result.RouteValues["action"]);

            result = (RedirectToRouteResult)controller.RedirectToLocal(2);
            Assert.AreEqual("Index", result.RouteValues["action"]);

            result = (RedirectToRouteResult)controller.RedirectToLocal(3);
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

        [TestMethod]
        public void RedirectToLocalRandomId()
        {
            var controller = new AccountController();

            var result = (RedirectToRouteResult)controller.RedirectToLocal(new Random().Next());
            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Register()
        {
            var controller = new AccountController();
            var result = controller.Register();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


    }
}
