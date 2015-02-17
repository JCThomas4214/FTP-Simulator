using Stardome.DomainObjects;
using Stardome.Repositories;
using System.Collections.Generic;

namespace Stardome.Services.Domain
{
    public class FileService : IFileService
    {
        private readonly IFileRepository repository;

        public FileService(IFileRepository aRepository)
        {
            repository = aRepository;
        }

        public IEnumerable<File> GetFolders()
        {
            return repository.GetAll();
        }
    }
}