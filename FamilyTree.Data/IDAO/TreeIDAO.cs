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
        void DeleteLinkedUser(UserLink linkObject);
        //-----Adds-----
        void AddIndividual(Individual individual);
        void AddFamilyName(Family familyName);
        void AddRelative(Relationship relative);
        void AddInverse(Relationship invObject);
        void AddCouple(int pid, int rid);
        void AddCoupleChild(int cid);
        void AddLink(UserLink otherUser);

        // -----Get Lists-----
        IList<Family> GetFamilies(string uid);
        IList<Relationship> GetRelationships(int pid);
        IList<Individual> GetIndividuals(int fid);
        IList<relaBEAN> GetRelatives(int pid);
        IList<relaBEAN> GetTypes();
        IList<relaBEAN> GetRoles();
        IList<relaBEAN> GetListForRelatives(int fid, int pid);
        IList<Family> GetLinkFamilies(string uid);
        IList<UserLink> GetLinkList(string uid, int fid);

        //-----Get Singulars-----
        Individual GetIndividual(int pid);
        Family GetFamily(int fid);
        relaBEAN GetRelationship(int rid);
        Relationship GetRelDelete(int rid);
        Relationship GetRelative(int rid);
        string GetRelativeGender(int pid);
        int GetNumberOfChildren(int pid);
        Couple GetCoupleRelation(int pid, int rid);
        int GetPlotWidth(int pid);
        UserLink GetUserLink(int lid);
        //-----Edits-----
        void EditFamilyName(Family familyName);
        void EditIndividual(Individual individual);
        void EditRelative(Relationship relaObject);
        void EditLink(UserLink linkObject);
    }
}
