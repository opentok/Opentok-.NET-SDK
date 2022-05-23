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

        /// <summary>
        /// Defines signal payload for the Signal API.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public SignalProperties(string data = null, string type = null)
        {
            this.data = data;
            this.type = type;
        }

        /// <summary>
        /// Defines signal payload for the Signal API.
        /// </summary>
        /// <param name="data"></param>
        public SignalProperties(string data = null)
        {
            this.data = data;
        }

        public string data { get; set; }
        public string type { get; set; }
    }
}
