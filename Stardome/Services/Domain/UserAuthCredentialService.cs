using Stardome.DomainObjects;
using Stardome.Repositories;

namespace Stardome.Services.Domain
{
    public class UserAuthCredentialService : IUserAuthCredentialService
    {
        private readonly IUserAuthCredentialRepository repository;

        public UserAuthCredentialService(IUserAuthCredentialRepository aRepository)
        {
            repository = aRepository;
        }

        public UserAuthCredential GetById(int aUserAuthCredentialId)
        {
            return repository.GetById(aUserAuthCredentialId);
        }

        public UserAuthCredential GetByUsername(string aUsername)
        {
            return repository.GetByUsername(aUsername);
        }
    }
}