using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyTree.Services.IDAO;
using FamilyTree.Data;
using FamilyTree.Data.BEANS;
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

        public IList<Family> GetFamilies(string uid)
        {
            return _treeDAO.GetFamilies(uid);
        }
        public Family GetFamily(int fid)
        {
            return _treeDAO.GetFamily(fid);
        }
        //public int GetUserID(string email)
        //{
        //    return _treeDAO.GetUserID(email);
        //}
        public void AddFamilyName(Family familyName)
        {
            _treeDAO.AddFamilyName(familyName);
        }
        public IList<relaBEAN> GetRelatives(int pid)
        {
            return _treeDAO.GetRelatives(pid);
        }

        public void EditFamilyName(Family famObject)
        {
            _treeDAO.EditFamilyName(famObject);
        }
        public void DeleteFamilyName(Family famObject)
        {
            _treeDAO.DeleteFamilyName(famObject);
        }
        public void AddIndividual(Individual individual)
        {
            _treeDAO.AddIndividual(individual);
        }
        public void DeleteIndividual(Individual individual)
        {
            _treeDAO.DeleteIndividual(individual);
        }
        public void EditIndividual(Individual individual)
        {
            _treeDAO.EditIndividual(individual);
        }
        public IList<Individual>GetIndividuals(int fid)
        {
            return _treeDAO.GetIndividuals(fid);
        }
        public Individual GetIndividual(int pid)
        {
            return _treeDAO.GetIndividual(pid);
        }
        public IList<relaBEAN> GetRelationships(int fid)
        {
            return _treeDAO.GetRelationships(fid);
        }
    }
}
