using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Stardome.Services.Application
{
    public class AesEncryption
    {
        private const string SPassPhrase = "St@rd0m3";
        private const string SSaltValue = "St@rd0m3S@lt";
        
        private const string SHashAlgorithm = "SHA1";
        private const string SInitVector = "@1b2C3d4E5f6G7h8"; // must be 16 bytes
        private const int SPasswordIterations = 2;
        private const int SKeySize = 256;

        public static string Encrypt(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(SInitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(SSaltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            var password = new PasswordDeriveBytes(
                                                            SPassPhrase,
                                                            saltValueBytes,
                                                            SHashAlgorithm,
                                                            SPasswordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(SKeySize / 8);

            // Create uninitialized Aes encryption object.
            var symmetricKey = new AesManaged();

            // Set encryption mode to Cipher Block Chaining
            symmetricKey.Mode = CipherMode.CBC;


            // Generate encryptor from the existing key bytes and initialization vector. 
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            var memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            var cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            var cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }

        public static string Decrypt(string cipherText)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            var initVectorBytes = Encoding.ASCII.GetBytes(SInitVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(SSaltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be derived. 
            var password = new PasswordDeriveBytes(
                                                            SPassPhrase,
                                                            saltValueBytes,
                                                            SHashAlgorithm,
                                                            SPasswordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(SKeySize / 8);

            // Create uninitialized Aes encryption object.
            var symmetricKey = new AesManaged();

            // Set encryption mode to Cipher Block Chaining
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization vector. 
            var decryptor = symmetricKey.CreateDecryptor(keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            var memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            var cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            var plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            var decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            var plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);

            // Return decrypted string.   
            return plainText;
        }
    }
}