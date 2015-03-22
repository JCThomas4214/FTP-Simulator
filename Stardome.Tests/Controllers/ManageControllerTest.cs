using System;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stardome.Controllers;
using Stardome.DomainObjects;
using Stardome.Models;
using Stardome.Services.Domain;

namespace Stardome.Tests.Controllers
{
    [TestClass]
    public class ManageControllerTest
    {
        private Mock<IUserAuthCredentialService> aMockUserAuthCredentialService;
        private Mock<ISiteSettingsService> aMockSiteSettingsService;
        private Mock<IRoleService> aMockRoleService;
        private Mock<ControllerContext> controllerContext;
        private Mock<IPrincipal> principal;
        private AdminController adminController;
        private ManageController manageController;

        private UserAuthCredential userAuthCredentialAdmin;
        private UserAuthCredential userAuthCredentialProducer;
        private UserAuthCredential userAuthCredentialUser;
        [TestInitialize]
        public void Init()
        {
            aMockUserAuthCredentialService = new Mock<IUserAuthCredentialService>();
            aMockSiteSettingsService = new Mock<ISiteSettingsService>();
            aMockRoleService = new Mock<IRoleService>();
            controllerContext = new Mock<ControllerContext>();
            principal = new Mock<IPrincipal>();
            
            adminController = new AdminController(aMockUserAuthCredentialService.Object, aMockSiteSettingsService.Object, aMockRoleService.Object);
            manageController = new ManageController(adminController);

            principal.SetupGet(x => x.Identity.Name).Returns("username");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            manageController.ControllerContext = controllerContext.Object;

            userAuthCredentialAdmin = new UserAuthCredential
            {
                Id = 4,
                RoleId = (int)Enums.Roles.Admin,
                Role = new Role { Id = (int)Enums.Roles.Admin },
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
            userAuthCredentialProducer = new UserAuthCredential
            {
                Id = 4,
                RoleId = (int)Enums.Roles.Producer,
                Role = new Role { Id = (int)Enums.Roles.Producer },
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
            userAuthCredentialUser = new UserAuthCredential
            {
                Id = 4,
                RoleId = (int)Enums.Roles.User,
                Role = new Role { Id = (int)Enums.Roles.User },
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
        }
        
        [TestMethod]
        public void Actions_Admin()
        {
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredentialAdmin);
            aMockSiteSettingsService.Setup(aService => aService.GetFilePath()).Returns("C:\\test\\123_4\\");
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(Headers.Content)).Returns(new SiteSetting() { Value = "content" });
            
            ViewResult result = manageController.Actions() as ViewResult;
            ContentModel resultsModel = result.Model as ContentModel;

            Boolean isTrue = resultsModel.RoleId == (int)Enums.Roles.Admin;
            isTrue = isTrue && String.Equals(resultsModel.RootPath, "C:/test/123_4/");
            Assert.IsTrue( isTrue );
        }

        [TestMethod]
        public void Actions_Producer()
        {
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredentialProducer);
            aMockSiteSettingsService.Setup(aService => aService.GetFilePath()).Returns("C:\\test\\123_4\\");
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(Headers.Content)).Returns(new SiteSetting() { Value = "content" });

            ViewResult result = manageController.Actions() as ViewResult;
            ContentModel resultsModel = result.Model as ContentModel;

            Boolean isTrue = resultsModel.RoleId == (int) Enums.Roles.Producer;
            isTrue = isTrue && String.Equals(resultsModel.RootPath, "C:/test/123_4/");
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void Actions_User()
        {
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredentialUser);
            aMockSiteSettingsService.Setup(aService => aService.GetFilePath()).Returns("C:\\test\\123_4\\");
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(Headers.Content)).Returns(new SiteSetting() { Value = "content" });

            ViewResult result = manageController.Actions() as ViewResult;
            ContentModel resultsModel = result.Model as ContentModel;

            Boolean isTrue = resultsModel.RoleId == (int)Enums.Roles.User;
            isTrue = isTrue && String.Equals(resultsModel.RootPath, "C:/test/123_4/");
            Assert.IsTrue(isTrue);
        }
    }
}
