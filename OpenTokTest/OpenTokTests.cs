using AutoFixture;
using OpenTokSDK;
using Xunit;

namespace OpenTokSDKTest
{
    public class OpenTokTests
    {
        [Fact]
        public void SetCustomUserAgent_ShouldSetUserAgentOnClient()
        {
            var fixture = new Fixture();
            var customUserAgent = fixture.Create<string>();
            var sut = new OpenTok(fixture.Create<int>(), fixture.Create<string>());
            sut.SetCustomUserAgent(customUserAgent);
            Assert.Equal(customUserAgent, sut.Client.CustomUserAgent);
        }
    }
}