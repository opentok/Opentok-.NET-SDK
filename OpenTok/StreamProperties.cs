using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace OpenTokSDK
{
    public class StreamProperties
    {

        internal StreamProperties()
        {
        }

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

        public void addLayoutClass(string layoutClass)
        {
            LayoutClassList.Add(layoutClass);
        }

        /**
         * The stream ID.
         */
        [JsonProperty("id")]
        public string Id { get; set; }

        /**
         * The layout class list as a list of strings.
         */
        [JsonProperty("layoutClassList")]
        public List<string> LayoutClassList { get; set; }
        
    }
}
