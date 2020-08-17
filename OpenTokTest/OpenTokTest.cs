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
        public void GenerateTokenWithInitialLayoutClass()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);

            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            List<string> initalLayoutClassList = new List<string>();
            initalLayoutClassList.Add("focus");
            string token = opentok.GenerateToken(sessionId, initialLayoutClassList: initalLayoutClassList);

            Assert.NotNull(token);
            var data = CheckToken(token, apiKey);

            Assert.Equal(data["partner_id"], apiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], Role.PUBLISHER.ToString());
            Assert.Equal(data["initial_layout_class_list"], "focus");
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
        public void StartArchiveWithSDResolutionTest()
        {
            string sessionId = "SESSIONID";
            string resolution = "640x480";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"outputMode\" : \"composed\",\n" +
                                " \"resolution\" : \"640x480\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"" + sessionId + "\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void TestArchiveCustomLayout()
        {
            var expected = @"{""sessionId"":""abcd12345"",""name"":""an_archive_name"",""hasVideo"":true,""hasAudio"":true,""outputMode"":""composed"",""layout"":{""type"":""custom"",""stylesheet"":""stream.instructor {position: absolute; width: 100%;  height:50%;}""}}";
            var httpClient = new HttpClient();
            var data = new Dictionary<string, object>() { { "sessionId", "abcd12345" }, { "name", "an_archive_name" }, { "hasVideo", true }, { "hasAudio", true }, { "outputMode", "composed" } };
            var layout = new ArchiveLayout { Type = LayoutType.custom, StyleSheet = "stream.instructor {position: absolute; width: 100%;  height:50%;}" };
            data.Add("layout", layout);
            var headers = new Dictionary<string, string>();
            headers.Add("Content-type", "application/json");
            var clientType = typeof(HttpClient);
            var layoutString = (string)clientType.GetMethod("GetRequestPostData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(httpClient, new object[] { data, headers });
            Assert.Equal(expected, layoutString);            
        }

        [Fact]
        public void TestArchiveNonCustomLayout()
        {
            var expectedString = @"{""sessionId"":""abcd12345"",""name"":""an_archive_name"",""hasVideo"":true,""hasAudio"":true,""outputMode"":""composed"",""layout"":{""type"":""pip""}}";
            var httpClient = new HttpClient();
            var data = new Dictionary<string, object>() { { "sessionId", "abcd12345" }, { "name", "an_archive_name" }, { "hasVideo", true }, { "hasAudio", true }, { "outputMode", "composed" } };
            var layout = new ArchiveLayout { Type = LayoutType.pip };
            data.Add("layout", layout);
            var headers = new Dictionary<string, string>();
            headers.Add("Content-type", "application/json");
            var clientType = typeof(HttpClient);
            var layoutString = (string)clientType.GetMethod("GetRequestPostData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(httpClient, new object[] { data, headers });
            Assert.Equal(expectedString, layoutString);            
        }

        [Fact]
        public void StartArchiveCustomLayout()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"outputMode\" : \"composed\",\n" +
                                " \"resolution\" : \"1280x720\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"" + sessionId + "\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout { Type = LayoutType.custom, StyleSheet = "stream.instructor {position: absolute; width: 100%;  height:50%;}" };
            Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution, layout: layout);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveCustomLayoutMissingStylesheet()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"outputMode\" : \"composed\",\n" +
                                " \"resolution\" : \"1280x720\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"" + sessionId + "\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout { Type = LayoutType.custom };
            try
            {
                Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution, layout: layout);
                Assert.True(false, "StartArchive should have thrown an exception");
            }
            catch(OpenTokArgumentException ex)
            {
                Assert.Equal("Could not set layout, stylesheet must be set if and only if type is custom", ex.Message);
            }
        }

        [Fact]
        public void StartArchiveWithHDResolutionTest()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"outputMode\" : \"composed\",\n" +
                                " \"resolution\" : \"1280x720\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"" + sessionId + "\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void CreateInvalidIndividualModeArchivingWithResolution()
        {
            string sessionId = "SESSIONID";
            string resolution = "640x480";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"outputMode\" : \"individual\",\n" +
                                " \"resolution\" : \"640x480\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"" + sessionId + "\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive;
            try
            {
                archive = opentok.StartArchive(sessionId, outputMode: OutputMode.INDIVIDUAL, resolution: resolution);
                Assert.True(false);
            }
            catch (OpenTokArgumentException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void StartArchiveNoResolutionTest()
        {
            string sessionId = "SESSIONID";
            string resolution = "640x480";
            string returnString = "{\n" +
                                " \"createdAt\" : 1395183243556,\n" +
                                " \"duration\" : 0,\n" +
                                " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                " \"name\" : \"\",\n" +
                                " \"partnerId\" : 123456,\n" +
                                " \"reason\" : \"\",\n" +
                                " \"sessionId\" : \"SESSIONID\",\n" +
                                " \"resolution\" : \"640x480\",\n" +
                                " \"size\" : 0,\n" +
                                " \"status\" : \"started\",\n" +
                                " \"url\" : null\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(sessionId);

            Assert.NotNull(archive);
            Assert.Equal(sessionId, archive.SessionId);
            Assert.NotNull(archive.Id);
            Assert.Equal(resolution, archive.Resolution);

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
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            opentok.DeleteArchive(archiveId);

            mockClient.Verify(httpClient => httpClient.Delete(It.Is<string>(
                url => url.Equals("v2/project/" + apiKey + "/archive/" + archiveId)),
                It.IsAny<Dictionary<string, string>>()), Times.Once());
        }

        [Fact]
        public void GetStreamTestFromOpenTokInstance()
        {
            string sessionId = "SESSIONID";
            string streamId = "be8f21f4-a133-43ae-a20a-bb29a1caaa46";
            string returnString = "{\n" +
                                    " \"id\" : \"" + streamId + "\",\n" +
                                    " \"name\" : \"johndoe\",\n" +
                                    " \"layoutClassList\" : [\"asdf\"],\n" +
                                    " \"videoType\" : \"screen\",\n" +
                                   " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Stream stream = opentok.GetStream(sessionId, streamId);

            List<string> LayoutClassList = new List<string>();
            LayoutClassList.Add("asdf");

            Assert.NotNull(stream);
            Assert.Equal(streamId, stream.Id);
            Assert.Equal("johndoe", stream.Name);
            Assert.Equal("screen", stream.VideoType);
            Assert.Equal(LayoutClassList, stream.LayoutClassList);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + this.apiKey + "/session/" + sessionId +"/stream/" + streamId))), Times.Once());
        }

        [Fact]
        public void GetStreamEmptyTestFromOpenTokInstance()
        {
            string sessionId = "SESSIONID";
            string streamId = "be8f21f4-a133-43ae-a20a-bb29a1caaa46";
            string returnString = "{\n" +
                                    " \"id\" : \"" + streamId + "\",\n" +
                                    " \"name\" : \"johndoe\",\n" +
                                    " \"layoutClassList\" : [],\n" +
                                    " \"videoType\" : \"screen\",\n" +
                                   " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Stream stream = opentok.GetStream(sessionId, streamId);

            List<string> LayoutClassList = new List<string>();

            Assert.NotNull(stream);
            Assert.Equal(streamId, stream.Id);
            Assert.Equal("johndoe", stream.Name);
            Assert.Equal("screen", stream.VideoType);
            Assert.Equal(LayoutClassList, stream.LayoutClassList);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + this.apiKey + "/session/" + sessionId +"/stream/" + streamId))), Times.Once());
        }

        [Fact]
        public void GetStreamTestThrowArgumentException()
        {
            string returnString = "";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            try
            {
                Stream stream = opentok.GetStream(null, null);

            }
            catch (OpenTokArgumentException e)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void ListStreamTestFromOpenTokInstance()
        {
            string sessionId = "SESSIONID";
            List<string> LayoutClassListOne = new List<string>();
            List<string> LayoutClassListTwo = new List<string>();
            LayoutClassListOne.Add("layout1");
            LayoutClassListTwo.Add("layout2");

            string returnString = "{\n" +
                                " \"count\" : 2,\n" +
                                " \"items\" : [ {\n" +
                                " \"id\" : \"ef546c5a-4fd7-4e59-ab3d-f1cfb4148d1d\",\n" +
                                " \"name\" : \"johndoe\",\n" +
                                " \"layoutClassList\" : [\"layout1\"],\n" +
                                " \"videoType\" : \"screen\",\n" +
                                " }, {\n" +
                                " \"createdAt\" : 1394321113000,\n" +
                                " \"duration\" : 1294,\n" +
                                " \"id\" : \"1f546c5a-4fd7-4e59-ab3d-f1cfb4148d1d\",\n" +
                                " \"name\" : \"janedoe\",\n" +
                                " \"layoutClassList\" : [\"layout2\"],\n" +
                                " \"videoType\" : \"camera\",\n" +
                                " } ]\n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            StreamList streamList = opentok.ListStreams(sessionId);
            Stream streamOne = streamList[0];
            Stream streamTwo = streamList[1];

            Assert.NotNull(streamList);
            Assert.Equal(2, streamList.Count);
            Assert.Equal("ef546c5a-4fd7-4e59-ab3d-f1cfb4148d1d", streamOne.Id);
            Assert.Equal("johndoe", streamOne.Name);
            Assert.Equal(LayoutClassListOne, streamOne.LayoutClassList);
            Assert.Equal("screen", streamOne.VideoType);

            Assert.Equal("1f546c5a-4fd7-4e59-ab3d-f1cfb4148d1d", streamTwo.Id);
            Assert.Equal("janedoe", streamTwo.Name);
            Assert.Equal(LayoutClassListTwo, streamTwo.LayoutClassList);
            Assert.Equal("camera", streamTwo.VideoType);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + this.apiKey + "/session/" + sessionId + "/stream"))), Times.Once());
        }
        [Fact]
        public void ListStreamTestThrowArgumentException()
        {
            string returnString = "";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            try
            {
                StreamList streamlist = opentok.ListStreams(null);

            }
            catch (OpenTokArgumentException e)
            {
                Assert.True(true);
            }
        }


        [Fact]
        public void ForceDisconnectOpenTokArgumentTest()
        {
            string connectionId = "";
            string sessionId = "";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            try
            {
                opentok.ForceDisconnect(sessionId, connectionId);
            } catch (OpenTokArgumentException e)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void ForceDisconnectTest()
        {
            string connectionId = "3b0c260e-801e-47e9-a245-4ee3ffd9bd6f";
            string sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            opentok.ForceDisconnect(sessionId, connectionId);

            mockClient.Verify(httpClient => httpClient.Delete(It.Is<string>(
                url => url.Equals("v2/project/" + apiKey + "/session/" + sessionId + "/connection/" + connectionId)),
                It.IsAny<Dictionary<string, string>>()), Times.Once());
        }

        [Fact]
        public void SignalOpenTokArgumentExceptionTest()
        {
            string sessionId = "";
            SignalProperties signalProperties = new SignalProperties("data");

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()));

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            try
            {
                opentok.Signal(sessionId, signalProperties);
            }
            catch (OpenTokArgumentException e)
            {
                Assert.True(true);
            }
        }
        [Fact]
        public void SignalToAllTest()
        {
            string sessionId = "SESSIONID";
            SignalProperties signalProperties = new SignalProperties("data");

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()));

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            opentok.Signal(sessionId, signalProperties);
        }

        [Fact]
        public void SignalWithDataAndTypeTest()
        {
            string sessionId = "SESSIONID";
            SignalProperties signalProperties = new SignalProperties("data", "type");

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()));

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            opentok.Signal(sessionId, signalProperties);
        }

        [Fact]
        public void SignalToSingleConnection()
        {
            string sessionId = "SESSIONID";
            string connectionId = "CONNECTIONID";
            SignalProperties signalProperties = new SignalProperties("data");

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()));

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            opentok.Signal(sessionId, signalProperties, connectionId);
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

        [Fact]
        public void StartBroadcastTest()
        {
            string sessionId = "SESSIONID";
            string returnString = "{\n" +
                                  " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                  " \"sessionId\" : \"SESSIONID\",\n" +
                                  " \"projectId\" : 123456,\n" +
                                  " \"createdAt\" : 1395183243556,\n" +
                                  " \"updatedAt\" : 1395183243556,\n" +
                                  " \"resolution\" : \"640x480\",\n" +
                                  " \"status\" : \"started\",\n" +
                                  " \"broadcastUrls\": { \n" +
                                    " \"hls\": \"http://server/fakepath/playlist.m3u8\", \n" +
                                  " } \n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(broadcast.Status, Broadcast.BroadcastStatus.STARTED);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartBroadcastWithHDResolutionTest()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string returnString = "{\n" +
                                  " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                  " \"sessionId\" : \"SESSIONID\",\n" +
                                  " \"projectId\" : 123456,\n" +
                                  " \"createdAt\" : 1395183243556,\n" +
                                  " \"updatedAt\" : 1395183243556,\n" +
                                  " \"resolution\" : \"1280x720\",\n" +
                                  " \"status\" : \"started\",\n" +
                                  " \"broadcastUrls\": { \n" +
                                    " \"hls\": \"http://server/fakepath/playlist.m3u8\", \n" +
                                  " } \n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, resolution: resolution);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.Equal(resolution, broadcast.Resolution);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(broadcast.Status, Broadcast.BroadcastStatus.STARTED);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartBroadcastWithSDResolutionTest()
        {
            string sessionId = "SESSIONID";
            string resolution = "640x480";
            string returnString = "{\n" +
                                  " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                  " \"sessionId\" : \"SESSIONID\",\n" +
                                  " \"projectId\" : 123456,\n" +
                                  " \"createdAt\" : 1395183243556,\n" +
                                  " \"updatedAt\" : 1395183243556,\n" +
                                  " \"resolution\" : \"640x480\",\n" +
                                  " \"status\" : \"started\",\n" +
                                  " \"broadcastUrls\": { \n" +
                                    " \"hls\": \"http://server/fakepath/playlist.m3u8\", \n" +
                                  " } \n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, resolution: resolution);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.Equal(resolution, broadcast.Resolution);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(broadcast.Status, Broadcast.BroadcastStatus.STARTED);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartBroadcastOnlyWithRTMPTest()
        {
            string sessionId = "SESSIONID";
            List<Rtmp> rtmpList = new List<Rtmp>();
            rtmpList.Add(new Rtmp("foo", "rtmp://myfooserver/myfooapp", "myfoostream"));
            rtmpList.Add(new Rtmp("bar", "rtmp://mybarserver/mybarapp", "mybarstream"));

            string returnString = "{\n" +
                                  " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                  " \"sessionId\" : \"SESSIONID\",\n" +
                                  " \"projectId\" : 123456,\n" +
                                  " \"createdAt\" : 1395183243556,\n" +
                                  " \"updatedAt\" : 1395183243556,\n" +
                                  " \"resolution\" : \"640x480\",\n" +
                                  " \"status\" : \"started\",\n" +
                                  " \"broadcastUrls\": { \n" +
                                    " \"rtmp\": [ \n" +
                                        " { \n" +
                                            " \"status\": \"connecting\", \n" +
                                            " \"id\": \"foo\", \n" +
                                            " \"serverUrl\": \"rtmp://myfooserver/myfooapp\", \n" +
                                            " \"streamName\": \"myfoostream\" \n" +
                                        " }, \n" +
                                        " { \n" +
                                            " \"status\": \"connecting\", \n" +
                                            " \"id\": \"bar\", \n" +
                                            " \"serverUrl\": \"rtmp://mybarserver/mybarapp\", \n" +
                                            " \"streamName\": \"mybarstream\" \n" +
                                        " } \n" +
                                    " ] \n" +
                                  " } \n" +
                                " } ";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, hls: false, rtmpList: rtmpList);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.NotNull(broadcast.RtmpList);
            Assert.Equal(broadcast.RtmpList.Count(), 2);
            Assert.Null(broadcast.Hls);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(broadcast.Status, Broadcast.BroadcastStatus.STARTED);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartBroadcastWithRTMPandHLSTest()
        {
            string sessionId = "SESSIONID";
            List<Rtmp> rtmpList = new List<Rtmp>();
            rtmpList.Add(new Rtmp("foo", "rtmp://myfooserver/myfooapp", "myfoostream"));
            rtmpList.Add(new Rtmp("bar", "rtmp://mybarserver/mybarapp", "mybarstream"));

            string returnString = "{\n" +
                                  " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                  " \"sessionId\" : \"SESSIONID\",\n" +
                                  " \"projectId\" : 123456,\n" +
                                  " \"createdAt\" : 1395183243556,\n" +
                                  " \"updatedAt\" : 1395183243556,\n" +
                                  " \"resolution\" : \"640x480\",\n" +
                                  " \"status\" : \"started\",\n" +
                                  " \"broadcastUrls\": { \n" +
                                    " \"hls\": \"http://server/fakepath/playlist.m3u8\", \n" +
                                    " \"rtmp\": [ \n" +
                                        " { \n" +
                                            " \"status\": \"connecting\", \n" +
                                            " \"id\": \"foo\", \n" +
                                            " \"serverUrl\": \"rtmp://myfooserver/myfooapp\", \n" +
                                            " \"streamName\": \"myfoostream\" \n" +
                                        " }, \n" +
                                        " { \n" +
                                            " \"status\": \"connecting\", \n" +
                                            " \"id\": \"bar\", \n" +
                                            " \"serverUrl\": \"rtmp://mybarserver/mybarapp\", \n" +
                                            " \"streamName\": \"mybarstream\" \n" +
                                        " } \n" +
                                    " ] \n" +
                                  " } \n" +
                                " } ";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, rtmpList: rtmpList);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.NotNull(broadcast.RtmpList);
            Assert.Equal(broadcast.RtmpList.Count(), 2);
            Assert.NotNull(broadcast.Hls);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(broadcast.Status, Broadcast.BroadcastStatus.STARTED);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StopBroadcastTest()
        {
            string broadcastId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            string returnString = "{\n" +
                                  " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                  " \"sessionId\" : \"SESSIONID\",\n" +
                                  " \"projectId\" : 123456,\n" +
                                  " \"createdAt\" : 1395183243556,\n" +
                                  " \"updatedAt\" : 1395183243556,\n" +
                                  " \"resolution\" : \"640x480\",\n" +
                                  " \"broadcastUrls\": null \n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StopBroadcast(broadcastId);

            Assert.NotNull(broadcast);
            Assert.Equal(broadcastId, broadcast.Id.ToString());
            Assert.NotNull(broadcast.Id);
            

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/broadcast/" + broadcastId + "/stop")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void GetBroadcastTest()
        {
            string broadcastId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            string returnString = "{\n" +
                                  " \"id\" : \"30b3ebf1-ba36-4f5b-8def-6f70d9986fe9\",\n" +
                                  " \"sessionId\" : \"SESSIONID\",\n" +
                                  " \"projectId\" : 123456,\n" +
                                  " \"createdAt\" : 1395183243556,\n" +
                                  " \"updatedAt\" : 1395183243556,\n" +
                                  " \"resolution\" : \"640x480\",\n" +
                                  " \"status\" : \"started\",\n" +
                                  " \"broadcastUrls\": { \n" +
                                    " \"hls\": \"http://server/fakepath/playlist.m3u8\", \n" +
                                  " } \n" +
                                " }";
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.GetBroadcast(broadcastId);

            Assert.NotNull(broadcast);
            Assert.Equal(broadcastId, broadcast.Id.ToString());
            Assert.NotNull(broadcast.Id);
            
            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + this.apiKey + "/broadcast/" + broadcastId))), Times.Once());
        }

        [Fact]
        public void SetStreamClassListsTest()
        {
            string sessionId = "SESSIONID";
            string streamId = "STREAMID";
            List<StreamProperties> streamPropertiesList = new List<StreamProperties>();
            StreamProperties streamProperties = new StreamProperties(streamId);
            streamProperties.addLayoutClass("focus");
            streamPropertiesList.Add(streamProperties);

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Put(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns("This function should not return anything");

            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            opentok.Client = mockClient.Object;
            opentok.SetStreamClassLists(sessionId, streamPropertiesList);
            
            mockClient.Verify(httpClient => httpClient.Put(It.Is<string>(url => url.Equals("v2/project/" + apiKey + "/session/" + sessionId + "/stream")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }
    }
}

