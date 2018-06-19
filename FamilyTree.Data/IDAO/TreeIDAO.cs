using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyTree.Data.BEANS;

namespace FamilyTree.Data.IDAO
{
    public interface TreeIDAO
    {
        IList<FamilyTree.Data.Family> GetFamilies(string uid);
        Family GetFamily(int fid);
        //int GetUserID(string email);
        void AddFamilyName(Family familyName);
        void EditFamilyName(Family familyName);
        void DeleteFamilyName(Family familyName);
        void AddIndividual(Individual individual);
        void DeleteIndividual(Individual individual);
        IList<Individual> GetIndividuals(int fid);
        IList<relaBEAN> GetRelationships(int fid);
        Individual GetIndividual(int pid);
        IList<relaBEAN> GetRelatives(int pid);
        void EditIndividual(Individual individual);


    }
}
