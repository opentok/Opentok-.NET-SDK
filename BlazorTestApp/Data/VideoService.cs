using System.Net;
using LanguageExt;
using OpenTokSDK;
using OpenTokSDK.Render;
using static LanguageExt.Prelude;

namespace BlazorTestApp.Data;

public class VideoService : IVideoService
{
    private readonly OpenTok openTok;
    private Option<SessionCredentials> credentials = Option<SessionCredentials>.None;

    public VideoService(OpenTokOptions options)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        this.openTok = new OpenTok(Convert.ToInt32(options.ApiKey), options.ApiSecret);
    }

    public void CreateSession() =>
        Some(this.openTok.CreateSession(mediaMode: MediaMode.ROUTED))
            .Bind(SessionCredentials.FromExistingSession)
            .IfSome(this.JoinSession);

    public void JoinSession(SessionCredentials sessionCredentials) => this.credentials = sessionCredentials;

    public async Task DeleteArchiveAsync(Guid archiveId) => await this.openTok.DeleteArchiveAsync(archiveId.ToString());

    public int GetApiKey() => this.openTok.ApiKey;

    public Option<SessionCredentials> GetCredentials() => this.credentials;

    public Option<SessionInformation> GetSessionInformation() => this.credentials
        .Map(cred => new SessionInformation(cred, this.GetApiKey().ToString()));

    public async Task<Option<RenderItem>> StartExperienceComposerAsync(string url, string streamName) =>
        await this.GetCredentials()
            .Map(session => CreateRenderingRequest(url, streamName, session))
            .MapAsync(this.StartRenderAsync)
            .Match(Some, () => Option<RenderItem>.None);

    public async Task StopExperienceComposerAsync(RenderItem rendering) =>
        await this.openTok.StopRenderAsync(rendering.Id);

    public async Task StopArchiveAsync(Guid archiveId) => await this.openTok.StopArchiveAsync(archiveId.ToString());

    public async Task<Archive> GetArchiveAsync(Guid archiveId) =>
        await this.openTok.GetArchiveAsync(archiveId.ToString());

    public async Task<ArchiveList> ListArchivesAsync() => await this.openTok.ListArchivesAsync();

    public async Task<Archive> StartArchiveAsync(string sessionId) => await this.openTok.StartArchiveAsync(
        sessionId,
        "Blazor Sample App",
        true,
        true,
        OutputMode.COMPOSED,
        "1920x1080",
        new ArchiveLayout {Type = LayoutType.bestFit, ScreenShareType = ScreenShareLayoutType.BestFit}
    );

    private Task<RenderItem> StartRenderAsync(StartRenderRequest request) => this.openTok.StartRenderAsync(request);

    private static StartRenderRequest CreateRenderingRequest(string url, string streamName, SessionCredentials cred) =>
        new(cred.SessionId, cred.Token, new Uri(url), new Uri(url), streamName, 60);
}