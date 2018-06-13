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
        private b7039648Entities _context;
        public int userID;
        public TreeDAO()
        {
            _context = new b7039648Entities();
        }

        

        public IList<Family> GetFamilies(string userEmail)
        {
            int famCheck = GetUserID(userEmail);
            IQueryable<FamilyTree.Data.Family> _families;
            _families = from fam
                        in _context.Families
                        where fam.headofFamilyID == famCheck
                        select fam;

            return _families.ToList<Family>();
                        
        }

        public int GetUserID(string email)
        {
            IQueryable<FamilyTree.Data.User> _ID;
            _ID = from use 
                  in _context.Users
                  where use.userEmail == email
                  select use;
            User checker = _ID.ToList().First();
            userID = checker.userID;

            return userID;
        }

        public void AddFamilyName(Family familyName)
        {
            _context.Families.Add(familyName);
            _context.SaveChanges();
        }

        public void AddIndividual(Individual individual)
        {
            _context.Individuals.Add(individual);
            _context.SaveChanges();
        }
    }
}
