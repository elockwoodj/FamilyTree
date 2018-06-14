using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyTree.Data;

namespace FamilyTree.Services.IDAO
{
    public interface TreeIService
    {
        IList<FamilyTree.Data.Family> GetFamilies(string uid);

        int GetUserID(string email);
        void AddFamilyName(Family familyName);
        void AddIndividual(Individual individual);
    }
}
