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
        private readonly Fixture fixture;
        private readonly Mock<HttpClient> mockClient;
        private readonly OpenTok sut;
        private readonly int apiKey;

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
            var expectedResponse = this.fixture.Create<StartRenderResponse>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = StartRenderRequestDataBuilder.Build().Create();
            this.mockClient.Setup(httpClient => httpClient.PostAsync(
                    expectedUrl,
                    It.Is<Dictionary<string, string>>(dictionary => dictionary.ContainsKey(contentTypeKey) && dictionary[contentTypeKey] == contentType),
                    It.Is<Dictionary<string, object>>(dictionary =>
                        dictionary.SequenceEqual(request.ToDataDictionary()))))
                .ReturnsAsync(serializedResponse);
            _ = await this.sut.StartRenderAsync(request);
            this.mockClient.Verify(httpClient => httpClient.PostAsync(
                    It.Is<string>(url => url == expectedUrl),
                    It.Is<Dictionary<string, string>>(dictionary => dictionary.ContainsKey(contentTypeKey) && dictionary[contentTypeKey] == contentType),
                It.Is<Dictionary<string, object>>(dictionary =>
                    dictionary.SequenceEqual(request.ToDataDictionary()))), 
                Times.Once);
        }
        
        [Fact]
        public async Task StartRenderAsync_ShouldReturnResponse()
        {
            const string contentTypeKey = "Content-Type";
            const string contentType = "application/json";
            var expectedUrl = $"v2/project/{this.apiKey}/render";
            var expectedResponse = this.fixture.Create<StartRenderResponse>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = StartRenderRequestDataBuilder.Build().Create();
            this.mockClient.Setup(httpClient => httpClient.PostAsync(
                    expectedUrl,
                    It.Is<Dictionary<string, string>>(dictionary => dictionary.ContainsKey(contentTypeKey) && dictionary[contentTypeKey] == contentType),
                    It.Is<Dictionary<string, object>>(dictionary =>
                        dictionary.SequenceEqual(request.ToDataDictionary()))))
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
    }
}