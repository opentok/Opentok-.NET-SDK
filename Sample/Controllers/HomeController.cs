using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using OpenTokSDK;
using OpenTokSDK.Exceptions;
using System.Configuration;

namespace Sample.Controllers
{
    public class HomeController : Controller
    {
        private OpenTok opentok = new OpenTok(Convert.ToInt32(ConfigurationManager.AppSettings["opentok_key"]),
                                    ConfigurationManager.AppSettings["opentok_secret"]);

        // GET Home/Index
        public ActionResult Index()
        {
            ViewBag.Title = "Sample app";
            return View();
        }

        // GET Home/HostView
        public ActionResult HostView()
        {
            return OpenTokView();
        }

        // GET Home/ParticipantView
        public ActionResult ParticipantView()
        {
            return OpenTokView();
        }

        private ActionResult OpenTokView()
        {
            try
            {  
                string sessionId = GetSessionId(HttpContext.ApplicationInstance.Application);
                ViewBag.apikey = opentok.ApiKey;
                ViewBag.sessionId = sessionId;
                ViewBag.token = opentok.GenerateToken(sessionId);            
            }
            catch (OpenTokException)
            {
                ViewBag.errorMessage = "Could not generate token";
            }
            return View();
        }
        private string GetSessionId(HttpApplicationState Application)
        {
                      
                if (Application["sessionId"] == null)
                {
                    Application.Lock();
                    Application["sessionId"] = opentok.CreateSession().Id;
                    Application.UnLock();
                }
                return (string)Application["sessionId"];
            
        }
    }
}