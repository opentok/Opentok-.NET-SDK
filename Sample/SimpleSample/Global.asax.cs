using OpenTokSDK;
using OpenTokSDK.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimpleSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            OpenTok opentok = new OpenTok(Convert.ToInt32(ConfigurationManager.AppSettings["opentok_key"]),
                        ConfigurationManager.AppSettings["opentok_secret"]);
            try
            {
                Application["sessionId"] = opentok.CreateSession().Id;
            }
            catch(OpenTokException)
            {
                Application["error"] = "Error: session could not be generated";
            }            
        }
    }
}
