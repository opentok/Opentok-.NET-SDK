using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OpenTokSDK;
using OpenTokSDK.Util;
using OpenTokSDK.Exception;

namespace OpenTokSDKTest
{
    public class OpenTokTest
    {
        private int apiKey = 123456;
        private string apiSecret = "1234567890abcdef1234567890abcdef1234567890";

        [Fact]
        public void InitializationTest()
        {
            var opentok = new OpenTok(apiKey, apiSecret);
            Assert.IsType(typeof(OpenTok), opentok);
        }
        
        // TODO: all create session and archive tests should verify the HTTP request body

        [Fact]
        public void CreateSimpleSessionTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);
            
            HttpClient client = mockClient.Object;

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = client;
            Session session = opentok.CreateSession();

            Assert.NotNull(session);
            Assert.Equal(this.apiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(session.MediaMode, MediaMode.RELAYED);
            Assert.Equal(session.Location, "");

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateRoutedSessionTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            HttpClient client = mockClient.Object;

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = client;
            Session session = opentok.CreateSession(mediaMode: MediaMode.ROUTED);

            Assert.NotNull(session);
            Assert.Equal(this.apiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(session.MediaMode, MediaMode.ROUTED);
            Assert.Equal(session.Location, "");

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateSessionWithLocationTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            HttpClient client = mockClient.Object;

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = client;
            Session session = opentok.CreateSession(location: "0.0.0.0");

            Assert.NotNull(session);
            Assert.Equal(this.apiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(session.MediaMode, MediaMode.RELAYED);
            Assert.Equal(session.Location, "0.0.0.0");

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), 
                            It.IsAny<Dictionary<string, object>>()), Times.Once());
        }
   
        [Fact]
        public void CreateRoutedSessionWithLocationTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            HttpClient client = mockClient.Object;

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = client;
            Session session = opentok.CreateSession(location: "0.0.0.0", mediaMode: MediaMode.ROUTED);

            Assert.NotNull(session);
            Assert.Equal(this.apiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(session.MediaMode, MediaMode.ROUTED);
            Assert.Equal(session.Location, "0.0.0.0");

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateAlwaysArchivedSessionTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><sessions><Session><" +
                                "session_id>" + sessionId + "</session_id><partner_id>123456</partner_id><create_dt>" +
                                "Mon Mar 17 00:41:31 PDT 2014</create_dt></Session></sessions>";
            var expectedUrl = "session/create";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            HttpClient client = mockClient.Object;

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = client;
            Session session = opentok.CreateSession(mediaMode: MediaMode.ROUTED, archiveMode: ArchiveMode.ALWAYS);

            Assert.NotNull(session);
            Assert.Equal(this.apiKey, session.ApiKey);
            Assert.Equal(sessionId, session.Id);
            Assert.Equal(session.MediaMode, MediaMode.ROUTED);
            Assert.Equal(session.ArchiveMode, ArchiveMode.ALWAYS);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals(expectedUrl)), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateInvalidSessionLocationTest()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns("This function should not return anything");

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session;
            try
            {
                session = opentok.CreateSession(location: "A location");
                Assert.True(false);
            }
            catch (OpenTokArgumentException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void CreateInvalidAlwaysArchivedReplayedSessionTest()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns("This function should not return anything");

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session;
            try
            {
                session = opentok.CreateSession(mediaMode: MediaMode.RELAYED, archiveMode: ArchiveMode.ALWAYS);
                Assert.True(false);
            }
            catch (OpenTokArgumentException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void GenerateTokenTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            
            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";          
            string token = opentok.GenerateToken(sessionId);

            Assert.NotNull(token);
            var data = CheckToken(token, apiKey);

            Assert.Equal(data["partner_id"], apiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], Role.PUBLISHER.ToString());
        }

        [Fact]
        public void GenerateTokenWithRoleTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);

            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            string token = opentok.GenerateToken(sessionId, role:Role.SUBSCRIBER);

            Assert.NotNull(token);
            var data = CheckToken(token, apiKey);

            Assert.Equal(data["partner_id"], apiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], Role.SUBSCRIBER.ToString());
        }
        
        
        [Fact]
        public void GenerateTokenWithExpireTimeTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            double expireTime = OpenTokUtils.GetCurrentUnixTimeStamp() + 10;

            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            string token = opentok.GenerateToken(sessionId, expireTime: expireTime);

            Assert.NotNull(token);
            var data = CheckToken(token, apiKey);

            Assert.Equal(data["partner_id"], apiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], Role.PUBLISHER.ToString());
            Assert.Equal(data["expire_time"], ((long) expireTime).ToString());
        }

        [Fact]
        public void GenerateTokenWithConnectionDataTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            double expireTime = OpenTokUtils.GetCurrentUnixTimeStamp() + 10;
            string connectionData =  "Somedatafortheconnection";
            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            string token = opentok.GenerateToken(sessionId, data:connectionData);

            Assert.NotNull(token);
            var data = CheckToken(token, apiKey);

            Assert.Equal(data["partner_id"], apiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], Role.PUBLISHER.ToString());
            Assert.Equal(data["connection_data"], connectionData);
        }

        [Fact]
        public void GenerateInvalidTokensTest()
        {
            string token;
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            var exceptions = new List<Exception>();
            try
            {
                // Generate token with empty sessionId
                token = opentok.GenerateToken(null);
            }
            catch(OpenTokArgumentException e)
            {
                exceptions.Add(e);
            }

            try
            {
                // Generate token with empty sessionId
                token = opentok.GenerateToken("");
            }
            catch (OpenTokArgumentException e)
            {
                exceptions.Add(e);
            }

            try
            {
                // Generate token with empty sessionId
                token = opentok.GenerateToken("NOT A VALID SESSION ID");
            }
            catch (OpenTokArgumentException e)
            {
                exceptions.Add(e);
            }

            Assert.Equal(exceptions.Count, 3);
            foreach(Exception exception in exceptions)
            {
                Assert.True(exception is OpenTokArgumentException);
            }

        }

        [Fact]
        public void GetArchiveTest()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = "{\n" +
                                    " \"createdAt\" : 1395187836000,\n" +
                                    " \"duration\" : 62,\n" +
                                    " \"id\" : \"" + archiveId + "\",\n" +
                                    " \"name\" : \"\",\n" +
                                    " \"partnerId\" : 123456,\n" +
                                    " \"reason\" : \"\",\n" +
                                    " \"sessionId\" : \"SESSIONID\",\n" +
                                    " \"size\" : 8347554,\n" +
                                    " \"status\" : \"available\",\n" +
                                    " \"url\" : \"http://tokbox.com.archive2.s3.amazonaws.com/123456%2F" +
                                    archiveId + "%2Farchive.mp4?Expires=1395194362&AWSAccessKeyId=AKIAI6LQCPIXYVWCQV6Q&Si" +
                                    "gnature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\"\n" + " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);
            
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.GetArchive(archiveId);

            Assert.NotNull(archive);
            Assert.Equal(this.apiKey, archive.PartnerId);
            Assert.Equal(archiveId, archive.Id.ToString());
            Assert.Equal(1395187836000L, archive.CreatedAt);
            Assert.Equal(62, archive.Duration);
            Assert.Equal("", archive.Name);
            Assert.Equal("SESSIONID", archive.SessionId);
            Assert.Equal(8347554, archive.Size);
            Assert.Equal(ArchiveStatus.AVAILABLE, archive.Status);
            Assert.Equal("http://tokbox.com.archive2.s3.amazonaws.com/123456%2F" + archiveId + "%2Farchive.mp4?Expires=13951" +
                    "94362&AWSAccessKeyId=AKIAI6LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", archive.Url);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/"+this.apiKey+"/archive/"+archiveId))), Times.Once());
        }

        [Fact]
        public void GetExpiredArchiveTest()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = "{\n" +
                                    " \"createdAt\" : 1395187836000,\n" +
                                    " \"duration\" : 62,\n" +
                                    " \"id\" : \"" + archiveId + "\",\n" +
                                    " \"name\" : \"\",\n" +
                                    " \"partnerId\" : 123456,\n" +
                                    " \"reason\" : \"\",\n" +
                                    " \"sessionId\" : \"SESSIONID\",\n" +
                                    " \"size\" : 8347554,\n" +
                                    " \"status\" : \"expired\",\n" +
                                    " \"url\" : null\n" + " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.GetArchive(archiveId);

            Assert.NotNull(archive);
            Assert.Equal(ArchiveStatus.EXPIRED, archive.Status);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + this.apiKey + "/archive/" + archiveId))), Times.Once());
        }

        public void GetArchiveWithUnknownPropertiesTest()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = "{\n" +
                                    " \"createdAt\" : 1395187836000,\n" +
                                    " \"duration\" : 62,\n" +
                                    " \"id\" : \"" + archiveId + "\",\n" +
                                    " \"name\" : \"\",\n" +
                                    " \"partnerId\" : 123456,\n" +
                                    " \"reason\" : \"\",\n" +
                                    " \"sessionId\" : \"SESSIONID\",\n" +
                                    " \"size\" : 8347554,\n" +
                                    " \"status\" : \"expired\",\n" +
                                    " \"notarealproperty\" : \"not a real value\",\n" +
                                    " \"url\" : null\n" + " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.GetArchive(archiveId);

            Assert.NotNull(archive);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + this.apiKey + "/archive/" + archiveId))), Times.Once());
        }

        
        [Fact]
        public void ListArchivesTest()
        {
            string returnString = "{\n" +
                                " \"count\" : 6,\n" +
                                " \"items\" : [ {\n" +
                                " \"createdAt\" : 1395187930000,\n" +
                                " \"duration\" : 22,\n" +
                                " \"id\" : \"ef546c5a-4fd7-4e59-ab3d-f1cfb4148d1d\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 2909274,\n" +
                                " \"status\" : \"available\",\n" +
                                " \"url\" : \"http://tokbox.com.archive2.s3.amazonaws.com/123456%2Fef546c5" +
                                "a-4fd7-4e59-ab3d-f1cfb4148d1d%2Farchive.mp4?Expires=1395188695&AWSAccessKeyId=AKIAI6" +
                                "LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\"\n" +
                                " }, {\n" +
                                " \"createdAt\" : 1395187910000,\n" +
                                " \"duration\" : 14,\n" +
                                " \"id\" : \"5350f06f-0166-402e-bc27-09ba54948512\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 1952651,\n" +
                                " \"status\" : \"available\",\n" +
                                " \"url\" : \"http://tokbox.com.archive2.s3.amazonaws.com/123456%2F5350f06" +
                                "f-0166-402e-bc27-09ba54948512%2Farchive.mp4?Expires=1395188695&AWSAccessKeyId=AKIAI6" +
                                "LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\"\n" +
                                " }, {\n" +
                                " \"createdAt\" : 1395187836000,\n" +
                                " \"duration\" : 62,\n" +
                                " \"id\" : \"f6e7ee58-d6cf-4a59-896b-6d56b158ec71\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 8347554,\n" +
                                " \"status\" : \"available\",\n" +
                                " \"url\" : \"http://tokbox.com.archive2.s3.amazonaws.com/123456%2Ff6e7ee5" +
                                "8-d6cf-4a59-896b-6d56b158ec71%2Farchive.mp4?Expires=1395188695&AWSAccessKeyId=AKIAI6" +
                                "LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\"\n" +
                                " }, {\n" +
                                " \"createdAt\" : 1395183243000,\n" +
                                " \"duration\" : 544,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 78499758,\n" +
                                " \"status\" : \"available\",\n" +
                                " \"url\" : \"http://tokbox.com.archive2.s3.amazonaws.com/123456%2F30b3ebf" +
                                "1-ba36-4f5b-8def-6f70d9986fe9%2Farchive.mp4?Expires=1395188695&AWSAccessKeyId=AKIAI6" +
                                "LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\"\n" +
                                " }, {\n" +
                                " \"createdAt\" : 1394396753000,\n" +
                                " \"duration\" : 24,\n" +
                                " \"id\" : \"b8f64de1-e218-4091-9544-4cbf369fc238\",\n" +
                                " \"name\" : \"showtime again\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 2227849,\n" +
                                " \"status\" : \"available\",\n" +
                                " \"url\" : \"http://tokbox.com.archive2.s3.amazonaws.com/123456%2Fb8f64de" +
                                "1-e218-4091-9544-4cbf369fc238%2Farchive.mp4?Expires=1395188695&AWSAccessKeyId=AKIAI6" +
                                "LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\"\n" +
                                " }, {\n" +
                                " \"createdAt\" : 1394321113000,\n" +
                                " \"duration\" : 1294,\n" +
                                " \"id\" : \"832641bf-5dbf-41a1-ad94-fea213e59a92\",\n" +
                                " \"name\" : \"showtime\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 42165242,\n" +
                                " \"status\" : \"available\",\n" +
                                " \"url\" : \"http://tokbox.com.archive2.s3.amazonaws.com/123456%2F832641b" +
                                "f-5dbf-41a1-ad94-fea213e59a92%2Farchive.mp4?Expires=1395188695&AWSAccessKeyId=AKIAI6" +
                                "LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\"\n" +
                                " } ]\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            ArchiveList archives = opentok.ListArchives();

            Assert.NotNull(archives);
            Assert.Equal(6, archives.Count);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/"+apiKey + "/archive?offset=0"))), Times.Once());
        }

        [Fact]
        public void StartArchiveTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);
            
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(sessionId, null);

            Assert.NotNull(archive);
            Assert.Equal(sessionId, archive.SessionId);
            Assert.NotNull(archive.Id);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/"+ apiKey +"/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveIndividualTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"outputMode\" : \"individual\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \""+ sessionId +"\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.INDIVIDUAL);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.INDIVIDUAL, archive.OutputMode);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveVoiceOnlyTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null,\n" +
                                " \"hasVideo\" : false,\n" +
                                " \"hasAudio\" : true\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(sessionId, hasVideo: false);

            Assert.NotNull(archive);
            Assert.Equal(sessionId, archive.SessionId);
            Assert.NotNull(archive.Id);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StopArchiveTest()
        {
            string archiveId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), 
                It.IsAny<Dictionary<string, string>>(), 
                It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StopArchive(archiveId);

            Assert.NotNull(archive);
            Assert.Equal("SESSIONID", archive.SessionId);
            Assert.Equal(archiveId, archive.Id.ToString());

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(
                url => url.Equals("v2/project/" + apiKey + "/archive/" + archiveId +"/stop")), 
                It.IsAny<Dictionary<string, string>>(), 
                It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void DeleteArchiveTest()
        {
            string archiveId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>()));

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            opentok.DeleteArchive(archiveId);

            mockClient.Verify(httpClient => httpClient.Delete(It.Is<string>(
                url => url.Equals("v2/project/" + apiKey + "/archive/" + archiveId)),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        private Dictionary<string,string> CheckToken(string token, int apiKey)
        {
            string baseToken = OpenTokUtils.Decode64(token.Substring(4));
            char[] sep = { '&' };
            string[] tokenFields = baseToken.Split(sep);
            var tokenData = new Dictionary<string, string>();

            for (int i = 0; i < tokenFields.Length; i ++)
            {
                tokenData.Add(tokenFields[i].Split('=')[0], tokenFields[i].Split('=')[1]);
            }
            return tokenData;
        }
    }
}
