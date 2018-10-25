using System;
using System.Collections.Generic;


namespace OpenTokSDK
{
    /**
     * Defines signal payload for the Signal API.
    */
    public class SignalProperties
    {
        internal SignalProperties()
        {

        }
        public SignalProperties(string data = null, string type = null)
        {
            this.data = data;
            this.type = type;
        }
        public SignalProperties(string data = null)
        {
            this.data = data;

        }

        public string data { get; set; }
        public string type { get; set; }
    }
}
