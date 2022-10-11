using System;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// </summary>
    public struct StartRenderResponse
    {
        /// <summary>
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
        public StartRenderResponse(string id, string sessionId, string projectId, int createdAt, int updatedAt, Uri url,
            ScreenResolution resolution, string status, string streamId)
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
        }

        public string Id { get; set; }

        public string SessionId { get; set; }

        public string ProjectId { get; set; }

        public int CreatedAt { get; set; }

        public int UpdatedAt { get; set; }

        public Uri Url { get; set; }

        public ScreenResolution Resolution { get; set; }

        public string Status { get; set; }

        public string StreamId { get; set; }
    }
}