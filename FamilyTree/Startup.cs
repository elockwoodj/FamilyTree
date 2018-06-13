using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FamilyTree.Startup))]
namespace FamilyTree
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
