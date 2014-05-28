using OpenTokSDK;
using OpenTokSDK.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleSample.Controllers
{
    public class HomeController : Controller
    {
        private OpenTok opentok = new OpenTok(Convert.ToInt32(ConfigurationManager.AppSettings["opentok_key"]),
                                    ConfigurationManager.AppSettings["opentok_secret"]);

        public ActionResult Index()
        {
            HttpApplicationState Application = HttpContext.ApplicationInstance.Application;
         
            if (!String.IsNullOrEmpty((string) Application["error"]))
            {
                ViewBag.Error = (string) Application["error"];
            }
         
            try
            {
                ViewBag.sessionId = Application["sessionId"];
                ViewBag.token = opentok.GenerateToken(ViewBag.sessionId);
                ViewBag.apiKey = opentok.ApiKey;
            }
            catch(OpenTokException)
            {
                ViewBag.Error = "Token for the session could not be generated";
            }
            
            return View();
        }
    }
}