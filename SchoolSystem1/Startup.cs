using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SchoolSystem1.StartupOwin))]

namespace SchoolSystem1
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
