using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    var token = opentokService.Session.GenerateToken();
                    var locals = new Dictionary<string, string>
                    {
                        { "Token", token },
                        { "ApiKey", opentokService.OpenTok.ApiKey.ToString() },
                        { "SessionId", opentokService.Session.Id }
                    };

                    return View["index", locals];
                };
        }
    }
}
