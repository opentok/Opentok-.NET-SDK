using System;
using Newtonsoft.Json;

namespace OpenTokSDK
{   
    /**
    * Represents a string in an OpenTok session.
    */
    public class Rtmp
    {
        /**
        * The stream id.
        */
        [JsonProperty("id")]
        public string Id { get; set; }

        /**
        * The server URL.
        */
        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }

        /**
        * The stream name.
        */
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
