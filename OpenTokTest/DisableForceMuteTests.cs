using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class DisableForceMuteTests : TestBase
    {
        [Fact]
        public void DisableForceMuteExceptionWithEmptySessionId()
        {
            string sessionId = "";

            OpenTok openTok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => openTok.DisableForceMute(sessionId));
            Assert.Contains("The sessionId cannot be empty.", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public void DisableForceMuteCorrectUrl()
        {
            string sessionId = "SESSIONID";

            string expectedUrl = $"v2/project/{ApiKey}/session/{sessionId}/mute";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.Post(expectedUrl, It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.DisableForceMute(sessionId);

            mockClient.Verify();
        }

        [Fact]
        public void DisableForceMuteHeaderAndDataCorrect()
        {
            string sessionId = "SESSIONID";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.DisableForceMute(sessionId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "active", false } },
                dataSent);
        }

        [Fact]
        public async Task DisableForceMuteAsyncExceptionWithEmptySessionId()
        {
            string sessionId = "";

            OpenTok openTok = new OpenTok(ApiKey, ApiSecret);

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await openTok.DisableForceMuteAsync(sessionId));
            Assert.Contains("The sessionId cannot be empty.", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public async Task DisableForceMuteAsyncCorrectUrl()
        {
            string sessionId = "SESSIONID";

            string expectedUrl = $"v2/project/{ApiKey}/session/{sessionId}/mute";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.PostAsync(expectedUrl, It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.DisableForceMuteAsync(sessionId);

            mockClient.Verify();
        }

        [Fact]
        public async Task DisableForceMuteAsyncWithNullExcludedStreamHeaderAndDataCorrect()
        {
            string sessionId = "SESSIONID";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.DisableForceMuteAsync(sessionId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "active", false } }, dataSent);
        }

        [Fact]
        public async Task DisableForceMuteAsyncWithExcludedStreamHeaderAndDataCorrect()
        {
            string sessionId = "SESSIONID";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.DisableForceMuteAsync(sessionId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "active", false } },
                dataSent);
        }
    }
}