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
    }
}