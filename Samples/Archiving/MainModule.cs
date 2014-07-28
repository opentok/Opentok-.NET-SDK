using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

using Nancy;
using OpenTokSDK;

namespace Archiving
{

    public class MainModule : NancyModule
    {

        public MainModule(OpenTokService opentokService)
        {

            Get["/"] = _ => View["index"];

            Get["/host"] = _ =>
            {
                dynamic locals = new ExpandoObject();

                locals.Token = opentokService.Session.GenerateToken();
                locals.ApiKey = opentokService.OpenTok.ApiKey.ToString();
                locals.SessionId = opentokService.Session.Id;

                return View["host", locals];
            };

            Get["/participant"] = _ =>
            {
                dynamic locals = new ExpandoObject();

                locals.Token = opentokService.Session.GenerateToken();
                locals.ApiKey = opentokService.OpenTok.ApiKey.ToString();
                locals.SessionId = opentokService.Session.Id;

                return View["participant", locals];
            };

            Get["/history"] = _ =>
            {
                // TODO: does casting work here?
                //var page = Request.Query.page.HasValue ? (int)Request.Query.page : 1;
                //var offset = (page - 1) * 5;
                //ArchiveList archives = opentokService.OpenTok.ListArchives(offset, 5);

                //string showPrevious = page > 1 ? "/history?page=" + (page - 1).ToString() : null;
                //string showNext = archives.TotalCount > (offset + 5) ? "/history?page=" + (page - 1).ToString() : null;

                dynamic locals = new ExpandoObject();
                //locals.Archives = archives;
                //locals.ShowPrevious = showPrevious;
                //locals.ShowNext = showNext;
                locals.Test = "Hello World";

                return View["history", locals];
            };

            Get["/download/{id}"] = parameters =>
            {
                Archive archive = opentokService.OpenTok.GetArchive(parameters.id);
                // TODO: JSON serialization
                return archive;
            };

            Get["/start"] = _ =>
            {
                Archive archive = opentokService.OpenTok.StartArchive(opentokService.Session.Id);
                return archive;
            };

            Get["/stop/{id}"] = parameters =>
            {
                Archive archive = opentokService.OpenTok.StopArchive(parameters.id);
                return archive;
            };

            Get["/delete/{id}"] = parameters =>
            {
                opentokService.OpenTok.DeleteArchive(parameters.id);
                return Response.AsRedirect("/history");
            };
        }
    }
}
