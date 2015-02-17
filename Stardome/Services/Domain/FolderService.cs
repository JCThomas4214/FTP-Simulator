using Stardome.DomainObjects;
using Stardome.Repositories;
using System.Collections.Generic;

namespace Stardome.Services.Domain
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository repository;

        public FolderService(IFolderRepository aRepository)
        {
            repository = aRepository;
        }

        public IEnumerable<Folder> GetFolders()
        {
            return repository.GetAll();
        }
    }
}