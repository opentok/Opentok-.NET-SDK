#region

using System.Net;
using OpenTokSDK;

#endregion

namespace BlazorHelloWorld.Infrastructure;

public class VideoService
{
    private readonly OpenTok openTok;
    private readonly string apiKey;

    public VideoService(OpenTokOptions options)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        this.openTok = InitializeClient(options);
    }

    private OpenTok InitializeClient(OpenTokOptions options)
    {
        if (!string.IsNullOrEmpty(options.ApplicationId) && !string.IsNullOrEmpty(options.PrivateKey))
        {
            return new OpenTok(options.ApplicationId, options.PrivateKey);
        }

        if (!string.IsNullOrEmpty(options.ApiKey) && !string.IsNullOrEmpty(options.ApiSecret) 
            && int.TryParse(options.ApiKey, out var key))
        {
            return new OpenTok(key, options.ApiSecret);
        }

        throw new ArgumentException("Missing credentials");
    }

    public SessionCredentials CreateSession()
    {
        var session = openTok.CreateSession(mediaMode: MediaMode.ROUTED);
        return new SessionCredentials(session.Id, session.GenerateToken());
    }

    public string GetOpenTokId() => this.openTok.GetOpenTokId();

    public async Task<Archive> StartArchiving(string sessionId)
    {
        return await openTok.StartArchiveAsync(sessionId);
    }

    public async Task StopArchiving(string archiveId)
    {
        await openTok.StopArchiveAsync(archiveId);
    }
}

public record OpenTokOptions(string ApiKey, string ApiSecret, string ApplicationId, string PrivateKey);

public record SessionCredentials(string SessionId, string Token);