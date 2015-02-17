<<<<<<< HEAD
﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Controllers;

namespace Stardome.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Users()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Users() as ViewResult;

            // Assert
            Assert.AreEqual("User Management Page", result.ViewBag.Message);
        }

        [TestMethod]
        public void Content()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Content() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Settings()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Settings() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
=======
﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Controllers;

namespace Stardome.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Users()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Users() as ViewResult;

            // Assert
            Assert.AreEqual("User Management Page", result.ViewBag.Message);
        }

        [TestMethod]
        public void Content()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Content() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Settings()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Settings() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
>>>>>>> aba16b88a015fe8ab76ee47dc611a91b823828d4
