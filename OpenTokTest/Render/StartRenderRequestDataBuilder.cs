using System;
using AutoFixture;
using OpenTokSDK.Render;

namespace OpenTokSDKTest.Render
{
    public class StartRenderRequestDataBuilder
    {
        private int? maxDuration;
        private RenderResolution? resolution;
        private string sessionId;
        private string streamName;
        private string token;
        private Uri url;
        private StartRenderRequest.PublisherProperties properties;

        private StartRenderRequestDataBuilder()
        {
            var fixture = new Fixture();
            this.sessionId = fixture.Create<string>();
            this.token = fixture.Create<string>();
            this.url = fixture.Create<Uri>();
            this.maxDuration = default;
            this.resolution = default;
            this.streamName = fixture.Create<string>();
            this.properties = default;
        }

        public static StartRenderRequestDataBuilder Build() => new StartRenderRequestDataBuilder();

        public StartRenderRequestDataBuilder WithSessionId(string value)
        {
            this.sessionId = value;
            return this;
        }

        public StartRenderRequestDataBuilder WithToken(string value)
        {
            this.token = value;
            return this;
        }

        public StartRenderRequestDataBuilder WithUrl(Uri value)
        {
            this.url = value;
            return this;
        }

        public StartRenderRequestDataBuilder WithResolution(RenderResolution? value)
        {
            this.resolution = value;
            return this;
        }

        public StartRenderRequest Create() =>
            new StartRenderRequest(
                this.sessionId,
                this.token,
                this.url,
                this.maxDuration ?? default,
                this.resolution ?? default,
                this.properties);

        public StartRenderRequestDataBuilder WithMaxDuration(int? value)
        {
            this.maxDuration = value;
            return this;
        }
        
        public StartRenderRequestDataBuilder WithProperties(StartRenderRequest.PublisherProperties value)
        {
            this.properties = value;
            return this;
        }
    }
}