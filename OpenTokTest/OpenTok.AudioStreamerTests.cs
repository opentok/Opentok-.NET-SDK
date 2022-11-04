using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using OpenTokSDK;
using OpenTokSDK.AudioStreamer;
using OpenTokSDK.Util;
using OpenTokSDKTest.AudioStreamer;
using Xunit;

namespace OpenTokSDKTest
{
    public class OpenTokAudioStreamerTests
    {
        private readonly int apiKey;
        private readonly Fixture fixture;
        private readonly Mock<HttpClient> mockClient;
        private readonly OpenTok sut;

        public OpenTokAudioStreamerTests()
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
        public async Task ConnectAsync_ShouldReturnResponse()
        {
            const string contentTypeKey = "Content-Type";
            const string contentType = "application/json";
            var apiVersion = this.fixture.Create<string>();
            var expectedUrl = $"{apiVersion}/project/{this.apiKey}/connect";
            var expectedResponse = this.fixture.Create<ConnectResponse>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = ConnectRequestDataBuilder.Build().Create();
            this.mockClient.Setup(httpClient => httpClient.PostAsync(
                    expectedUrl,
                    It.Is<Dictionary<string, string>>(dictionary =>
                        dictionary.ContainsKey(contentTypeKey) && dictionary[contentTypeKey] == contentType),
                    It.Is<Dictionary<string, object>>(dictionary =>
                        dictionary.SequenceEqual(request.ToDataDictionary()))))
                .ReturnsAsync(serializedResponse);
            var response = await this.sut.ConnectAudioStreamerAsync(apiVersion, request);
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task StopAsync_ShouldStopConnection()
        {
            var apiVersion = this.fixture.Create<string>();
            var callId = this.fixture.Create<string>();
            var expectedUrl = $"{apiVersion}/project/{this.apiKey}/connect/{callId}/stop";
            await this.sut.StopAudioStreamerAsync(apiVersion, callId);
            this.mockClient.Verify(httpClient => httpClient.PostAsync(
                    expectedUrl,
                    It.Is<Dictionary<string, string>>(dictionary => !dictionary.Any()),
                    It.Is<Dictionary<string, object>>(dictionary => !dictionary.Any())),
                Times.Once);
        }
    }
}