using System;
using System.Collections.Generic;
using Stardome.DomainObjects;

namespace Stardome.Models
{
    public class MainModel
    {
        public int RoleId { get; set; }
    }

    public class User : MainModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
    }

    public class SettingModel : MainModel
    {
        public List<SiteSetting> Settings { get; set; } 
    }

    public class ContentModel : MainModel
    {
        public string RootPath { get; set; }   
        public string SelectedDir { get; set; }
        public List<string> List { get; set; }
    }
}
