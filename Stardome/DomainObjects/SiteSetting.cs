
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
    using System.ComponentModel.DataAnnotations;
    
public partial class SiteSetting
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Settings Name is required")]
    
    public string Name { get; set; }

    [Required(ErrorMessage = "A Value is required for Settings")]
    public string Value { get; set; }

}

}