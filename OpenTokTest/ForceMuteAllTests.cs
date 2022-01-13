using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class ForceMuteAllTests : TestBase
    {
        [Fact]
        public void ForceMuteAllExceptionWithEmptySessionId()
        {
            string sessionId = "";

            OpenTok openTok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => openTok.ForceMuteAll(sessionId, null));
            Assert.Contains("The sessionId cannot be empty.", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public void ForceMuteAllCorrectUrl()
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
            opentok.ForceMuteAll(sessionId, null);

            mockClient.Verify();
        }

        [Fact]
        public void ForceMuteAllWithNullExcludedStreamHeaderAndDataCorrect()
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
            opentok.ForceMuteAll(sessionId, null);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> {{"Content-Type", "application/json"}}, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> {{"active", true}, {"excludedStreamIds", null}}, dataSent);
        }

        [Fact]
        public void ForceMuteAllWithExcludedStreamHeaderAndDataCorrect()
        {
            string sessionId = "SESSIONID";
            string[] excludedStreamIds =
            {
                "excludedStreamId1",
                "excludedStreamId2"
            };

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
            opentok.ForceMuteAll(sessionId, excludedStreamIds);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> {{"Content-Type", "application/json"}}, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> {{"active", true}, {"excludedStreamIds", excludedStreamIds}},
                dataSent);
        }
        
        [Fact]
        public async Task ForceMuteAllAsyncExceptionWithEmptySessionId()
        {
            string sessionId = "";

            OpenTok openTok = new OpenTok(ApiKey, ApiSecret);

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await openTok.ForceMuteAllAsync(sessionId, null));
            Assert.Contains("The sessionId cannot be empty.", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public async Task ForceMuteAllAsyncCorrectUrl()
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
            await opentok.ForceMuteAllAsync(sessionId, null);

            mockClient.Verify();
        }

        [Fact]
        public async Task ForceMuteAllAsyncWithNullExcludedStreamHeaderAndDataCorrect()
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
            await opentok.ForceMuteAllAsync(sessionId, null);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> {{"Content-Type", "application/json"}}, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> {{"active", true}, {"excludedStreamIds", null}}, dataSent);
        }

        [Fact]
        public async Task ForceMuteAllAsyncWithExcludedStreamHeaderAndDataCorrect()
        {
            string sessionId = "SESSIONID";
            string[] excludedStreamIds =
            {
                "excludedStreamId1",
                "excludedStreamId2"
            };

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
            await opentok.ForceMuteAllAsync(sessionId, excludedStreamIds);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> {{"Content-Type", "application/json"}}, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> {{"active", true}, {"excludedStreamIds", excludedStreamIds}},
                dataSent);
        }
    }
}