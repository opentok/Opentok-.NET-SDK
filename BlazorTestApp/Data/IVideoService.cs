using LanguageExt;
using OpenTokSDK;
using OpenTokSDK.Render;

namespace BlazorTestApp.Data;

public interface IVideoService
{
    void CreateSession();

    void AssignCredentials(SessionCredentials sessionCredentials);

    Task DeleteArchiveAsync(Guid archiveId);

    Task StopArchiveAsync(Guid archiveId);

    Task<Archive> GetArchiveAsync(Guid archiveId);

    Task<ArchiveList> ListArchivesAsync();

    Task<Archive> StartArchiveAsync(string sessionId);

    int GetApiKey();

    Option<SessionCredentials> GetCredentials();

    Option<SessionInformation> GetSessionInformation();

    Task<Option<RenderItem>> StartExperienceComposerAsync(string url, string streamName);

    Task StopExperienceComposerAsync(RenderItem rendering);
}