using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class ForceDisconnect : TestBase
    {
        [Fact]
        public void ForceDisconnectSessionNullArgumentException()
        {
            string connectionId = "gffgdsfdsf";
            string sessionId = "";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.ForceDisconnect(sessionId, connectionId));
            Assert.NotNull(exception);
            Assert.Equal("sessionId", exception.ParamName);
        }
        
        [Fact]
        public void ForceDisconnectConnectionNullArgumentException()
        {
            string connectionId = "";
            string sessionId = "fdsfdsf";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.ForceDisconnect(sessionId, connectionId));
            Assert.NotNull(exception);
            Assert.Equal("connectionId", exception.ParamName);
        }
        
        [Fact]
        public void ForceDisconnectSessionInvalidException()
        {
            string connectionId = "5fdsfdsfs";
            string sessionId = "bvcxz";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.ForceDisconnect(sessionId, connectionId));
            Assert.NotNull(exception);
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public async Task ForceDisconnectSessionNullArgumentExceptionAsync()
        {
            string connectionId = "gffgdsfdsf";
            string sessionId = "";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(() => opentok.ForceDisconnectAsync(sessionId, connectionId));
            Assert.NotNull(exception);
            Assert.Equal("sessionId", exception.ParamName);
        }
        
        [Fact]
        public async Task ForceDisconnectConnectionNullArgumentExceptionAsync()
        {
            string connectionId = "";
            string sessionId = "fdsfdsf";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(() => opentok.ForceDisconnectAsync(sessionId, connectionId));
            Assert.NotNull(exception);
            Assert.Equal("connectionId", exception.ParamName);
        }
        
        [Fact]
        public async Task ForceDisconnectSessionInvalidExceptionAsync()
        {
            string connectionId = "5fdsfdsfs";
            string sessionId = "bvcxz";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.ForceDisconnectAsync(sessionId, connectionId));
            Assert.NotNull(exception);
            Assert.Equal("sessionId", exception.ParamName);
        }
        
        [Fact]
        public void ForceDisconnectSuccess()
        {
            string connectionId = "3b0c260e-801e-47e9-a245-4ee3ffd9bd6f";
            string sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.ForceDisconnect(sessionId, connectionId);

            var expectedUrl = $"v2/project/{ApiKey}/session/{sessionId}/connection/{connectionId}";
            mockClient.Verify(httpClient => httpClient.Delete(It.Is<string>(url => url.Equals(expectedUrl)),
                It.IsAny<Dictionary<string, string>>()), Times.Once());
        }
        
        [Fact]
        public async Task ForceDisconnectSuccessAsync()
        {
            string connectionId = "3b0c260e-801e-47e9-a245-4ee3ffd9bd6f";
            string sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX";

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Delete(It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.ForceDisconnectAsync(sessionId, connectionId);

            var expectedUrl = $"v2/project/{ApiKey}/session/{sessionId}/connection/{connectionId}";
            mockClient.Verify(httpClient => httpClient.DeleteAsync(It.Is<string>(url => url.Equals(expectedUrl)),
                It.IsAny<Dictionary<string, string>>()), Times.Once());
        }
    }
}