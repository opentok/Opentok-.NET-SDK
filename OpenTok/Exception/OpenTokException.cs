using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK.Exception
{
    /// <summary>
    /// Defines exceptions in the OpenTok SDK.
    /// </summary>
    public class OpenTokException : System.Exception
    {
        private System.Exception exception;
        private string message;

        /// <summary>
        /// Constructor. Do not use.
        /// </summary>
        public OpenTokException()
        {
        }

        /// <summary>
        /// Constructor. Do not use.
        /// </summary>
        /// <param name="message"></param>
        public OpenTokException(string message)
            : base(message)
        {
            this.message = message;
        }

        /// <summary>
        /// Constructor. Do not use.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public OpenTokException(string message, System.Exception exception)
            : base(message)
        {
            this.message = message;
            this.exception = exception;
        }

        //GGB override Message property
        public string GetMessage()
        {
            return message;
        }

        public System.Exception GetException()
        {
            return exception;
        }
    }

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
