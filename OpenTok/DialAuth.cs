namespace OpenTokSDK
{
    /// <summary>
    /// This object contains the username and password to be used in the the SIP INVITE​ request for HTTP digest authentication, if it is required by your SIP platform.
    /// </summary>
    public class DialAuth
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
