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

        public void EditIndividual(Individual indObject)
        {
            IQueryable<Individual> _indEdit;
            _indEdit = from iEdit in _context.Individuals
                       where iEdit.individualID == indObject.individualID
                       select iEdit;
            Individual indEdit = _indEdit.First();

            indEdit.fullName = indObject.fullName;
            indEdit.gender = indObject.gender;
            indEdit.placeOfBirth = indObject.placeOfBirth;
            indEdit.dateOfBirth = indObject.dateOfBirth;
            indEdit.dateOfDeath = indObject.dateOfDeath;
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
                   from rol_ in _context.Roles
                   from typ_ in _context.RelationshipTypes
                   where rel_.personID == pid 
                   && rel_.relativeID == ind_.individualID 
                   && rel_.relativeRole == rol_.roleID 
                   && rel_.relationshipTypeID == typ_.typeID
                   select new relaBEAN
                   {
                       relationshipID = rel_.relationshipID,
                       personID = pid,
                       relativeID = ind_.individualID,
                       relationshipStartDate = rel_.relationshipStartDate,
                       relationshipEndDate = rel_.relationshipEndDate,
                       notableInformation = rel_.notableInformation,
                       roleDescription = rol_.roleDescription,
                       fullName = ind_.fullName,
                       dateOfBirth = ind_.dateOfBirth,
                       dateOfDeath = ind_.dateOfDeath,
                       gender = ind_.gender,
                       placeOfBirth = ind_.placeOfBirth,
                       relativeRole = rel_.relativeRole,
                       familyID = rel_.familyID,
                       typeDescription = typ_.typeDescription
                   };

            return _rel.ToList();
        }

        public IList<relaBEAN> GetRelationships(int fid)
        {
            var _famLists = GetIndividuals(fid);
            IList<relaBEAN> relatives;
             
            foreach (var per in _famLists)
            {
                    relatives = (GetRelatives(per.individualID));
                return relatives;
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


        public void AddRelative(Relationship relaObject)
        {
            _context.Relationships.Add(relaObject);
            _context.SaveChanges();
        }
        public void EditRelative(Relationship relaObject)
        {
            IQueryable<Relationship> _relEdit;
            _relEdit = from rEdit in _context.Relationships
                       where rEdit.relationshipID == relaObject.relationshipID
                       select rEdit;
            Relationship relEdit = _relEdit.First();

            relEdit.notableInformation = relaObject.notableInformation;
            relEdit.relationshipStartDate = relaObject.relationshipStartDate;
            relEdit.relationshipEndDate = relaObject.relationshipEndDate;
            _context.SaveChanges();
        }

        public relaBEAN GetRelationship(int rid)
        {
            IQueryable<relaBEAN> _relBEAN;
            _relBEAN = from rBEAN in _context.Relationships
                       where rBEAN.relationshipID == rid
                       select new relaBEAN
                       {
                           relationshipID = rBEAN.relationshipID,
                           personID = rBEAN.personID,
                           notableInformation = rBEAN.notableInformation,
                           relationshipStartDate = rBEAN.relationshipStartDate,
                           relationshipEndDate = rBEAN.relationshipEndDate
                       };
            var rel = _relBEAN.First();


            return _relBEAN.ToList().First();
        }

        public Relationship GetRelDelete(int rid)
        {
            IQueryable<Relationship> _relDel;
            _relDel = from rDel in _context.Relationships
                      where rDel.relationshipID == rid
                      select rDel;
            return _relDel.First();
        }
        public IList<relaBEAN> GetTypes()
        {
            IQueryable<relaBEAN> _typ;
            _typ = from typ in _context.RelationshipTypes
                   select new relaBEAN
                   {
                       typeDescription = typ.typeDescription,
                       relationshipTypeID = typ.typeID
                   };
            return _typ.ToList<relaBEAN>();
        }
        public IList<relaBEAN> GetRoles()
        {
            IQueryable<relaBEAN> _role;
            _role = from rol in _context.Roles
                    select new relaBEAN
                    {
                        roleDescription = rol.roleDescription,
                        relativeRole = rol.roleID
                    };
            return _role.ToList<relaBEAN>();
        }
        public IList<relaBEAN> GetListForRelatives(int fid ,int pid)
        {
            IQueryable<relaBEAN> _rela;
            _rela = from ind in _context.Individuals
                    where ind.familyID == fid && ind.individualID != pid
                    select new relaBEAN
                    {
                        fullName = ind.fullName,
                        relativeID = ind.individualID
                    };
            return _rela.ToList<relaBEAN>();
        }
        public void DeleteRelative(Relationship relaObject)
        {
            _context.Relationships.Remove(relaObject);
            _context.SaveChanges();
        }

    }
}
