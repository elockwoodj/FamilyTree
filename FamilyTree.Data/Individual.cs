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
    
    public partial class Individual
    {
        public int individualID { get; set; }
        public string fullName { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public Nullable<System.DateTime> dateOfDeath { get; set; }
        public string gender { get; set; }
        public string placeOfBirth { get; set; }
        public int familyID { get; set; }
    }
}
