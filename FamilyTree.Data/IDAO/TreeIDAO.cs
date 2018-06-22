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

        //-----Deletes -----
        void DeleteFamilyName(Family familyName);
        void DeleteIndividual(Individual individual);
        void DeleteRelative(Relationship relative);

        //-----Adds-----
        void AddIndividual(Individual individual);
        void AddFamilyName(Family familyName);
        void AddRelative(Relationship relative);
        void AddInverse(Relationship invObject);


        // -----Get Lists-----
        IList<FamilyTree.Data.Family> GetFamilies(string uid);
        IList<relaBEAN> GetRelationships(int fid);
        IList<Individual> GetIndividuals(int fid);
        IList<relaBEAN> GetRelatives(int pid);
        IList<relaBEAN> GetTypes();
        IList<relaBEAN> GetRoles();
        IList<relaBEAN> GetListForRelatives(int fid, int pid);


        //-----Get Singulars-----
        Individual GetIndividual(int pid);
        Family GetFamily(int fid);
        relaBEAN GetRelationship(int rid);
        Relationship GetRelDelete(int rid);
        Relationship GetRelative(int rid);
        string GetRelativeGender(int pid);


        //-----Edits-----
        void EditFamilyName(Family familyName);
        void EditIndividual(Individual individual);
        void EditRelative(Relationship relaObject);

    }
}
