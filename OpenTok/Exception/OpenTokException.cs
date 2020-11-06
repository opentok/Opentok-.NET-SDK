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
    
        /// <summary>
        /// Construct opentok exception
        /// </summary>
        public OpenTokException()
        {
        }

        /// <summary>
        /// Construct OpentTokException with a message
        /// </summary>
        /// <param name="message"></param>
        public OpenTokException(string message)
            : base(message){}
        
        /// <summary>
        /// Construct OpenTokException with a message and an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public OpenTokException(string message, System.Exception exception)
            : base(message, exception){}

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
