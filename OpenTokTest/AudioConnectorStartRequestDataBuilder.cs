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
        private AudioConnectorStartRequest.WebSocket.SupportedAudioRates rate;
        private bool bidirectionalAudio;

        private AudioConnectorStartRequestDataBuilder()
        {
            var fixture = new Fixture();
            sessionId = fixture.Create<string>();
            token = fixture.Create<string>();
            uri = fixture.Create<Uri>();
        }

        public static AudioConnectorStartRequestDataBuilder Build() => new();

        public AudioConnectorStartRequestDataBuilder WithSessionId(string value)
        {
            sessionId = value;
            return this;
        }

        public AudioConnectorStartRequestDataBuilder WithToken(string value)
        {
            token = value;
            return this;
        }

        public AudioConnectorStartRequestDataBuilder WithUri(Uri value)
        {
            uri = value;
            return this;
        }

        public AudioConnectorStartRequestDataBuilder WithStream(string value)
        {
            if (streams is null)
            {
                streams = new List<string>();
            }

            streams.Add(value);
            return this;
        }

        public AudioConnectorStartRequestDataBuilder WithHeader(string key, string value)
        {
            if (headers is null)
            {
                headers = new Dictionary<string, string>();
            }

            headers.Add(key, value);
            return this;
        }
        
        public AudioConnectorStartRequestDataBuilder WithAudioRate(AudioConnectorStartRequest.WebSocket.SupportedAudioRates rate)
        {
            this.rate = rate;
            return this;
        }
        
        public AudioConnectorStartRequestDataBuilder WithBidirectionalAudio()
        {
            bidirectionalAudio = true;
            return this;
        }

        public AudioConnectorStartRequest Create() => 
            new(sessionId, token, new AudioConnectorStartRequest.WebSocket(uri, streams?.ToArray(), headers, rate, bidirectionalAudio));
    }
}