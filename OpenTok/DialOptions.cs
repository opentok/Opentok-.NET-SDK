using System.Collections.Generic;

namespace OpenTokSDK
{
    /// <summary>
    /// Used to define options for the the <see cref="OpenTok.Dial"/> method. These are
    /// custom headers to be added to the SIP ​INVITE​ request initiated from OpenTok to
    /// your SIP platform.
    /// </summary>
    public class DialOptions
    {
        /// <summary>
        /// A dictionary of custom headers to be added to the SIP ​INVITE​ request initiated
        /// from OpenTok to your SIP platform.
        /// </summary>
        public Dictionary<string, object> Headers { get; set; }

        /// <summary>
        ///  Contains the username and password to be used in the the SIP INVITE​ request.
        /// </summary>
        public DialAuth Auth { get; set; }

        /// <summary>
        /// Indicates whether the media must be transmitted encrypted (​true​) or not (​false​, the default).
        /// </summary>
        public bool? Secure { get; set; }

        /// <summary>
        /// The number or string that will be sent to the final SIP number as the caller.
        /// </summary>
        /// <remarks>
        /// This must be a string in the form of "from@example.com", where <c>from</c> can be a string or a number. If it is set
        /// to a number (for example, "14155550101@example.com"), it will show up as the incoming number on PSTN phones. 
        /// If it is undefined or set to a string (for example, "joe@example.com"), +00000000 will show up as the incoming 
        /// number on PSTN phones.
        /// </remarks>
        public string From { get; set; }

        /// <summary>
        /// Whether the SIP call will include video (​true​) or not (​false​, the default).
        /// </summary>
        /// <remarks>With video included, the SIP client's video is included in the OpenTok stream 
        /// that is sent to the OpenTok session. The SIP client will receive a single composed video of the published streams 
        /// in the OpenTok session.</remarks>
        public bool? Video { get; set; }

        /// <summary>
        /// Whether the SIP endpoint observes
        /// <a href="https://tokbox.com/developer/guides/moderation/#force_mute">force mute moderation</a>
        /// (true) or not (false, the default).
        /// </summary>
        public bool? ObserveForceMute { get; set; }
    }
}
