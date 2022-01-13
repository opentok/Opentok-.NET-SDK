using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class ForceMuteStreamTests : TestBase
    {

        [Fact]
        public void ForceMuteStreamExceptionWithEmptySessionId()
        {
            string sessionId = "";
            string streamId = "1234567890";

            OpenTok openTok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => openTok.ForceMuteStream(sessionId, streamId));
            Assert.Contains("The sessionId cannot be empty.", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public void ForceMuteStreamExceptionWithEmptyStreamId()
        {
            string sessionId = "1234567890";
            string streamId = "";

            OpenTok openTok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => openTok.ForceMuteStream(sessionId, streamId));
            Assert.Contains("The streamId cannot be empty.", exception.Message);
            Assert.Equal("streamId", exception.ParamName);
        }

        [Fact]
        public void ForceMuteStreamCorrectUrl()
        {
            string sessionId = "SESSIONID";
            string streamId = "1234567890";
            
            string expectedUrl = $"v2/project/{this.ApiKey}/session/{sessionId}/stream/{streamId}/mute";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.Post(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.ForceMuteStream(sessionId, streamId);

            mockClient.Verify();
        }

        [Fact]
        public void ForceMuteSteamHeaderAndDataCorrect()
        {
            string sessionId = "SESSIONID";
            string streamId = "1234567890";

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
            opentok.ForceMuteStream(sessionId, streamId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.Null(dataSent);
        }

        [Fact]
        public async Task ForceMuteStreamAsyncExceptionWithEmptySessionId()
        {
            string sessionId = "";
            string streamId = "1234567890";

            OpenTok openTok = new OpenTok(ApiKey, ApiSecret);

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await openTok.ForceMuteStreamAsync(sessionId, streamId));
            Assert.Contains("The sessionId cannot be empty.", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public async Task ForceMuteStreamAsyncExceptionWithEmptyStreamId()
        {
            string sessionId = "1234567890";
            string streamId = "";

            OpenTok openTok = new OpenTok(ApiKey, ApiSecret);

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await openTok.ForceMuteStreamAsync(sessionId, streamId));
            Assert.Contains("The streamId cannot be empty.", exception.Message);
            Assert.Equal("streamId", exception.ParamName);
        }

        [Fact]
        public async Task ForceMuteStreamAsyncCorrectUrl()
        {
            string sessionId = "SESSIONID";
            string streamId = "1234567890";

            string expectedUrl = $"v2/project/{this.ApiKey}/session/{sessionId}/stream/{streamId}/mute";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.PostAsync(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.ForceMuteStreamAsync(sessionId, streamId);

            mockClient.Verify();
        }

        [Fact]
        public async Task ForceMuteSteamAsyncHeaderAndDataCorrect()
        {
            string sessionId = "SESSIONID";
            string streamId = "1234567890";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.ForceMuteStreamAsync(sessionId, streamId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.Null(dataSent);
        }
    }
}