//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FamilyTree.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Family
    {
        public int familyID { get; set; }

        [Display(Name = "Owner's User Name")]
        public string ownerUserName { get; set; }

        [Display(Name = "Family Name")]
        public string familyName { get; set; }
    }
}
