using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace FamilyTree.Data.BEANS
{
    public class relaBEAN
    {
        public relaBEAN() { }

        //from Relationships
        public int relationshipID { get; set; }
        public int personID { get; set; }
        public int relativeID { get; set; }
        public int relationshipTypeID { get; set; }
        public int relativeRole { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Relationship Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? relationshipStartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Relationship End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? relationshipEndDate { get; set; }
        [Display(Name ="Notable Information")]
        public string notableInformation { get; set; }
        public int familyID { get; set; }

        //from Individuals
        [Display(Name ="Full Name of Individual")]
        public string fullName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfBirth { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Death")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfDeath { get; set; }
        [Display(Name ="Gender")]
        public string gender { get; set; }
        [Display(Name ="Place of Birth")]
        public string placeOfBirth { get; set; }

        //from Type
        [Display(Name ="Relationship Type")]
        public string typeDescription { get; set; }

        //from Roles
        [Display(Name ="Role in Relationship")]
        public string roleDescription { get; set; }

    }
}
