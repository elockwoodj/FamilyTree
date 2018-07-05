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
        IList<Family> GetFamilies(string uid);
        Family GetFamily(int fid);
        //int GetUserID(string email);
        void DeleteLinkedUser(UserLink linkObject);
        void AddFamilyName(Family familyName);
        void EditFamilyName(Family famObject);
        void DeleteFamilyName(Family famObject);
        void AddIndividual(Individual individual);
        void DeleteIndividual(Individual individual);
        void EditIndividual(Individual individual);
        IList<Individual> GetIndividuals(int fid);
        IList<Relationship> GetRelationships(int pid);
        Individual GetIndividual(int pid);
        IList<relaBEAN> GetRelatives(int pid);
        void AddRelative(Relationship relaObject);
        void AddInverse(Relationship invObject);
        Couple GetCoupleRelation(int pid,int rid);
        IList<relaBEAN> GetTypes();
        IList<relaBEAN> GetRoles();
        IList<relaBEAN> GetListForRelatives(int fid, int pid);
        void EditRelative(Relationship relaObject);
        relaBEAN GetRelationship(int rid);
        Relationship GetRelDelete(int rid);
        void DeleteRelative(Relationship relObject);
        UserLink GetUserLink(int lid);
        Relationship GetRelative(int rid);
        string GetRelativeGender(int pid);
        int GetNumberOfChildren(int pid);
        void AddCouple(int pid, int rid);
        void AddCoupleChild(int cid);
        int GetPlotWidth(int pid);
        void AddLink(UserLink otherUser);
        IList<Family> GetLinkFamilies(string uid);
        IList<UserLink> GetLinkList(string uid, int fid);
        void EditLink(UserLink linkObject);
    }
}
