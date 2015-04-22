using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Repositories;
using Moq;
using Stardome.DomainObjects;
using Stardome.Services.Domain;

namespace Stardome.Tests.Services.Domain
{
    [TestClass]
    public class FolderServiceTests
    {

        readonly Mock<IFolderRepository> aMockRepository = new Mock<IFolderRepository>();

        private const int id1 = 1;
        private const int id2 = 2;
        private const int userId = 1;
        private static readonly Folder folder1 = new Folder { Id = id1, Name = "folder1", Path = "path1" }; 
        private static readonly Folder folder2 = new Folder { Id = id2, Name = "folder2", Path = "path2" };

        private List<Folder> folderList = new List<Folder>
        {
            folder1,
            folder2
        };

        private const String folderName = "fn";
        private const String folderPath2 = "path2";
        /*



        public IEnumerable<Folder> GetFoldersStartingFolderPath(string folderPath)
        {
            return repository.GetFoldersStartingFolderPath(folderPath);
        }*/
        [TestMethod]
        public void GetFoldersStartingFolderPath_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetFoldersStartingFolderPath(folderPath2)).Returns(new List<Folder>{folder1});

            // Act
            var result = service.GetFoldersStartingFolderPath(folderPath2);

            // Assert
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void GetFoldersStartingFolderPath_Empty_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetFoldersStartingFolderPath(String.Empty)).Returns((IEnumerable<Folder>) null);

            // Act
            var result = service.GetFoldersStartingFolderPath(String.Empty);

            // Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void GetFolders_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetAll()).Returns(new List<Folder>{folder1, folder2});

            // Act
            var result = service.GetFolders();

            // Assert
            Assert.IsTrue(result.Count() == 2);
        }

        [TestMethod]
        public void GetFolders_Empty_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetAll()).Returns((IEnumerable<Folder>) null);

            // Act
            var result = service.GetFolders();

            // Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void GetFolderByFolderName_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetFolderByFolderName(folderName)).Returns(folder1);

            // Act
            var result = service.GetFolderByFolderName(folderName);

            // Assert
            Assert.IsTrue(result.Id == id1);
        }

        [TestMethod]
        public void GetFolderByFolderName_Empty_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetFolderByFolderName(folderName)).Returns((Folder) null);

            // Act
            var result = service.GetFolderByFolderName(folderName);

            // Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void GetFolderByFolderPath_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetFolderByFolderPath(folderPath2)).Returns(folder2);

            // Act
            var result = service.GetFolderByFolderPath(folderPath2);

            // Assert
            Assert.IsTrue(result.Id == id2);
        }

        [TestMethod]
        public void AddFolder_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.AddFolder(folder1)).Returns(String.Empty);

            // Act
            var result = service.AddFolder(folder1);

            // Assert
            Assert.IsTrue(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void AddFolder_FailMessage()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.AddFolder(folder1)).Returns("error");

            // Act
            var result = service.AddFolder(folder1);

            // Assert
            Assert.IsTrue(!String.IsNullOrEmpty(result));
        } 

        [TestMethod]
        public void DeleteFolder_ShouldPass()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.DeleteFolder(folder1)).Returns(String.Empty);

            // Act
            var result = service.DeleteFolder(folder1);

            // Assert
            Assert.IsTrue(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void DeleteFolder_FailMessage()
        {
            // Arrange
            FolderService service = new FolderService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.DeleteFolder(folder1)).Returns("error");

            // Act
            var result = service.DeleteFolder(folder1);

            // Assert
            Assert.IsTrue(!String.IsNullOrEmpty(result));
        } 
    }
}
