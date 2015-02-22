using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stardome.DomainObjects;
using Stardome.Infrastructure.Repository;
using Stardome.Models;


namespace Stardome.Repositories
{
    public interface ISiteSettingsRepository : IObjectRepository<SiteSetting>
    {
        String UpdateSiteSettings(List<Stardome.DomainObjects.SiteSetting> lstSiteSettings);

        IEnumerable<SiteSetting> GetAll();

        

    }
}
