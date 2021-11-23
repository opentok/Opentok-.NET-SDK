namespace OpenTokSDK
{
    /// <summary>
    /// Used to set the username and password to be used in the <see cref="OpenTok.Dial"/> method.
    /// These are used in the SIP INVITE​ request for HTTP digest authentication, if it is required
    /// by your SIP platform. See the <see cref="DialOptions"/> class.
    /// </summary>
    public class DialAuth
    {
        /// <summary>
        /// The username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password.
        /// </summary>
        public string Password { get; set; }
    }
}
