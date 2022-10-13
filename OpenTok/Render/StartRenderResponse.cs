﻿using System;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// TODO
    /// </summary>
    public struct StartRenderResponse
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

        /// <summary>
        /// TODO
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public string ProjectId { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public int CreatedAt { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public int UpdatedAt { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public ScreenResolution Resolution { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public string StreamId { get; }
    }
}