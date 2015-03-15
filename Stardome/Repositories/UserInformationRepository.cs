using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Stardome.DomainObjects;

namespace Stardome.Repositories
{
    public class UserInformationRepository : BaseContentRepository<UserInformation>, IUserInformationRepository
    {
        public StardomeEntitiesCS sdContext { get; private set; }

        public UserInformationRepository(StardomeEntitiesCS ctx)
            : base(ctx)
        {
            sdContext = ctx;
        }

        public override UserInformation GetById(object id)
        {
            if (id is int)
            {
                return sdContext.UserInformations.SingleOrDefault(x => x.Id == (int)id);
            }
            return null;
        }

        public IEnumerable<UserInformation> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<UserInformation> GetObjectSet()
        {
            return sdContext.UserInformations;
        }
    }
}