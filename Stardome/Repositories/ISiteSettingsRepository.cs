using System;
using System.Collections.Generic;
using Stardome.DomainObjects;
using Stardome.Infrastructure.Repository;


namespace Stardome.Repositories
{
    public interface ISiteSettingsRepository : IObjectRepository<SiteSetting>
    {
        String UpdateSiteSettings(List<SiteSetting> lstSiteSettings);

        IEnumerable<SiteSetting> GetAll();
    }
}
