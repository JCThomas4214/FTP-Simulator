using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stardome.DomainObjects;
using Stardome.Repositories;
using Stardome.Services.Application;

namespace Stardome.Services.Domain
{
    interface ISiteSettingsService
    {

        SiteSetting GetById(int aSiteSettingId);
        IEnumerable<SiteSetting> GetAll();
        String UpdateSiteSettings(List<Stardome.DomainObjects.SiteSetting> lstSiteSettings);
    }
}
