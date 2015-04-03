using Stardome.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stardome.Services.Domain
{
    public interface IAccessService
    {
        Access GetAccessByFolderName(string FolderName, int UserID);
         Access GetAccessByFolderPath(string FolderPath, int UserID);
        List<Access> GetAccessByUserId(int UserId);
        String AddAccess(Access aAccess);
        String DeleteAccess(Access aAccess);
    }
}
