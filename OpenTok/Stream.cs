using System.Collections.Generic;
using Newtonsoft.Json;


namespace OpenTokSDK
{
    /// <summary>
    /// Represents a stream in an OpenTok session.
    /// </summary>
    public class Stream
    {

        internal Stream()
        {
        }

        internal void CopyStream(Stream stream)
        {
            Id = stream.Id;
            Name = stream.Name;
            LayoutClassList = stream.LayoutClassList;
            VideoType = stream.VideoType;
        }

        /// <summary>
        /// The layout class list as a list of strings.
        /// </summary>
        [JsonProperty("layoutClassList")]
        public List<string> LayoutClassList { get; set; }

        /// <summary>
        /// The video type as a string.
        /// </summary>
        [JsonProperty("videoType")]
        public string VideoType { get; set; }

        /// <summary>
        /// The stream ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the stream.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
