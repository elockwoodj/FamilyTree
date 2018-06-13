using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Data.BEANS
{
    public class TreeBEAN
    {
        public TreeBEAN() { }

        public int individualID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime dateOfDeath { get; set; }
        public string gender { get; set; }
        public string placeOfBirth { get; set; }
        public int userID { get; set; }
        public string userEmail { get; set; }



    }
}
