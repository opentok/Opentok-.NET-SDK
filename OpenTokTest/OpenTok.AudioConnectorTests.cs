using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using OpenTokSDK;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class OpenTokAudioConnectorTests
    {
        private readonly int apiKey;
        private readonly Fixture fixture;
        private readonly Mock<HttpClient> mockClient;
        private readonly OpenTok sut;

        public OpenTokAudioConnectorTests()
        {
            this.fixture = new Fixture();
            this.fixture.Customize(new SupportMutableValueTypesCustomization());
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
            var expectedUrl = $"v2/project/{this.apiKey}/connect";
            var expectedResponse = this.fixture.Create<AudioConnector>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = AudioConnectorStartRequestDataBuilder.Build().Create();
            this.mockClient.Setup(httpClient => httpClient.PostAsync(
                    expectedUrl,
                    It.Is<Dictionary<string, string>>(dictionary =>
                        dictionary.ContainsKey(contentTypeKey) && dictionary[contentTypeKey] == contentType),
                    It.Is<Dictionary<string, object>>(dictionary =>
                        dictionary.SequenceEqual(request.ToDataDictionary()))))
                .ReturnsAsync(serializedResponse);
            var response = await this.sut.StartAudioConnectorAsync(request);
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task StopAsync_ShouldStopConnection()
        {
            var connectionId = this.fixture.Create<string>();
            var expectedUrl = $"v2/project/{this.apiKey}/connect/{connectionId}/stop";
            await this.sut.StopAudioConnectorAsync(connectionId);
            this.mockClient.Verify(httpClient => httpClient.PostAsync(
                    expectedUrl,
                    It.Is<Dictionary<string, string>>(dictionary => !dictionary.Any()),
                    It.Is<Dictionary<string, object>>(dictionary => !dictionary.Any())),
                Times.Once);
        }
    }
}