#region

using System.Net;
using OpenTokSDK;

#endregion

namespace BlazorHelloWorld.Infrastructure;

public class VideoService
{
    private readonly OpenTok openTok;

    public VideoService(OpenTokOptions options)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        openTok = new OpenTok(Convert.ToInt32(options.ApiKey), options.ApiSecret);
    }

    public SessionCredentials CreateSession()
    {
        var session = openTok.CreateSession(mediaMode: MediaMode.ROUTED);
        return new SessionCredentials(session.Id, session.GenerateToken());
    }

    public int GetApiKey()
    {
        return openTok.ApiKey;
    }
}

public record OpenTokOptions(string ApiKey, string ApiSecret);

public record SessionCredentials(string SessionId, string Token);