using Stardome.DomainObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Stardome.Repositories
{
    public class AccessRepository : BaseContentRepository<Access>, IAccessRepository
    {
        public StardomeEntitiesCS sdContext { get; private set; }

        public AccessRepository(StardomeEntitiesCS ctx)
            : base(ctx)
        {
            sdContext = ctx;
        }

        public override Access GetById(object id)
        {
            if (id is int)
            {
                return sdContext.Accesses.SingleOrDefault(x => x.Id == (int)id);
            }
            return null;
        }
        public override DbSet<Access> GetObjectSet()
        {
            return sdContext.Accesses;
        }

        public Access GetAccessByFolderName(string FolderName, int UserID)
        {
            Folder f = sdContext.Folders.FirstOrDefault(x => x.Name == FolderName);
           return sdContext.Accesses.SingleOrDefault(a => a.FolderId == f.Id && a.UserId == UserID );
        }

        public Access GetAccessByFolderPath(string FolderPath, int UserID)
        {
                Folder f = sdContext.Folders.FirstOrDefault(x => x.Name == FolderPath);
                return sdContext.Accesses.SingleOrDefault(a => a.FolderId == f.Id && a.UserId == UserID);

        }
        public List<Access> GetAccessByUserId(int UserId)
        {
            return Find(aAccess => aAccess.UserId.Equals(UserId)).ToList();
        }


        public String AddAccess(Access aAccess)
        {
            string msg = "Added successfully";
            try
            {
                sdContext.Accesses.Add(aAccess);
                sdContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }
        public String DeleteAccess(Access aAccess)
        {
            string msg = "Deleted successfully";
            try
            {
                Delete(aAccess);
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }

    }
}