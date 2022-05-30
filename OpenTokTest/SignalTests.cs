using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class SignalTests : TestBase
    {
        [Fact]
        public void SignalOpenTokArgumentException()
        {
            string sessionId = "";
            SignalProperties signalProperties = new SignalProperties("data");

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = Assert.Throws<OpenTokArgumentException>(() => opentok.Signal(sessionId, signalProperties));
            Assert.Equal("sessionId", exception.ParamName);
        }
        
        [Fact]
        public async Task SignalOpenTokArgumentExceptionAsync()
        {
            string sessionId = "";
            SignalProperties signalProperties = new SignalProperties("data");

            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, object>>()));

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;

            var exception = await Assert.ThrowsAsync<OpenTokArgumentException>(async () => await opentok.SignalAsync(sessionId, signalProperties));
            Assert.Equal("sessionId", exception.ParamName);
        }

        [Fact]
        public void SignalWithDataAndType()
        {
            string sessionId = "SESSIONID";
            SignalProperties signalProperties = new SignalProperties("data", "type");

            string actualUrl = null;
            Dictionary<string, string> actualHeaders = null;
            Dictionary<string, object> actualData = null;
            
            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string,string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    actualUrl = url;
                    actualHeaders = headers;
                    actualData = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.Signal(sessionId, signalProperties);
            
            Assert.NotNull(actualUrl);
            Assert.Equal($"v2/project/{ApiKey}/session/{sessionId}/signal", actualUrl);
            
            Assert.NotNull(actualHeaders);
            Assert.NotNull(actualData);
            
            Assert.True(actualData.ContainsKey("data"));
            Assert.Equal("data", actualData["data"]);
            Assert.Equal("type", actualData["type"]);
        }
        
        [Fact]
        public async Task SignalWithDataAndTypeAsync()
        {
            string sessionId = "SESSIONID";
            SignalProperties signalProperties = new SignalProperties("data", "type");

            string actualUrl = null;
            Dictionary<string, string> actualHeaders = null;
            Dictionary<string, object> actualData = null;
            
            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string,string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    actualUrl = url;
                    actualHeaders = headers;
                    actualData = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.SignalAsync(sessionId, signalProperties);
            
            Assert.NotNull(actualUrl);
            Assert.NotNull(actualHeaders);
            Assert.NotNull(actualData);
            
            Assert.True(actualData.ContainsKey("data"));
            Assert.Equal("data", actualData["data"]);
            Assert.Equal("type", actualData["type"]);
        }

        [Fact]
        public void SignalToSingleConnection()
        {
            string sessionId = "SESSIONID";
            string connectionId = "CONNECTIONID";
            SignalProperties signalProperties = new SignalProperties("data");

            string actualUrl = null;
            Dictionary<string, string> actualHeaders = null;
            Dictionary<string, object> actualData = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string,string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    actualUrl = url;
                    actualHeaders = headers;
                    actualData = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            opentok.Signal(sessionId, signalProperties, connectionId);
            
            Assert.NotNull(actualUrl);
            Assert.Equal($"v2/project/{ApiKey}/session/{sessionId}/connection/{connectionId}/signal", actualUrl);
            
            Assert.NotNull(actualHeaders);
            Assert.NotNull(actualData);
            
            Assert.True(actualData.ContainsKey("data"));
        }
        
        [Fact]
        public async Task SignalToSingleConnectionAsync()
        {
            string sessionId = "SESSIONID";
            string connectionId = "CONNECTIONID";
            SignalProperties signalProperties = new SignalProperties("data");

            string actualUrl = null;
            Dictionary<string, string> actualHeaders = null;
            Dictionary<string, object> actualData = null;

            var mockClient = new Mock<HttpClient>();
            mockClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, object>>()))
                .Callback<string, Dictionary<string,string>, Dictionary<string, object>>((url, headers, data) =>
                {
                    actualUrl = url;
                    actualHeaders = headers;
                    actualData = data;
                });

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Client = mockClient.Object;
            await opentok.SignalAsync(sessionId, signalProperties, connectionId);
            
            Assert.NotNull(actualUrl);
            Assert.Equal($"v2/project/{ApiKey}/session/{sessionId}/connection/{connectionId}/signal", actualUrl);
            
            Assert.NotNull(actualHeaders);
            Assert.NotNull(actualData);
            
            Assert.True(actualData.ContainsKey("data"));
        }
    }
}