using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyTree.Data;
using FamilyTree.Data.BEANS;

namespace FamilyTree.Services.IDAO
{
    public interface TreeIService
    {
        IList<FamilyTree.Data.Family> GetFamilies(string uid);
        Family GetFamily(int fid);
        //int GetUserID(string email);
        void AddFamilyName(Family familyName);
        void EditFamilyName(Family famObject);
        void DeleteFamilyName(Family famObject);
        void AddIndividual(Individual individual);
        void DeleteIndividual(Individual individual);
        void EditIndividual(Individual individual);
        IList<Individual> GetIndividuals(int fid);
        IList<relaBEAN> GetRelationships(int fid);
        Individual GetIndividual(int pid);
        IList<relaBEAN> GetRelatives(int pid);
        void AddRelative(Relationship relaObject);
        IList<relaBEAN> GetTypes();
        IList<relaBEAN> GetRoles();
    }
}
