using System;
using Newtonsoft.Json;

namespace OpenTokSDK
{
    /// <summary>
    /// Represents a string in an OpenTok session.
    /// </summary>
    public class Rtmp
    {
        /// <summary>
        /// The stream id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The server URL.
        /// </summary>
        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }

        /// <summary>
        /// The stream name.
        /// </summary>
        [JsonProperty("streamName")]
        public string StreamName { get; set; }

        public Rtmp()
        {
            
        }

        public Rtmp(string id, string serverUrl, string streamName)
        {
            Id = id;
            ServerUrl = serverUrl;
            StreamName = streamName;
        }

    }
}
