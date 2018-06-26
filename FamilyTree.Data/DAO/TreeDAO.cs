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

        // ----- Get Lists -----

        public IList<Family> GetFamilies(string uid)
        {
            IQueryable<FamilyTree.Data.Family> _families;
            _families = from fam
                        in _context.Families
                        where fam.ownerUserName == uid
                        select fam;

            return _families.ToList<Family>();
                        
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
        public Relationship GetRelative(int rid)
        {
            IQueryable<Relationship> _rel;
            _rel = from rel in _context.Relationships
                   where rel.relationshipID == rid
                   select rel;
            return _rel.First();
        }

        public IList<relaBEAN> GetRelationships(int fid)
        {
            var _famLists = GetIndividuals(fid);
            IList<relaBEAN> relatives;
             
            foreach (var per in _famLists)
            {
                    relatives = (GetRelatives(per.individualID));
                return relatives;
                
            }

            return null;
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

        // ----- Get Single Objects -----

            //All these methods will fetch a single record out of their respective tables that matches the ID passed to it
        public Family GetFamily(int fid)
        {
            IQueryable<Family> _fam;
            _fam = from fam
                   in _context.Families
                   where fam.familyID == fid
                   select fam;

            return _fam.First();
        }

        public Couple GetCoupleRelation(int pid, int rid)
        {
            IQueryable<Couple> _cou;
            _cou = from cou in _context.Couples
                   where cou.personOne == pid && cou.personTwo == rid
                   select cou;
            return _cou.First();
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

        // This is different to the GetRelationship method as it doesn't require additional information from separate tables
        // Therefore not requiring the use of a ViewModel
        public Relationship GetRelDelete(int rid)
        {
            IQueryable<Relationship> _relDel;
            _relDel = from rDel in _context.Relationships
                      where rDel.relationshipID == rid
                      select rDel;
            return _relDel.First();
        }

        public Individual GetIndividual(int pid)
        {
            IQueryable<Individual> _ind;
            _ind = from ind in _context.Individuals
                   where ind.individualID == pid
                   select ind;

            return _ind.ToList().First();
        }
        public string GetRelativeGender(int pid)
        {
            IQueryable<Individual> _ind;
            _ind = from ind in _context.Individuals
                   where ind.individualID == pid
                   select ind;
            string genderPicker = _ind.First().gender;
            return genderPicker;
        }

        // ----- Adds -----
        //Adds familyName to the Family table
        public void AddFamilyName(Family familyName)
        {
            _context.Families.Add(familyName);
            _context.SaveChanges();
        }

        //Adds individual to the Individual table
        public void AddIndividual(Individual individual)
        {
            _context.Individuals.Add(individual);
            _context.SaveChanges();
        }

        //Adds relaObject to the Relationship table
        public void AddRelative(Relationship relaObject)
        {
            _context.Relationships.Add(relaObject);
            _context.SaveChanges();
        }
        public void AddInverse(Relationship invObject)
        {
            _context.Relationships.Add(invObject);
            _context.SaveChanges();
        }
        // ----- Edits -----
        //Accesses the Family table, pulling an object with the same ID passed in famObject, rewriting information and saving
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

        public void AddCouple(int pid, int rid)
        {
            //Add a couple to the table
            IQueryable<Relationship> _rel;
            _rel = from rel in _context.Relationships
                   where rel.personID == pid
                   && rel.relationshipTypeID == 3
                   select rel;
            int numberOfChildren = _rel.Count();
            Couple coupObject = new Couple
            {
                NumberOfChildren = numberOfChildren,
                personOne = pid,
                personTwo = rid
            };
            _context.Couples.Add(coupObject);
            _context.SaveChanges();
            //Need Couple ID to Add coupleID to partners individual record
            Couple coupStore =GetCoupleRelation(pid, rid);
            int coupleID = coupStore.coupleID;

            //Add Couple ID to their record
            IQueryable<Individual> pidEdit;
            pidEdit = from _pidEdit in _context.Individuals
                       where _pidEdit.individualID == pid
                       select _pidEdit;
            Individual pidIDEdit = pidEdit.First();
            pidIDEdit.coupleID = coupleID;
            _context.SaveChanges();

            IQueryable<Individual> ridEdit;
            ridEdit = from _ridEdit in _context.Individuals
                      where _ridEdit.individualID == rid
                      select _ridEdit;
            Individual ridIDEdit = ridEdit.First();
            ridIDEdit.coupleID = coupleID;
            _context.SaveChanges();
        }
        public void AddCoupleChild(int cid)
        {
            //Increase the number of children someone has by one
            IQueryable<Couple> _couAdd;
            _couAdd = from couAdd in _context.Couples
                      where couAdd.coupleID == cid
                      select couAdd;
            Couple coupAdd = _couAdd.First();
            coupAdd.NumberOfChildren = coupAdd.NumberOfChildren + 1;
            _context.SaveChanges();

        }
        //Accesses the Individual table, pulling an object with the same ID passed by indObject, rewriting information and saving
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
            indEdit.isParent = indObject.isParent;
            _context.SaveChanges();
        }

        //Accesses the Relationship table, pulling an object with the same ID passed by relaObject, rewriting information and saving
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

        // ----- Deletes -----

        // Removes an object from the Individual table that matches indObject
        public void DeleteIndividual(Individual indObject)
        {
            _context.Individuals.Remove(indObject);
            _context.SaveChanges();
        }

        //Removes an object from the Relationship table that matches relaObject
        public void DeleteRelative(Relationship relaObject)
        {
            _context.Relationships.Remove(relaObject);
            _context.SaveChanges();
        }
    
        //Removes an object from the Family table that matches famObject
        public void DeleteFamilyName(Family famObject)
        {
            _context.Families.Remove(famObject);
            _context.SaveChanges();
        }


        // ----- USED FOR PLOTTING -----


        //Works out how many children a person has, used for plotting
        public int GetNumberOfChildren(int pid)
        {
            IQueryable<Relationship> _chi;
            _chi = from chi in _context.Relationships
                   where chi.personID == pid && chi.relativeRole == 5 // Checks how many relationships have role 5, associated with "Child"
                   select chi;
            int numberChildren = _chi.Count();

            return numberChildren;
        }
        //Selects an individuals children from the database, using Couple table to determine an individuals children
        public IList<relaBEAN> GetChildren(int pid)
        {
            IQueryable<relaBEAN> _par;
            _par = from par in _context.Relationships
                   from ind in _context.Individuals
                   where par.personID == pid && par.relationshipTypeID == 5 && par.relativeID == ind.individualID// Selects all relatives of a person who's relationship type is Child, used for plotting
                   select new relaBEAN
                   {
                       fullName = ind.fullName,
                       relativeID = par.relativeID,
                       dateOfBirth = ind.dateOfBirth,
                       dateOfDeath = ind.dateOfDeath

                   };
            return _par.ToList<relaBEAN>();


        }
    }
}
