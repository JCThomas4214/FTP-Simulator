using System.Collections.Generic;
using Stardome.DomainObjects;
using Stardome.Infrastructure.Repository;
using Stardome.Models;

namespace Stardome.Repositories
{
    public interface IFolderRepository : IObjectRepository<Folder>
    {
        IEnumerable<Folder> GetAll();

        string AddFolder(Folder folder);

        string DeleteFolder(Folder folder);
        Folder GetFolderByFolderName(string FolderName);

        Folder GetFolderByFolderPath(string FolderPath);
    }
}