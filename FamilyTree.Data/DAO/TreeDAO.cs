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
            IQueryable<Family> _families;
            _families = from fam
                        in _context.Families
                        where fam.ownerUserName == uid
                        select fam;

            return _families.ToList<Family>();
                        
        }

        public IList<Family> GetLinkFamilies(string uid)
        {
            IQueryable<Family> _fam;

            //Select from the families table, all family entries that are enabled for this user, therefore having the same family ID's as shown in the link table
            _fam = from link in _context.UserLinks
                        from fam in _context.Families
                        where link.enabledUserName == uid
                        && fam.familyID == link.familyID
                        select fam;

            return _fam.ToList();
        }
        public IList<UserLink> GetLinkList(string uid, int fid)
        {
            IQueryable<UserLink> _link;
            _link = from link in _context.UserLinks
                    where link.ownerUserName == uid
                    && link.familyID == fid
                    select link;
            return _link.ToList();
        }

        public IList<Individual> GetIndividuals(int fid)
        {
            IQueryable<Individual> _indiv;
            _indiv = from ind
                     in _context.Individuals
                     where ind.familyID == fid
                     //This orderby was required for the first iteration as it required the parents to be plotted first
                     orderby ind.isParent descending
                     select ind;

            return _indiv.ToList();
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

        public IList<Relationship> GetRelationships(int pid)
        {
            IQueryable<Relationship> _rel;
            _rel = from rel in _context.Relationships
                   where rel.personID == pid
                   //Ordered by type ID descending as plotting requires marriages to be plotted first, simple fix
                   //orderby rel.relationshipTypeID descending
                   select rel;
            return _rel.ToList();
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
            return _typ.ToList();
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
            return _role.ToList();
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
            return _rela.ToList();
        }

        // ----- Get Single Objects -----

        //All these methods will fetch a single record out of their respective tables that matches the ID passed to it



        //Gets a specific Family name  and ID from the family table
        public Family GetFamily(int fid)
        {
            IQueryable<Family> _fam;
            _fam = from fam
                   in _context.Families
                   where fam.familyID == fid
                   select fam;

            return _fam.First();
        }
        //Gets a specific relationship
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

        public UserLink GetUserLink(int lid)
        {
            IQueryable<UserLink> _link;
            _link = from link in _context.UserLinks
                    where link.Id == lid
                    select link;
            return _link.First();
        }

        // This is different to the GetRelationship method as it doesn't require additional information from separate tables
        // Therefore not requiring the use of a ViewModel, as we are only deleting the record
        public Relationship GetRelDelete(int rid)
        {
            IQueryable<Relationship> _relDel;
            _relDel = from rDel in _context.Relationships
                      where rDel.relationshipID == rid
                      select rDel;
            return _relDel.First();
        }
        //Select an individual from the individuals table
        public Individual GetIndividual(int pid)
        {
            IQueryable<Individual> _ind;
            _ind = from ind in _context.Individuals
                   where ind.individualID == pid
                   select ind;

            return _ind.ToList().First();
        }
        //Grabs the gender for the relative, called when adding a new relative
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

        public void AddLink(UserLink otherUser)
        {
            _context.UserLinks.Add(otherUser);
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

        public void EditLink(UserLink linkObject)
        {
            IQueryable<UserLink> _link;
            _link = from link in _context.UserLinks
                    where link.Id == linkObject.Id
                    select link;
            UserLink linkEdit = _link.First();

            linkEdit.enabledUserName = linkObject.enabledUserName;
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
        public void DeleteLinkedUser(UserLink linkObject)
        {
            _context.UserLinks.Remove(linkObject);
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


        //Works out the width of each row, compares them against each other and returns the greatest value - used to calculate the width of the plot
        public int GetPlotWidth(int pid)
        {
            //Set initial row widths, these start at X unit widths as there is a border around the tree that must be maintained 
            //Row Two starts at one as, at bare minimum, you are plotting this person
            int rowOneEntries = 0;
            int rowTwoEntries = 1;
            int rowThreeEntries = 0;

            IList<Relationship> relList = GetRelationships(pid);

            foreach (var person in relList)
            {
                if (person.relationshipTypeID == 1) //They are this persons siblings
                {

                    var partCheck = GetRelationships(person.relativeID); //Load up childs relationships
                    bool partnerCheck = partCheck.Any(par => par.relationshipTypeID == 4); //Check if any of the relationships match the marriage type
                    if (partnerCheck == true) //If they do, add an additional width
                    {
                        rowTwoEntries = rowTwoEntries + 1;
                    }
                    rowTwoEntries = rowTwoEntries + 1;
                }

                else if(person.relationshipTypeID == 2) //They are this persons parents
                {
                    var partCheck = GetRelationships(person.relativeID); //Load up parents relationships
                    bool partnerCheck = partCheck.Any(par => par.relationshipTypeID == 4); //Check if any of the relationships match the marriage type
                    if (partnerCheck == true) //If they do, add an additional width
                    {
                        rowOneEntries = rowOneEntries + 1;
                    }
                    rowOneEntries = rowOneEntries + 1;
                }

                else if(person.relationshipTypeID == 3) //They are this persons child
                {
                    var partCheck = GetRelationships(person.relativeID); //Load up parents relationships
                    bool partnerCheck = partCheck.Any(par => par.relationshipTypeID == 4); //Check if any of the relationships match the marriage type
                    if (partnerCheck == true) //If they do, add an additional width
                    {
                        rowThreeEntries = rowThreeEntries + 1;
                    }
                    rowThreeEntries = rowThreeEntries + 1;
                }

                else if(person.relationshipTypeID == 4) //They are this persons partner
                {
                    rowTwoEntries = rowTwoEntries + 1;
                }
            }


                //Add all these values to a list

            List<int> widthList = new List<int>();
            widthList.Add(rowOneEntries);
            widthList.Add(rowTwoEntries);
            widthList.Add(rowThreeEntries);

                //Find the max width value between the three rows, returning this to be calculated

            int maxWidth = widthList.Max();


                //Return the maximum width value

            return maxWidth;

        }



        // ------------------------------ Method Graveyard ------------------------------


            //These were methods that were added at one stage as they could have potentially been useful, however in the long run I ended up discarding them
            //Usually these were discarded as I thought of a better way to achieve what they were initially made for 
            //For instance, the couple table was initially added as a way of being able to display different "Family units" within the same tree
            //However, I felt the way I ended up doing that functionality made more sense to myself and was more intuitive




        public void AddCouple(int pid, int rid)
        {
            //Pull the number of children from relationship table the individual has
            IQueryable<Relationship> _rel;
            _rel = from rel in _context.Relationships
                   where rel.personID == pid
                   && rel.relationshipTypeID == 3
                   select rel;
            //Store the number of children to be added to the couple table
            int numberOfChildren = _rel.Count();

            //Need new couple object to add into couple table
            Couple coupObject = new Couple
            {
                NumberOfChildren = numberOfChildren, //Used to help plotting
                personOne = pid, //Used as identifiers to pull coupleID from table to be inserted into individual table
                personTwo = rid //As above
            };

            //Add the object and save changes so the coupleID can be accessed straight away
            _context.Couples.Add(coupObject);
            _context.SaveChanges();

            //Need coupleID to Add coupleID to partners individual record
            Couple coupStore = GetCoupleRelation(pid, rid);
            int coupleID = coupStore.coupleID;

            //Add coupleID to their record
            IQueryable<Individual> pidEdit;
            pidEdit = from _pidEdit in _context.Individuals
                      where _pidEdit.individualID == pid
                      select _pidEdit;
            Individual pidIDEdit = pidEdit.First();
            pidIDEdit.coupleID = coupleID;
            _context.SaveChanges();

            //Add coupleID to partners record
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
            //Check if their isParent tag is false


        }
        public Couple GetCoupleRelation(int pid, int rid)
        {
            IQueryable<Couple> _cou;
            _cou = from cou in _context.Couples
                   where cou.personOne == pid && cou.personTwo == rid
                   select cou;
            return _cou.First();
        }
        //Selects an individuals children from the database
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
