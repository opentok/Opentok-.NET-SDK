using System;
using System.Linq;
using OpenTokSDK.AudioStreamer;
using OpenTokSDK.Exception;
using Xunit;

namespace OpenTokSDKTest.AudioStreamer
{
    public class ConnectRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ConnectRequest_ShouldThrowOpenTokException_GivenSessionIdIsNotProvided(string sessionId)
        {
            void Act() => ConnectRequestDataBuilder.Build().WithSessionId(sessionId).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(ConnectRequest.MissingSessionId, exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ConnectRequest_ShouldThrowOpenTokException_GivenTokenIsNotProvided(string streamName)
        {
            void Act() => ConnectRequestDataBuilder.Build().WithToken(streamName).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(ConnectRequest.MissingToken, exception.Message);
        }

        [Fact]
        public void ConnectRequest_ShouldThrowOpenTokException_GivenUrlLengthIsHigherThan2048()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2021).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");
            void Act() => ConnectRequestDataBuilder.Build().WithUri(uri).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(2049, uri.AbsoluteUri.Length);
            Assert.Equal(ConnectRequest.InvalidUrl, exception.Message);
        }

        [Fact]
        public void ConnectRequest_ShouldReturnInstance_GivenUrlHasMaximumLength()
        {
            var filler = string.Join(string.Empty, Enumerable.Range(0, 2020).Select(_ => 'a').ToArray());
            var uri = new Uri($"https://www.example.com?p={filler}/");
            var request = ConnectRequestDataBuilder.Build().WithUri(uri).Create();
            Assert.Equal(uri, request.Socket.Uri);
        }
        
        [Fact]
        public void ConnectRequest_ShouldReturnInstance_GivenUrlHasMinimumLength()
        {
            var uri = new Uri($"https://localh/");
            var request = ConnectRequestDataBuilder.Build().WithUri(uri).Create();
            Assert.Equal(uri, request.Socket.Uri);
        }

        [Theory]
        [InlineData("http://localh/")]
        [InlineData("https://local/")]
        public void ConnectRequest_ShouldThrowOpenTokException_GivenUrlLengthIsLowerThan15(string url)
        {
            void Act() => ConnectRequestDataBuilder.Build().WithUri(new Uri(url)).Create();
            var exception = Assert.Throws<OpenTokException>(Act);
            Assert.Equal(ConnectRequest.InvalidUrl, exception.Message);
        }

        [Fact]
        public void ConnectRequest_ShouldHaveEmptyStreams_GivenStreamsAreNotProvided() =>
            Assert.Empty(ConnectRequestDataBuilder.Build().Create()
                .Socket.Streams);

        [Fact]
        public void ConnectRequest_ShouldHaveEmptyHeaders_GivenHeadersAreNotProvided() =>
            Assert.Empty(ConnectRequestDataBuilder.Build().Create()
                .Socket.Headers);
        
        [Fact]
        public void ConnectRequest_ShouldHaveDefaultAudioRate_GivenAudioRateIsNotProvided() =>
            Assert.Equal(16000, ConnectRequestDataBuilder.Build().Create()
                .Socket.AudioRate);

        [Theory]
        [InlineData("sessionId", "token", "https://www.example.com/", new[] {"test"}, "key", "value")]
        [InlineData("sessionId", "token", "https://www.example.com/", new[] {"test1", "test2", "test3"}, "key",
            "value")]
        public void ConnectRequest_ShouldReturnInstance(string sessionId, string token, string url, string[] streams,
            string key, string value)
        {
            var builder = ConnectRequestDataBuilder
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
        public void ConnectRequest_ShouldReturnInstance_WhenStreamAreNull(string sessionId, string token, string url,
            string key, string value)
        {
            var builder = ConnectRequestDataBuilder
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
        public void ConnectRequest_ShouldReturnInstance_WhenHeadersAreNull(string sessionId, string token, string url,
            string[] streams)
        {
            var builder = ConnectRequestDataBuilder
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
    }
}