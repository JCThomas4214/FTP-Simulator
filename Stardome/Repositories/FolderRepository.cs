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

        public string AddFolder(Folder folder)
        {
            string msg="Updated successfully";
            try
            {
                sdContext.Folders.Add(folder);
                sdContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
            }
            
            return msg;
        }

        public string DeleteFolder(Folder folder)
        {
            string msg = "Deleted successfully";
            try
            {
                Delete(folder);
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }

        public Folder GetFolderByFolderName(string FolderName)
        {
            return sdContext.Folders.SingleOrDefault(x => x.Name == FolderName);
        }

        public Folder GetFolderByFolderPath(string FolderPath)
        {
            return sdContext.Folders.SingleOrDefault(x => x.Path == FolderPath);
        }
    }
}