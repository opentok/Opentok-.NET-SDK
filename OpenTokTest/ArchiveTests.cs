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
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StartArchive(SessionId, null);

            Assert.NotNull(archive);
            Assert.Equal(SessionId, archive.SessionId);
            Assert.NotEqual(Guid.Empty, archive.Id);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task StartArchiveAsync()
        {
            string responseJson = GetResponseJson();

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StartArchiveAsync(SessionId, null);

            Assert.NotNull(archive);
            Assert.Equal(SessionId, archive.SessionId);
            Assert.NotEqual(Guid.Empty, archive.Id);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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
        public async Task StartArchiveAsyncIndividual()
        {
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StartArchiveAsync(SessionId, outputMode: OutputMode.INDIVIDUAL);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.INDIVIDUAL, archive.OutputMode);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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
        public async Task StartArchiveAsyncWithSDResolution()
        {
            string sessionId = "SESSIONID";
            string resolution = "640x480";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StartArchiveAsync(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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
        public async Task StartArchiveAsyncScreenShareInvalidType()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            ArchiveLayout layout = new ArchiveLayout { Type = LayoutType.pip, ScreenShareType = ScreenShareLayoutType.BestFit };

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.StartArchiveAsync("abcd", layout: layout));
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
        public async Task StartArchiveAsyncCustomLayout()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout { Type = LayoutType.custom, StyleSheet = "stream.instructor {position: absolute; width: 100%;  height:50%;}" };
            Archive archive = await opentok.StartArchiveAsync(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution, layout: layout);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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
        public async Task StartArchiveAsyncVerticalLayout()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);
            
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout
            {
                Type = LayoutType.verticalPresentation,
                StyleSheet = ""
            };

            Archive archive = await opentok.StartArchiveAsync(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution, layout: layout);
            
            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);
            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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
        public async Task StartArchiveAsyncVerticalLayoutWithStyleSheet()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout
            {
                Type = LayoutType.verticalPresentation,
                StyleSheet = "blah"
            };

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () =>
                await opentok.StartArchiveAsync(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution,
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
        public async Task StartArchiveAsyncCustomLayoutMissingStylesheet()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            var layout = new ArchiveLayout { Type = LayoutType.custom };

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () =>
               await opentok.StartArchiveAsync(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution,
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
        public async Task StartArchiveAsyncWithHDResolution()
        {
            string sessionId = "SESSIONID";
            string resolution = "1280x720";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StartArchiveAsync(sessionId, outputMode: OutputMode.COMPOSED, resolution: resolution);

            Assert.NotNull(archive);
            Assert.Equal(OutputMode.COMPOSED, archive.OutputMode);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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
        public async Task StartArchiveAsyncCreateInvalidIndividualModeArchivingWithResolution()
        {
            string sessionId = "SESSIONID";
            string resolution = "640x480";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () =>
                await opentok.StartArchiveAsync(sessionId, outputMode: OutputMode.INDIVIDUAL, resolution: resolution));

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
        public async Task StartArchiveAsyncNoResolution()
        {
            string resolution = "640x480";
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StartArchiveAsync(SessionId);

            Assert.NotNull(archive);
            Assert.Equal(SessionId, archive.SessionId);
            Assert.NotEqual(Guid.Empty, archive.Id);
            Assert.Equal(resolution, archive.Resolution);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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
        public async Task StartArchiveAsyncVoiceOnly()
        {
            string responseJson = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StartArchiveAsync(SessionId, hasVideo: false);

            Assert.NotNull(archive);
            Assert.Equal(SessionId, archive.SessionId);
            Assert.NotEqual(Guid.Empty, archive.Id);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive")), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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
        public async Task StartArchiveAsyncAutoStreamMode()
        {
            string sessionId = "SESSIONID";
            string responseJson = GetResponseJson();
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson)
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StartArchiveAsync(sessionId, streamMode: StreamMode.Auto);

            Assert.NotNull(archive);
            Assert.Equal(StreamMode.Auto, archive.StreamMode);

            Assert.True(dataSent.ContainsKey("streamMode"));
            Assert.Equal("auto", dataSent["streamMode"]);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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

        [Fact]
        public async Task StartArchiveAsyncManualStreamMode()
        {
            string sessionId = "SESSIONID";
            string responseJson = GetResponseJson();
            Dictionary<string, object> dataSent = null;

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(responseJson)
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    dataSent = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StartArchiveAsync(sessionId, streamMode: StreamMode.Manual);

            Assert.NotNull(archive);
            Assert.Equal(StreamMode.Manual, archive.StreamMode);

            Assert.True(dataSent.ContainsKey("streamMode"));
            Assert.Equal("manual", dataSent["streamMode"]);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
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

        // Get Archive

        [Fact]
        public void GetArchive()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = GetResponseJson(new Dictionary<string, string> { { "archiveId", "936da01f-9abd-4d9d-80c7-02af85c822a8" } });
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret)
            {
                Client = mockClient.Object
            };
            Archive archive = opentok.GetArchive(archiveId);

            Assert.NotNull(archive);
            Assert.Equal(ApiKey, archive.PartnerId);
            Assert.Equal(archiveId, archive.Id.ToString());
            Assert.Equal(1395187836000L, archive.CreatedAt);
            Assert.Equal(62, archive.Duration);
            Assert.Equal("", archive.Name);
            Assert.Equal("SESSIONID", archive.SessionId);
            Assert.Equal(8347554, archive.Size);
            Assert.Equal(ArchiveStatus.AVAILABLE, archive.Status);
            Assert.Equal("http://tokbox.com.archive2.s3.amazonaws.com/123456%2F" + archiveId + "%2Farchive.mp4?Expires=13951" +
                    "94362&AWSAccessKeyId=AKIAI6LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", archive.Url);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId))), Times.Once());
        }

        [Fact]
        public async Task GetArchiveAsync()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = GetResponseJson(new Dictionary<string, string> { { "archiveId", "936da01f-9abd-4d9d-80c7-02af85c822a8" } });
            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret)
            {
                Client = mockClient.Object
            };
            Archive archive = await opentok.GetArchiveAsync(archiveId);

            Assert.NotNull(archive);
            Assert.Equal(ApiKey, archive.PartnerId);
            Assert.Equal(archiveId, archive.Id.ToString());
            Assert.Equal(1395187836000L, archive.CreatedAt);
            Assert.Equal(62, archive.Duration);
            Assert.Equal("", archive.Name);
            Assert.Equal("SESSIONID", archive.SessionId);
            Assert.Equal(8347554, archive.Size);
            Assert.Equal(ArchiveStatus.AVAILABLE, archive.Status);
            Assert.Equal("http://tokbox.com.archive2.s3.amazonaws.com/123456%2F" + archiveId + "%2Farchive.mp4?Expires=13951" +
                         "94362&AWSAccessKeyId=AKIAI6LQCPIXYVWCQV6Q&Signature=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", archive.Url);

            mockClient.Verify(httpClient => httpClient.GetAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId)), null), Times.Once());
        }

        [Fact]
        public void GetExpiredArchive()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = GetResponseJson(new Dictionary<string, string> { { "archiveId", "936da01f-9abd-4d9d-80c7-02af85c822a8" } });
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.GetArchive(archiveId);

            Assert.NotNull(archive);
            Assert.Equal(ArchiveStatus.EXPIRED, archive.Status);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId))), Times.Once());
        }

        [Fact]
        public async Task GetExpiredArchiveAsync()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = GetResponseJson(new Dictionary<string, string> { { "archiveId", "936da01f-9abd-4d9d-80c7-02af85c822a8" } });
            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.GetArchiveAsync(archiveId);

            Assert.NotNull(archive);
            Assert.Equal(ArchiveStatus.EXPIRED, archive.Status);

            mockClient.Verify(httpClient => httpClient.GetAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId)), null), Times.Once());
        }

        [Fact]
        public void GetArchiveWithUnknownProperties()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = GetResponseJson(new Dictionary<string, string> { { "archiveId", "936da01f-9abd-4d9d-80c7-02af85c822a8" } });
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.GetArchive(archiveId);

            Assert.NotNull(archive);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId))), Times.Once());
        }

        [Fact]
        public async Task GetArchiveWithUnknownPropertiesAsync()
        {
            string archiveId = "936da01f-9abd-4d9d-80c7-02af85c822a8";
            string returnString = GetResponseJson(new Dictionary<string, string> { { "archiveId", "936da01f-9abd-4d9d-80c7-02af85c822a8" } });
            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.GetArchiveAsync(archiveId);

            Assert.NotNull(archive);

            mockClient.Verify(httpClient => httpClient.GetAsync(It.Is<string>(url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId)), null), Times.Once());
        }

        // Delete Archive

        [Fact]
        public void DeleteArchive()
        {
            string archiveId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.DeleteArchive(archiveId);

            mockClient.Verify(httpClient => httpClient.Delete(It.Is<string>(
                    url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId)),
                It.IsAny<Dictionary<string, string>>()), Times.Once());
        }

        [Fact]
        public void DeleteArchiveFromArchiveObject()
        {
            Guid archiveId = new Guid("30b3ebf1-ba36-4f5b-8def-6f70d9986fe9");

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            
            Archive archive = new Archive(opentok);
            archive.Id = archiveId;

            archive.Delete();

            mockClient.Verify(httpClient => httpClient.Delete(It.Is<string>(
                    url => url.Equals($"v2/project/{ApiKey}/archive/{archiveId}")),
                It.IsAny<Dictionary<string, string>>()), Times.Once());
        }

        [Fact]
        public async Task DeleteArchiveAsync()
        {
            string archiveId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.DeleteAsync(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret)
            {
                Client = mockClient.Object
            };
            await opentok.DeleteArchiveAsync(archiveId);

            mockClient.Verify(httpClient => httpClient.DeleteAsync(It.Is<string>(
                    url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId)),
                It.IsAny<Dictionary<string, string>>()), Times.Once());
        }

        [Fact]
        public async Task DeleteArchiveAsyncFromArchiveObject()
        {
            Guid archiveId = new Guid("30b3ebf1-ba36-4f5b-8def-6f70d9986fe9");

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.DeleteAsync(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret)
            {
                Client = mockClient.Object
            };

            Archive archive = new Archive(opentok);
            archive.Id = archiveId;

            await archive.DeleteAsync();

            mockClient.Verify(httpClient => httpClient.DeleteAsync(It.Is<string>(
                    url => url.Equals("v2/project/" + ApiKey + "/archive/" + archiveId)),
                It.IsAny<Dictionary<string, string>>()), Times.Once());
        }

        // Set Archive Layout

        [Fact]
        public void SetArchiveLayoutScreenShareType()
        {
            var archiveId = "123456789";
            var expectedUrl = $"v2/project/{ApiKey}/archive/{archiveId}/layout";

            var expectedHeaders = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };

            var expectedData = new Dictionary<string, object>
            {
                {"type","bestFit" },
                {"screenshareType", "pip"}
            };

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient.Setup(c => c.Put(expectedUrl, expectedHeaders, expectedData))
                .Returns("")
                .Verifiable();
            
            var opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var layout = new ArchiveLayout
            {
                Type = LayoutType.bestFit, 
                ScreenShareType = ScreenShareLayoutType.Pip
            };

            Assert.True(opentok.SetArchiveLayout(archiveId, layout));
        }

        [Fact]
        public async Task SetArchiveLayoutAsyncScreenShareType()
        {
            var archiveId = "123456789";
            var expectedUrl = $"v2/project/{ApiKey}/archive/{archiveId}/layout";

            var expectedHeaders = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };

            var expectedData = new Dictionary<string, object>
            {
                {"type","bestFit" },
                {"screenshareType", "pip"}
            };

            var mockClient = new Mock<HttpClient>(MockBehavior.Strict);
            mockClient.Setup(c => c.PutAsync(expectedUrl, expectedHeaders, expectedData))
                .ReturnsAsync("")
                .Verifiable();

            var opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var layout = new ArchiveLayout
            {
                Type = LayoutType.bestFit,
                ScreenShareType = ScreenShareLayoutType.Pip
            };

            Assert.True(await opentok.SetArchiveLayoutAsync(archiveId, layout));
        }

        [Fact]
        public void SetArchiveLayoutCustomLayoutTypeNoStylesheetThrowsException()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var layout = new ArchiveLayout
            {
                Type = LayoutType.custom
            };

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.SetArchiveLayout("12345", layout));
            Assert.Contains("Invalid layout, layout is custom but no stylesheet provided", exception.Message);
            Assert.Equal("layout", exception.ParamName);
        }

        [Fact]
        public async Task SetArchiveLayoutAsyncCustomLayoutTypeNoStylesheetThrowsException()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var layout = new ArchiveLayout
            {
                Type = LayoutType.custom
            };

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.SetArchiveLayoutAsync("12345", layout));
            Assert.Contains("Invalid layout, layout is custom but no stylesheet provided", exception.Message);
            Assert.Equal("layout", exception.ParamName);
        }

        [Fact]
        public void SetArchiveLayoutNonCustomLayoutTypeStylesheetProvidedThrowsException()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var layout = new ArchiveLayout
            {
                Type = LayoutType.bestFit,
                StyleSheet = "bob"
            };

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.SetArchiveLayout("12345", layout));
            Assert.Contains("Invalid layout, layout is not custom, but stylesheet is set", exception.Message);
            Assert.Equal("layout", exception.ParamName);
        }

        [Fact]
        public async Task SetArchiveLayoutAsyncNonCustomLayoutTypeStylesheetProvidedThrowsException()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var layout = new ArchiveLayout
            {
                Type = LayoutType.bestFit,
                StyleSheet = "bob"
            };

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.SetArchiveLayoutAsync("12345", layout));
            Assert.Contains("Invalid layout, layout is not custom, but stylesheet is set", exception.Message);
            Assert.Equal("layout", exception.ParamName);
        }

        [Fact]
        public void SetArchiveLayoutTypeNotBestfitThrowsException()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var layout = new ArchiveLayout
            {
                ScreenShareType = ScreenShareLayoutType.BestFit,
                Type = LayoutType.pip
            };

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.SetArchiveLayout("12345", layout));
            Assert.Contains("Invalid layout, when ScreenShareType is set, Type must be bestFit", exception.Message);
            Assert.Equal("layout", exception.ParamName);
        }

        [Fact]
        public async Task SetArchiveLayoutAsyncTypeNotBestfitThrowsException()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var layout = new ArchiveLayout
            {
                ScreenShareType = ScreenShareLayoutType.BestFit,
                Type = LayoutType.pip
            };

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(() => opentok.SetArchiveLayoutAsync("12345", layout));
            Assert.Contains("Invalid layout, when ScreenShareType is set, Type must be bestFit", exception.Message);
            Assert.Equal("layout", exception.ParamName);

        // Stop Archive

        [Fact]
        public void StopArchive()
        {
            Guid archiveId = new Guid("30b3ebf1-ba36-4f5b-8def-6f70d9986fe9");
            string returnString = GetResponseJson();
            
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = opentok.StopArchive(archiveId.ToString());

            Assert.NotNull(archive);
            Assert.Equal("SESSIONID", archive.SessionId);
            Assert.Equal(archiveId, archive.Id);
            Assert.Equal(ArchiveStatus.STOPPED, archive.Status);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(
                    url => url.Equals($"v2/project/{ApiKey}/archive/{archiveId}/stop")),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public void StopArchiveFromArchiveObject()
        {
            Guid archiveId = new Guid("30b3ebf1-ba36-4f5b-8def-6f70d9986fe9");
            string returnString = GetResponseJson();

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            Archive archive = new Archive(opentok);
            archive.Id = archiveId;
            archive.SessionId = "SESSIONID";
            archive.Stop();

            Assert.NotNull(archive);
            Assert.Equal("SESSIONID", archive.SessionId);
            Assert.Equal(archiveId, archive.Id);
            Assert.Equal(ArchiveStatus.STOPPED, archive.Status);

            mockClient.Verify(httpClient => httpClient.Post(It.Is<string>(
                    url => url.Equals($"v2/project/{ApiKey}/archive/{archiveId}/stop")),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task StopArchiveAsync()
        {
            string archiveId = "30b3ebf1-ba36-4f5b-8def-6f70d9986fe9";
            string returnString = GetResponseJson();

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Archive archive = await opentok.StopArchiveAsync(archiveId);

            Assert.NotNull(archive);
            Assert.Equal("SESSIONID", archive.SessionId);
            Assert.Equal(archiveId, archive.Id.ToString());

            var expectedUrl = $"v2/project/{ApiKey}/archive/{archiveId}/stop";

            mockClient.Verify(httpClient => httpClient
                .PostAsync(It.Is<string>(
                    url => url.Equals(expectedUrl)), 
                    It.IsAny<Dictionary<string, string>>(), 
                    It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task StopArchiveAsyncFromArchiveObject()
        {
            Guid archiveId = new Guid("30b3ebf1-ba36-4f5b-8def-6f70d9986fe9");
            string returnString = GetResponseJson();

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            Archive archive = new Archive(opentok);
            archive.Id = archiveId;
            archive.SessionId = "SESSIONID";
            await archive.StopAsync();

            Assert.NotNull(archive);
            Assert.Equal("SESSIONID", archive.SessionId);
            Assert.Equal(archiveId, archive.Id);

            mockClient.Verify(httpClient => httpClient.PostAsync(It.Is<string>(
                    url => url.Equals($"v2/project/{ApiKey}/archive/{archiveId}/stop")),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        // List Archives

        [Fact]
        public void ListArchives()
        {
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            ArchiveList archives = opentok.ListArchives();

            Assert.NotNull(archives);
            Assert.Equal(6, archives.Count);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals($"v2/project/{ApiKey}/archive?offset=0"))), Times.Once());
        }

        [Fact]
        public void ListArchivesWithValidSessionId()
        {
            var sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            ArchiveList archives = opentok.ListArchives(sessionId: sessionId);

            Assert.NotNull(archives);
            Assert.Equal(6, archives.Count);

            mockClient.Verify(httpClient => httpClient.Get(It.Is<string>(url => url.Equals($"v2/project/{ApiKey}/archive?offset=0&sessionId={sessionId}"))), Times.Once());
        }

        [Fact]
        public void ListArchivesBadCount()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.ListArchives(count: -5));
            Assert.Equal("count cannot be smaller than 0", exception.Message);
        }

        [Fact]
        public void ListArchivesBadSessionId()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.ListArchives(sessionId: "This-is-not-a-valid-session-id"));
            Assert.Equal("Session Id is not valid", exception.Message);
        }

        [Fact]
        public async Task ListArchivesAsync()
        {
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            ArchiveList archives = await opentok.ListArchivesAsync();

            Assert.NotNull(archives);
            Assert.Equal(6, archives.Count);

            mockClient.Verify(httpClient => httpClient.GetAsync(It.Is<string>(url => url.Equals($"v2/project/{ApiKey}/archive?offset=0")), null), Times.Once());
        }

        [Fact]
        public async Task ListArchivesAsyncWithValidSessionId()
        {
            var sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            ArchiveList archives = await opentok.ListArchivesAsync(sessionId: sessionId);

            Assert.NotNull(archives);
            Assert.Equal(6, archives.Count);

            mockClient.Verify(httpClient => 
                httpClient.GetAsync(It.Is<string>(url => url.Equals($"v2/project/{ApiKey}/archive?offset=0&sessionId={sessionId}")), null), Times.Once());
        }

        [Fact]
        public async Task ListArchivesAsyncBadCount()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            var exception =
                await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.ListArchivesAsync(count: -5));
            Assert.Equal("count cannot be smaller than 0", exception.Message);
        }

        [Fact]
        public async Task ListArchivesAsyncBadSessionId()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () =>
                await opentok.ListArchivesAsync(sessionId: "This-is-not-a-valid-session-id"));

            Assert.Equal("Session Id is not valid", exception.Message);
        }
    }
}
