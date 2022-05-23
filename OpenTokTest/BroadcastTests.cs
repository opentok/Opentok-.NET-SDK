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
        public void StartBroadcast()
        {
            string sessionId = "SESSIONID";
            string returnString = GetResponseJson();

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
            var layout = new BroadcastLayout(ScreenShareLayoutType.BestFit);

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
        public void StartBroadcastWithHDResolution()
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
        public void StartBroadcastWithSDResolution()
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
        public void StartBroadcastOnlyWithRTMP()
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
        public void StartBroadcastWithRTMPandHLS()
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

        [Fact]
        public void StartBroadcastWithInvalidResolutionThrowsException()
        {
            string sessionId = "SESSIONID";
            string resolution = "300x300";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.StartBroadcast(sessionId, resolution: resolution));
            Assert.NotNull(exception);
            Assert.Contains("Invalid resolution. See https://www.dev.tokbox.com/developer/rest/#start_broadcast for valid resolutions.", exception.Message);
            Assert.Equal("resolution", exception.ParamName);
        }

        [Fact]
        public void StartBroadcastWithDvrAndLowLatencyThrowsException()
        {
            string sessionId = "SESSIONID";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.StartBroadcast(sessionId, dvr: true, lowLatency: true));
            Assert.NotNull(exception);
            Assert.Contains("Cannot set both dvr and lowLatency on HLS.", exception.Message);
        }

        [Fact]
        public void StartBroadcastWithDvr()
        {
            string sessionId = "SESSIONID";
            string returnString = GetResponseJson();

            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                 {
                     dataSent = data;
                 })
                .Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, hls: true, dvr: true);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());

            Assert.NotNull(broadcast);
            Assert.NotNull(dataSent);
            Assert.True(dataSent.ContainsKey("outputs"));

            var outputs = dataSent["outputs"] as IDictionary<string, object>;

            Assert.NotNull(outputs);
            Assert.True(outputs.ContainsKey("hls"));

            var hls = outputs["hls"] as IDictionary<string, bool>;
            Assert.NotNull(hls);

            Assert.True(hls["dvr"]);
            Assert.False(hls.ContainsKey("lowLatency"));
        }

        [Fact]
        public void StartBroadcastWithLowLatency()
        {
            string sessionId = "SESSIONID";
            string returnString = GetResponseJson();

            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    dataSent = data;
                })
                .Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StartBroadcast(sessionId, hls: true, lowLatency: true);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/broadcast")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());

            Assert.NotNull(broadcast);
            Assert.True(broadcast.Settings.Hls.LowLatency);
            Assert.NotNull(dataSent);
            Assert.True(dataSent.ContainsKey("outputs"));

            var outputs = dataSent["outputs"] as IDictionary<string, object>;

            Assert.NotNull(outputs);
            Assert.True(outputs.ContainsKey("hls"));

            var hls = outputs["hls"] as IDictionary<string, bool>;
            Assert.NotNull(hls);

            Assert.True(hls["lowLatency"]);
            Assert.False(hls["dvr"]);
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

        // Set Broadcast Layout

        [Fact]
        public void SetBroadcastLayoutScreenShareType()
        {
            var broadcastId = "12345";
            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{broadcastId}/layout";
            var layout = new BroadcastLayout(ScreenShareLayoutType.BestFit);
            var expectedHeaders = new Dictionary<string, string> { { "Content-Type", "application/json" } };

            var expectedContent = new Dictionary<string, object>
            {
                {"type","bestFit" },
                {"screenShareType", "bestFit"}
            };

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient.Setup(x => x.Put(expectedUrl, expectedHeaders, expectedContent))
                .Returns("")
                .Verifiable();


            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            opentok.SetBroadcastLayout(broadcastId, layout);

            mockClient.Verify(c => c.Put(expectedUrl, expectedHeaders, expectedContent));
        }

        [Fact]
        public void SetBroadcastLayoutScreenShareTypeCustom()
        {
            var broadcastId = "12345";
            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{broadcastId}/layout";
            var layout = new BroadcastLayout(BroadcastLayout.LayoutType.Custom)
            {
                Stylesheet = "test"
            };

            var expectedHeaders = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var expectedContent = new Dictionary<string, object>
            {
                {"type","custom" },
                {"stylesheet", "test"}
            };

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient.Setup(x => x.Put(expectedUrl, expectedHeaders, expectedContent))
                .Returns("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            opentok.SetBroadcastLayout(broadcastId, layout);

            mockClient.Verify(c => c.Put(expectedUrl, expectedHeaders, expectedContent));
        }

        [Fact]
        public void SetBroadcastLayoutScreenShareTypeInvalid()
        {
            var layout = new BroadcastLayout(ScreenShareLayoutType.BestFit) { Type = BroadcastLayout.LayoutType.Pip };
            var opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.SetBroadcastLayout("12345", layout));
            Assert.Equal($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}",
                exception.Message);
        }

        [Fact]
        public async Task SetBroadcastLayoutAsyncScreenShareTypeInvalid()
        {
            var layout = new BroadcastLayout(ScreenShareLayoutType.BestFit) { Type = BroadcastLayout.LayoutType.Pip };
            var opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.SetBroadcastLayoutAsync("12345", layout));
            Assert.Equal($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}",
                exception.Message);
        }

        [Fact]
        public void SetBroadcastLayoutTypeAndStylesheetInvalid()
        {
            var layout = new BroadcastLayout(ScreenShareLayoutType.BestFit)
            {
                Type = BroadcastLayout.LayoutType.Custom,
                Stylesheet = null
            };
            var opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.SetBroadcastLayout("12345", layout));
            Assert.Contains("Could not set the layout. Either an invalid JSON or an invalid layout options.",
                exception.Message);
            Assert.Equal("layout", exception.ParamName);
        }

        [Fact]
        public async Task SetBroadcastLayoutAsyncTypeAndStylesheetInvalid()
        {
            var layout = new BroadcastLayout(ScreenShareLayoutType.BestFit)
            {
                Type = BroadcastLayout.LayoutType.Custom,
                Stylesheet = null
            };
            var opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.SetBroadcastLayoutAsync("12345", layout));
            Assert.Contains("Could not set the layout. Either an invalid JSON or an invalid layout options.",
                exception.Message);
            Assert.Equal("layout", exception.ParamName);


        // Stop Broadcast

        [Fact]
        public void StopBroadcast()
        {
            string broadcastId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(
                    It.IsAny<string>(), 
                    It.IsAny<Dictionary<string, string>>(), 
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.StopBroadcast(broadcastId);

            Assert.NotNull(broadcast);
            Assert.Equal(broadcastId, broadcast.Id);
            Assert.NotNull(broadcast.Id);

            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{broadcastId}/stop";
            mockClient.Verify(httpClient => httpClient.Post(
                    It.Is<string>(url => url.Equals(expectedUrl)), 
                    It.IsAny<Dictionary<string, string>>(), 
                    It.IsAny<Dictionary<string, object>>()),
                Times.Once());
        }

        [Fact]
        public async Task StopBroadcastAsync()
        {
            string broadcastId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = await opentok.StopBroadcastAsync(broadcastId);

            Assert.NotNull(broadcast);
            Assert.Equal(broadcastId, broadcast.Id);
            Assert.NotNull(broadcast.Id);

            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{broadcastId}/stop";
            mockClient.Verify(httpClient => httpClient.PostAsync(
                    It.Is<string>(url => url.Equals(expectedUrl)),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()),
                Times.Once());
        }


        // Get Broadcast

        [Fact]
        public void GetBroadcast()
        {
            string broadcastId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = opentok.GetBroadcast(broadcastId);

            Assert.NotNull(broadcast);
            Assert.Equal(broadcastId, broadcast.Id);
            Assert.NotNull(broadcast.Id);

            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{broadcastId}";
            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals(expectedUrl))), Times.Once());
        }

        [Fact]
        public async Task GetBroadcastAsync()
        {
            string broadcastId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Broadcast broadcast = await opentok.GetBroadcastAsync(broadcastId);

            Assert.NotNull(broadcast);
            Assert.Equal(broadcastId, broadcast.Id);
            Assert.NotNull(broadcast.Id);

            var expectedUrl = $"v2/project/{ApiKey}/broadcast/{broadcastId}";
            mockClient.Verify(httpClient => httpClient.GetAsync(It.Is<string>(url => url.Equals(expectedUrl)), null), Times.Once());
        }
    }
}
