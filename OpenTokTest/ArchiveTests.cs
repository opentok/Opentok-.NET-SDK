using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OpenTokSDKTest
{
    public class ArchiveTests : TestBase
    {
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
        public void AddStreamToArchiveAsyncInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.AddStreamToArchiveAsync(archiveId, streamId));
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
        public void RemoveStreamFromArchiveAsyncInvalidArchiveIdThrowsException(string archiveId)
        {
            string streamId = "1234567890";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.RemoveStreamFromArchiveAsync(archiveId, streamId));
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
