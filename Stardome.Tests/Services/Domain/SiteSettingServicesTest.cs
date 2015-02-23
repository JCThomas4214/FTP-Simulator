using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Repositories;
using Moq;
using Stardome.DomainObjects;
using Stardome.Services.Domain;
using System.Collections.Generic;

namespace Stardome.Tests.Services.Domain
{
    [TestClass]
    public class SiteSettingServicesTest
    {

        readonly Mock<ISiteSettingsRepository> aMockRepository = new Mock<ISiteSettingsRepository>();

        private static int id = 1;
        private static string Name = "Site Name";
        private static string Value = "Stardome";
        private SiteSetting siteSettings1 = new SiteSetting{ Id = id, Name = Name, Value = Value };

        private SiteSetting siteSettings2 = new SiteSetting { Id = 2, Name = "File Path", Value = "C:\\Stardome"};

        private List<SiteSetting> lstSiteSettings= new List<SiteSetting>();
            
        

        [TestMethod]
        public void GetByIdTest()
        {
            // Arrange
            SiteSettingsService service = new SiteSettingsService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetById(id)).Returns(siteSettings1);

            // Act
            var result = service.GetById(id);

            // Assert
            Assert.IsTrue(result.Id == id);
            Assert.IsTrue(result.Value == Value);
            Assert.IsTrue(result.Name == Name);
        }


        [TestMethod]
        public void GetSiteSettingsTest()
        {
            lstSiteSettings.Add(siteSettings1);
            lstSiteSettings.Add(siteSettings2);
            SiteSettingsService service = new SiteSettingsService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetAll()).Returns(lstSiteSettings);

            // Act
            List<SiteSetting> result =(List<SiteSetting>) service.GetAll();

            // Assert
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result[0].Name== "Site Name");
            Assert.IsTrue(result[1].Name == "File Path");
            

        }
        
    }
}
