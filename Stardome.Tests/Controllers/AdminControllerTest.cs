using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stardome.Controllers;
using Stardome.DomainObjects;
using Stardome.Services.Domain;

namespace Stardome.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTest
    {
        static readonly Mock<IUserAuthCredentialService> aMockUserAuthCredentialService = new Mock<IUserAuthCredentialService>();
        static readonly Mock<IUserInformationService> aMockUserInformationService = new Mock<IUserInformationService>();
        static readonly Mock<ISiteSettingsService> aMockSiteSettingsService = new Mock<ISiteSettingsService>();


        AdminController controller = new AdminController(aMockUserAuthCredentialService.Object, aMockUserInformationService.Object, aMockSiteSettingsService.Object);
        [TestMethod]
        public void Users()
        {

            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(Headers.Users)).Returns(new SiteSetting(){Value = "users"});
            ViewResult result = controller.Users() as ViewResult;

            Assert.AreEqual("users", result.ViewBag.Message);
        }

        [TestMethod]
        public void Content()
        {
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(Headers.Content)).Returns(new SiteSetting() { Value = "content" });
            ViewResult result = controller.Content() as ViewResult;

            Assert.AreEqual("content", result.ViewBag.Message);
        }

        [TestMethod]
        public void Settings()
        {
            IEnumerable<SiteSetting> siteSettings = new List<SiteSetting>
            {
                new SiteSetting{ Category = "category", Name = "name", Value = "value", Id = 0 },
                new SiteSetting{ Category = "category1", Name = "name1", Value = "value1", Id = 1 }
            };
            aMockSiteSettingsService.Setup(aService => aService.GetAll()).Returns(siteSettings);
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(Headers.Settings)).Returns(new SiteSetting() { Value = "settings" });

            ViewResult result = controller.Settings() as ViewResult;
            List<SiteSetting> resultList = result.Model as List<SiteSetting>;
            Boolean isSame = resultList[0].Category == "category" && resultList[0].Name == "name" && resultList[0].Value == "value";
            isSame = isSame && resultList[1].Category == "category1" && resultList[1].Name == "name1" && resultList[1].Value == "value1";

            Assert.IsTrue(isSame);
        }
        [TestMethod]
        public void Settings_NoSettings()
        {
            IEnumerable<SiteSetting> siteSettings = new List<SiteSetting>();
            aMockSiteSettingsService.Setup(aService => aService.GetAll()).Returns(siteSettings);
            aMockSiteSettingsService.Setup(aService => aService.FindSiteSetting(Headers.Settings)).Returns(new SiteSetting() { Value = "settings" });

            // Act
            ViewResult result = controller.Settings() as ViewResult;

            List<SiteSetting> resultList = result.Model as List<SiteSetting>;
            Assert.IsTrue(resultList.Count == 0);
        }

        [TestMethod]
        public void GetUsers()
        {
            // Act
            ViewResult result = controller.Settings() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
