using System.Net;
using OpenTokSDK;

namespace BlazorTestApp.Data;

public class VideoService : IVideoService
{
    private readonly OpenTok openTok;

    public VideoService(OpenTokOptions options)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        this.openTok = new OpenTok(Convert.ToInt32(options.ApiKey), options.ApiSecret);
    }

    public Session CreateSession() => this.openTok.CreateSession(mediaMode: MediaMode.ROUTED);

    public async Task DeleteArchiveAsync(Guid archiveId) => await this.openTok.DeleteArchiveAsync(archiveId.ToString());

    public int GetApiKey() => this.openTok.ApiKey;

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
}