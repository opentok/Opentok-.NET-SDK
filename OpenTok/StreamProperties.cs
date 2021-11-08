using System.Collections.Generic;
using Newtonsoft.Json;


namespace OpenTokSDK
{
    /// <summary>
    /// Use for setting the layout class list for streams in a session
    /// </summary>
    public class StreamProperties
    {

        internal StreamProperties()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamProperties"/> class.
        /// </summary>
        /// <param name="Id">The stream ID.</param>
        /// <param name="LayoutClassList">The layout class list as a list of strings.</param>
        public StreamProperties(string Id = null, List<string> LayoutClassList = null)
        {
            this.Id = Id;
            if (LayoutClassList != null)
            {
                this.LayoutClassList = LayoutClassList;
            } else
            {
                this.LayoutClassList = new List<string>();
            }
            
        }

        /// <summary>
        /// Adds layout class
        /// </summary>
        /// <param name="layoutClass">Name of layout class</param>
        public void addLayoutClass(string layoutClass)
        {
            LayoutClassList.Add(layoutClass);
        }

        /// <summary>
        /// The stream ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The layout class list as a list of strings.
        /// </summary>
        [JsonProperty("layoutClassList")]
        public List<string> LayoutClassList { get; set; }
        
    }
}
