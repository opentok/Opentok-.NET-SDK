using System;
using Newtonsoft.Json;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// TODO
    /// </summary>
    public struct RenderItem
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionId"></param>
        /// <param name="projectId"></param>
        /// <param name="createdAt"></param>
        /// <param name="updatedAt"></param>
        /// <param name="url"></param>
        /// <param name="resolution"></param>
        /// <param name="status"></param>
        /// <param name="streamId"></param>
        /// <param name="reason"></param>
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
        /// TODO
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public double CreatedAt { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public double UpdatedAt { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [JsonConverter(typeof(ScreenResolutionConverter))]
        public ScreenResolution Resolution { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string StreamId { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Reason { get; set; }
    }
}