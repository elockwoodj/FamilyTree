using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Data.BEANS
{
    public class memberBEAN
    {
        public memberBEAN() { }

        public int individualID { get; set; }
        public string fullName { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public DateTime? dateOfDeath { get; set; }
        public string gender { get; set; }
        public string placeOfBirth { get; set; }
        public int familyID { get; set; }
    }
}
