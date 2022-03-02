using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class SessionTests : TestBase
    {
        [Theory]
        [InlineData(SecurityProtocolType.Tls11)]
        [InlineData(SecurityProtocolType.Tls12)]
        [InlineData((SecurityProtocolType)0)]
        public void CreateSessionFailedDueToTLS(SecurityProtocolType protocolType)
        {
            ServicePointManager.SecurityProtocol = protocolType;
            var e = new WebException("Test Exception");

            try
            {
                OpenTokUtils.ValidateTlsVersion(e);
                Assert.NotEqual(SecurityProtocolType.Tls11, protocolType);
            }
            catch (OpenTokWebException ex)
            {
                Assert.Equal("Error with request submission.\nThis application appears to not support TLS1.2.\nPlease enable TLS 1.2 and try again.", ex.Message);
                Assert.Equal(SecurityProtocolType.Tls11, protocolType);
            }

        }

        // TODO: all create session and archive tests should verify the HTTP request body

        [Fact]
        public void CreateSessionSimple()
        {
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + SessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);
            
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Session session = opentok.CreateSession();

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(SessionId, session.Id);
            Assert.Equal(MediaMode.RELAYED, session.MediaMode);
            Assert.Equal("", session.Location);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task CreateSessionAsyncSimple()
        {
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                  "session_id>" + SessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                  "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret)
            {
                Client = mockClient.Object
            };
            Session session = await opentok.CreateSessionAsync();

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(SessionId, session.Id);
            Assert.Equal(MediaMode.RELAYED, session.MediaMode);
            Assert.Equal("", session.Location);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }
        
        [Fact]
        public void CreateSessionRouted()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);
            
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Session session = opentok.CreateSession(mediaMode: MediaMode.ROUTED);

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(MediaMode.ROUTED, session.MediaMode);
            Assert.Equal("", session.Location);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task CreateSessionAsyncRouted()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                  "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                  "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Session session = await opentok.CreateSessionAsync(mediaMode: MediaMode.ROUTED);

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(MediaMode.ROUTED, session.MediaMode);
            Assert.Equal("", session.Location);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateSessionWithLocation()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            HttpClient client = mockClient.Object;

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = client;
            Session session = opentok.CreateSession(location: "0.0.0.0");

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(MediaMode.RELAYED, session.MediaMode);
            Assert.Equal("0.0.0.0", session.Location);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(),
                            It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task CreateSessionAsyncWithLocation()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                  "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                  "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(returnString);

            HttpClient client = mockClient.Object;

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = client;
            Session session = await opentok.CreateSessionAsync(location: "0.0.0.0");

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(MediaMode.RELAYED, session.MediaMode);
            Assert.Equal("0.0.0.0", session.Location);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateSessionRoutedWithLocation()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);
            
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Session session = opentok.CreateSession(location: "0.0.0.0", mediaMode: MediaMode.ROUTED);

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(MediaMode.ROUTED, session.MediaMode);
            Assert.Equal("0.0.0.0", session.Location);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task CreateSessionAsyncRoutedWithLocation()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                  "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                  "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Session session = await opentok.CreateSessionAsync(location: "0.0.0.0", mediaMode: MediaMode.ROUTED);

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(MediaMode.ROUTED, session.MediaMode);
            Assert.Equal("0.0.0.0", session.Location);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateSessionAlwaysArchived()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);
            
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Session session = opentok.CreateSession(mediaMode: MediaMode.ROUTED, archiveMode: ArchiveMode.ALWAYS);

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(MediaMode.ROUTED, session.MediaMode);
            Assert.Equal(ArchiveMode.ALWAYS, session.ArchiveMode);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task CreateSessionAsyncAlwaysArchived()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                  "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                  "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Session session = await opentok.CreateSessionAsync(mediaMode: MediaMode.ROUTED, archiveMode: ArchiveMode.ALWAYS);

            Assert.NotNull(session);
            Assert.Equal(ApiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(MediaMode.ROUTED, session.MediaMode);
            Assert.Equal(ArchiveMode.ALWAYS, session.ArchiveMode);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateSessionInvalidLocation()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns("This function should not return anything");

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception =
                Assert.Throws<OpenTokArgumentException>(() => opentok.CreateSession("A location"));

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task CreateSessionAsyncInvalidLocation()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync("This function should not return anything");

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception =
                await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.CreateSessionAsync("A location"));

            Assert.NotNull(exception);
        }

        [Fact]
        public void CreateSessionInvalidAlwaysArchivedReplayed()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns("This function should not return anything");

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() =>
                opentok.CreateSession(mediaMode: MediaMode.RELAYED, archiveMode: ArchiveMode.ALWAYS));

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task CreateSessionAsyncInvalidAlwaysArchivedReplayed()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync("This function should not return anything");

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () =>
                await opentok.CreateSessionAsync(mediaMode: MediaMode.RELAYED, archiveMode: ArchiveMode.ALWAYS));

            Assert.NotNull(exception);
        }
    }
}
