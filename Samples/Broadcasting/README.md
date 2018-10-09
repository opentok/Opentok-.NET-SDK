# OpenTok Broadcast Sample for .NET

This is a simple demo app that shows how you can use the OpenTok .NET SDK to create
[live-streaming broadcasts](https://tokbox.com/developer/guides/broadcast/live-streaming)
of an OpenTok session.

**Note**: These instructions assume you will be using Visual Studio. Since the application runs as a 
[self hosted OWIN-compatible console application](https://github.com/NancyFx/Nancy/wiki/Hosting-nancy-with-owin#katana---httplistener-selfhost),
you should open Visual Studio using 'Run as administrator'. The sample projects are contained inside
the `OpenTok.sln` solution at the top level.

## Running the App

First, add your own API Key and API Secret to the Application Settings. For your convenience, the
`App.config` file is set up for you to place your values into it.

```
    <add key="API_KEY" value="000000" />
    <add key="API_SECRET" value="abcdef1234567890abcdef" />
```

Next, make sure the Broadcasting project is set as the Solution's Startup project. This can be done
by opening the Properties of the solution, and selecting it under Common Properties > Startup Project.

Lastly, the dependencies of the application are referenced using NuGet. Package Restore is a feature of
NuGET 2.7+ and as long as your installation of Visual Studio has the NuGet extension installed with a
later version, the package should be installed automatically on launch. Otherwise, use the 
[NuGET Packge Restore guide](http://docs.nuget.org/docs/reference/package-restore).

Choose Start (Ctrl+F5) to run the application.

Visit <http://localhost:3000/host> in your browser. Then, mute the audio on your computer
(to prevent feedback) and load the  <http://localhost:3000/participant> page in another browser tab
(or in a browser on another computer). The two client pages are connected in an OpenTok session.

In the host page, set broadast options -- maximum duration and resolution -- and then click the **Start Broadcast** button to start the live streaming broadcast of the session. (The maximum
duration setting is optional. The default maximum duration of a broadcast is 2 hours
(7200 seconds). 

Then, visit <http://localhost:3000/broadcast> in Safari. This page shows the live streaming
HLS broadcast of the session. Safari supports HLS streams natively. To support the HLS
broadcast in other browsers, you will need to use an extension or script such as
[videojs-http-streaming](https://github.com/videojs/http-streaming).

In the host page, click the **Toggle Layout** button to switch the layout of the broadcast between
horizontal and vertical presentation. Click any stream to make it the focus (larger) video in
the layout.

## Walkthrough

This demo application uses the same frameworks and libraries as the HelloWorld sample. If you have
not already gotten familiar with the code in that project, consider doing so before continuing.

Each section will focus on a route handler within the
main module (MainModule.cs).

#### Starting a live streaming broadcast

Start by visiting the host page at <http://localhost:8080/host> and using the application to start broadcasting.
Your browser will first ask you to approve permission to use the camera and microphone.
Once you've accepted, your image will appear inside the section titled 'Host'. To start to broadcast
press the 'Start broadcast' button. Once the broadcasting has begun the button will turn
green and change to 'Stop broadcast'. 

Next we will see how the host view is implemented on the server. The route handler for this page is
shown below:

```csharp
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
```

If you've completed the HelloWorld walkthrough, this should look familiar. This handler simply
generates the strings that the client (JavaScript) needs to connect to the session: `ApiKey`,
`SessionId` and `Token`. It also passes a broadcast ID (if a broadcast is already in progress), and the layout state (the focus stream ID and the current layout type), which will be discussed later. 


After the user has connected to the session, they press the
'Start broadcast' button, which sends an XHR (or Ajax) request to the <http://localhost:8080/start>
URL. The route handler for this URL is shown below:

```csharp
Post["/start"] = _ =>
{
  bool horizontal = Request.Form["layout"] == "horizontalPresentation";
  BroadcastLayout layoutType = new BroadcastLayout(horizontal ? BroadcastLayout.LayoutType.HorizontalPresentation : BroadcastLayout.LayoutType.VerticalPresentation);
  int maxDuration = 7200;
  if (Request.Form["maxDuration"] != null) {
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
```

In this handler, the `StartBroadcast()` method of the `OpenTok` instance is called with the `Id`
for the session. This will trigger the broadcast to start. The second optional parameter `hls` is for enabling or disabling HLS. The third parameter is a list of RTMP stream URLs.  Each RTMP object contains an `Id`, the `serverName` and the `serverUrl`. Use this parameter if you want to broadcast the session via RTMP. If you do not want RTMP streams you can set it as `null`.
The next parameter is `resolution`, which is optional, and it could be either '640x480' or '1280x720'.
The parameter `maxDuration` is the maximum duration of the broadcast, in seconds. 

The last parameter `layout` is the initial layout type of the broadcast, discussed ahead.

The response sent back to the client's XHR
request will be the JSON representation of the broadcast, which Nancy knows how to serialize from the object.

#### Stopping the broadcast

When the host client receives this response, the `Start broadcast` button is hidden and the `Stop broadcast` is displayed.

When the user presses the button this time, another XHR request
is sent to the <http://localhost:8080/stop/{id}> URL where `id` represents the ID of the broadcast. The route handler for this request is shown below:

```csharp
Get["/stop/{id}"] = parameters =>
{
	Broadcast broadcast = opentokService.OpenTok.StopBroadcast(parameters.id);
	opentokService.broadcastId = "";
	return broadcast;
};
```

This handler is very similar to the previous one. Instead of calling the `StartBroadast()` method,
the `StopBroadcast()` method is called. This method takes an broadcast ID as its parameter, which
is different for each time a session starts a new broadcast. But the client has sent this to the server
as part of the URL, so the `parameters.id` expression is used to retrieve it from the route matched
segment.

#### Viewing the broadcast stream

When you load the broadcast URL http://localhost:3000/broadcast the route handler for this request is shown below:

```csharp
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
	} 
    else
    {	
		return Response.AsText("There's no broadcast running right now.");
	}
};
```

The router method for the broadcast URL calls the Opentok.GetBroadcast(), defined in the OpenTok .NET SDK, to get information about the broadcast. The Broadcast object returned to the GetBroadcast() completion handler includes the HLS broadcast URL (the broadcastUrls.hls property) as well as the status of the broadcast. If the status of the broacast is 'started', the server redirects the client to the URL of the HLS stream.

Again, only Safari natively support viewing of an HLS stream. In other clients, you will need to use a HLS viewing extension.

### Changing Broadcast Layout

When you start the broadcast, by calling the OpenTok.StartBroadcast() method of the OpenTok .NET SDK, you could set the desired broadcast layout. This sets the initial layout type of the broadcast. In our case, we set it to 'horizontalPresentation' or 'verticalPresentation', which are two of the predefined layout types for live streaming broadcasts.

You can change the layout dynamically. The host view includes a Toggle layout button. This toggles the layout of the streams between a horizontal and vertical presentation. When you click this button, the host client switches makes an HTTP GET request to the '/broadcast/{broadcastId}/layout/{layout}' endpoint:

```csharp
 Get["/broadcast/{id}/layout/{layout}"] = parameters =>
 {
 	bool horizontal = parameters.layout == "horizontalPresentation";
	BroadcastLayout layout = new BroadcastLayout(horizontal ? BroadcastLayout.LayoutType.HorizontalPresentation : BroadcastLayout.LayoutType.VerticalPresentation);
 	opentokService.OpenTok.SetBroadcastLayout(parameters.id, layout);
 	return HttpStatusCode.OK;
 };
```

This calls the OpenTok.SetBroadcastLayout() method of the OpenTok .NET SDK, setting the broadcast layout to the layout type defined in the `layout` GET request's parameter. In this app, the layout type is set to horizontalPresentation or verticalPresentation, two of the predefined layout types available to live streaming broadcasts.

In the host view you can click any stream to set it to be the focus stream in the broadcast layout. (Click outside of the mute audio icon.) Doing so sends an HTTP POST request to the /focus endpoint:

```csharp
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
	opentokService.OpenTok.SetStreamClassLists(opentokService.Session.Id, 	streamPropertiesList);
	return HttpStatusCode.OK;
};
```

First, we're getting a list of the streams in a session using the OpenTok.ListStreams() method of the OpenTok .NET SDK.
For each one of these streams, we create a new StreamProperties object where we set the stream Id and the stream layout class, and we're adding it to a List of StreamProperties objects. 
In this case, we're adding the layout class `focus` only to the stream with the Id equals to `opentokService.focusStreamId` which causes it to be the large stream displayed in the broadcast. 

Finally we pass the complete list of streams properties to the method OpenTok.SetStreamClassLists() that receives the session Id and the streamPropertiesList.

To see this effect, you should open the host and participant pages on different computers (using different cameras). Or, if you have multiple cameras connected to your machine, you can use one camera for publishing from the host, and use another for the participant. Or, if you are using a laptop with an external monitor, you can load the host page with the laptop closed (no camera) and open the participant page with the laptop open.

The host client page also uses OpenTok signaling to notify other clients when the layout type and focus stream changes, and they then update the local display of streams in the HTML DOM accordingly. However, this is not necessary. The layout of the broadcast is unrelated to the layout of streams in the web clients.

When you view the broadcast stream, the layout type and focus stream changes, based on calls to the OpenTok.setBroadcastLayout() and OpenTok.setStreamClassLists() methods during the broadcast.

For more information, see [Configuring video layout for OpenTok live streaming broadcasts](https://tokbox.com/developer/guides/broadcast/live-streaming/#configuring-video-layout-for-opentok-live-streaming-broadcasts).