using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stardome.DomainObjects;
using Stardome.Repositories;
using Stardome.Services.Application;

namespace Stardome.Services.Domain
{
    public class SiteSettingsService
    {
        private readonly SiteSettingsRepository repository;
        public SiteSettingsService(SiteSettingsRepository aRepository)
        {
            repository = aRepository;
        }

        public SiteSetting GetById(int aSiteSettingId)
        {
            return repository.GetById(aSiteSettingId);
        }

        public String GetSiteName
        {
            get{return repository.GetById(1).Value;}
        }

        public String GetFilePath
        {
            get{return repository.GetById(2).Value;}
        }
   
        public IEnumerable<SiteSetting> GetSiteSettings()
        {
            return repository.GetAll();
        }


        public String UpdateSiteSettings(List<Stardome.DomainObjects.SiteSetting> lstSiteSettings)
        {
            return repository.UpdateSiteSettings(lstSiteSettings);
           
        }
    }
}