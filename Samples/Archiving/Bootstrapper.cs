using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;

namespace Archiving
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<OpenTokService>().AsSingleton();
        }
    }
}
