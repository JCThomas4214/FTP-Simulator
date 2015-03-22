using System.Collections.Generic;
using Stardome.DomainObjects;

namespace Stardome.Services.Domain
{
    public interface IUserAuthCredentialService
    {
        UserAuthCredential GetById(int aUserAuthCredentialId);
        UserAuthCredential GetByUsername(string aUsername);
        UserAuthCredential GetByEmail(string aEmail);
        string EncryptPassword(string password);
        string DecryptPassword(string password);
        IEnumerable<UserAuthCredential> GetUserAuthCredentials();
        void DeleteAUser(UserAuthCredential aUser);
        void UpdateAUser(UserAuthCredential aUser);
    }
}