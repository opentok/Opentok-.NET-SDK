using LanguageExt;
using OpenTokSDK;

namespace BlazorTestApp.Data;

public interface IVideoService
{
    void CreateSession();

    void JoinSession(SessionCredentials sessionCredentials);

    Task DeleteArchiveAsync(Guid archiveId);

    Task StopArchiveAsync(Guid archiveId);

    Task<Archive> GetArchiveAsync(Guid archiveId);

    Task<ArchiveList> ListArchivesAsync();

    Task<Archive> StartArchiveAsync(string sessionId);

    int GetApiKey();

    Option<SessionCredentials> GetCredentials();

    Option<SessionInformation> GetSessionInformation();
}