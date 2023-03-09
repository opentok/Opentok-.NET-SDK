using System;
using System.Collections.Generic;
using AutoFixture;
using OpenTokSDK;

namespace OpenTokSDKTest
{
    public class AudioConnectorStartRequestDataBuilder
    {
        private Dictionary<string, string> headers;
        private string sessionId;
        private List<string> streams;
        private string token;
        private Uri uri;

        private AudioConnectorStartRequestDataBuilder()
        {
            var fixture = new Fixture();
            this.sessionId = fixture.Create<string>();
            this.token = fixture.Create<string>();
            this.uri = fixture.Create<Uri>();
        }

        public static AudioConnectorStartRequestDataBuilder Build() => new AudioConnectorStartRequestDataBuilder();

        public AudioConnectorStartRequestDataBuilder WithSessionId(string value)
        {
            this.sessionId = value;
            return this;
        }

        public AudioConnectorStartRequestDataBuilder WithToken(string value)
        {
            this.token = value;
            return this;
        }

        public AudioConnectorStartRequestDataBuilder WithUri(Uri value)
        {
            this.uri = value;
            return this;
        }

        public AudioConnectorStartRequestDataBuilder WithStream(string value)
        {
            if (this.streams is null)
            {
                this.streams = new List<string>();
            }

            this.streams.Add(value);
            return this;
        }

        public AudioConnectorStartRequestDataBuilder WithHeader(string key, string value)
        {
            if (this.headers is null)
            {
                this.headers = new Dictionary<string, string>();
            }

            this.headers.Add(key, value);
            return this;
        }

        public AudioConnectorStartRequest Create() => 
            new AudioConnectorStartRequest(this.sessionId, this.token, new AudioConnectorStartRequest.WebSocket(this.uri, this.streams?.ToArray(), this.headers));
    }
}