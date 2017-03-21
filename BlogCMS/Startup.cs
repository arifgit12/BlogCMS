using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogCMS.Startup))]
namespace BlogCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
