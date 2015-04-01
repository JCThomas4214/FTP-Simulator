using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Stardome.DomainObjects;

namespace Stardome.Repositories
{
    public class UserAuthCredentialRepository : BaseContentRepository<UserAuthCredential>, IUserAuthCredentialRepository
    {
        public StardomeEntitiesCS sdContext { get; private set; }

        public UserAuthCredentialRepository(StardomeEntitiesCS ctx)
            : base(ctx)
        {
            sdContext = ctx;
        }

        public override UserAuthCredential GetById(object id)
        {
            if (id is int)
            {
                return sdContext.UserAuthCredentials.SingleOrDefault(x => x.Id == (int) id);
            }
            return null;
        }

        public UserAuthCredential GetByUsername(string username)
        {
            return
                sdContext.UserAuthCredentials.SingleOrDefault(
                    aUser => aUser.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public UserAuthCredential GetByEmail(string email)
        {
            return
               (from aUserAuth in sdContext.UserAuthCredentials
                join aUserInfo in sdContext.UserInformations on
                  new { aUserAuth.Id } equals  new {Id = aUserInfo.UserId}
                where aUserInfo.Email.Equals(email,StringComparison.OrdinalIgnoreCase)
                select aUserAuth).SingleOrDefault();

        }

        public IEnumerable<UserAuthCredential> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<UserAuthCredential> GetObjectSet()
        {
            return sdContext.UserAuthCredentials;
        }
    }
}