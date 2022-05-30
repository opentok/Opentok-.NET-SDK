using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Util;
using OpenTokSDK.Exception;
using System.Net;

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
            Assert.IsType<OpenTok>(opentok);
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
            Assert.Equal("focus", data["initial_layout_class_list"]);
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

            Assert.Equal(3, exceptions.Count);
            foreach(Exception exception in exceptions)
            {
                Assert.True(exception is OpenTokArgumentException);
            }

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
        public void ListArchivesTestWithValidSessionId()
        {
            var sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
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
            ArchiveList archives = opentok.ListArchives(sessionId:sessionId);

            Assert.NotNull(archives);
            Assert.Equal(6, archives.Count);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + apiKey + $"/archive?offset=0&sessionId={sessionId}"))), Times.Once());
        }

        [Fact]
        public void TestListArchivesBadCount()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            try
            {
                opentok.ListArchives(count: -5);
                Assert.True(false, "TestListArchivesBadCount should not have reached here as the count passed in was negative");
            }
            catch (OpenTokArgumentException ex)
            {
                Assert.Equal("count cannot be smaller than 0", ex.Message);
            }            

        }

        [Fact]
        public void TestListArchivesBadSessionId()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            try
            {
                opentok.ListArchives(sessionId: "This-is-not-a-valid-session-id");
                Assert.True(false, "TestListArchivesBadCount should not have reached here as the count passed in was negative");
            }
            catch (OpenTokArgumentException ex)
            {
                Assert.Equal("Session Id is not valid", ex.Message);
            }

        }

        [Fact(Skip = "This tests a private method, private methods should be free to be changed or refactored.")]
        public void TestArchiveScreenShareLayout()
        {
            var expected = @"{""sessionId"":""abcd12345"",""name"":""an_archive_name"",""hasVideo"":true,""hasAudio"":true,""outputMode"":""composed"",""layout"":{""type"":""bestFit"",""screensharetype"":""bestFit""}}";
            var httpClient = new HttpClient();
            var data = new Dictionary<string, object>() { { "sessionId", "abcd12345" }, { "name", "an_archive_name" }, { "hasVideo", true }, { "hasAudio", true }, { "outputMode", "composed" } };
            var layout = new ArchiveLayout { Type = LayoutType.bestFit, ScreenShareType=ScreenShareLayoutType.BestFit };
            data.Add("layout", layout);
            var headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            var clientType = typeof(HttpClient);
            var layoutString = (string)clientType.GetMethod("GetRequestPostData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(httpClient, new object[] { data, headers });
            Assert.Equal(expected, layoutString);
        }

        [Fact(Skip = "This tests a private method, private methods should be free to be changed or refactored.")]
        public void TestSetArchiveScreenShareType()
        {
            var opentok = new OpenTok(apiKey, apiSecret);
            var layout = new ArchiveLayout { Type = LayoutType.bestFit, ScreenShareType = ScreenShareLayoutType.Pip };
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var archiveId = "123456789";
            var expectedUrl = $"v2/project/{apiKey}/archive/{archiveId}/layout";
            var mockClient = new Mock<HttpClient>();
            opentok.Client = mockClient.Object;
            mockClient.Setup(c => c.Put(expectedUrl, headers, It.Is<Dictionary<string, object>>(x => (string)x["type"] == "bestFit" && (string)x["screenshareType"] == "pip")));
            Assert.True(opentok.SetArchiveLayout(archiveId, layout));
        }

        [Fact]
        public void TestSetArchiveScreenShareTypeInvalid()
        {
            var opentok = new OpenTok(apiKey, apiSecret);
            var layout = new ArchiveLayout { Type = LayoutType.pip, ScreenShareType = ScreenShareLayoutType.Pip };

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.SetArchiveLayout("12345", layout));
            
            Assert.Contains("Invalid layout, when ScreenShareType is set, Type must be bestFit", exception.Message);
            Assert.Equal("layout", exception.ParamName);
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
            headers.Add("Content-Type", "application/json");
            var clientType = typeof(HttpClient);
            var layoutString = (string)clientType.GetMethod("GetRequestPostData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(httpClient, new object[] { data, headers });
            Assert.Equal(expected, layoutString);            
        }

        [Fact(Skip = "This tests a private method, private methods should be free to be changed or refactored.")]
        public void TestArchiveNonCustomLayout()
        {
            var expectedString = @"{""sessionId"":""abcd12345"",""name"":""an_archive_name"",""hasVideo"":true,""hasAudio"":true,""outputMode"":""composed"",""layout"":{""type"":""pip""}}";
            var httpClient = new HttpClient();
            var data = new Dictionary<string, object>() { { "sessionId", "abcd12345" }, { "name", "an_archive_name" }, { "hasVideo", true }, { "hasAudio", true }, { "outputMode", "composed" } };
            var layout = new ArchiveLayout { Type = LayoutType.pip };
            data.Add("layout", layout);
            var headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            var clientType = typeof(HttpClient);
            var layoutString = (string)clientType.GetMethod("GetRequestPostData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(httpClient, new object[] { data, headers });
            Assert.Equal(expectedString, layoutString);            
        }

        [Fact(Skip = "This tests a private method, private methods should be free to be changed or refactored.")]
        public void TestArchiveNonCustomLayoutEmptyString()
        {
            var expectedString = @"{""sessionId"":""abcd12345"",""name"":""an_archive_name"",""hasVideo"":true,""hasAudio"":true,""outputMode"":""composed"",""layout"":{""type"":""pip""}}";
            var httpClient = new HttpClient();
            var data = new Dictionary<string, object>() { { "sessionId", "abcd12345" }, { "name", "an_archive_name" }, { "hasVideo", true }, { "hasAudio", true }, { "outputMode", "composed" } };
            var layout = new ArchiveLayout { Type = LayoutType.pip, StyleSheet=string.Empty };
            data.Add("layout", layout);
            var headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            var clientType = typeof(HttpClient);
            var layoutString = (string)clientType.GetMethod("GetRequestPostData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(httpClient, new object[] { data, headers });
            Assert.Equal(expectedString, layoutString);
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
            } catch (OpenTokArgumentException)
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
        public void TestStartBroadcastScreenShareInvalidType()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            BroadcastLayout layout = new BroadcastLayout(BroadcastLayout.LayoutType.Pip) { ScreenShareType = ScreenShareLayoutType.BestFit };
            try
            {
                opentok.StartBroadcast("abcd", layout: layout);
                Assert.True(false, "Should have seen an exception");
            }
            catch (OpenTokArgumentException ex)
            {

                Assert.Equal($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}", ex.Message);
            }
        }
    }
}

