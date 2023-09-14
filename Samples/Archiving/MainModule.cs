using System.Dynamic;
using Nancy;
using OpenTokSDK;

namespace Archiving
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
				locals.Token = opentokService.Session.GenerateToken();
				return View["host", locals];
			});
			Get("/participant", _ =>
			{
				dynamic locals = new ExpandoObject();
				locals.ApiKey = opentokService.OpenTok.ApiKey.ToString();
				locals.SessionId = opentokService.Session.Id;
				locals.Token = opentokService.Session.GenerateToken();
				return View["participant", locals];
			});
			Get("/history", _ =>
			{
				var page = Request.Query.page.HasValue ? (int) Request.Query.page : 1;
				var offset = (page - 1) * 5;
				var archives = opentokService.OpenTok.ListArchives(offset, 5);
				var showPrevious = page > 1 ? "/history?page=" + (page - 1) : null;
				var showNext = archives.TotalCount > offset + 5 ? "/history?page=" + (page + 1) : null;
				dynamic locals = new ExpandoObject();
				locals.Archives = archives;
				locals.ShowPrevious = showPrevious;
				locals.ShowNext = showNext;
				return View["history", locals];
			});
			Get("/download/{id}", parameters =>
			{
				Archive archive = opentokService.OpenTok.GetArchive(parameters.id);
				return Response.AsRedirect(archive.Url);
			});
			Post("/start", _ =>
			{
				var archive = opentokService.OpenTok.StartArchive(
					opentokService.Session.Id,
					".NET Archiving Sample App",
					hasAudio: (bool) this.Request.Form.hasAudio,
					hasVideo: (bool) this.Request.Form.hasVideo,
					outputMode: this.Request.Form.outputMode == "composed" ? OutputMode.COMPOSED : OutputMode.INDIVIDUAL
				);
				return archive;
			});
			Get("/stop/{id}", parameters =>
			{
				Archive archive = opentokService.OpenTok.StopArchive(parameters.id);
				return archive;
			});
			Get("/delete/{id}", parameters =>
			{
				opentokService.OpenTok.DeleteArchive(parameters.id);
				return Response.AsRedirect("/history");
			});
		}
	}
}