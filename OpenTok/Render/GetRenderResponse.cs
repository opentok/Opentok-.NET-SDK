using System;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// TODO
    /// </summary>
    public struct GetRenderResponse
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
        /// <param name="reason"></param>
        public GetRenderResponse(string id, string sessionId, string projectId, int createdAt, int updatedAt, Uri url,
            ScreenResolution resolution, string status, string reason)
        {
            this.Id = id;
            this.SessionId = sessionId;
            this.ProjectId = projectId;
            this.CreatedAt = createdAt;
            this.UpdatedAt = updatedAt;
            this.Url = url;
            this.Resolution = resolution;
            this.Status = status;
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
        public int CreatedAt { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public int UpdatedAt { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public ScreenResolution Resolution { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Reason { get; set; }
    }
}