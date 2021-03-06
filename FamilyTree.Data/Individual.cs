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


    public partial class Individual
    {
        public int individualID { get; set; }
        [Display(Name = "Full Name")]
        public string fullName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Death")]
        public Nullable<System.DateTime> dateOfDeath { get; set; }

        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Display(Name = "Place of Birth")]
        public string placeOfBirth { get; set; }

        public int familyID { get; set; }
        public int isParent { get; set; }
        public Nullable<int> coupleID { get; set; }

        [Display(Name = "Notes")]
        public string notes { get; set; }
    }
}
