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
    
    public partial class Relationship
    {
        public int relationshipID { get; set; }
        public int individualOneID { get; set; }
        public int individualTwoID { get; set; }
        public Nullable<int> famID { get; set; }
        public int relationshipTypeID { get; set; }
        public int individualOneRole { get; set; }
        public int individualTwoRole { get; set; }
        public Nullable<System.DateTime> relationshipStartDate { get; set; }
        public Nullable<System.DateTime> relationshipEndDate { get; set; }
        public string notableInformation { get; set; }
    }
}
