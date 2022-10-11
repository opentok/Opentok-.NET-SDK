using System;
using System.Linq;
using OpenTokSDK.Exception;
using OpenTokSDK.Render;
using Xunit;

namespace OpenTokSDKTest.Render
{
    public class StartRenderRequestTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenSessionIdIsNotProvided(string sessionId)
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithSessionId(sessionId).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.MissingSessionId, exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenStreamNameIsNotProvided(string streamName)
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithStreamName(streamName).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.PublisherProperty.MissingStreamName, exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenTokenIsNotProvided(string token)
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithToken(token).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.MissingToken, exception.Message);
        }

        [Theory]
        [InlineData("http://localh/")]
        [InlineData("https://local/")]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenUrlLengthIsLowerThan15(string url)
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithUrl(new Uri(url)).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidUrl, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenUrlLengthIsHigherThan2048()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2021).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");
            void Act() => StartRenderRequestDataBuilder.Build().WithUrl(uri).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(2049, uri.AbsoluteUri.Length);
            Assert.Equal(StartRenderRequest.InvalidUrl, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenMaxDurationIsLowerThan60()
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithMaxDuration(59).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidMaxDuration, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldHave7200AsDefaultMaxDuration()
        {
            var request = StartRenderRequestDataBuilder.Build().Create();
            Assert.Equal(7200, request.MaxDuration);
        }

        [Fact]
        public void ConstructorShouldThrowOpenTokExceptionGivenMaxDurationIsHigherThan36000()
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithMaxDuration(36001).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidMaxDuration, exception.Message);
        }

        [Theory]
        [InlineData("http://localh/")]
        [InlineData("https://local/")]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenStatusCallbackUrlLengthIsLowerThan15(string url)
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithStatusCallbackUrl(new Uri(url)).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidStatusCallbackUrl, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenStatusCallbackUrlLengthIsHigherThan2048()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2021).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");
            void Act() => StartRenderRequestDataBuilder.Build().WithStatusCallbackUrl(uri).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(2049, uri.AbsoluteUri.Length);
            Assert.Equal(StartRenderRequest.InvalidStatusCallbackUrl, exception.Message);
        }

        [Theory]
        [InlineData(ScreenResolution.FullHighDefinitionLandscape)]
        [InlineData(ScreenResolution.FullHighDefinitionPortrait)]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenResolutionIsNotAllowed(
            ScreenResolution resolution)
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithResolution(resolution).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidResolution, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldHave1280x720AsDefaultResolution()
        {
            var request = StartRenderRequestDataBuilder.Build().Create();
            Assert.Equal(ScreenResolution.HighDefinitionLandscape, request.Resolution);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenStreamNameExceeds200length()
        {
            var streamName = string.Join(string.Empty, Enumerable.Range(0, 201).Select(_ => 'a').ToArray());
            void Act() => StartRenderRequestDataBuilder.Build().WithStreamName(streamName).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.PublisherProperty.OverflowStreamName, exception.Message);
        }

        [Theory]
        [InlineData("sessionId", "token", "https://www.example.com/", "http://www.example.com/callback", "stream", 1200, ScreenResolution.StandardDefinitionLandscape)]
        [InlineData("sessionId", "token", "https://www.example.com/", "http://www.example.com/callback", "stream", 60, ScreenResolution.StandardDefinitionPortrait)]
        [InlineData("sessionId", "token", "https://www.example.com/", "http://www.example.com/callback", "stream", 36000, ScreenResolution.HighDefinitionLandscape)]
        [InlineData("sessionId", "token", "https://www.example.com/", "http://www.example.com/callback", "stream", 15647, ScreenResolution.HighDefinitionPortrait)]
        [InlineData("sessionId", "token", "https://www.example.com/", "http://www.example.com/callback", "stream", null, ScreenResolution.StandardDefinitionLandscape)]
        [InlineData("sessionId", "token", "https://www.example.com/", "http://www.example.com/callback", "stream", 36000, null)]
        public void StartRenderRequest_ShouldReturnInstance(string sessionId, string token, string url, string callbackUrl, string streamName, int? maxDuration, ScreenResolution? resolution)
        {
            var request = StartRenderRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUrl(new Uri(url))
                .WithStatusCallbackUrl(new Uri(callbackUrl))
                .WithStreamName(streamName)
                .WithMaxDuration(maxDuration)
                .WithResolution(resolution)
                .Create();
            Assert.Equal(sessionId, request.SessionId);
            Assert.Equal(token, request.Token);
            Assert.Equal(url, request.Url.AbsoluteUri);
            Assert.Equal(callbackUrl, request.StatusCallbackUrl.AbsoluteUri);
            Assert.Equal(streamName, request.Properties.Name);
            Assert.Equal(maxDuration ?? 7200, request.MaxDuration);
            Assert.Equal(resolution ?? ScreenResolution.HighDefinitionLandscape, request.Resolution);
        }
    }
}