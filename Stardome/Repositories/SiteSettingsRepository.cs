using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stardome.DomainObjects;
using System.Data.Entity;

namespace Stardome.Repositories
{
    public class SiteSettingsRepository : BaseContentRepository<SiteSetting>
    {
        public StardomeEntitiesCS sdContext { get; private set; }

        public SiteSettingsRepository(StardomeEntitiesCS ctx)
            : base(ctx)
        {
            sdContext = ctx;
            
        }
        public override SiteSetting GetById(object id)
        {
            if (id is int)
            {
                return sdContext.SiteSettings.SingleOrDefault(x => x.Id == (int)id);
            }
            return null;
        }

        public IEnumerable<SiteSetting> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<SiteSetting> GetObjectSet()
        {
            return sdContext.SiteSettings;
        }
    }
}