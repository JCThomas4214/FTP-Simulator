using System;
using System.Collections.Generic;
using Stardome.DomainObjects;

namespace Stardome.Services.Domain
{
    public interface ISiteSettingsService
    {

        SiteSetting GetById(int aSiteSettingId);
        IEnumerable<SiteSetting> GetAll();
        String UpdateSiteSettings(List<SiteSetting> lstSiteSettings);
        SiteSetting FindSiteSetting(String header);
        string GetFilePath();
    }
}
