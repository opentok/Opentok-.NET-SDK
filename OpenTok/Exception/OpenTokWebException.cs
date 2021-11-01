namespace OpenTokSDK.Exception
{
    /// <summary>
    /// Defines an exception object thrown when a REST API call results in an error response.
    /// </summary>
    public class OpenTokWebException : OpenTokException
    {
        /// <summary>
        /// Constructor. Do not use.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public OpenTokWebException(string message, System.Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// Constructor. Do not use.
        /// </summary>
        /// <param name="message"></param>
        public OpenTokWebException(string message)
            : base(message)
        {
        }
    }
}
