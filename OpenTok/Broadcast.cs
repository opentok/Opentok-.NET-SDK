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

        /// <summary>
        /// Provides details on an HLS broadcast stream. This object includes an <c>hls</c> property
        /// that specifies whether
        /// <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#dvr">DVR functionality</a>
        /// and <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#low-latency">low-latency mode</a>
        /// are enabled for the HLS stream.
        /// </summary>
        public class BroadcastSettings
        {
            /// <summary>
            /// Provides details on the HLS stream.
            /// </summary>
            [JsonProperty("hls")]
            public BroadcastHlsSettings Hls { get; private set; }
        }

        /// <summary>
        /// Provides details on an HLS stream.
        /// </summary>
        public class BroadcastHlsSettings
        {
            /// <summary>
            /// Whether
            /// <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#low-latency">low-latency mode</a>
            /// is enabled for the HLS stream.
            /// </summary>
            [JsonProperty("lowLatency")]
            public bool LowLatency { get; private set; }

            /// <summary>
            /// Whether
            /// <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#dvr">DVR functionality</a>
            /// is enabled for the HLS stream.
            /// </summary>
            [JsonProperty("dvr")]
            public bool DVR { get; private set; }
        }

        private readonly OpenTok _opentok;

        /// <summary>
        /// Initializes a new instance of the <see cref="Broadcast"/> class.
        /// </summary>
        protected Broadcast()
        {

        }

        internal Broadcast(OpenTok opentok)
        {
            _opentok = opentok;
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
            StreamMode = broadcast.StreamMode;
            Settings = broadcast.Settings;
            MultiBroadcastTag = broadcast.MultiBroadcastTag;

            if (BroadcastUrls == null)
                return;

            if (BroadcastUrls.ContainsKey("hls"))
            {
                Hls = BroadcastUrls["hls"].ToString();
            }

            if (BroadcastUrls.ContainsKey("rtmp"))
            {
                RtmpList = new List<Rtmp>();
                foreach (var jsonToken in (JArray)BroadcastUrls["rtmp"])
                {
                    var item = (JObject) jsonToken;
                    Rtmp rtmp = new Rtmp
                    {
                        Id = item.GetValue("id")?.ToString(),
                        ServerUrl = item.GetValue("serverUrl")?.ToString(),
                        StreamName = item.GetValue("streamName")?.ToString()
                    };
                    RtmpList.Add(rtmp);
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
        public string SessionId { get; set; }

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
        public string Hls { get; set; }

        /// <summary>
        /// The broadcast HLS and RTMP URLs.
        /// </summary>
        [JsonProperty("broadcastUrls")]
        private Dictionary<string, object> BroadcastUrls { get; set; }

        /// <summary>
        /// Whether streams included in the broadcast are selected automatically ("auto", the default) or manually
        /// </summary>
        [JsonProperty("streamMode")]
        public StreamMode StreamMode { get; set; }

        /// <summary>
        /// Provides details on an HLS broadcast stream. This includes information on
        /// whether the stream supports DVR functionality and low-latency mode.
        /// </summary>
        [JsonProperty("settings")]
        public BroadcastSettings Settings { get; set; }
        
        /// <summary>
        /// The unique tag for simultaneous broadcasts (if one was set).
        /// </summary>
        [JsonProperty("multiBroadcastTag")]
        public string MultiBroadcastTag { get; set; }

        /// <summary>
        /// Stops the live broadcasting if it is started.
        /// </summary>
        public void Stop()
        {
            if (_opentok != null)
            {
                Broadcast broadcast = _opentok.StopBroadcast(Id);
                Status = broadcast.Status;
            }
        }
    }
}
