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
        /// You will specify streams to be included in the archive using the
        /// <see cref="OpenTok.AddStreamToArchive"/>,
        /// <see cref="OpenTok.RemoveStreamFromArchive"/>,
        /// <see cref="OpenTok.AddStreamToBroadcast"/> and
        /// <see cref="OpenTok.RemoveStreamFromBroadcast"/> methods (or the
        /// <see cref="OpenTok.AddStreamToArchiveAsync"/>,
        /// <see cref="OpenTok.AddStreamToBroadcastAsync"/>,
        /// <see cref="OpenTok.RemoveStreamFromBroadcastAsync"/> and
        /// <see cref="OpenTok.RemoveStreamFromArchiveAsync"/> methods).
        /// </summary>
        [Description("manual")]
        Manual
    }
}