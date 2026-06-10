using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using OpenTokSDK.Exception;

namespace OpenTokSDK
{
    /// <summary>
    ///     Represents a request to send audio from a Vonage Video API session to a WebSocket.
    /// </summary>
    public class AudioConnectorStartRequest
    {
        private const int MinimumUrlLength = 15;
        private const int MaximumUrlLength = 2048;

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
        ///     Creates a new instance of ConnectRequest.
        /// </summary>
        /// <param name="sessionId"> The OpenTok session ID that includes the OpenTok streams you want to include in the WebSocket stream.
        /// </param>
        /// <param name="token">
        /// The OpenTok token to be used for the Audio Connector connection to the OpenTok session.
        /// You can add token data to identify that the connection is the Audio Connector endpoint or for other identifying data.
        /// (The OpenTok client libraries include properties for inspecting the connection data for a client connected to a session.)
        /// See the <a href="https://tokbox.com/developer/guides/create-token/">Token Creation developer guide</a>.
        /// </param>
        /// <param name="socket">Options for configuring the connect call for WebSocket.</param>
        public AudioConnectorStartRequest(string sessionId, string token, WebSocket socket)
        {
            ValidateSessionId(sessionId);
            ValidateToken(token);
            this.SessionId = sessionId;
            this.Token = token;
            this.Socket = socket;
        }

        /// <summary>
        ///     The ID of a session (generated with the same `APIKEY` as specified in the URL) where the streams will be gathered
        ///     from.
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        ///     A valid OpenTok token with a Subscriber role.
        /// </summary>
        public string Token { get; }

        /// <summary>
        ///     Options for configuring the connect call for WebSocket.
        /// </summary>
        public WebSocket Socket { get; }

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

        /// <summary>
        ///     Options for configuring the connect call for WebSocket.
        /// </summary>
        public class WebSocket
        {
            /// <summary>
            ///     Creates a new instance of WebSocket.
            /// </summary>
            /// <param name="uri">
            ///     A publicly reachable WebSocket URI controlled by the customer for the destination of the connect
            ///     call. (f.e. wss://service.com/wsendpoint)".
            /// </param>
            /// <param name="streams">
            ///     An array of stream IDs for the OpenTok streams you want to include in the WebSocket stream. If you omit this property, all streams in the session will be included.
            /// </param>
            /// <param name="headers">
            ///      An object of key-value pairs of headers to be sent to your WebSocket server with each message, with a maximum length of 512 bytes.
            /// </param>
            /// <param name="audioRate">The audio sampling rate in Hz.</param>
            /// <param name="bidirectionalAudio">Indicates whether bidirectional audio is enabled or not.</param>
            public WebSocket(Uri uri, string[] streams = null, Dictionary<string, string> headers = null, SupportedAudioRates audioRate = SupportedAudioRates.AUDIO_RATE_8000Hz, bool bidirectionalAudio = false)
            {
                ValidateUri(uri);
                this.Uri = uri;
                this.Streams = streams ?? Array.Empty<string>();
                this.Headers = headers ?? new Dictionary<string, string>();
                this.AudioRate = audioRate;
                this.HasBidirectionalAudio = bidirectionalAudio;
            }

            /// <summary>
            ///     A publicly reachable WebSocket URI controlled by the customer for the destination of the connect call. (f.e.
            ///     wss://service.com/wsendpoint)"
            /// </summary>
            [JsonProperty(PropertyName = "uri")]
            public Uri Uri { get; }

            /// <summary>
            ///     The stream IDs of the participants' whose audio is going to be connected. If not provided, all streams in session
            ///     will be selected.
            /// </summary>
            [JsonProperty(PropertyName = "streams")]
            public string[] Streams { get; }

            /// <summary>
            ///     An object of key/val pairs with additional properties to send to your Websocket server, with a maximum length of
            ///     512 bytes.
            /// </summary>
            [JsonProperty(PropertyName = "headers")]
            public Dictionary<string, string> Headers { get; }

            /// <summary>
            /// The audio sampling rate in Hz.
            /// </summary>
            [JsonProperty(PropertyName = "audioRate")]
            public SupportedAudioRates AudioRate { get; set; }
            
            /// <summary>
            /// Indicates whether bidirectional audio is enabled or not.
            /// </summary>
            [JsonProperty(PropertyName = "bidirectional")]
            public bool HasBidirectionalAudio { get; set; }
            
            /// <summary>
            ///     Configures how audio is serialized on the WebSocket wire. By default, audio is sent as raw binary PCM 16-bit
            ///     frames.
            /// </summary>
            [JsonProperty(PropertyName = "audioTransport", NullValueHandling = NullValueHandling.Ignore)]
            public AudioTransport AudioTransport { get; set; }

            private static void ValidateUri(Uri url)
            {
                if (url.AbsoluteUri.Length < MinimumUrlLength || url.AbsoluteUri.Length > MaximumUrlLength)
                {
                    throw new OpenTokException(InvalidUrl);
                }
            }
            
            /// <summary>
            ///     A number representing the audio sampling rate in Hz.
            /// </summary>
            public enum SupportedAudioRates
            {
                /// <summary>
                ///     8000Hz
                /// </summary>
                AUDIO_RATE_8000Hz = 8000,

                /// <summary>
                ///     16000Hz
                /// </summary>
                AUDIO_RATE_16000Hz = 16000,
                
                /// <summary>
                ///     16000Hz
                /// </summary>
                AUDIO_RATE_24000Hz = 24000,
            }
        }
        
        
        
        /// <summary>
        /// Converts request to dictionary.
        /// </summary>
        /// <returns>Dictionary containing instance values.</returns>
        public Dictionary<string, object> ToDataDictionary() =>
            new Dictionary<string, object>
            {
                {"sessionId", this.SessionId},
                {"token", this.Token},
                {"websocket", this.Socket},
            };
        
        /// <summary>
        ///     Represents the configuration for how audio is serialized on the WebSocket wire.
        /// </summary>
        public class AudioTransport
        {
            /// <summary>
            ///     Serialization format: 'binary' (raw PCM16, the default) or 'json'.
            /// </summary>
            [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter), true)]
            [JsonProperty(PropertyName = "transport")]
            public AudioTransportType Transport { get; set; }

            /// <summary>
            ///     Encoding format. Required when <see cref="Transport"/> is <see cref="AudioTransportType.Json"/>. Set to
            ///     'base64'.
            /// </summary>
            [JsonProperty(PropertyName = "encoding", NullValueHandling = NullValueHandling.Ignore)]
            public string Encoding { get; set; }

            /// <summary>
            ///     The JSON key for the outbound audio data. Defaults to 'audio'.
            /// </summary>
            [JsonProperty(PropertyName = "audio_field", NullValueHandling = NullValueHandling.Ignore)]
            public string AudioField { get; set; }

            /// <summary>
            ///     The JSON key for inbound audio data when bidirectional is enabled. Defaults to the same value as
            ///     <see cref="AudioField"/>.
            /// </summary>
            [JsonProperty(PropertyName = "receive_audio_field", NullValueHandling = NullValueHandling.Ignore)]
            public string ReceiveAudioField { get; set; }

            /// <summary>
            ///     Extra key-value pairs included in every outbound JSON audio message.
            /// </summary>
            [JsonProperty(PropertyName = "static_fields", NullValueHandling = NullValueHandling.Ignore)]
            public Dictionary<string, string> StaticFields { get; set; }
        }
        
        /// <summary>
        ///     Defines the serialization format for audio on the WebSocket wire.
        /// </summary>
        public enum AudioTransportType
        {
            /// <summary>
            ///     Raw PCM16 binary frames (the default).
            /// </summary>
            [Description("binary")]
            Binary,

            /// <summary>
            ///     JSON-wrapped audio frames. Requires <see cref="AudioTransport.Encoding"/> to be set to 'base64'.
            /// </summary>
            [Description("json")]
            Json,
        }
    }
}