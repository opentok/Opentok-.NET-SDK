using System;
using Newtonsoft.Json;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// Represents a rendering.
    /// </summary>
    public struct RenderItem
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">The ID of the rendering.</param>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="projectId">The project ID.</param>
        /// <param name="createdAt">The creation date.</param>
        /// <param name="updatedAt">The last update date.</param>
        /// <param name="url">The URL.</param>
        /// <param name="resolution">The screen resolution.</param>
        /// <param name="status">The status.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <param name="reason">The reason.</param>
        public RenderItem(string id, string sessionId, string projectId, int createdAt, int updatedAt, Uri url,
            RenderResolution resolution, string status, string streamId, string reason)
        {
            this.Id = id;
            this.SessionId = sessionId;
            this.ProjectId = projectId;
            this.CreatedAt = createdAt;
            this.UpdatedAt = updatedAt;
            this.Url = url;
            this.Resolution = resolution;
            this.Status = status;
            this.StreamId = streamId;
            this.Reason = reason;
        }

        /// <summary>
        /// The ID of the rendering.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The session ID.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The project ID.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The creation date.
        /// </summary>
        public double CreatedAt { get; set; }

        /// <summary>
        /// The last update date.
        /// </summary>
        public double UpdatedAt { get; set; }

        /// <summary>
        /// The URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// The Experience Composer renderer resolution.
        /// </summary>
        [JsonConverter(typeof(RenderResolutionConverter))]
        public RenderResolution Resolution { get; set; }

        /// <summary>
        /// The status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The stream ID.
        /// </summary>
        public string StreamId { get; set; }

        /// <summary>
        /// The reason, when the status is either "stopped" or "failed". If the status is "stopped",
        /// the reason field will contain either "Max Duration Exceeded" or "Stop Requested."
        /// If the status is "failed", the reason will contain a more specific error message.
        /// </summary>
        public string Reason { get; set; }
    }
}