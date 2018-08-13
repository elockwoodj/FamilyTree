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

    public partial class Relationship
    {
        public int relationshipID { get; set; }
        public int personID { get; set; }
        public int relativeID { get; set; }
        public int relationshipTypeID { get; set; }
        public int relativeRole { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Relationship Start Date")]
        public Nullable<System.DateTime> relationshipStartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Relationship End Date")]
        public Nullable<System.DateTime> relationshipEndDate { get; set; }
        [Display(Name = "Notable Information")]

        public string notableInformation { get; set; }
        public int familyID { get; set; }
    }
}
