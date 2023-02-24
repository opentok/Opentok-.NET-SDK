using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OpenTokSDK;
using OpenTokSDK.Exception;
using Xunit;

namespace OpenTokSDKTest
{
    public class AudioConnectorStartRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AudioConnectorStartRequest_ShouldThrowOpenTokException_GivenSessionIdIsNotProvided(string sessionId)
        {
            void Act() => AudioConnectorStartRequestDataBuilder.Build().WithSessionId(sessionId).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(AudioConnectorStartRequest.MissingSessionId, exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AudioConnectorStartRequest_ShouldThrowOpenTokException_GivenTokenIsNotProvided(string streamName)
        {
            void Act() => AudioConnectorStartRequestDataBuilder.Build().WithToken(streamName).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(AudioConnectorStartRequest.MissingToken, exception.Message);
        }

        [Fact]
        public void AudioConnectorStartRequest_ShouldThrowOpenTokException_GivenUrlLengthIsHigherThan2048()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2021).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");
            void Act() => AudioConnectorStartRequestDataBuilder.Build().WithUri(uri).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(2049, uri.AbsoluteUri.Length);
            Assert.Equal(AudioConnectorStartRequest.InvalidUrl, exception.Message);
        }

        [Fact]
        public void AudioConnectorStartRequest_ShouldReturnInstance_GivenUrlHasMaximumLength()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2020).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");
            var request = AudioConnectorStartRequestDataBuilder.Build().WithUri(uri).Create();
            Assert.Equal(uri, request.Socket.Uri);
        }

        [Fact]
        public void AudioConnectorStartRequest_ShouldReturnInstance_GivenUrlHasMinimumLength()
        {
            var uri = new Uri("https://localh/");
            var request = AudioConnectorStartRequestDataBuilder.Build().WithUri(uri).Create();
            Assert.Equal(uri, request.Socket.Uri);
        }

        [Theory]
        [InlineData("http://localh/")]
        [InlineData("https://local/")]
        public void AudioConnectorStartRequest_ShouldThrowOpenTokException_GivenUrlLengthIsLowerThan15(string url)
        {
            void Act() => AudioConnectorStartRequestDataBuilder.Build().WithUri(new Uri(url)).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(AudioConnectorStartRequest.InvalidUrl, exception.Message);
        }

        [Fact]
        public void AudioConnectorStartRequest_ShouldHaveEmptyStreams_GivenStreamsAreNotProvided() =>
            Assert.Empty(AudioConnectorStartRequestDataBuilder.Build().Create()
                .Socket.Streams);

        [Fact]
        public void AudioConnectorStartRequest_ShouldHaveEmptyHeaders_GivenHeadersAreNotProvided() =>
            Assert.Empty(AudioConnectorStartRequestDataBuilder.Build().Create()
                .Socket.Headers);

        [Theory]
        [InlineData("sessionId", "token", "https://www.example.com/", new[] {"test"}, "key", "value")]
        [InlineData("sessionId", "token", "https://www.example.com/", new[] {"test1", "test2", "test3"}, "key",
            "value")]
        public void AudioConnectorStartRequest_ShouldReturnInstance(string sessionId, string token, string url, string[] streams,
            string key, string value)
        {
            var builder = AudioConnectorStartRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUri(new Uri(url))
                .WithHeader(key, value);
            streams.Aggregate(builder, (dataBuilder, stream) => dataBuilder.WithStream(stream));
            var request = builder.Create();
            Assert.Equal(sessionId, request.SessionId);
            Assert.Equal(token, request.Token);
            Assert.Equal(url, request.Socket.Uri.AbsoluteUri);
            Assert.Equal(streams, request.Socket.Streams);
            Assert.Equal(1, request.Socket.Headers.Count);
            Assert.Equal(key, request.Socket.Headers.Keys.First());
            Assert.Equal(value, request.Socket.Headers.Values.First());
        }

        [Theory]
        [InlineData("sessionId", "token", "https://www.example.com/", "key", "value")]
        public void AudioConnectorStartRequest_ShouldReturnInstance_WhenStreamAreNull(string sessionId, string token, string url,
            string key, string value)
        {
            var builder = AudioConnectorStartRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUri(new Uri(url))
                .WithHeader(key, value);
            var request = builder.Create();
            Assert.Equal(sessionId, request.SessionId);
            Assert.Equal(token, request.Token);
            Assert.Equal(url, request.Socket.Uri.AbsoluteUri);
            Assert.Empty(request.Socket.Streams);
            Assert.Equal(1, request.Socket.Headers.Count);
            Assert.Equal(key, request.Socket.Headers.Keys.First());
            Assert.Equal(value, request.Socket.Headers.Values.First());
        }

        [Theory]
        [InlineData("sessionId", "token", "https://www.example.com/", new[] {"test1", "test2", "test3"})]
        public void AudioConnectorStartRequest_ShouldReturnInstance_WhenHeadersAreNull(string sessionId, string token, string url,
            string[] streams)
        {
            var builder = AudioConnectorStartRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUri(new Uri(url));
            streams.Aggregate(builder, (dataBuilder, stream) => dataBuilder.WithStream(stream));
            var request = builder.Create();
            Assert.Equal(sessionId, request.SessionId);
            Assert.Equal(token, request.Token);
            Assert.Equal(url, request.Socket.Uri.AbsoluteUri);
            Assert.Equal(streams, request.Socket.Streams);
            Assert.Empty(request.Socket.Headers);
        }

        [Theory]
        [InlineData("sessionId", "token", "https://www.example.com/", new[] {"test"}, "key", "value")]
        public void ToDataDictionary_ShouldReturnValuesAsDictionary(string sessionId, string token, string url,
            string[] streams,
            string key, string value)
        {
            var headers = new Dictionary<string, string> {{key, value}};
            var expectedSerialized = JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                {"sessionId", sessionId},
                {"token", token},
                {"webSocket", new AudioConnectorStartRequest.WebSocket(new Uri(url), streams, headers)},
            });
            var builder = AudioConnectorStartRequestDataBuilder
                .Build()
                .WithSessionId(sessionId)
                .WithToken(token)
                .WithUri(new Uri(url))
                .WithHeader(key, value);
            streams.Aggregate(builder, (dataBuilder, stream) => dataBuilder.WithStream(stream));
            var result = builder.Create().ToDataDictionary();
            Assert.Equal(expectedSerialized, JsonConvert.SerializeObject(result));
        }
    }
}