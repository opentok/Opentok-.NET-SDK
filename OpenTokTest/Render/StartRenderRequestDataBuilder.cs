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

        private StartRenderRequestDataBuilder()
        {
            var fixture = new Fixture();
            this.sessionId = fixture.Create<string>();
            this.token = fixture.Create<string>();
            this.url = fixture.Create<Uri>();
            this.maxDuration = default;
            this.resolution = default;
            this.streamName = fixture.Create<string>();
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

        public StartRenderRequest Create()
        {
            if (this.maxDuration.HasValue && this.resolution.HasValue)
            {
                return new StartRenderRequest(
                    this.sessionId,
                    this.token,
                    this.url,
                    this.streamName,
                    this.maxDuration.Value,
                    this.resolution.Value);
            }

            if (this.maxDuration.HasValue)
            {
                return new StartRenderRequest(
                    this.sessionId,
                    this.token,
                    this.url,
                    this.streamName,
                    this.maxDuration.Value);
            }

            if (this.resolution.HasValue)
            {
                return new StartRenderRequest(
                    this.sessionId,
                    this.token,
                    this.url,
                    this.streamName,
                    resolution: this.resolution.Value);
            }

            return new StartRenderRequest(
                this.sessionId,
                this.token,
                this.url,
                this.streamName);
        }

        public StartRenderRequestDataBuilder WithMaxDuration(int? value)
        {
            this.maxDuration = value;
            return this;
        }

        public StartRenderRequestDataBuilder WithStreamName(string value)
        {
            this.streamName = value;
            return this;
        }
    }
}