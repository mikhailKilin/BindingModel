using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ModelBinding.Startup))]
namespace ModelBinding
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
