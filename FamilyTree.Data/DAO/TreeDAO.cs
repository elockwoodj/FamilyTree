using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyTree.Data.IDAO;
using FamilyTree.Data.BEANS;

namespace FamilyTree.Data.DAO
{
    public class TreeDAO : TreeIDAO
    {
        private b7039648Entities1 _context;
        public int userID;
        public TreeDAO()
        {
            _context = new b7039648Entities1();
        }

        // might need to bean everything, using the AspNetUsers table for the usernames - they're all individual so it shouldn't affect the rest of the application
        // If I bean everything, linq statements change but everything else should be more or less fine
        // Check Butcher work to get an example

        public IList<Family> GetFamilies(string uid)
        {
            IQueryable<FamilyTree.Data.Family> _families;
            _families = from fam
                        in _context.Families
                        where fam.ownerUserName == uid
                        select fam;

            return _families.ToList<Family>();
                        
        }

        public Family GetFamily(int fid)
        {
            IQueryable<Family> _fam;
            _fam = from fam
                   in _context.Families
                   where fam.familyID == fid
                   select fam;

            return _fam.First();
        }

        //public int GetUserID(string email)
        //{
        //    IQueryable<FamilyTree.Data.AspNetUser> _ID;
        //    _ID = from use 
        //          in _context.AspNetUsers
        //          where use.userEmail == email
        //          select use;
        //    User checker = _ID.ToList().First();
        //    userID = checker.userID;

        //    return userID;
        //}

        public void AddFamilyName(Family familyName)
        {
            _context.Families.Add(familyName);
            _context.SaveChanges();
        }
        public void EditFamilyName(Family famObject)
        {
            IQueryable<Family> _famEdit;
            _famEdit = from fEdit in _context.Families
                       where fEdit.familyID == famObject.familyID
                       select fEdit;
            Family famEdit = _famEdit.First();

            famEdit.familyName = famObject.familyName;

            _context.SaveChanges();
        }
        public void DeleteFamilyName(Family famObject)
        {
            _context.Families.Remove(famObject);
            _context.SaveChanges();
        }

        public void AddIndividual(Individual individual)
        {
            _context.Individuals.Add(individual);
            _context.SaveChanges();
        }

        public void DeleteIndividual(Individual indObject)
        {
            _context.Individuals.Remove(indObject);
            _context.SaveChanges();
        }

        public IList<Individual> GetIndividuals(int fid)
        {
            IQueryable<Individual> _indiv;
            _indiv = from ind
                     in _context.Individuals
                     where ind.familyID == fid
                     select ind;

            return _indiv.ToList<Individual>();
        }

        public Individual GetIndividual(int pid)
        {
            IQueryable<Individual> _ind;
            _ind = from ind in _context.Individuals
                   where ind.individualID == pid
                   select ind;

            return _ind.ToList().First();
        }

        public IList<relaBEAN> GetRelatives(int pid)
        {
            IQueryable<relaBEAN> _rel;
            _rel = from rel_ in _context.Relationships
                   from ind_ in _context.Individuals
                   //from rol_ in _context.Roles
                   //from typ_ in _context.RelationshipTypes
                   where rel_.personID == pid && rel_.relativeID == ind_.individualID //&& rel_.relativeRole == rol_.roleID //&& rel_.relationshipTypeID == typ_.typeID
                   select new relaBEAN
                   {
                       relationshipID = rel_.relationshipID,
                       personID = pid,
                       relativeID = ind_.individualID,
                       relationshipStartDate = rel_.relationshipStartDate,
                       relationshipEndDate = rel_.relationshipEndDate,
                       notableInformation = rel_.notableInformation,
                       fullName = ind_.fullName,
                       dateOfBirth = ind_.dateOfBirth,
                       dateOfDeath = ind_.dateOfDeath,
                       gender = ind_.gender,
                       placeOfBirth = ind_.placeOfBirth,
                       relativeRole = rel_.relativeRole,
                       familyID = rel_.familyID
                       //relationshipType = typ_.typeDescription,
                   };

            return _rel.ToList<relaBEAN>();
        }

        public IList<relaBEAN> GetRelationships(int fid)
        {
            var _famLists = GetIndividuals(fid);
            IList<relaBEAN> relatives;
             
            foreach (var per in _famLists)
            {
                    relatives = GetRelatives(per.individualID);
                //if (relatives != null)
                //{
                //    return relatives.ToList<relaBEAN>();
                //}
                //else
                //{
                //    return null;
                //};
            }

            return null;
            
            
        }
        //VVV Speak to Dan about Partial Views etc, need to work out how this will work
        //public IList<RelationshipBEAN> GetRelationships(int fid, int pid, int pName)
        //{
        //    IQueryable<RelationshipBEAN> _relaBEAN;
        //    _relaBEAN = from _relationship in _context.Relationships
        //                from _individual in _context.Individuals
        //                from _type in _context.RelationshipTypes
        //                    //from _rolesOne in _context.Roles
        //                    //from _rolesTwo in _context.Roles
        //                where _relationship.familyID == fid
        //                && (_relationship.individualOneID == _individual.individualID && _relationship.individualTwoID == _individual.individualID)
        //                && _relationship.relationshipTypeID == _type.typeID
        //                //&& _relationship.individualOneRole == _rolesOne.roleID
        //                //&& _relationship.individualTwoRole == _rolesTwo.roleID
        //                select new RelationshipBEAN
        //                {
        //                    firstNameOne = _individual.firstName,
        //                    lastNameOne = _individual.lastName,
        //                    firstNameTwo = _individual.firstName,
        //                    lastNameTwo = _individual.lastName,
        //                    //iOneRole = _rolesOne.roleDescription,
        //                    //iTwoRole = _rolesTwo.roleDescription,
        //                    typeDescription = _type.typeDescription,
        //                    relationshipStartDate = _relationship.relationshipStartDate,
        //                    relationshipEndDate = _relationship.relationshipEndDate,
        //                    notableInformation = _relationship.notableInformation
        //                };
        //    return _relaBEAN.ToList<RelationshipBEAN>();


        //    IQueryable<RelationshipBEAN> _relaBEAN;
        //    _relaBEAN = from _relationship in _context.Relationships
        //                from _type in _context.RelationshipTypes
        //                where _relationship.familyID == fid
        //                && _relationship.relationshipTypeID == _type.typeID
        //                select new RelationshipBEAN
        //                {
        //                    relationshipId = _relationship.relationshipID,

        //                    Individuals
        //                    individualOneID = pid,
        //                    individualTwoID = _relationship.individualTwoID,
        //                    individualTwoName = _relationship.individualTwoID,

        //                    Relationship Type
        //                    relationshipTypeId = _relationship.relationshipTypeID,
        //                    typeDescription = _type.typeDescription,

        //                    Roles
        //                    iOneRole = _relationship.individualOneRole,
        //                    iTwoRole = _relationship.individualTwoRole,

        //                    Relationship Dates
        //                    relationshipStartDate = _relationship.relationshipStartDate,
        //                    relationshipEndDate = _relationship.relationshipEndDate,

        //                    notableInformation = _relationship.notableInformation,

        //                    familyId = _relationship.familyID
        //                };
        //    return _relaBEAN.ToList<RelationshipBEAN>();
        //}
    }
}
