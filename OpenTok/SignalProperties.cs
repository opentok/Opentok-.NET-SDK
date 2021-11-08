using System;
using System.Collections.Generic;


namespace OpenTokSDK
{
    /// <summary>
    /// Defines signal payload for the Signal API.
    /// </summary>
    public class SignalProperties
    {
        internal SignalProperties()
        {

        }

        /// <param name="data">The data string for the signal</param>
        /// <param name="type">The type string for the signal</param>
        public SignalProperties(string data = null, string type = null)
        {
            this.data = data;
            this.type = type;
        }

        /// <param name="data">The data string for the signal. You can send a maximum of 8kB.</param>
        public SignalProperties(string data = null)
        {
            this.data = data;
        }

        /// <summary>
        /// The data string for the signal. You can send a maximum of 8kB.
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// The type string for the signal. You can send a maximum of 128 characters, and only the following characters are 
        /// allowed: A-Z, a-z, numbers (0-9), '-', '_', and '~'.
        /// </summary>
        public string type { get; set; }
    }
}
