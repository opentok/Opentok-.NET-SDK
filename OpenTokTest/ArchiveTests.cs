using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class ArchiveTests : TestBase
    {
        // StartArchive

        [Fact]
        public void StartArchive()
        {
            string responseJson = GetResponseJson();

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(SessionId, null);

            Assert.NotNull(archive);
            Assert.Equal(SessionId, archive.SessionId);
            Assert.NotEqual(Guid.Empty, archive.Id);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveIndividual()
        {
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(SessionId, outputMode: OutputMode.INDIVIDUAL);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.INDIVIDUAL, archive.OutputMode);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveWithSDResolution()
        {
            string sessionId = "SESSIONID";
            string resolution = "640x480";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveScreenShareInvalidType()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            ArchiveLayout layout = new ArchiveLayout { Type = LayoutType.pip, ScreenShareType = ScreenShareLayoutType.BestFit };

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.StartArchive("abcd", layout: layout));
            Assert.Equal($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}", exception.Message);
        }

        [Fact]
        public void StartArchiveCustomLayout()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout { Type = LayoutType.custom, StyleSheet = "stream.instructor {position: absolute; width: 100%;  height:50%;}" };
            Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution, layout: layout);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveVerticalLayout()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout
            {
                Type = LayoutType.verticalPresentation,
                StyleSheet = ""
            };
            Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution, layout: layout);
            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);
            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveVerticalLayoutWithStyleSheet()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout
            {
                Type = LayoutType.verticalPresentation,
                StyleSheet = "blah"
            };

            var exception = Assert.Throws<OpenTokArgumentException>(() =>
                opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution,
                    layout: layout));

            Assert.Equal("Could not set layout, stylesheet must be set if and only if type is custom", exception.Message);
        }

        [Fact]
        public void StartArchiveCustomLayoutMissingStylesheet()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout { Type = LayoutType.custom };

            var exception = Assert.Throws<OpenTokArgumentException>(() =>
                opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution,
                    layout: layout));

            Assert.Equal("Could not set layout, stylesheet must be set if and only if type is custom", exception.Message);
        }

        [Fact]
        public void StartArchiveWithHDResolution()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveCreateInvalidIndividualModeArchivingWithResolution()
        {
            string sessionId = "SESSIONID";
            string resolution = "640x480";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = Assert.Throws<OpenTokArgumentException>(() =>
                opentok.StartArchive(sessionId, outputMode: OutputMode.INDIVIDUAL, resolution: resolution));

            Assert.Equal("Resolution can't be specified for Individual Archives", exception.Message);
        }

        [Fact]
        public void StartArchiveNoResolution()
        {
            string resolution = "640x480";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(SessionId);

            Assert.NotNull(archive);
            Assert.Equal(SessionId, archive.SessionId);
            Assert.NotEqual(Guid.Empty, archive.Id);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveVoiceOnly()
        {
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>())).Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(SessionId, hasVideo: false);

            Assert.NotNull(archive);
            Assert.Equal(SessionId, archive.SessionId);
            Assert.NotEqual(Guid.Empty, archive.Id);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveAutoStreamMode()
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
            Archive archive = opentok.StartArchive(sessionId, streamMode: StreamMode.Auto);

            Assert.NotNull(archive);
            Assert.Equal(StreamMode.Auto, archive.StreamMode);

            Assert.True(dataSent.ContainsKey("streamMode"));
            Assert.Equal("auto", dataSent["streamMode"]);

            mockClient.Verify(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StartArchiveManualStreamMode()
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
            Archive archive = opentok.StartArchive(sessionId, streamMode: StreamMode.Manual);

            Assert.NotNull(archive);
            Assert.Equal(StreamMode.Manual, archive.StreamMode);

            Assert.True(dataSent.ContainsKey("streamMode"));
            Assert.Equal("manual", dataSent["streamMode"]);

            mockClient.Verify(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }


        // AddStreamToArchive

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AddStreamToArchiveInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            Assert.Throws<OpenTokArgumentException>(() => opentok.AddStreamToArchive(archiveId, streamId));
        }

        [Fact]
        public void AddStreamToArchiveCorrectUrl()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            var expectedUrl = $"v2/project/{ApiKey}/archive/{archiveId}/streams";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.Patch(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.AddStreamToArchive(archiveId, streamId);

            mockClient.Verify();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void AddStreamToArchiveHeaderAndDataCorrect(bool hasAudio, bool hasVideo)
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
            opentok.AddStreamToArchive(archiveId, streamId, hasAudio, hasVideo);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "addStream", streamId }, { "hasAudio", hasAudio }, { "hasVideo", hasVideo } }, dataSent);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task AddStreamToArchiveAsyncInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.AddStreamToArchiveAsync(archiveId, streamId));
        }

        [Fact]
        public async Task AddStreamToArchiveAsyncCorrectUrl()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            var expectedUrl = $"v2/project/{ApiKey}/archive/{archiveId}/streams";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.PatchAsync(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.AddStreamToArchiveAsync(archiveId, streamId);

            mockClient.Verify();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task AddStreamToArchiveAsyncHeaderAndDataCorrect(bool hasAudio, bool hasVideo)
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
            await opentok.AddStreamToArchiveAsync(archiveId, streamId, hasAudio, hasVideo);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "addStream", streamId }, { "hasAudio", hasAudio }, { "hasVideo", hasVideo } }, dataSent);
        }

        // RemoveStreamFromArchive

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RemoveStreamFromArchiveInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            Assert.Throws<OpenTokArgumentException>(() => opentok.RemoveStreamFromArchive(archiveId, streamId));
        }

        [Fact]
        public void RemoveStreamFromArchiveCorrectUrl()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            var expectedUrl = $"v2/project/{ApiKey}/archive/{archiveId}/streams";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.Patch(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.RemoveStreamFromArchive(archiveId, streamId);

            mockClient.Verify();
        }

        [Fact]
        public void RemoveStreamFromArchiveHeaderAndDataCorrect()
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
            opentok.RemoveStreamFromArchive(archiveId, streamId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "removeStream", streamId } }, dataSent);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task RemoveStreamFromArchiveAsyncInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.RemoveStreamFromArchiveAsync(archiveId, streamId));
        }

        [Fact]
        public async Task RemoveStreamFromArchiveAsyncCorrectUrl()
        {
            string archiveId = "ARCHIVEID";
            string streamId = "1234567890";

            var expectedUrl = $"v2/project/{ApiKey}/archive/{archiveId}/streams";

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient
                .Setup(httpClient => httpClient.PatchAsync(expectedUrl, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync("")
                .Verifiable();

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.RemoveStreamFromArchiveAsync(archiveId, streamId);

            mockClient.Verify();
        }

        [Fact]
        public async Task RemoveStreamFromArchiveAsyncHeaderAndDataCorrect()
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
            await opentok.RemoveStreamFromArchiveAsync(archiveId, streamId);

            Assert.NotNull(headersSent);
            Assert.Equal(new Dictionary<string, string> { { "Content-Type", "application/json" } }, headersSent);

            Assert.NotNull(dataSent);
            Assert.Equal(new Dictionary<string, object> { { "removeStream", streamId } }, dataSent);
        }
    }
}
