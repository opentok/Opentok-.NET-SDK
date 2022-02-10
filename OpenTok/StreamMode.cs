using System.ComponentModel;

namespace OpenTokSDK
{
    /// <summary>
    /// Whether streams included in an archive or broadcast are selected automatically or manually.
    /// See the <c>streamMode</c> parameter of the <see cref="OpenTok.StartArchive"/> and
    /// <see cref="OpenTok.StartBroadcast"/> methods).
    /// </summary>
    public enum StreamMode
    {
        /// <summary>
        /// All streams in the session can be included.
        /// </summary>
        [Description("auto")]
        Auto,
        /// <summary>
        /// You will specify streams to be included in the archive or broadcast.
        /// For an archive, use the <see cref="OpenTok.AddStreamToArchive"/> and
        /// <see cref="OpenTok.RemoveStreamFromArchive"/> methods (or the
        /// <see cref="OpenTok.AddStreamToArchiveAsync"/> and
        /// <see cref="OpenTok.RemoveStreamFromArchiveAsync"/> methods).
        /// For a broadcast, use the <see cref="OpenTok.AddStreamToBroadcast"/> and
        /// <see cref="OpenTok.RemoveStreamFromBroadcast"/> methods (or the
        /// <see cref="OpenTok.AddStreamToBroadcastAsync"/> and
        /// <see cref="OpenTok.RemoveStreamFromBroadcastAsync"/> methods).
        /// </summary>
        [Description("manual")]
        Manual
    }
}
