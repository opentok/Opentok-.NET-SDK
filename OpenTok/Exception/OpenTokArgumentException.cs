using System;

namespace OpenTokSDK.Exception
{
    /// <summary>
    /// Defines an exception object thrown when an invalid argument is passed into a method.
    /// </summary>
    public class OpenTokArgumentException : ArgumentException
    {
        /// <inheritdoc cref="ArgumentException"/>
        public OpenTokArgumentException(string message)
            :base(message)
        {
            
        }

        /// <inheritdoc cref="ArgumentException"/>
        public OpenTokArgumentException(string message, string paramName)
            : base(message, paramName)
        {
        }
    }
}