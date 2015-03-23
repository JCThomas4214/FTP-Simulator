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
using System.Web;
using System.Web.Routing;

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
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(SiteSettings.Content)).Returns(new SiteSetting() { Value = "content" });

            ViewResult result = manageController.Actions() as ViewResult;
            ContentModel resultsModel = result.Model as ContentModel;

            Boolean isTrue = resultsModel.RoleId == (int)Enums.Roles.Admin;
            isTrue = isTrue && String.Equals(resultsModel.RootPath, "C:/test/123_4/");
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void Actions_Producer()
        {
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredentialProducer);
            aMockSiteSettingsService.Setup(aService => aService.GetFilePath()).Returns("C:\\test\\123_4\\");
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(SiteSettings.Content)).Returns(new SiteSetting() { Value = "content" });

            ViewResult result = manageController.Actions() as ViewResult;
            ContentModel resultsModel = result.Model as ContentModel;

            Boolean isTrue = resultsModel.RoleId == (int)Enums.Roles.Producer;
            isTrue = isTrue && String.Equals(resultsModel.RootPath, "C:/test/123_4/");
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void Actions_User()
        {
            aMockUserAuthCredentialService.Setup(aService => aService.GetByUsername("username")).Returns(userAuthCredentialUser);
            aMockSiteSettingsService.Setup(aService => aService.GetFilePath()).Returns("C:\\test\\123_4\\");
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(SiteSettings.Content)).Returns(new SiteSetting() { Value = "content" });

            ViewResult result = manageController.Actions() as ViewResult;
            ContentModel resultsModel = result.Model as ContentModel;

            Boolean isTrue = resultsModel.RoleId == (int)Enums.Roles.User;
            isTrue = isTrue && String.Equals(resultsModel.RootPath, "C:/test/123_4/");
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void Upload_Action_Test()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            serverMock.Setup(x => x.MapPath(@"C:\Stardome:")).Returns(@"c:\Stardome");
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);
            var sut = new ManageController();
            sut.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), sut);

            var file1Mock = new Mock<HttpPostedFileBase>();
            file1Mock.Setup(x => x.FileName).Returns("file1.txt");
            var file2Mock = new Mock<HttpPostedFileBase>();
            file2Mock.Setup(x => x.FileName).Returns("file2.doc");
            var files = new[] { file1Mock.Object, file2Mock.Object };
            var actual = sut.Actions(files, @"C:\Stardome");

            file1Mock.Verify(x => x.SaveAs(@"c:\Stardome\file1.txt"));
            file2Mock.Verify(x => x.SaveAs(@"c:\Stardome\file2.doc"));
        }
    }
}
