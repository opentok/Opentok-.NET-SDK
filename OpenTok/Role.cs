using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK
{
    /**
    * Defines values for the role parameter of the GenerateToken method of the OpenTok class.
    */
    public enum Role
    {
        /**
         * A publisher can publish streams, subscribe to streams, and signal. (This is the default
         * value if you do not set a role when calling GenerateToken method of the OpenTok class.
         */
        PUBLISHER,
        /**
         *   A subscriber can only subscribe to streams.
         */
        SUBSCRIBER,
        /**
         * In addition to the privileges granted to a publisher, in clients using the OpenTok.js
         * library, a moderator can call the <code>forceUnpublish()</code> and
         * <code>forceDisconnect()</code> methods of the Session object.
         */
        MODERATOR
    }

    /**
     * For internal use.
     */
    static class RoleExtensions
    {
        public static string ToString(this Role role)
        {
            return role.ToString().ToLowerInvariant();
        }
    }
}
