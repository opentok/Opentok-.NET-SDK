using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class StreamTests : TestBase
    {
        // Get Stream

        [Fact]
        public void GetStream()
        {
            string sessionId = "SESSIONID";
            string streamId = "be8f21f4-a133-43ae-a20a-bb29a1caaa46";
            string returnString = GetResponseJson(new Dictionary<string, string>
            {
                {"streamId", streamId}
            });
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            Stream stream = opentok.GetStream(sessionId, streamId);

            Assert.NotNull(stream);
            Assert.Equal(streamId, stream.Id);
            Assert.Equal("johndoe", stream.Name);
            Assert.Equal("screen", stream.VideoType);
            Assert.NotEmpty(stream.LayoutClassList);
            Assert.Contains("asdf", stream.LayoutClassList);

            mockClient.Verify(
                httpClient => httpClient.Get(It.Is<string>(url =>
                    url.Equals($"v2/project/{ApiKey}/session/{sessionId}/stream/{streamId}"))), Times.Once());
        }

        [Fact]
        public void GetStreamEmpty()
        {
            string sessionId = "SESSIONID";
            string streamId = "be8f21f4-a133-43ae-a20a-bb29a1caaa46";
            string returnString = GetResponseJson(new Dictionary<string, string>
            {
                {"streamId", streamId}
            });
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Stream stream = opentok.GetStream(sessionId, streamId);

            Assert.NotNull(stream);
            Assert.Equal(streamId, stream.Id);
            Assert.Equal("johndoe", stream.Name);
            Assert.Equal("screen", stream.VideoType);
            Assert.Empty(stream.LayoutClassList);

            mockClient.Verify(
                httpClient => httpClient.Get(It.Is<string>(url =>
                    url.Equals($"v2/project/{ApiKey}/session/{sessionId}/stream/{streamId}"))), Times.Once());
        }

        [Fact]
        public void GetStreamSessionIdNullThrowArgumentException()
        {
            string streamId = "be8f21f4-a133-43ae-a20a-bb29a1caaa46";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.GetStream(null, streamId));
            Assert.Contains("The sessionId cannot be null or empty", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public void GetStreamStreamIdNullThrowArgumentException()
        {
            string sessionId = "SESSIONID";
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.GetStream(sessionId, null));
            Assert.Contains("The streamId cannot be null or empty", exception.Message);
            Assert.Equal("streamId", exception.ParamName);
        }

        [Fact]
        public async Task GetStreamAsync()
        {
            string sessionId = "5b8594d7-60f2-49ec-b863-d69ab46aff9e";
            string streamId = "be8f21f4-a133-43ae-a20a-bb29a1caaa46";
            string returnString = GetResponseJson(new Dictionary<string, string>
            {
                {"streamId", streamId}
            });
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            Stream stream = await opentok.GetStreamAsync(sessionId, streamId);

            Assert.NotNull(stream);
            Assert.Equal(streamId, stream.Id);
            Assert.Equal("johndoe", stream.Name);
            Assert.Equal("screen", stream.VideoType);
            Assert.NotEmpty(stream.LayoutClassList);
            Assert.Contains("asdf", stream.LayoutClassList);

            mockClient.Verify(httpClient => httpClient.GetAsync(
                    It.Is<string>(url => url.Equals($"v2/project/{ApiKey}/session/{sessionId}/stream/{streamId}")),
                    null),
                Times.Once());
        }

        [Fact]
        public async Task GetStreamAsyncEmpty()
        {
            string sessionId = "SESSIONID";
            string streamId = "be8f21f4-a133-43ae-a20a-bb29a1caaa46";
            string returnString = GetResponseJson(new Dictionary<string, string>
            {
                {"streamId", streamId}
            });
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            Stream stream = await opentok.GetStreamAsync(sessionId, streamId);

            Assert.NotNull(stream);
            Assert.Equal(streamId, stream.Id);
            Assert.Equal("johndoe", stream.Name);
            Assert.Equal("screen", stream.VideoType);
            Assert.Empty(stream.LayoutClassList);

            mockClient.Verify(httpClient => httpClient.GetAsync(
                    It.Is<string>(url => url.Equals($"v2/project/{ApiKey}/session/{sessionId}/stream/{streamId}")),
                    null),
                Times.Once());
        }

        [Fact]
        public async Task GetStreamAsyncSessionIdNullThrowArgumentException()
        {
            string streamId = "be8f21f4-a133-43ae-a20a-bb29a1caaa46";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception =
                await Assert.ThrowsAsync<OpenTokArgumentException>(async () =>
                    await opentok.GetStreamAsync(null, streamId));
            Assert.Contains("The sessionId cannot be null or empty", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public async Task GetStreamAsyncStreamIdNullThrowArgumentException()
        {
            string sessionId = "SESSIONID";
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception =
                await Assert.ThrowsAsync<OpenTokArgumentException>(async () =>
                    await opentok.GetStreamAsync(sessionId, null));
            Assert.Contains("The streamId cannot be null or empty", exception.Message);
            Assert.Equal("streamId", exception.ParamName);
        }

        // List Streams

        [Fact]
        public void ListStreams()
        {
            string sessionId = "SESSIONID";
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Get(It.IsAny<string>())).Returns(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            StreamList streamList = opentok.ListStreams(sessionId);

            Stream streamOne = streamList[0];
            Stream streamTwo = streamList[1];

            Assert.NotNull(streamList);
            Assert.Equal(2, streamList.Count);

            Assert.Equal("ef546c5a-4fd7-4e59-ab3d-f1cfb4148d1d", streamOne.Id);
            Assert.Equal("johndoe", streamOne.Name);
            Assert.Contains("layout1", streamOne.LayoutClassList);
            Assert.Equal("screen", streamOne.VideoType);

            Assert.Equal("1f546c5a-4fd7-4e59-ab3d-f1cfb4148d1d", streamTwo.Id);
            Assert.Equal("janedoe", streamTwo.Name);
            Assert.Contains("layout2", streamTwo.LayoutClassList);
            Assert.Equal("camera", streamTwo.VideoType);

            mockClient.Verify(
                httpClient =>
                    httpClient.Get(It.Is<string>(url =>
                        url.Equals($"v2/project/{ApiKey}/session/" + sessionId + "/stream"))), Times.Once());
        }

        [Fact]
        public async Task ListStreamsAsync()
        {
            string sessionId = "SESSIONID";
            string returnString = GetResponseJson();
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>(), null))
                .ReturnsAsync(returnString);

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            StreamList streamList = await opentok.ListStreamsAsync(sessionId);

            Stream streamOne = streamList[0];
            Stream streamTwo = streamList[1];

            Assert.NotNull(streamList);
            Assert.Equal(2, streamList.Count);

            Assert.Equal("ef546c5a-4fd7-4e59-ab3d-f1cfb4148d1d", streamOne.Id);
            Assert.Equal("johndoe", streamOne.Name);
            Assert.Contains("layout1", streamOne.LayoutClassList);
            Assert.Equal("screen", streamOne.VideoType);

            Assert.Equal("1f546c5a-4fd7-4e59-ab3d-f1cfb4148d1d", streamTwo.Id);
            Assert.Equal("janedoe", streamTwo.Name);
            Assert.Contains("layout2", streamTwo.LayoutClassList);
            Assert.Equal("camera", streamTwo.VideoType);

            mockClient.Verify(httpClient =>
                    httpClient.GetAsync(
                        It.Is<string>(url => url.Equals($"v2/project/{ApiKey}/session/" + sessionId + "/stream")),
                        null),
                Times.Once());
        }

        [Fact]
        public void ListStreamsStreamIdNullThrowArgumentException()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.ListStreams(null));
            Assert.Contains("The sessionId cannot be null or empty", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public async Task ListStreamsAsyncStreamIdNullThrowArgumentException()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            var exception =
                await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.ListStreamsAsync(null));
            Assert.Contains("The sessionId cannot be null or empty", exception.Message);
            Assert.Equal("sessionId", exception.ParamName);
        }

        // Stream Class Lists

        [Fact]
        public void SetStreamClassLists()
        {
            string sessionId = "SESSIONID";
            string streamId = "STREAMID";

            List<StreamProperties> streamPropertiesList = new List<StreamProperties>();
            StreamProperties streamProperties = new StreamProperties(streamId);
            streamProperties.addLayoutClass("focus");
            streamPropertiesList.Add(streamProperties);

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Put(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>())).Returns("This function should not return anything");

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.SetStreamClassLists(sessionId, streamPropertiesList);

            var expectedUrl = $"v2/project/{ApiKey}/session/{sessionId}/stream";
            mockClient.Verify(
                httpClient => httpClient.Put(It.Is<string>(url => url.Equals(expectedUrl)),
                    It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Fact]
        public async Task SetStreamClassListsAsync()
        {
            string sessionId = "SESSIONID";
            string streamId = "STREAMID";

            List<StreamProperties> streamPropertiesList = new List<StreamProperties>();
            StreamProperties streamProperties = new StreamProperties(streamId);
            streamProperties.addLayoutClass("focus");
            streamPropertiesList.Add(streamProperties);

            string actualUrl = null;
            Dictionary<string, object> actualData = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PutAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string, string>, Dictionary<string, object>>((sId, headers, data) =>
                {
                    actualUrl = sId;
                    actualData = data;
                })
                .ReturnsAsync("This function should not return anything");

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.SetStreamClassListsAsync(sessionId, streamPropertiesList);

            var expectedUrl = $"v2/project/{ApiKey}/session/{sessionId}/stream";
            mockClient.Verify(
                httpClient => httpClient.PutAsync(It.Is<string>(url => url.Equals(expectedUrl)),
                    It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
            
            Assert.NotNull(actualUrl);
            Assert.Equal(expectedUrl, actualUrl);
            Assert.NotNull(actualData);
            Assert.NotEmpty(actualData);
            Assert.True(actualData.ContainsKey("items"));
        }
    }
}