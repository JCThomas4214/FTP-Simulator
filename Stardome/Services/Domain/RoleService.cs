using System.Collections.Generic;
using Stardome.DomainObjects;
using Stardome.Repositories;

namespace Stardome.Services.Domain
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository repository;

        public RoleService(IRoleRepository aRepository)
        {
            repository = aRepository;
        }

        public IEnumerable<Role> GetRoles()
        {
            return repository.GetAll();
        }
    }
}