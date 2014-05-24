using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTokSDK.Exceptions
{
    /**
     * Defines exceptions in the OpenTok SDK.
     */
    public class OpenTokException : Exception
    {
        private Exception exception;
        private string message;

        /**
         * Constructor. Do not use.
         */
        public OpenTokException()
        {
        }

        /**
         * Constructor. Do not use.
         */
        public OpenTokException(string message)
            : base(message)
        {
            this.message = message;
        }

        /**
         * Constructor. Do not use.
         */
        public OpenTokException(string message, Exception exception)
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

        public Exception GetException()
        {
            return exception;
        }
    }

    /**
     * Defines an exception object thrown when an invalid argument is passed into a method.
     */
    public class OpenTokArgumentException : OpenTokException
    {
        /**
         * Constructor. Do not use.
         */
        public OpenTokArgumentException(string message)
            : base(message)
        {
        }
    }

    /**
     * Defines an exception object thrown when a REST API call results in an error response.
     */
    public class OpenTokWebException : OpenTokException
    {
       /**
        * Constructor. Do not use.
        */
        public OpenTokWebException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /**
         * Constructor. Do not use.
         */
        public OpenTokWebException(string message)
            : base(message)
        {
        }
    }


}
