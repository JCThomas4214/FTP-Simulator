using System.Collections.Generic;
using Stardome.DomainObjects;
using Stardome.Infrastructure.Repository;

namespace Stardome.Repositories
{
    public interface IRoleRepository : IObjectRepository<Role>
    {
        IEnumerable<Role> GetAll();
    }
}
