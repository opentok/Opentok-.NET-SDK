using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using OpenTokSDK;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest;

public class ConnectionTests : TestBase
{
    private readonly int apiKey;
    private readonly Mock<HttpClient> mockClient;
    private readonly OpenTok sut;

    public ConnectionTests()
    {
        var fixture = new Fixture();
        this.apiKey = fixture.Create<int>();
        this.mockClient = new Mock<HttpClient>();
        this.sut = new OpenTok(this.apiKey, fixture.Create<string>())
        {
            Client = this.mockClient.Object,
        };
    }

    [Fact]
    public async Task ListConnections()
    {
        var sessionId = "b40ef09b-3811-4726-b508-e41a0f96c68f";
        var returnString = GetResponseJson();
        mockClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(returnString);
        var connections = await sut.ListConnections(sessionId);
        mockClient.Verify(
            httpClient =>
                httpClient.GetAsync(It.Is<string>(url => url.Equals($"v2/project/{apiKey}/session/{sessionId}/connection")), It.IsAny<Dictionary<string, string>>()),
            Times.Once());
        Assert.Equal(1, connections.Count);
        Assert.Equal("e9f8c166-6c67-440d-994a-04fb6dfed007", connections.ApplicationId);
        Assert.Equal(sessionId, connections.SessionId);
        Assert.Single(connections.Items);
        Assert.Equal(new Connection(){ ConnectionId = "string",ConnectionState = "Connected", CreatedAt = 1384221730000}, connections.Items[0]);
    }
}