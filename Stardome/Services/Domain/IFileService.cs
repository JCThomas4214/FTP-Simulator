using Stardome.DomainObjects;
using System.Collections.Generic;

namespace Stardome.Services.Domain
{
    public interface IFileService
    {
        IEnumerable<File> GetFolders();
    }
}