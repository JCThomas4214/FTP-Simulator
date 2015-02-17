using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Stardome.DomainObjects;

namespace Stardome.Repositories
{
    public class FileRepository : BaseContentRepository<File>, IFileRepository
    {
        public StardomeEntitiesCS sdContext { get; private set; }

        public FileRepository(StardomeEntitiesCS ctx)
            : base(ctx)
        {
            sdContext = ctx;
        }

        public override File GetById(object id)
        {
            if (id is int)
            {
                return sdContext.Files.SingleOrDefault(x => x.Id == (int) id);
            }
            return null;
        }

        public IEnumerable<File> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<File> GetObjectSet()
        {
            return sdContext.Files;
        }
    }
}