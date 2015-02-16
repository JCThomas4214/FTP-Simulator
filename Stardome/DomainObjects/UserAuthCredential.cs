//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Stardome.DomainObjects
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAuthCredential
    {
        public UserAuthCredential()
        {
            this.Accesses = new HashSet<Access>();
            this.Logs = new HashSet<Log>();
            this.UserInformations = new HashSet<UserInformation>();
        }
    
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> PasswordModifiedOn { get; set; }
        public Nullable<System.DateTime> LastLoginDateTime { get; set; }
        public Nullable<System.DateTime> FailedLoginAttemptDateTime { get; set; }
        public Nullable<int> NumFailedLoginAttempts { get; set; }
        public System.DateTime AccountCreatedOn { get; set; }
        public int Id { get; set; }
    
        public virtual ICollection<Access> Accesses { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserInformation> UserInformations { get; set; }
    }
}