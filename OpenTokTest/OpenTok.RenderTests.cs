using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using OpenTokSDK;
using OpenTokSDK.Render;
using OpenTokSDK.Util;
using OpenTokSDKTest.Render;
using Xunit;

namespace OpenTokSDKTest
{
    public class OpenTokRenderTests
    {
        private readonly int apiKey;
        private readonly Fixture fixture;
        private readonly Mock<HttpClient> mockClient;
        private readonly OpenTok sut;

        public OpenTokRenderTests()
        {
            this.fixture = new Fixture();
            this.apiKey = this.fixture.Create<int>();
            this.mockClient = new Mock<HttpClient>();
            this.sut = new OpenTok(this.apiKey, this.fixture.Create<string>())
            {
                Client = this.mockClient.Object,
            };
        }

        [Fact]
        public async Task StartRenderAsync_ShouldInitiateRendering()
        {
            const string contentTypeKey = "Content-Type";
            const string contentType = "application/json";
            var expectedUrl = $"v2/project/{this.apiKey}/render";
            var expectedResponse = this.fixture.Create<RenderItem>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = StartRenderRequestDataBuilder.Build().Create();
            this.mockClient.Setup(httpClient => httpClient.PostAsync(
                    expectedUrl,
                    MatchesContentTypeDictionary(contentTypeKey, contentType),
                    Matches(request.ToDataDictionary())))
                .ReturnsAsync(serializedResponse);
            _ = await this.sut.StartRenderAsync(request);
            this.mockClient.Verify(httpClient => httpClient.PostAsync(
                    It.Is<string>(url => url == expectedUrl),
                    MatchesContentTypeDictionary(contentTypeKey, contentType),
                    Matches(request.ToDataDictionary())),
                Times.Once);
        }

        private static Dictionary<string, string> MatchesContentTypeDictionary(string contentTypeKey,
            string contentType) =>
            It.Is<Dictionary<string, string>>(dictionary =>
                dictionary.ContainsKey(contentTypeKey) && dictionary[contentTypeKey] == contentType);

        private static T Matches<T>(T input) where T : class =>
            It.Is<T>(element => JsonConvert.SerializeObject(element) == JsonConvert.SerializeObject(input));

        [Fact]
        public async Task StartRenderAsync_ShouldReturnResponse()
        {
            const string contentTypeKey = "Content-Type";
            const string contentType = "application/json";
            var expectedUrl = $"v2/project/{this.apiKey}/render";
            var expectedResponse = this.fixture.Create<RenderItem>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = StartRenderRequestDataBuilder.Build().Create();
            this.mockClient.Setup(httpClient => httpClient.PostAsync(
                    expectedUrl,
                    MatchesContentTypeDictionary(contentTypeKey, contentType),
                    Matches(request.ToDataDictionary())))
                .ReturnsAsync(serializedResponse);
            var response = await this.sut.StartRenderAsync(request);
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task StopRenderAsync_ShouldDeleteRendering()
        {
            var renderId = this.fixture.Create<string>();
            await this.sut.StopRenderAsync(renderId);
            this.mockClient.Verify(httpClient => httpClient.DeleteAsync(
                    $"v2/project/{this.apiKey}/render/{renderId}",
                    It.Is<Dictionary<string, string>>(dictionary => dictionary.Count == 0)),
                Times.Once);
        }

        [Fact]
        public async Task ListRendersAsync_ShouldReturnResponse_GivenCountAndOffsetAreProvided()
        {
            var expectedResponse = this.fixture.Create<ListRendersResponse>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = new ListRendersRequest(5, 6);
            var expectedUrl = $"v2/project/{this.apiKey}/render?count=6&offset=5";
            this.mockClient.Setup(httpClient => httpClient.GetAsync(expectedUrl, null))
                .ReturnsAsync(serializedResponse);
            var response = await this.sut.ListRendersAsync(request);
            Assert.Equal(expectedResponse.Count, response.Count);
            Assert.True(expectedResponse.Items.SequenceEqual(response.Items));
        }

        [Fact]
        public async Task ListRendersAsync_ShouldReturnResponse_GivenCountIsProvided()
        {
            var expectedResponse = this.fixture.Create<ListRendersResponse>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = new ListRendersRequest(5);
            var expectedUrl = $"v2/project/{this.apiKey}/render?count=5";
            this.mockClient.Setup(httpClient => httpClient.GetAsync(expectedUrl, null))
                .ReturnsAsync(serializedResponse);
            var response = await this.sut.ListRendersAsync(request);
            Assert.Equal(expectedResponse.Count, response.Count);
            Assert.True(expectedResponse.Items.SequenceEqual(response.Items));
        }

        [Fact]
        public async Task GetRenderAsync_ShouldReturnResponse()
        {
            var expectedResponse = this.fixture.Create<RenderItem>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var renderId = this.fixture.Create<string>();
            var expectedUrl = $"v2/project/{this.apiKey}/render/{renderId}";
            this.mockClient.Setup(httpClient => httpClient.GetAsync(expectedUrl, null))
                .ReturnsAsync(serializedResponse);
            var response = await this.sut.GetRenderAsync(renderId);
            Assert.Equal(expectedResponse, response);
        }
    }
}