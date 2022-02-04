namespace OpenTokSDK.Exception
{
    /// <summary>
    /// Defines exceptions in the OpenTok SDK.
    /// </summary>
    public class OpenTokException : System.Exception
    {

        /// <inheritdoc cref="Exception"/>
        public OpenTokException()
        {
        }

        /// <summary>
        /// Construct OpentTokException with a message
        /// </summary>
        /// <param name="message"></param>
        public OpenTokException(string message)
            : base(message) { }

        /// <summary>
        /// Construct OpenTokException with a message and an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public OpenTokException(string message, System.Exception exception)
            : base(message, exception) { }

        /// <summary>
        /// Get's the message of the exception
        /// </summary>
        /// <returns></returns>
        public string GetMessage()
        {
            return Message;
        }

        /// <summary>
        /// Get's the inner exception
        /// </summary>
        /// <returns></returns>
        public System.Exception GetException()
        {
            return InnerException;
        }
    }
}
