using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Stardome.DomainObjects;

namespace Stardome.Repositories
{
    public class FolderRepository : BaseContentRepository<Folder>, IFolderRepository
    {
        public StardomeEntitiesCS sdContext { get; private set; }

        public FolderRepository(StardomeEntitiesCS ctx)
            : base(ctx)
        {
            sdContext = ctx;
        }

        public override Folder GetById(object id)
        {
            if (id is int)
            {
                return sdContext.Folders.SingleOrDefault(x => x.Id == (int) id);
            }
            return null;
        }

        public IEnumerable<Folder> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<Folder> GetObjectSet()
        {
            return sdContext.Folders;
        }
    }
}