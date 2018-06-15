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

        public void AddIndividual(Individual individual)
        {
            _context.Individuals.Add(individual);
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
        //VVV List all information out of _relationship required, this will all be displayed VVV
        public IList<RelationshipBEAN> GetRelationships(int fid)
        {
            IQueryable<RelationshipBEAN> _relaBEAN;
            _relaBEAN = from _relationship in _context.Relationships
                        from _individualOne in _context.Individuals
                        from _individualTwo in _context.Individuals
                        from _rolesOne in _context.Roles
                        from _rolesTwo in _context.Roles
                        from _type in _context.RelationshipTypes
                        where _relationship.familyID == fid
                        where _relationship.individualOneID == _individualOne.individualID
                        where _relationship.individualTwoID == _individualTwo.individualID
                        where _relationship.individualOneRole == _rolesOne.roleID
                        where _relationship.individualTwoRole == _rolesTwo.roleID
                        where _relationship.relationshipTypeID == _type.typeID
                        select new RelationshipBEAN
                        {
                            firstNameOne = _individualOne.firstName,
                            lastNameOne = _individualOne.lastName,
                            firstNameTwo = _individualTwo.firstName,
                            lastNameTwo = _individualTwo.lastName,
                            iOneRole = _rolesOne.roleDescription,
                            iTwoRole = _rolesTwo.roleDescription,
                            typeDescription = _type.typeDescription
                        };
            return _relaBEAN.ToList<RelationshipBEAN>();
                        
        }
    }
}
