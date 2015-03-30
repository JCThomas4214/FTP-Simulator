using Stardome.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stardome.Models
{
    public class Structure
    {
        public IList<Folder> folder { get; set; }
        public IList<File> files { get; set; }
    }

    public class FolderUserAccessModel : Structure
    {
        public User User { get; set; }
        public Status FolderStatus { get; set; }
    }
}