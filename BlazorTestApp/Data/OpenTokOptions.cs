namespace BlazorTestApp.Data;

public class OpenTokOptions
{
    public OpenTokOptions(string apiKey, string apiSecret, string sessionId)
    {
        this.ApiKey = apiKey;
        this.ApiSecret = apiSecret;
        this.SessionId = sessionId;
    }

    public OpenTokOptions()
    {
    }

    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string SessionId { get; set; }
}