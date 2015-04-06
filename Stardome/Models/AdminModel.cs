using System.Collections.Generic;
using Stardome.DomainObjects;
using System.Web.Mvc;

namespace Stardome.Models
{
    public class MainModel
    {
        public int RoleId { get; set; }
        public JsonResult RolesList { get; set; }
    }

    public class User : MainModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public string DisplayName {
            get
            {
                return FirstName + " " + LastName + "(" + Username + ")";
            }
         }
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
        public List<User> UserList { get; set; }
    }

    public class UploadFilesResult
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
    }


}
