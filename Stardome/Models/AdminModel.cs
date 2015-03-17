using System;
using System.Collections.Generic;

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
        public List<string> Categories { get; set; }
        public List<Tuple<string, string, string, int>> Settings { get; set; } 
    }

    public class ContentModel : MainModel
    {
        public string RootPath { get; set; }   
        public string SelectedDir { get; set; }
    }
}
