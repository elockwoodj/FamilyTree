using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Data.IDAO
{
    public interface TreeIDAO
    {
        IList<FamilyTree.Data.Family> GetFamilies(string uid);
        int GetUserID(string email);
        void AddFamilyName(Family familyName);
        void AddIndividual(Individual individual);

    }
}
