using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OpenTokSDKTest
{
    public class BroadcastTests : TestBase
    {
        // StartBroadcast

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

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(Broadcast.BroadcastStatus.STARTED, broadcast.Status);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartBroadcastWithScreenShareType()
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
            var expectedUrl = $"v2/project/{ApiKey}/broadcast";
            var outputs = new Dictionary<string, object> { { "hls", new object() } };
            var data = new Dictionary<string, object>
            {
                { "sessionId", sessionId },
                { "maxDuration", 7200 },
                { "outputs", outputs }
            };
            var layout = new BroadcastLayout(ScreenShareLayoutType.BestFit);
            data.Add("layout", layout);

            mockClient.Setup(httpClient => httpClient.Post(
                expectedUrl,
                It.IsAny<Dictionary<string, string>>(),
                It.Is<Dictionary<string, object>>(x =>
                   (string)x["sessionId"] == sessionId && x["layout"] == layout && (int)x["maxDuration"] == 7200 && ((Dictionary<string, object>)x["outputs"]).ContainsKey("hls")
                ))).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            Broadcast broadcast = opentok.StartBroadcast(sessionId, layout: layout);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(Broadcast.BroadcastStatus.STARTED, broadcast.Status);
        }

        [Fact]
        public void StartBroadcastScreenShareInvalidType()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            BroadcastLayout layout = new BroadcastLayout(BroadcastLayout.LayoutType.Pip) { ScreenShareType = ScreenShareLayoutType.BestFit };

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.StartBroadcast("abcd", layout: layout));
            Assert.Equal($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}", exception.Message);
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

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, resolution: resolution);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.Equal(resolution, broadcast.Resolution);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(Broadcast.BroadcastStatus.STARTED, broadcast.Status);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, resolution: resolution);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.Equal(resolution, broadcast.Resolution);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(Broadcast.BroadcastStatus.STARTED, broadcast.Status);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, hls: false, rtmpList: rtmpList);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.NotNull(broadcast.RtmpList);
            Assert.Equal(2, broadcast.RtmpList.Count);
            Assert.Null(broadcast.Hls);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(Broadcast.BroadcastStatus.STARTED, broadcast.Status);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, rtmpList: rtmpList);

            Assert.NotNull(broadcast);
            Assert.Equal(sessionId, broadcast.SessionId);
            Assert.NotNull(broadcast.RtmpList);
            Assert.Equal(2, broadcast.RtmpList.Count);
            Assert.NotNull(broadcast.Hls);
            Assert.NotNull(broadcast.Id);
            Assert.Equal(Broadcast.BroadcastStatus.STARTED, broadcast.Status);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartBroadcastWithManualStreamMode()
        {
            string sessionId = "SESSIONID";
            string responseJson = GetResponseJson();
            
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(responseJson)
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, streamMode: StreamMode.Manual);


            mockClient.Verify(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());

            Assert.NotNull(broadcast);
            Assert.Equal(StreamMode.Manual, broadcast.StreamMode);
            Assert.NotNull(dataSent);
            Assert.True(dataSent.ContainsKey("streamMode"));
            Assert.Equal("manual", dataSent["streamMode"]);
        }

        [Fact]
        public void StartBroadcastWithAutoStreamMode()
        {
            string sessionId = "SESSIONID";
            string responseJson = GetResponseJson();

            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(responseJson)
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, streamMode: StreamMode.Auto);


            mockClient.Verify(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());

            Assert.NotNull(broadcast);
            Assert.Equal(StreamMode.Auto, broadcast.StreamMode);
            Assert.NotNull(dataSent);
            Assert.True(dataSent.ContainsKey("streamMode"));
            Assert.Equal("auto", dataSent["streamMode"]);
        }

        // AddStreamToBroadcast

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AddStreamToBroadcastInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            Assert.Throws<OpenTokArgumentException>(() => opentok.AddStreamToBroadcast(archiveId, streamId));
        }

        [Fact]
        public void AddStreamToBroadcastCorrectUrl()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{archiveId}/streams";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.Patch(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.AddStreamToBroadcast(archiveId, streamId);

            mockClient.Verify();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void AddStreamToBroadcastHeaderAndDataCorrect(bool hasAudio, bool hasVideo)
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.Patch(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.AddStreamToBroadcast(archiveId, streamId, hasAudio, hasVideo);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "addStream", streamId }, { "hasAudio", hasAudio }, { "hasVideo", hasVideo } }, dataSent);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task AddStreamToBroadcastAsyncInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.AddStreamToBroadcastAsync(archiveId, streamId));
        }

        [Fact]
        public async Task AddStreamToBroadcastAsyncCorrectUrl()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{archiveId}/streams";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.PatchAsync(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.AddStreamToBroadcastAsync(archiveId, streamId);

            mockClient.Verify();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task AddStreamToBroadcastAsyncHeaderAndDataCorrect(bool hasAudio, bool hasVideo)
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PatchAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.AddStreamToBroadcastAsync(archiveId, streamId, hasAudio, hasVideo);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "addStream", streamId }, { "hasAudio", hasAudio }, { "hasVideo", hasVideo } }, dataSent);
        }

        // RemoveStreamFromBroadcast

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RemoveStreamFromBroadcastInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            Assert.Throws<OpenTokArgumentException>(() => opentok.RemoveStreamFromBroadcast(archiveId, streamId));
        }

        [Fact]
        public void RemoveStreamFromBroadcastCorrectUrl()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{archiveId}/streams";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.Patch(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.RemoveStreamFromBroadcast(archiveId, streamId);

            mockClient.Verify();
        }

        [Fact]
        public void RemoveStreamFromBroadcastHeaderAndDataCorrect()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.Patch(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.RemoveStreamFromBroadcast(archiveId, streamId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "removeStream", streamId } }, dataSent);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task RemoveStreamFromBroadcastAsyncInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.RemoveStreamFromBroadcastAsync(archiveId, streamId));
        }

        [Fact]
        public async Task RemoveStreamFromBroadcastAsyncCorrectUrl()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{archiveId}/streams";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.PatchAsync(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.RemoveStreamFromBroadcastAsync(archiveId, streamId);

            mockClient.Verify();
        }

        [Fact]
        public async Task RemoveStreamFromBroadcastAsyncHeaderAndDataCorrect()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            Dictionary<string, string> headersSent = null;
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PatchAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    headersSent = headers;
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.RemoveStreamFromBroadcastAsync(archiveId, streamId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "removeStream", streamId } }, dataSent);
        }
    }
}
