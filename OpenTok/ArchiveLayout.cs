using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

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
        [JsonProperty("stylesheet", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue("")]
        public string StyleSheet { get; set; }

        /// <summary>
        /// The <see cref="ScreenShareLayoutType"/>to use when there is a screen-sharing 
        /// stream in the session. Note that to use this property, 
        /// you must set the <see cref="Type"/> property to <see cref="LayoutType.bestFit"/> 
        /// and leave the <see cref="StyleSheet"/> property unset. 
        /// For more information, see Layout types for screen sharing.
        /// NOTE: <see cref="LayoutType.custom"/> is not valid for this property
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter), true)]
        [JsonProperty("screensharetype", NullValueHandling = NullValueHandling.Ignore)]
        public ScreenShareLayoutType? ScreenShareType { get; set; }
    }
}
