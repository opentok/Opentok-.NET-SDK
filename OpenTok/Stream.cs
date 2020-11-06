using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace OpenTokSDK
{
    public class Stream
    {

        internal Stream()
        {
        }

        internal void CopyStream(Stream stream)
        {
            this.Id = stream.Id;
            this.Name = stream.Name;
            this.LayoutClassList = stream.LayoutClassList;
            this.VideoType = stream.VideoType;
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
