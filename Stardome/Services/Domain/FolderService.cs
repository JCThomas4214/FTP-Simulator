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

        public string AddFolder(Folder folder)
        {
            return repository.AddFolder(folder);
        }

        public string DeleteFolder(Folder folder)
        {
            return repository.DeleteFolder(folder);
        }

        public Folder GetFolderByFolderName(string folderName)
        {
            return repository.GetFolderByFolderName(folderName);
        }

        public Folder GetFolderByFolderPath(string folderPath)
        {
            return repository.GetFolderByFolderPath(folderPath);
        }

        public IEnumerable<Folder> GetFoldersStartingFolderPath(string folderPath)
        {
            return repository.GetFoldersStartingFolderPath(folderPath);
        }
    }
}