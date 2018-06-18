using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Data.BEANS
{
    public class relBEAN
    {
        public relBEAN() { }

        //from Relationships
        public int relationshipID { get; set; }
        public int personID { get; set; }
        public int relativeID { get; set; }
        public int relationshipTypeID { get; set; }
        public int relativeRole { get; set; }
        public DateTime? relationshipStartDate { get; set; }
        public DateTime? relationshipEndDate { get; set; }
        public string notableInformation { get; set; }
        public int familyID { get; set; }

        //from Individuals
        public string fullName { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public DateTime? dateOfDeath { get; set; }
        public string gender { get; set; }
        public string placeOfBirth { get; set; }
    }
}
