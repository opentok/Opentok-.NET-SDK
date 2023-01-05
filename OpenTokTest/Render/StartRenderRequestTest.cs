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
        private readonly string validSessionId;
        private readonly RenderResolution validResolution;
        private readonly Uri validUrl;
        private readonly int validDuration;
        private readonly string validToken;
        private readonly StartRenderRequest.PublisherProperties validProperties;

        public StartRenderRequestTest()
        {
            this.validSessionId = "sessionId";
            this.validToken = "token";
            this.validUrl = new Uri("https://www.example.com/");
            this.validDuration = 1200;
            this.validResolution = RenderResolution.StandardDefinitionLandscape;
            this.validProperties = new StartRenderRequest.PublisherProperties("name");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenSessionIdIsNotProvided(string value)
        {
            void Act() => new StartRenderRequest(value, this.validToken, this.validUrl, this.validDuration, this.validResolution,
                this.validProperties);

            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.MissingSessionId, exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenTokenIsNotProvided(string value)
        {
            void Act() => new StartRenderRequest(this.validSessionId, value, this.validUrl, this.validDuration, this.validResolution,
                this.validProperties);

            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.MissingToken, exception.Message);
        }

        [Theory]
        [InlineData("http://localh/")]
        [InlineData("https://local/")]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenUrlLengthIsLowerThan15(string value)
        {
            void Act() => new StartRenderRequest(this.validSessionId, this.validToken, new Uri(value), this.validDuration,
                this.validResolution, this.validProperties);

            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidUrl, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenUrlLengthIsHigherThan2048()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2021).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");

            void Act() => new StartRenderRequest(this.validSessionId, this.validToken, uri, this.validDuration, this.validResolution,
                this.validProperties);

            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(2049, uri.AbsoluteUri.Length);
            Assert.Equal(StartRenderRequest.InvalidUrl, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldReturnInstance_GivenUrlHasMaximumLength()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2020).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");
            var request = new StartRenderRequest(this.validSessionId, this.validToken, uri, this.validDuration, this.validResolution,
                this.validProperties);
            Assert.Equal(uri, request.Url);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenMaxDurationIsLowerThan60()
        {
            void Act() => new StartRenderRequest(this.validSessionId, this.validToken, this.validUrl, 59, this.validResolution,
                this.validProperties);

            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidMaxDuration, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldHave7200AsDefaultMaxDuration()
        {
            var request = new StartRenderRequest(this.validSessionId, this.validToken, this.validUrl);
            Assert.Equal(7200, request.MaxDuration);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokExceptionGivenMaxDurationIsHigherThan36000()
        {
            void Act() => new StartRenderRequest(this.validSessionId, this.validToken, this.validUrl, 36001, this.validResolution,
                this.validProperties);

            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidMaxDuration, exception.Message);
        }

        [Theory]
        [InlineData(RenderResolution.FullHighDefinitionLandscape)]
        [InlineData(RenderResolution.FullHighDefinitionPortrait)]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenResolutionIsNotAllowed(
            RenderResolution value)
        {
            void Act() => new StartRenderRequest(this.validSessionId, this.validToken, this.validUrl, this.validDuration, value,
                this.validProperties);

            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.InvalidResolution, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldHave1280x720AsDefaultResolution()
        {
            var request = new StartRenderRequest(this.validSessionId, this.validToken, this.validUrl);
            Assert.Equal(RenderResolution.HighDefinitionLandscape, request.Resolution);
        }

        [Fact]
        public void StartRenderRequest_ShouldNullAsDefaultProperties()
        {
            var request = new StartRenderRequest(this.validSessionId, this.validToken, this.validUrl, this.validDuration, this.validResolution);
            Assert.Null(request.Properties);
        }

        [Fact]
        public void StartRenderRequest_ShouldThrowOpenTokException_GivenStreamNameExceeds200length()
        {
            var streamName = string.Join(string.Empty, Enumerable.Range(0, 201).Select(_ => 'a').ToArray());

            void Act() => new StartRenderRequest(this.validSessionId, this.validToken, this.validUrl, this.validDuration, this.validResolution,
                new StartRenderRequest.PublisherProperties(streamName));

            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(StartRenderRequest.PublisherProperties.OverflowStreamName, exception.Message);
        }

        [Fact]
        public void StartRenderRequest_ShouldReturnInstance_GivenStreamNameHasMaximumLength()
        {
            var streamName = string.Join(string.Empty, Enumerable.Range(0, 200).Select(_ => 'a').ToArray());
            var request = new StartRenderRequest(this.validSessionId, this.validToken, this.validUrl, this.validDuration, this.validResolution,
                new StartRenderRequest.PublisherProperties(streamName));
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
        [InlineData("sessionId", "token", "https://www.example.com/", "stream", 36000,
            RenderResolution.StandardDefinitionLandscape)]
        public void StartRenderRequest_ShouldReturnInstance(string sessionId, string token, string url,
            string streamName, int maxDuration, RenderResolution resolution)
        {
            var request = StartRenderRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUrl(new Uri(url))
                .WithProperties(new StartRenderRequest.PublisherProperties(streamName))
                .WithMaxDuration(maxDuration)
                .WithResolution(resolution)
                .Create();
            Assert.Equal(sessionId, request.SessionId);
            Assert.Equal(token, request.Token);
            Assert.Equal(url, request.Url.AbsoluteUri);
            Assert.Equal(streamName, request.Properties.Name);
            Assert.Equal(maxDuration, request.MaxDuration);
            Assert.Equal(resolution, request.Resolution);
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
                {"properties", new StartRenderRequest.PublisherProperties(streamName).ToDataDictionary()},
            });
            var result = StartRenderRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUrl(new Uri(url))
                .WithProperties(new StartRenderRequest.PublisherProperties(streamName))
                .WithMaxDuration(maxDuration)
                .WithResolution(resolution)
                .Create()
                .ToDataDictionary();
            Assert.Equal(expectedSerialized, JsonConvert.SerializeObject(result));
        }
    }
}