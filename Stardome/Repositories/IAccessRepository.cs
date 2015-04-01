using Stardome.DomainObjects;
using Stardome.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stardome.Repositories
{
    public interface IAccessRepository : IObjectRepository<Access>
    {
        Access GetAccessByFolderName(string FolderName);
        Access GetAccessByFolderPath(string FolderPath);
        List<Access> GetAccessByUserId(int UserId);
        String AddAccess(Access aAccess);
        String DeleteAccess(Access aAccess);

    }
}
