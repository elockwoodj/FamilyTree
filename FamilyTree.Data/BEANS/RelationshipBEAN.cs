﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Data.BEANS
{
    public class RelationshipBEAN
    {
        public int individualID { get; set; }

        public int relationshipId { get; set; }
        //public int familyID { get; set; }
        public int individualOneID { get; set; }
        public int individualTwoID { get; set; }
        //public string relationshipType { get; set; }
        //public string roleDescription { get; set; }
        //public int relationshipTypeID { get; set; }
        public int iOneRole { get; set; }
        public int iTwoRole { get; set; }
        public string iOneRoleName { get; set; }
        public string iTwoRoleName { get; set; }
        public DateTime? relationshipStartDate { get; set; }
        public DateTime? relationshipEndDate { get; set; }
        public string notableInformation { get; set; }
        public int relationshipTypeId { get; set; }
        public string typeDescription { get; set; }
        public string firstNameOne { get; set; }
        public string firstNameTwo { get; set; }
        public string lastNameOne { get; set; }
        public string lastNameTwo { get; set; }

        public int familyId { get; set; }



    }
}
