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
        /// Manually add streams
        /// </summary>
        [Description("manual")]
        Manual
    }
}
