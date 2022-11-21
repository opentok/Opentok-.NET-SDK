using System.Net;
using OpenTokSDK;

namespace BlazorTestApp.Data;

public class OpenTokService
{
    public OpenTokService(OpenTokOptions options)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        this.OpenTok = new OpenTok(Convert.ToInt32(options.ApiKey), options.ApiSecret);
    }

    public OpenTok OpenTok { get; }

    public Session CreateSession() => this.OpenTok.CreateSession(mediaMode: MediaMode.ROUTED);
}