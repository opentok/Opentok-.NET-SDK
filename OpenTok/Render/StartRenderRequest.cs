using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;
using OpenTokSDK.Exception;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// </summary>
    public class StartRenderRequest
    {
        private const int MinimumUrlLength = 15;
        private const int MaximumUrlLength = 2048;
        private const int MinimumMaxDuration = 60;
        private const int MaximumMaxDuration = 36000;
        private const int MaximumPropertyNameLength = 200;

        /// <summary>
        ///     Indicates SessionId needs to be provided.
        /// </summary>
        public const string MissingSessionId = "SessionId needs to be provided.";

        /// <summary>
        ///     Indicates Token needs to be provided.
        /// </summary>
        public const string MissingToken = "Token needs to be provided.";

        /// <summary>
        ///     Indicates Url length should be between 15 and 2048.
        /// </summary>
        public const string InvalidUrl = "Url length should be between 15 and 2048.";

        /// <summary>
        ///     Indicates Url length should be between 15 and 2048.
        /// </summary>
        public const string InvalidStatusCallbackUrl = "Url length should be between 15 and 2048.";

        /// <summary>
        ///     Indicates MaxDuration value should be between 60s and 36000s.
        /// </summary>
        public const string InvalidMaxDuration = "MaxDuration value should be between 60s and 36000s.";

        /// <summary>
        ///     Indicates the provided resolution is not supported.
        /// </summary>
        public const string InvalidResolution = "The provided resolution is not supported.";

        private readonly ScreenResolution[] allowedResolutions =
        {
            ScreenResolution.HighDefinitionLandscape,
            ScreenResolution.HighDefinitionPortrait,
            ScreenResolution.StandardDefinitionLandscape,
            ScreenResolution.StandardDefinitionPortrait,
        };

        /// <summary>
        /// </summary>
        /// <param name="sessionId">
        ///     The ID of a session (generated with the same `APIKEY` as specified in the URL) which you wish
        ///     to start rendering into
        /// </param>
        /// <param name="token">
        ///     A valid OpenTok token with a Publisher role and (optionally) connection data to be associated with
        ///     the output stream.
        /// </param>
        /// <param name="url">
        ///     A publically reachable URL controlled by the customer and capable of generating the content to be
        ///     rendered without user intervention. The absolute URI should have a minimum length of 15 and maximum length of 2048.
        /// </param>
        /// <param name="resolution">
        ///     Resolution of the display area for the composition. Allowed values are 480x640 (SD Portrait),
        ///     640x480 (SD Landscape), 720x1280 (HD Portrait), 1280x720 (HD Landscape). The default value is 1280x720 (HD
        ///     Landscape).
        /// </param>
        /// <param name="statusCallbackUrl">
        ///     URL of the customer service where the callbacks will be received. The absolute URI
        ///     should have a minimum length of 15 and maximum length of 2048.
        /// </param>
        /// <param name="streamName">
        ///     The name of the composed output stream which will be published to the session. The minimum
        ///     length is 1 and the maximum one is 200.
        /// </param>
        /// <param name="maxDuration">
        ///     The maximum time allowed for the Render, in seconds. After this time, the Render will be
        ///     stopped automatically, if it is still running. The minimum duration is 1s and the maximum one is 36000s (10 hours).
        /// </param>
        public StartRenderRequest(string sessionId, string token, Uri url,
            Uri statusCallbackUrl, string streamName, int maxDuration = 7200,
            ScreenResolution resolution = ScreenResolution.HighDefinitionLandscape)
        {
            ValidateSessionId(sessionId);
            ValidateToken(token);
            ValidateUrl(url, InvalidUrl);
            ValidateUrl(statusCallbackUrl, InvalidStatusCallbackUrl);
            ValidateMaxDuration(maxDuration);
            this.ValidateResolution(resolution);
            this.SessionId = sessionId;
            this.Token = token;
            this.Url = url;
            this.MaxDuration = maxDuration;
            this.Resolution = resolution;
            this.StatusCallbackUrl = statusCallbackUrl;
            this.Properties = new PublisherProperty(streamName);
        }

        /// <summary>
        ///     The ID of a session (generated with the same `APIKEY` as specified in the URL) which you wish to start rendering
        ///     into
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        ///     A valid OpenTok token with a Publisher role and (optionally) connection data to be associated with the output
        ///     stream.
        /// </summary>
        public string Token { get; }

        /// <summary>
        ///     A publically reachable URL controlled by the customer and capable of generating the content to be rendered without
        ///     user intervention. The absolute URI should have a minimum length of 15 and maximum length of 2048.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        ///     The maximum time allowed for the Render, in seconds. After this time, the Render will be stopped automatically, if
        ///     it is still running. The minimum duration is 1s and the maximum one is 36000s (10 hours).
        /// </summary>
        public int MaxDuration { get; }

        /// <summary>
        ///     Resolution of the display area for the composition. Allowed values are 480x640 (SD Portrait), 640x480 (SD
        ///     Landscape), 720x1280 (HD Portrait), 1280x720 (HD Landscape). The default value is 1280x720 (HD Landscape).
        /// </summary>
        public ScreenResolution Resolution { get; }

        /// <summary>
        ///     URL of the customer service where the callbacks will be received. The absolute URI should have a minimum length of
        ///     15 and maximum length of 2048.
        /// </summary>
        public Uri StatusCallbackUrl { get; }

        /// <summary>
        ///     Initial configuration of Publisher properties for the composed output stream.
        /// </summary>
        public PublisherProperty Properties { get; }

        private static void ValidateSessionId(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new OpenTokException(MissingSessionId);
            }
        }

        private static void ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new OpenTokException(MissingToken);
            }
        }

        private static void ValidateUrl(Uri url, string message)
        {
            if (url.AbsoluteUri.Length < MinimumUrlLength || url.AbsoluteUri.Length > MaximumUrlLength)
            {
                throw new OpenTokException(message);
            }
        }

        private static void ValidateMaxDuration(int maxDuration)
        {
            if (maxDuration < MinimumMaxDuration || maxDuration > MaximumMaxDuration)
            {
                throw new OpenTokException(InvalidMaxDuration);
            }
        }

        private void ValidateResolution(ScreenResolution resolution)
        {
            if (!this.allowedResolutions.Contains(resolution))
            {
                throw new OpenTokException(InvalidResolution);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ToDataDictionary() =>
            new Dictionary<string, object>
            {
                {"sessionId", this.SessionId},
                {"token", this.Token},
                {"url", this.Url},
                {"statusCallbackUrl", this.StatusCallbackUrl},
                {"maxDuration", this.MaxDuration},
                {"resolution", this.Resolution.AsString(EnumFormat.Description)},
                {"properties", this.Properties},
            };

        /// <summary>
        ///     Initial configuration of Publisher properties for the composed output stream.
        /// </summary>
        public class PublisherProperty
        {
            /// <summary>
            ///     Indicates StreamName needs to be provided.
            /// </summary>
            public const string MissingStreamName = "StreamName needs to be provided.";

            /// <summary>
            ///     Indicates StreamName cannot exceeds 200 characters.
            /// </summary>
            public const string OverflowStreamName = "StreamName cannot exceeds 200 characters.";

            /// <summary>
            /// </summary>
            /// <param name="name">
            ///     The name of the composed output stream which will be published to the session. The minimum length is
            ///     1 and the maximum one is 200.
            /// </param>
            public PublisherProperty(string name)
            {
                ValidateName(name);
                this.Name = name;
            }

            /// <summary>
            ///     The name of the composed output stream which will be published to the session. The minimum length is 1 and the
            ///     maximum one is 200.
            /// </summary>
            public string Name { get; }

            private static void ValidateName(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new OpenTokException(MissingStreamName);
                }

                if (name.Length > MaximumPropertyNameLength)
                {
                    throw new OpenTokException(OverflowStreamName);
                }
            }
        }
    }
}