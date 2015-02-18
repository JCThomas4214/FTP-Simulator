
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Stardome.DomainObjects
{

using System;
    using System.Collections.Generic;
    
public partial class Folder
{

    public Folder()
    {

        this.Accesses = new HashSet<Access>();

        this.Files = new HashSet<File>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedOn { get; set; }

    public string ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedOn { get; set; }

    public int StatusId { get; set; }



    public virtual ICollection<Access> Accesses { get; set; }

    public virtual ICollection<File> Files { get; set; }

    public virtual Status Status { get; set; }

}

}
