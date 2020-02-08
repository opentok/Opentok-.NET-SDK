using Owin;
using System.Net;

namespace Archiving
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            app.UseNancy();
        }
    }
}
