using Stardome.DomainObjects;

namespace Stardome.Services.Domain
{
    public interface IUserAuthCredentialService
    {
        UserAuthCredential GetById(int aUserAuthCredentialId);
        UserAuthCredential GetByUsername(string aUsername);
    }
}