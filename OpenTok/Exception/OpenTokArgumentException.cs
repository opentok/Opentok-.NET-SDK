namespace OpenTokSDK.Exception
{
    /// <summary>
    /// Defines an exception object thrown when an invalid argument is passed into a method.
    /// </summary>
    public class OpenTokArgumentException : OpenTokException
    {
        /// <summary>
        /// Constructor. Do not use.
        /// </summary>
        /// <param name="message"></param>
        public OpenTokArgumentException(string message)
            : base(message)
        {
        }
    }
}
