using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Repositories;
using Moq;
using Stardome.DomainObjects;
using Stardome.Services.Domain;

namespace Stardome.Tests.Services.Domain
{
    [TestClass]
    public class AccessServiceTests
    {

        readonly Mock<IAccessRepository> aMockRepository = new Mock<IAccessRepository>();

        private const int id1 = 1;
        private const int id2 = 2;
        private const int userId = 1;
        private static readonly Access acess1 = new Access { Id = id1, UserId = userId, FolderId = 1 };
        private static readonly Access acess2 = new Access { Id = id2, UserId = userId, FolderId = 2 };

        private List<Access> accessList = new List<Access>
        {
            acess1,
            acess2
        };

        private static readonly String folderName = "fn";
        private static readonly String folderPath = "fnpath";

        [TestMethod]
        public void GetAccessByFolderName_ShouldPass()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetAccessByFolderName(folderName, userId)).Returns(acess1);

            // Act
            var result = service.GetAccessByFolderName(folderName, userId);

            // Assert
            Assert.IsTrue(result.Id == id1);
        }

        [TestMethod]
        public void GetAccessByFolderPath_ShouldPass()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetAccessByFolderPath(folderPath, userId)).Returns(acess2);

            // Act
            var result = service.GetAccessByFolderPath(folderPath, userId);

            // Assert
            Assert.IsTrue(result.Id == id2);
        }

        [TestMethod]
        public void GetAccessByUserId_ShouldPass()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetAccessByUserId(userId)).Returns(accessList);

            // Act
            var result = service.GetAccessByUserId(userId);

            // Assert
            Assert.IsTrue(result.Count == 2);
        }

        [TestMethod]
        public void GetAccessByUserId_EmptyList_ShouldPass()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetAccessByUserId(userId)).Returns(new List<Access>());

            // Act
            var result = service.GetAccessByUserId(userId);

            // Assert
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void AddAccess_ShouldPass()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.AddAccess(acess1)).Returns(String.Empty);

            // Act
            var result = service.AddAccess(acess1);

            // Assert
            Assert.IsTrue(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void AddAccess_FailMessage()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.AddAccess(acess1)).Returns("error");

            // Act
            var result = service.AddAccess(acess1);

            // Assert
            Assert.IsTrue(!String.IsNullOrEmpty(result));
        } 

        [TestMethod]
        public void DeleteAccess_ShouldPass()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.DeleteAccess(acess1)).Returns(String.Empty);

            // Act
            var result = service.DeleteAccess(acess1);

            // Assert
            Assert.IsTrue(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void DeleteAccess_FailMessage()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.DeleteAccess(acess1)).Returns("error");

            // Act
            var result = service.DeleteAccess(acess1);

            // Assert
            Assert.IsTrue(!String.IsNullOrEmpty(result));
        } 

        [TestMethod]
        public void GetById_ShouldPass()
        {
            // Arrange
            AccessService service = new AccessService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetById(id1)).Returns(acess1);

            // Act
            var result = service.GetById(id1);

            // Assert
            Assert.IsTrue(result.Id == id1);
            Assert.IsTrue(result.UserId == 1);
            Assert.IsTrue(result.FolderId == 1);
        }
    }
}
