namespace BlazorTestApp.Data;

public class OpenTokOptions
{
    public OpenTokOptions(string apiKey, string apiSecret)
    {
        this.ApiKey = apiKey;
        this.ApiSecret = apiSecret;
    }

    public OpenTokOptions()
    {
    }

    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}