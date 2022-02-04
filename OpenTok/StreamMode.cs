using System.ComponentModel;

namespace OpenTokSDK
{
    /// <summary>
    /// Whether streams included are selected automatically or manually.
    /// </summary>
    public enum StreamMode
    {
        /// <summary>
        /// All streams in the session can be included.
        /// </summary>
        [Description("auto")]
        Auto,
        /// <summary>
        /// Specify which streams to be included.
        /// </summary>
        [Description("manual")]
        Manual
    }
}