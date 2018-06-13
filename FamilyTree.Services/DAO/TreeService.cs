using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyTree.Services.IDAO;
using FamilyTree.Data;
using FamilyTree.Data.DAO;
using FamilyTree.Data.IDAO;


namespace FamilyTree.Services.DAO
{
    public class TreeService : TreeIService
    {
        private TreeIDAO _treeDAO;
        public TreeService()
        {
            _treeDAO = new TreeDAO();
        }

        public IList<Family> GetFamilies(string email)
        {
            return _treeDAO.GetFamilies(email);
        }
        public int GetUserID(string email)
        {
            return _treeDAO.GetUserID(email);
        }
        public void AddFamilyName(Family familyName)
        {
            _treeDAO.AddFamilyName(familyName);
        }
        public void AddIndividual(Individual individual)
        {
            _treeDAO.AddIndividual(individual);
        }
    }
}
