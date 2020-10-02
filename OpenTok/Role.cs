using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK
{
    /// <summary>
    /// Defines values for the role parameter of the GenerateToken method of the OpenTok class.
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// A publisher can publish streams, subscribe to streams, and signal. (This is the default
        /// value if you do not set a role when calling GenerateToken method of the OpenTok class.
        /// </summary>
        PUBLISHER,
        /// <summary>
        /// A subscriber can only subscribe to streams.
        /// </summary>
        SUBSCRIBER,
        /// <summary>
        /// In addition to the privileges granted to a publisher, in clients using the OpenTok.js
        /// library, a moderator can call the forceUnpublish() and forceDisconnect() methods of the Session object.
        /// </summary>
        MODERATOR
    }

    /// <summary>
    /// For internal use.
    /// </summary>
    internal static class RoleExtensions
    {
        public static string ToString(this Role role)
        {
            return role.ToString().ToLowerInvariant();
        }
    }
}
