using Stardome.DomainObjects;
using Stardome.Repositories;
using System;
using System.Collections.Generic;

namespace Stardome.Services.Domain
{
    public class AccessService : IAccessService 
    {
        private readonly IAccessRepository repository;

        public AccessService(IAccessRepository aRepository)
        {
            repository = aRepository;
        }

        public Access GetAccessByFolderName(string folderName, int userId)
        {
            return repository.GetAccessByFolderName(folderName, userId);
        }


        public Access GetAccessByFolderPath(string folderPath, int userId)
        {
            return repository.GetAccessByFolderPath(folderPath, userId);
        }

        public List<Access> GetAccessByUserId(int userId)
        {
            return repository.GetAccessByUserId(userId);
        }

        public String AddAccess(Access anAccess)
        {
            return repository.AddAccess(anAccess);
        }


        public Access GetById(int id)
        {
            return repository.GetById(id);
        }

        public String DeleteAccess(Access anAccess)
        {
            return repository.DeleteAccess(anAccess);
        }

    
    }


}