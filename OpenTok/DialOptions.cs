using System.Collections.Generic;

namespace OpenTokSDK
{
    public class DialOptions
    {
        /// <summary>
        /// This object defines custom headers to be added to the SIP ​INVITE​ request initiated from OpenTok to your SIP platform.
        /// </summary>
        public Dictionary<string, object> Headers { get; set; }

        /// <summary>
        ///  This object contains the username and password to be used in the the SIP INVITE​ request for HTTP digest authentication, 
        ///  if it is required by your SIP platform.
        /// </summary>
        public DialAuth Auth { get; set; }

        /// <summary>
        /// A Boolean flag that indicates whether the media must be transmitted encrypted (​true​) or not (​false​, the default).
        /// </summary>
        public bool? Secure { get; set; }

        /// <summary>
        /// The number or string that will be sent to the final SIP number as the caller.
        /// </summary>
        /// <remarks>
        /// It must be a string in the form of from@example.com, where from can be a string or a number. If from is set
        /// to a number (for example, "14155550101@example.com"), it will show up as the incoming number on PSTN phones. 
        /// If from is undefined or set to a string (for example, "joe@example.com"), +00000000 will show up as the incoming 
        /// number on PSTN phones.
        /// </remarks>
        public string From { get; set; }

        /// <summary>
        /// A Boolean flag that indicates whether the SIP call will include video (​true​) or not (​false​, the default).
        /// </summary>
        /// <remarks>This is a beta feature. With video included, the SIP client's video is included in the OpenTok stream 
        /// that is sent to the OpenTok session. The SIP client will receive a single composed video of the published streams 
        /// in the OpenTok session.</remarks>
        public bool? Video { get; set; }

        /// <summary>
        /// A boolean flag that indicates whether the SIP end point observes force mute moderation (true) or not (false, the default).
        /// </summary>
        public bool? ObserveForceMute { get; set; }
    }
}
