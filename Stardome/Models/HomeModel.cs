using System.Collections.Generic;
using Stardome.DomainObjects;

namespace Stardome.Models
{
    public class UserManagement
        {
            public IList<UserAuthCredential> UserList { get; set; }
            public IList<Role> Roles { get; set; } 
        }
}
