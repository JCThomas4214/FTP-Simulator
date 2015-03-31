using Stardome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stardome.Repositories
{
    public interface IFolderPermissionRepository
    {
        FolderUserAccessModel GetFoldersForUser(int UserId);

        String UpdateFolderPermissions(int UserId, List<string> SelectedFolders);
    }
}
