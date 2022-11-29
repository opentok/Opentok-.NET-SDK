using OpenTokSDK;

namespace BlazorTestApp.Data;

public interface IVideoService
{
    Session CreateSession();

    Task DeleteArchiveAsync(Guid archiveId);

    Task StopArchiveAsync(Guid archiveId);

    Task<Archive> GetArchiveAsync(Guid archiveId);

    Task<ArchiveList> ListArchivesAsync();

    Task<Archive> StartArchiveAsync(string sessionId);

    int GetApiKey();
}