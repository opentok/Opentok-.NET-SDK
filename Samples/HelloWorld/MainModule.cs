using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

using Nancy;
using OpenTokSDK;

namespace HelloWorld
{

    public class MainModule : NancyModule
    {

        public MainModule(OpenTokService opentokService)
        {

            Get["/"] = _ =>
                {
                    dynamic locals = new ExpandoObject();

                    locals.Token = opentokService.Session.GenerateToken();
                    locals.ApiKey = opentokService.OpenTok.ApiKey.ToString();
                    locals.SessionId = opentokService.Session.Id;

                    return View["index", locals];
                };
        }
    }
}
