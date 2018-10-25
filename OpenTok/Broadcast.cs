using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenTokSDK
{
    /**
    * Represents a broadcast of an OpenTok session.
    */
    public class Broadcast
    {
        /**
        * Defines values returned by the Status property of a Broadcast object.
        */
        public enum BroadcastStatus
        {
            /**
             * The broadcast is stopped
             */
            STOPPED,
            /**
             * The broadcast is started
             */
            STARTED
        }

        private OpenTok opentok;

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
                if (BroadcastUrls.ContainsKey("hls")) {
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

        /**
         * The broadcast ID.
         */
        [JsonProperty("id")]
        public string Id { get; set; }

        /**
         * The session ID of the OpenTok session associated with this broadcast.
         */
        [JsonProperty("sessionId")]
        public String SessionId { get; set; }

        /**
         * The OpenTok API key associated with the broadcast.
         */
        [JsonProperty("projectId")]
        public int ProjectId { get; set; }

        /**
         * The time the broadcast started, expressed in milliseconds since the Unix epoch (January 1, 1970, 00:00:00 UTC).
         */
        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        /**
         * The time the broadcast was updated, in milliseconds since the Unix epoch  (January 1, 1970, 00:00:00 UTC).
         */
        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

        /**
         * The resolution of the broadcast: either "640x480" (SD, the default) or "1280x720" (HD).
         */
        [JsonProperty("resolution")]
        public string Resolution { get; set; }

        /**
         * The status of the broadcast: either "started" or "stopped".
         */
        [JsonProperty("status")]
        public BroadcastStatus Status { get; set; }

        /**
         * The maximun duration of the broadcast
         */
        [JsonProperty("maxDuration")]
        public int MaxDuration { get; set; }


        /**
         * The RTMP List
         */
        public List<Rtmp> RtmpList { get; set; }
   
        /**
         * HLS Url
         */
        public String Hls { get; set; }

        /**
         * The broadcast HLS and RTMP URLs
         */
        [JsonProperty("broadcastUrls")]
        private Dictionary<string, object> BroadcastUrls { get; set; }

        /**
        * Stops the live broadcasting if it is started.
        */
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
