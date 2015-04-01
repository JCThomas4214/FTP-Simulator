using System.Collections.Generic;
using Stardome.DomainObjects;

namespace Stardome.Services.Domain
{
    public interface IFolderService
    {
        IEnumerable<Folder> GetFolders();
        string AddFolder(Folder folder);
        string DeleteFolder(Folder folder);
        Folder GetFolderByFolderName(string FolderName);
        Folder GetFolderByFolderPath(string FolderPath);
    }
}