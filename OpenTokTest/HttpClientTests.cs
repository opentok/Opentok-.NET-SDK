using System.Net;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest;

public class HttpClientTests
{
    private const string NetFrameworkDescription = ".NET Framework 4.8.4682";
    private const string NetCoreDescription = ".NET 8.0.1";
    private static readonly SecurityProtocolType WithTls13 = HttpClient.Tls13 | SecurityProtocolType.Tls12;
    private static readonly SecurityProtocolType WithoutTls13 = SecurityProtocolType.Tls12;

    [Fact]
    public void IsNetFrameworkWithTls13_ReturnsFalse_WhenNotNetFramework()
    {
        var result = HttpClient.IsNetFrameworkWithTls13(NetCoreDescription, WithTls13);
        Assert.False(result);
    }

    [Fact]
    public void IsNetFrameworkWithTls13_ReturnsFalse_WhenNetFrameworkButNoTls13()
    {
        var result = HttpClient.IsNetFrameworkWithTls13(NetFrameworkDescription, WithoutTls13);
        Assert.False(result);
    }

    [Fact]
    public void IsNetFrameworkWithTls13_ReturnsTrue_WhenNetFrameworkAndTls13Enabled()
    {
        var result = HttpClient.IsNetFrameworkWithTls13(NetFrameworkDescription, WithTls13);
        Assert.True(result);
    }

    [Fact]
    public void IsNetFrameworkWithTls13_ReturnsFalse_WhenNeitherNetFrameworkNorTls13()
    {
        var result = HttpClient.IsNetFrameworkWithTls13(NetCoreDescription, WithoutTls13);
        Assert.False(result);
    }
}
