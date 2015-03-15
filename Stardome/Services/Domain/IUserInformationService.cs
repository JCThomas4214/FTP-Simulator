using Stardome.DomainObjects;

namespace Stardome.Services.Domain
{
    public interface IUserInformationService
    {
        UserInformation GetById(int aUserInformationId);
        void DeleteAUser(UserInformation aUser);
    }
}