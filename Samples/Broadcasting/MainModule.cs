using System;
using System.Collections.Generic;
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
			Get("/", _ => View["index"]);
			Get("/host", _ =>
			{
				dynamic locals = new ExpandoObject();
				locals.ApiKey = opentokService.OpenTok.ApiKey.ToString();
				locals.SessionId = opentokService.Session.Id;
				locals.Token =
					opentokService.Session.GenerateToken(Role.PUBLISHER, 0, null, new List<string>(new[] {"focus"}));
				locals.InitialBroadcastId = opentokService.broadcastId;
				locals.FocusStreamId = opentokService.focusStreamId;
				locals.InitialLayout = OpenTokUtils.convertToCamelCase(opentokService.layout.ToString());
				return View["host", locals];
			});
			Get("/participant", _ =>
			{
				dynamic locals = new ExpandoObject();
				locals.ApiKey = opentokService.OpenTok.ApiKey.ToString();
				locals.SessionId = opentokService.Session.Id;
				locals.Token = opentokService.Session.GenerateToken();
				locals.FocusStreamId = opentokService.focusStreamId;
				locals.Layout = OpenTokUtils.convertToCamelCase(opentokService.layout.ToString());
				return View["participant", locals];
			});
			Post("/start", _ =>
			{
				bool horizontal = Request.Form["layout"] == "horizontalPresentation";
				var layoutType = new BroadcastLayout(horizontal
					? BroadcastLayout.LayoutType.HorizontalPresentation
					: BroadcastLayout.LayoutType.VerticalPresentation);
				var maxDuration = 7200;
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
				opentokService.broadcastId = broadcast.Id;
				return broadcast;
			});
			Get("/stop/{id}", parameters =>
			{
				Broadcast broadcast = opentokService.OpenTok.StopBroadcast(parameters.id);
				opentokService.broadcastId = "";
				return broadcast;
			});
			Get("/broadcast", _ =>
			{
				if (!string.IsNullOrEmpty(opentokService.broadcastId))
				{
					try
					{
						var broadcast = opentokService.OpenTok.GetBroadcast(opentokService.broadcastId);
						if (broadcast.Status == Broadcast.BroadcastStatus.STARTED)
						{
							return Response.AsRedirect(broadcast.Hls);
						}

						return this.Response.AsText("Broadcast not in progress.");
					}
					catch (Exception ex)
					{
						return Response.AsText("Could not get broadcast " + opentokService.broadcastId +
						                       ". Exception Message: " + ex.Message);
					}
				}

				return this.Response.AsText("There's no broadcast running right now.");
			});
			Get("/broadcast/{id}/layout/{layo,ut}", parameters =>
			{
				bool horizontal = parameters.layout == "horizontalPresentation";
				var layout = new BroadcastLayout(horizontal
					? BroadcastLayout.LayoutType.HorizontalPresentation
					: BroadcastLayout.LayoutType.VerticalPresentation);
				opentokService.OpenTok.SetBroadcastLayout(parameters.id, layout);
				return HttpStatusCode.OK;
			});
			Post("/focus", _ =>
			{
				string focusStreamId = Request.Form["focus"];
				opentokService.focusStreamId = focusStreamId;
				var streamList = opentokService.OpenTok.ListStreams(opentokService.Session.Id);
				var streamPropertiesList = new List<StreamProperties>();
				foreach (var stream in streamList)
				{
					var streamProperties = new StreamProperties(stream.Id);
					if (focusStreamId.Equals(stream.Id))
					{
						streamProperties.addLayoutClass("focus");
					}

					streamPropertiesList.Add(streamProperties);
				}

				opentokService.OpenTok.SetStreamClassLists(opentokService.Session.Id, streamPropertiesList);
				return HttpStatusCode.OK;
			});
		}
	}
}