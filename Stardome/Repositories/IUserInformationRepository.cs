using Stardome.DomainObjects;
using Stardome.Infrastructure.Repository;

namespace Stardome.Repositories
{
    public interface IUserInformationRepository : IObjectRepository<UserInformation>
    {
        void Add(UserInformation user);
    }
}