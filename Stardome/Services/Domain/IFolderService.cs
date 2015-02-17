using System.Collections.Generic;
using Stardome.DomainObjects;

namespace Stardome.Services.Domain
{
    public interface IFolderService
    {
        IEnumerable<Folder> GetFolders();
    }
}