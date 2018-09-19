﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace OpenTokSDK
{
    public class Stream
    {

        protected Stream()
        {

        }

        /**
         * The layout class list as a list of strings.
         */
        [JsonProperty("layoutClassList")]
        public List<string> LayoutClassList { get; set; }

        /**
         * The video type as a string.
         */
        [JsonProperty("videoType")]
        public string VideoType { get; set; }

        /**
         * The stream ID.
         */
        [JsonProperty("id")]
        public string Id { get; set; }

        /**
         * The name of the stream.
         */
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
