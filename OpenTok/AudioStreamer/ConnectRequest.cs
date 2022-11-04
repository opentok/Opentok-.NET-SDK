using System;
using System.Collections.Generic;
using OpenTokSDK.Exception;

namespace OpenTokSDK.AudioStreamer
{
    /// <summary>
    ///     POST WebSocket connect
    /// </summary>
    public class ConnectRequest
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
        /// <param name="sessionId">
        ///     The ID of a session (generated with the same `APIKEY` as specified in the URL) where the
        ///     streams will be gathered from.
        /// </param>
        /// <param name="token">A valid OpenTok token with a Subscriber role.</param>
        /// <param name="socket">Options for configuring the connect call for WebSocket.</param>
        public ConnectRequest(string sessionId, string token, WebSocket socket)
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
            ///     The stream IDs of the participants' whose audio is going to be connected. If not provided, all
            ///     streams in session will be selected.
            /// </param>
            /// <param name="headers">
            ///     An object of key/val pairs with additional properties to send to your Websocket server, with a
            ///     maximum length of 512 bytes.
            /// </param>
            /// <param name="audioRate">A number representing the audio sampling rate in Hz. If not provided 16000 will be used.</param>
            public WebSocket(Uri uri, string[] streams = null,
                Dictionary<string, string> headers = null, int audioRate = 16000)
            {
                ValidateUri(uri);
                this.Uri = uri;
                this.Streams = streams ?? new string[0];
                this.Headers = headers ?? new Dictionary<string, string>();
                this.AudioRate = audioRate;
            }

            /// <summary>
            ///     A publicly reachable WebSocket URI controlled by the customer for the destination of the connect call. (f.e.
            ///     wss://service.com/wsendpoint)"
            /// </summary>
            public Uri Uri { get; }

            /// <summary>
            ///     The stream IDs of the participants' whose audio is going to be connected. If not provided, all streams in session
            ///     will be selected.
            /// </summary>
            public string[] Streams { get; }

            /// <summary>
            ///     An object of key/val pairs with additional properties to send to your Websocket server, with a maximum length of
            ///     512 bytes.
            /// </summary>
            public Dictionary<string, string> Headers { get; }

            /// <summary>
            ///     A number representing the audio sampling rate in Hz. If not provided 16000 will be used.
            /// </summary>
            public int AudioRate { get; }

            private static void ValidateUri(Uri url)
            {
                if (url.AbsoluteUri.Length < MinimumUrlLength || url.AbsoluteUri.Length > MaximumUrlLength)
                {
                    throw new OpenTokException(InvalidUrl);
                }
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
                {"webSocket", this.Socket},
            };
    }
}