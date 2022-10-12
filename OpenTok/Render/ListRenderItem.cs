using System;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// 
    /// </summary>
    public struct ListRenderItem
    {
        /// <summary>
        /// 
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
        public ListRenderItem(string id, string sessionId, string projectId, int createdAt, int updatedAt, Uri url,
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
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UpdatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScreenResolution Resolution { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StreamId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Reason { get; set; }
    }
}