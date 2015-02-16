using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Stardome.DomainObjects;

namespace Stardome.Repositories
{
    public class RoleRepository : BaseContentRepository<Role>, IRoleRepository
    {
        public StardomeEntitiesCS sdContext { get; private set; }

        public RoleRepository(StardomeEntitiesCS ctx)
            : base(ctx)
        {
            sdContext = ctx;
        }

        public override Role GetById(object id)
        {
            if (id is int)
            {
                return sdContext.Roles.SingleOrDefault(x => x.Id == (int) id);
            }
            return null;
        }

        public IEnumerable<Role> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<Role> GetObjectSet()
        {
            return sdContext.Roles;
        }
    }
}