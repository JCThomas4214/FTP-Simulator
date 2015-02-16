using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stardome.DomainObjects;
using Stardome.Repositories;
using Stardome.Services.Domain;


namespace Stardome.Tests.Services.Domain
{
    [TestClass]
    public class UserAuthCredentialServiceTests
    {
        readonly Mock<IUserAuthCredentialRepository> aMockRepository = new Mock<IUserAuthCredentialRepository>();

        private static int id = 1;
        private static string username = "usn";
        private UserAuthCredential userAuthCredential = new UserAuthCredential { Id = id, Username = username };

        [TestMethod]
        public void GetById()
        {
            // Arrange
            UserAuthCredentialService service = new UserAuthCredentialService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetById(id)).Returns(userAuthCredential);

            // Act
            var result = service.GetById(id);

            // Assert
            Assert.IsTrue(result.Id == id);
        }

        [TestMethod]
        public void GetById_Null()
        {
            // Arrange
            UserAuthCredentialService service = new UserAuthCredentialService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetById(id)).Returns((UserAuthCredential)null);

            // Act
            var result = service.GetById(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetByUsername()
        {
            // Arrange
            UserAuthCredentialService service = new UserAuthCredentialService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetByUsername(username)).Returns(userAuthCredential);

            // Act
            var result = service.GetByUsername(username);

            // Assert
            Assert.IsTrue(result.Username.Equals(username));
        }

        [TestMethod]
        public void GetByUsername_Null()
        {
            // Arrange
            UserAuthCredentialService service = new UserAuthCredentialService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.GetByUsername(username)).Returns((UserAuthCredential)null);

            // Act
            var result = service.GetByUsername(username);

            // Assert
            Assert.IsNull(result);
        }
    }
}
