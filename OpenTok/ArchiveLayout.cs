using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenTokSDK
{
    /// <summary>
    /// Layout the archive is going to use
    /// </summary>
    public class ArchiveLayout
    {
        /// <summary>
        /// The type of layout you'd like to use
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]        
        public LayoutType Type { get; set; }

        /// <summary>
        /// The stylesheet to use for the layout. Must be set if using custom.
        /// </summary>
        [JsonProperty("stylesheet", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StyleSheet { get; set; }
    }
}
