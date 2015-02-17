using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardome.Services.Application;

namespace Stardome.Tests.Services.Application
{
    [TestClass]
    public class AesEncryptionTests
    {
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AesEncrypt_plainTextIsNull()
        {
            // Arrange and Act
            string result = AesEncryption.Encrypt(null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AesEncrypt_plainTextIsEmpty()
        {
            // Arrange and Act
            string result = AesEncryption.Encrypt("");
        }

        [TestMethod]
        public void AesEncrypt_plainTextIsTest()
        {
            // Arrange 
            string plainText = "Test";

            // Act
            string result = AesEncryption.Encrypt(plainText);

            // Assert
            Assert.IsTrue(result.Equals("Rqx7DMiEIVI+jbIypf+7RA=="));
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AesDecrypt_cipherTextIsNull()
        {
            // Arrange and Act
            string result = AesEncryption.Decrypt(null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AesDecrypt_cipherTextIsEmpty()
        {
            // Arrange and Act
            string result = AesEncryption.Decrypt("");
        }

        [TestMethod]
        public void AesDecrypt_cipherTextIsTest()
        {
            // Arrange 
            string cipherText = "Rqx7DMiEIVI+jbIypf+7RA==";

            // Act
            string result = AesEncryption.Decrypt(cipherText);

            // Assert
            Assert.IsTrue(result.Equals("Test"));
        }
    }
}
