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
        Access GetAccessByFolderName(string FolderName);
        Access GetAccessByFolderPath(string FolderPath);
        List<Access> GetAccessByUserId(int UserId);
        String AddAccess(Access aAccess);
        String DeleteAccess(Access aAccess);
    }
}
