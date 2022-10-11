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

        public OpenTokRenderTests()
        {
            this.fixture = new Fixture();
            this.mockClient = new Mock<HttpClient>();
            this.sut = new OpenTok(this.fixture.Create<int>(), this.fixture.Create<string>())
            {
                Client = this.mockClient.Object,
            };
        }

        [Fact]
        public async Task StartRenderAsync_ShouldReturnAFuckingResponse_GivenThisTestHasNoValue()
        {
            var expectedResponse = this.fixture.Create<StartRenderResponse>();
            var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
            var request = StartRenderRequestDataBuilder.Build().Create();
            this.mockClient.Setup(httpClient => httpClient.PostAsync(
                    It.Is<string>(url => url.EndsWith(OpenTok.StartRenderEndpoint)),
                    It.IsAny<Dictionary<string, string>>(),
                    It.Is<Dictionary<string, object>>(dictionary =>
                        dictionary.SequenceEqual(request.ToDataDictionary()))))
                .ReturnsAsync(serializedResponse);
            var response = await this.sut.StartRenderAsync(request);
            Assert.Equal(expectedResponse, response);
        }
    }
}