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
        /// <param name="id">The Id of the rendering.</param>
        /// <param name="sessionId">The Id of the session.</param>
        /// <param name="projectId">The Id of the project.</param>
        /// <param name="createdAt">The creation date.</param>
        /// <param name="updatedAt">The last update date.</param>
        /// <param name="url">The Url.</param>
        /// <param name="resolution">The screen resolution.</param>
        /// <param name="status">The status.</param>
        /// <param name="streamId">The Id of the stream.</param>
        /// <param name="reason">The reason.</param>
        public RenderItem(string id, string sessionId, string projectId, int createdAt, int updatedAt, Uri url,
            ScreenResolution resolution, string status, string streamId, string reason)
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
        /// The Id of the rendering.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Id of the session.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The Id of the project.
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
        /// The Url.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// The screen resolution.
        /// </summary>
        [JsonConverter(typeof(ScreenResolutionConverter))]
        public ScreenResolution Resolution { get; set; }

        /// <summary>
        /// The status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The Id of the stream.
        /// </summary>
        public string StreamId { get; set; }

        /// <summary>
        /// The reason.
        /// </summary>
        public string Reason { get; set; }
    }
}