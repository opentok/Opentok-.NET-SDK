using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenTokSDK
{
    /// <summary>
    /// Represents a broadcast of an OpenTok session.
    /// </summary>
    public class Broadcast
    {
        /// <summary>
        /// Defines values returned by the Status property of a Broadcast object.
        /// </summary>
        public enum BroadcastStatus
        {
            /// <summary>
            /// The broadcast is stopped.
            /// </summary>
            STOPPED,
            /// <summary>
            /// The broadcast is started.
            /// </summary>
            STARTED
        }

        private OpenTok opentok;

        /// <summary>
        /// Initializes a new instance of the <see cref="Broadcast"/> class.
        /// </summary>
        protected Broadcast()
        {

        }

        internal Broadcast(OpenTok opentok)
        {
            this.opentok = opentok;
        }

        internal void CopyBroadcast(Broadcast broadcast)
        {
            Id = broadcast.Id;
            SessionId = broadcast.SessionId;
            ProjectId = broadcast.ProjectId;
            CreatedAt = broadcast.CreatedAt;
            UpdatedAt = broadcast.UpdatedAt;
            Resolution = broadcast.Resolution;
            MaxDuration = broadcast.MaxDuration;
            Status = broadcast.Status;
            BroadcastUrls = broadcast.BroadcastUrls;

            if (BroadcastUrls != null)
            {
                if (BroadcastUrls.ContainsKey("hls"))
                {
                    Hls = BroadcastUrls["hls"].ToString();
                }

                if (BroadcastUrls.ContainsKey("rtmp"))
                {
                    RtmpList = new List<Rtmp>();
                    foreach (JObject item in (JArray)BroadcastUrls["rtmp"])
                    {
                        Rtmp rtmp = new Rtmp();
                        rtmp.Id = item.GetValue("id").ToString();
                        rtmp.ServerUrl = item.GetValue("serverUrl").ToString();
                        rtmp.StreamName = item.GetValue("streamName").ToString();
                        RtmpList.Add(rtmp);
                    }
                }
            }
        }

        /// <summary>
        /// The broadcast ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The session ID of the OpenTok session associated with this broadcast.
        /// </summary>
        [JsonProperty("sessionId")]
        public String SessionId { get; set; }

        /// <summary>
        /// The OpenTok API key associated with the broadcast.
        /// </summary>
        [JsonProperty("projectId")]
        public int ProjectId { get; set; }

        /// <summary>
        /// The time the broadcast started, expressed in milliseconds since the Unix epoch (January 1, 1970, 00:00:00 UTC).
        /// </summary>
        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        /// <summary>
        /// The time the broadcast was updated, in milliseconds since the Unix epoch  (January 1, 1970, 00:00:00 UTC).
        /// </summary>
        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

        /// <summary>
        /// The resolution of the broadcast: either "640x480" (SD, the default) or "1280x720" (HD).
        /// </summary>
        [JsonProperty("resolution")]
        public string Resolution { get; set; }

        /// <summary>
        /// The status of the broadcast: either "started" or "stopped".
        /// </summary>
        [JsonProperty("status")]
        public BroadcastStatus Status { get; set; }

        /// <summary>
        /// The maximun duration of the broadcast.
        /// </summary>
        [JsonProperty("maxDuration")]
        public int MaxDuration { get; set; }

        /// <summary>
        /// The RTMP List.
        /// </summary>
        public List<Rtmp> RtmpList { get; set; }

        /// <summary>
        /// HLS Url.
        /// </summary>
        public String Hls { get; set; }

        /// <summary>
        /// The broadcast HLS and RTMP URLs.
        /// </summary>
        [JsonProperty("broadcastUrls")]
        private Dictionary<string, object> BroadcastUrls { get; set; }

        /// <summary>
        /// Stops the live broadcasting if it is started.
        /// </summary>
        public void Stop()
        {
            if (opentok != null)
            {
                Broadcast broadcast = opentok.StopBroadcast(Id);
                Status = broadcast.Status;
            }
        }
    }
}
