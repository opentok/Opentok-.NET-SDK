using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

using Nancy;
using OpenTokSDK;
using OpenTokSDK.Util;

namespace Broadcasting
{

    public class MainModule : NancyModule
    {

        public MainModule(OpenTokService opentokService)
        {

            Get["/"] = _ => View["index"];

            Get["/host"] = _ =>
            {
                
                dynamic locals = new ExpandoObject();

                locals.ApiKey = opentokService.OpenTok.ApiKey.ToString();
                locals.SessionId = opentokService.Session.Id;
                locals.Token = opentokService.Session.GenerateToken(Role.PUBLISHER, 0, null, new List<string> (new string[] { "focus"}));
                locals.InitialBroadcastId = opentokService.broadcastId;
                locals.FocusStreamId = opentokService.focusStreamId;
                locals.InitialLayout = OpenTokUtils.convertToCamelCase(opentokService.layout.ToString());
                
                return View["host", locals];
            };

            Get["/participant"] = _ =>
            {
                dynamic locals = new ExpandoObject();

                locals.ApiKey = opentokService.OpenTok.ApiKey.ToString();
                locals.SessionId = opentokService.Session.Id;
                locals.Token = opentokService.Session.GenerateToken();
                locals.FocusStreamId = opentokService.focusStreamId;
                locals.Layout = OpenTokUtils.convertToCamelCase(opentokService.layout.ToString());
                
                return View["participant", locals];
            };

            Post["/start"] = _ =>
            {
                bool horizontal = Request.Form["layout"] == "horizontalPresentation";
                BroadcastLayout layoutType = new BroadcastLayout(horizontal ? BroadcastLayout.LayoutType.HorizontalPresentation : BroadcastLayout.LayoutType.VerticalPresentation);
                int maxDuration = 7200;
                if (Request.Form["maxDuration"] != null)
                {
                    maxDuration = int.Parse(Request.Form["maxDuration"]);
                }
                Broadcast broadcast = opentokService.OpenTok.StartBroadcast(
                    opentokService.Session.Id,
                    hls: true,
                    rtmpList: null,
                    resolution: Request.Form["resolution"],
                    maxDuration: maxDuration,
                    layout: layoutType
                );
                opentokService.broadcastId = broadcast.Id.ToString();
                return broadcast;
            };

            Get["/stop/{id}"] = parameters =>
            {
                Broadcast broadcast = opentokService.OpenTok.StopBroadcast(parameters.id);
                opentokService.broadcastId = "";
                return broadcast;
            };

            Get["/broadcast"] = _ =>
            {
                if (!String.IsNullOrEmpty(opentokService.broadcastId))
                {
                    try
                    {
                        Broadcast broadcast = opentokService.OpenTok.GetBroadcast(opentokService.broadcastId);
                        if(broadcast.Status == Broadcast.BroadcastStatus.STARTED)
                        {
                            return Response.AsRedirect(broadcast.Hls);
                        }
                        else
                        {
                            return Response.AsText("Broadcast not in progress.");
                        }
                    }
                        
                    catch (Exception ex)
                    {
                        return Response.AsText("Could not get broadcast " + opentokService.broadcastId);
                    }
                } else {
                    return Response.AsText("There's no broadcast running right now.");
                }
            };

            Get["/broadcast/{id}/layout/{layout}"] = parameters =>
            {
                bool horizontal = parameters.layout == "horizontalPresentation";
                BroadcastLayout layout = new BroadcastLayout(horizontal ? BroadcastLayout.LayoutType.HorizontalPresentation : BroadcastLayout.LayoutType.VerticalPresentation);
                opentokService.OpenTok.SetBroadcastLayout(parameters.id, layout);
                return HttpStatusCode.OK;
            };

            Post["/focus"] = _ =>
            {
                string focusStreamId = Request.Form["focus"];
                opentokService.focusStreamId = focusStreamId;
                StreamList streamList = opentokService.OpenTok.ListStreams(opentokService.Session.Id);
                List<StreamProperties> streamPropertiesList = new List<StreamProperties>();
                foreach (Stream stream in streamList)
                {
                    StreamProperties streamProperties = new StreamProperties(stream.Id, null);
                    if (focusStreamId.Equals(stream.Id))
                    {
                        streamProperties.addLayoutClass("focus");
                    }
                    streamPropertiesList.Add(streamProperties);
                }
                opentokService.OpenTok.SetStreamClassLists(opentokService.Session.Id, streamPropertiesList);
                return HttpStatusCode.OK;
            };
        }
    }
}
