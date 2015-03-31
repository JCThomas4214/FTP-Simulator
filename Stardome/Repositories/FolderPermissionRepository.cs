using Stardome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stardome.Repositories
{
    public class FolderPermissionRepository:IFolderPermissionRepository
    {

        FolderUserAccessModel GetFoldersForUser(int UserId)
        {
            return null;
        }

        String UpdateFolderPermissions(int UserId, List<string> SelectedFolders)
        {
            return "";
        }
    }
}