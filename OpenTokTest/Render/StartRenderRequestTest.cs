using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;
using Newtonsoft.Json;
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
        public void StartRenderRequest_ShouldReturnInstance_GivenUrlHasMaximumLength()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2020).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");
            var request = StartRenderRequestDataBuilder.Build().WithUrl(uri).Create();
            Assert.Equal(uri, request.Url);
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
        public void StartRenderRequest_ShouldThrowOpenTokExceptionGivenMaxDurationIsHigherThan36000()
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithMaxDuration(36001).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidMaxDuration, exception.Message);
        }

        [Theory]
        [InlineData(RenderResolution.FullHighDefinitionLandscape)]
        [InlineData(RenderResolution.FullHighDefinitionPortrait)]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenResolutionIsNotAllowed(
            RenderResolution resolution)
        {
            void Act() => StartRenderRequestDataBuilder.Build().WithResolution(resolution).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidResolution, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldHave1280x720AsDefaultResolution()
        {
            var request = StartRenderRequestDataBuilder.Build().Create();
            Assert.Equal(RenderResolution.HighDefinitionLandscape, request.Resolution);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenStreamNameExceeds200length()
        {
            var streamName = string.Join(string.Empty, Enumerable.Range(0, 201).Select(_ => 'a').ToArray());
            void Act() => StartRenderRequestDataBuilder.Build().WithStreamName(streamName).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.PublisherProperty.OverflowStreamName, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldReturnInstance_GivenStreamNameHasMaximumLength()
        {
            var streamName = string.Join(string.Empty, Enumerable.Range(0, 200).Select(_ => 'a').ToArray());
            var request = StartRenderRequestDataBuilder.Build().WithStreamName(streamName).Create();
            Assert.Equal(streamName, request.Properties.Name);
        }

        [Theory]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream",
            1200, RenderResolution.StandardDefinitionLandscape)]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream", 60,
            RenderResolution.StandardDefinitionPortrait)]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream",
            36000, RenderResolution.HighDefinitionLandscape)]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream",
            15647, RenderResolution.HighDefinitionPortrait)]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream", null,
            RenderResolution.StandardDefinitionLandscape)]
        [InlineData("sessionId", "token", "https://hel.fr/", "stream", 36000, null)]
        public void StartRenderRequest_ShouldReturnInstance(string sessionId, string token, string url,
            string streamName, int? maxDuration, RenderResolution? resolution)
        {
            var request = StartRenderRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUrl(new Uri(url))
                .WithStreamName(streamName)
                .WithMaxDuration(maxDuration)
                .WithResolution(resolution)
                .Create();
            Assert.Equal(sessionId, request.SessionId);
            Assert.Equal(token, request.Token);
            Assert.Equal(url, request.Url.AbsoluteUri);
            Assert.Equal(streamName, request.Properties.Name);
            Assert.Equal(maxDuration ?? 7200, request.MaxDuration);
            Assert.Equal(resolution ?? RenderResolution.HighDefinitionLandscape, request.Resolution);
        }

        [Theory]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream",
            1200, RenderResolution.StandardDefinitionLandscape)]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream", 60,
            RenderResolution.StandardDefinitionPortrait)]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream",
            36000, RenderResolution.HighDefinitionLandscape)]
        [InlineData("sessionId", "token", "https://www.example.com/", "stream",
            15647, RenderResolution.HighDefinitionPortrait)]
        public void ToDataDictionary_ShouldReturnValuesAsDictionary(string sessionId, string token, string url,
            string streamName, int maxDuration, RenderResolution resolution)
        {
            var expectedSerialized = JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                {"sessionId", sessionId},
                {"token", token},
                {"url", new Uri(url)},
                {"maxDuration", maxDuration},
                {"resolution", resolution.AsString(EnumFormat.Description)},
                {"properties", new StartRenderRequest.PublisherProperty(streamName).ToDataDictionary()},
            });
            var result = StartRenderRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUrl(new Uri(url))
                .WithStreamName(streamName)
                .WithMaxDuration(maxDuration)
                .WithResolution(resolution)
                .Create()
                .ToDataDictionary();
            Assert.Equal(expectedSerialized, JsonConvert.SerializeObject(result));
        }
    }
}