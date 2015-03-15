using Stardome.DomainObjects;
using Stardome.Repositories;

namespace Stardome.Services.Domain
{
    public class UserInformationService : IUserInformationService
    {
        private readonly IUserInformationRepository repository;

        public UserInformationService(IUserInformationRepository aRepository)
        {
            repository = aRepository;
        }

        public UserInformation GetById(int aUserInformationId)
        {
            return repository.GetById(aUserInformationId);
        }

        public void DeleteAUser(UserInformation aUser)
        {
            repository.Delete(aUser);
        }
    }
}