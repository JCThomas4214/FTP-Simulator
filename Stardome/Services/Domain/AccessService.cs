using Stardome.DomainObjects;
using Stardome.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stardome.Services.Domain
{
    public class AccessService : IAccessService 
    {
        private readonly IAccessRepository repository;

        public AccessService(IAccessRepository aRepository)
        {
            repository = aRepository;
        }

        public Access GetAccessByFolderName(string FolderName, int UserID)
        {
            return repository.GetAccessByFolderName(FolderName, UserID);
        }
        public Access GetAccessByFolderPath(string FolderPath, int UserID)
        {
            return repository.GetAccessByFolderPath(FolderPath, UserID);
        }
        public List<Access> GetAccessByUserId(int UserId)
        {
            return repository.GetAccessByUserId(UserId);
        }

        public String AddAccess(Access aAccess)
        {
            return repository.AddAccess(aAccess);
        }
        public String DeleteAccess(Access aAccess)
        {
            return repository.DeleteAccess(aAccess);
        }

    
    }


}