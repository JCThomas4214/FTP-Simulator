using System.Collections.Generic;
using Stardome.DomainObjects;

namespace Stardome.Services.Domain
{
    public interface IUserAuthCredentialService
    {
        UserAuthCredential GetById(int aUserAuthCredentialId);
        UserAuthCredential GetByUsername(string aUsername);
        string EncryptPassword(string password);
        string DecryptPassword(string password);
        IEnumerable<UserAuthCredential> GetUserAuthCredentials();
    }
}