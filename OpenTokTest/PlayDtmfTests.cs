using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using System.Collections.Generic;
using Xunit;

namespace OpenTokTest
{
    public class PlayDtmfTests : TestBase
    {
        [Fact]
        public void PlayDfmtThrowsExceptionWithEmptySession()
        {
            string sessionId = "";
            string digits = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            Assert.Throws<OpenTokArgumentException>(() => opentok.PlayDTMF(sessionId, digits));
        }

        [Fact]
        public void PlayDfmtNoConnectionIdCorrectUrl()
        {
            string sessionId = "SESSIONID";
            string digits = "1234567890";

            var expectedUrl = $"v2/project/<api_key>/session/{sessionId}/play-dtmf";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.Post(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.PlayDTMF(sessionId, digits);

            mockClient.Verify();
        }

        [Fact]
        public void PlayDfmtHeaderAndDataCorrect()
        {
            string sessionId = "SESSIONID";
            string digits = "1234567890";
            string connectionId = "CONNECTIONID";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.PlayDTMF(sessionId, digits, connectionId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "digits", digits } }, dataSent);
        }
    }
}
