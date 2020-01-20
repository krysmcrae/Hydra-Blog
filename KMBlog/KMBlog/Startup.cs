using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KMBlog.Startup))]
namespace KMBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
