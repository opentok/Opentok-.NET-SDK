using System;
using System.Collections.Generic;
using AutoFixture;
using OpenTokSDK.AudioStreamer;

namespace OpenTokSDKTest.AudioStreamer
{
    public class ConnectRequestDataBuilder
    {
        private Dictionary<string, string> headers;
        private string sessionId;
        private List<string> streams;
        private string token;
        private Uri uri;
        private int? audioRate;

        private ConnectRequestDataBuilder()
        {
            var fixture = new Fixture();
            this.sessionId = fixture.Create<string>();
            this.token = fixture.Create<string>();
            this.uri = fixture.Create<Uri>();
        }

        public static ConnectRequestDataBuilder Build() => new ConnectRequestDataBuilder();

        public ConnectRequestDataBuilder WithSessionId(string value)
        {
            this.sessionId = value;
            return this;
        }

        public ConnectRequestDataBuilder WithToken(string value)
        {
            this.token = value;
            return this;
        }

        public ConnectRequestDataBuilder WithUri(Uri value)
        {
            this.uri = value;
            return this;
        }
        
        public ConnectRequestDataBuilder WithAudioRate(int value)
        {
            this.audioRate = value;
            return this;
        }

        public ConnectRequestDataBuilder WithStream(string value)
        {
            if (this.streams is null)
            {
                this.streams = new List<string>();
            }

            this.streams.Add(value);
            return this;
        }

        public ConnectRequestDataBuilder WithHeader(string key, string value)
        {
            if (this.headers is null)
            {
                this.headers = new Dictionary<string, string>();
            }

            this.headers.Add(key, value);
            return this;
        }

        public ConnectRequest Create()
        {
            var socket = this.audioRate.HasValue
                ? new ConnectRequest.WebSocket(this.uri, this.streams?.ToArray(), this.headers, this.audioRate.Value)
                : new ConnectRequest.WebSocket(this.uri, this.streams?.ToArray(), this.headers);
            return new ConnectRequest(this.sessionId, this.token, socket);
        }
    }
}