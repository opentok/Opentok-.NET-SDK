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
        public String Id { get; set; }

        /**
        * The server URL.
        */
        [JsonProperty("serverUrl")]
        public String ServerUrl { get; set; }

        /**
        * The stream name.
        */
        [JsonProperty("streamName")]
        public String StreamName { get; set; }

    }
}
