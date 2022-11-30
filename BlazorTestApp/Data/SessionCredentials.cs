using LanguageExt;
using OpenTokSDK;

namespace BlazorTestApp.Data;

public record SessionCredentials(string SessionId, string Token, bool IsNewSession)
{
    public static Option<SessionCredentials> FromNewSession(string sessionId, string token) =>
        ParseCredentials(sessionId, token, true);

    private static Option<SessionCredentials> ParseCredentials(string sessionId, string token, bool isNewSession) =>
        string.IsNullOrEmpty(sessionId) && string.IsNullOrEmpty(token)
            ? Option<SessionCredentials>.None
            : new SessionCredentials(sessionId, token, isNewSession);

    public static Option<SessionCredentials> FromExistingSession(Session session) =>
        ParseCredentials(session.Id, session.GenerateToken(), false);
}