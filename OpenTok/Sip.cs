using System;

namespace OpenTokSDK
{
    public class Sip
    {
        /// <summary>
        /// A unique ID for the SIP call.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The OpenTok connection ID for the SIP call's connection in the
        /// OpenTok session. You can use this connection ID to terminate the
        /// SIP call.
        /// </summary>
        public Guid ConnectionId { get; set; }
        
        /// <summary>
        /// The OpenTok stream ID for the SIP call's stream in the OpenTok session.
        /// </summary>
        public Guid StreamId { get; set; }
    }
}