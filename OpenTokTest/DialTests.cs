using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OpenTokSDKTest
{
    public class DialTests : TestBase
    {
        [Fact]
        public void DialThrowsExceptionWithEmptySession()
        {
            string sessionId = "";
            string token = "1234567890";
            string sipUri = "SIPURI";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            Assert.Throws<OpenTokArgumentException>(() => opentok.Dial(sessionId, token, sipUri));
        }

        [Fact]
        public async Task DialAsyncThrowsExceptionWithEmptySession()
        {
            string sessionId = "";
            string token = "1234567890";
            string sipUri = "SIPURI";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            await Assert.ThrowsAsync<OpenTokArgumentException>(() => opentok.DialAsync(sessionId, token, sipUri));
        }

        [Fact]
        public void DialCorrectUrl()
        {
            // arrange
            string token = "1234567890";
            string sipUri = "SIPURI";

            var expectedUrl = $"v2/project/{ApiKey}/dial";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.Post(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(string.Empty)
                .Verifiable();


            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            // act
            opentok.Dial(SessionId, token, sipUri);

            // assert
            mockClient.Verify();
        }

        [Fact]
        public async Task DialAsyncCorrectUrl()
        {
            // arrange
            string token = "1234567890";
            string sipUri = "SIPURI";

            var expectedUrl = $"v2/project/{ApiKey}/dial";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.PostAsync(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(string.Empty)
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            // act
            await opentok.DialAsync(SessionId, token, sipUri);

            // assert
            mockClient.Verify();
        }

        [Fact]
        public void DialCorrectHeaders()
        {
            // arrange
            string token = "1234567890";
            string sipUri = "SIPURI";

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
            opentok.Dial(SessionId, token, sipUri);

            // assert
            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);
        }

        [Fact]
        public async Task DialAsyncCorrectHeaders()
        {
            // arrange
            string token = "1234567890";
            string sipUri = "SIPURI";

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
            await opentok.DialAsync(SessionId, token, sipUri);

            // assert
            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);
        }

        [Fact]
        public void DialCorrectData()
        {
            // arrange
            string token = "1234567890";
            string sipUri = "SIPURI";

            DialOptions dialOptions = new DialOptions
            {
                From = "bob",
                Headers = null,
                Auth = new DialAuth { Username = "Tim", Password = "Bob" },
                Secure = true,
                Video = true,
                ObserveForceMute = true
            };

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
            opentok.Dial(SessionId, token, sipUri, dialOptions);

            // assert
            Assert.NotNull(dataSent);
            Assert.Equal(SessionId, dataSent["sessionId"]);
            Assert.Equal(token, dataSent["token"]);

            Assert.True(dataSent.ContainsKey("sip"));

            dynamic sip = dataSent["sip"];

            Assert.Equal(dialOptions.From, sip.from);
            Assert.Equal(dialOptions.Headers, sip.headers);
            Assert.Equal(dialOptions.Auth, sip.auth);
            Assert.Equal(dialOptions.Secure, sip.secure);
            Assert.Equal(dialOptions.Video, sip.video);
            Assert.Equal(dialOptions.ObserveForceMute, sip.observeForceMute);
        }

        [Fact]
        public async Task DialAsyncCorrectData()
        {
            // arrange
            string token = "1234567890";
            string sipUri = "SIPURI";

            DialOptions dialOptions = new DialOptions
            {
                From = "bob",
                Headers = null,
                Auth = new DialAuth { Username = "Tim", Password = "Bob" },
                Secure = true,
                Video = true,
                ObserveForceMute = true
            };

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                })
                .ReturnsAsync(string.Empty);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.DialAsync(SessionId, token, sipUri, dialOptions);

            // assert
            Assert.NotNull(dataSent);
            Assert.Equal(SessionId, dataSent["sessionId"]);
            Assert.Equal(token, dataSent["token"]);

            Assert.True(dataSent.ContainsKey("sip"));

            dynamic sip = dataSent["sip"];

            Assert.Equal(dialOptions.From, sip.from);
            Assert.Equal(dialOptions.Headers, sip.headers);
            Assert.Equal(dialOptions.Auth, sip.auth);
            Assert.Equal(dialOptions.Secure, sip.secure);
            Assert.Equal(dialOptions.Video, sip.video);
            Assert.Equal(dialOptions.ObserveForceMute, sip.observeForceMute);
        }
    }
}
