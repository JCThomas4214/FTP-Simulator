using System;
using System.Collections.Generic;
using System.Linq;
using Stardome.DomainObjects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Stardome.Repositories
{
    public class SiteSettingsRepository : BaseContentRepository<SiteSetting>, ISiteSettingsRepository
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

        public String UpdateSiteSettings(List<SiteSetting> lstSiteSettings)
        {
            try
            {
                foreach (SiteSetting siteSettings in lstSiteSettings)
                {
                    sdContext.SiteSettings.Attach(siteSettings);
                    DbEntityEntry<SiteSetting> entry = sdContext.Entry(siteSettings);
                    entry.State = System.Data.EntityState.Modified;
                    sdContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Site Settings Updated Successfully....";
        }
    }
}