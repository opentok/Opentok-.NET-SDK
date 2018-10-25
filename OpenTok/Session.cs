using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

using OpenTokSDK.Util;
using OpenTokSDK.Exception;

namespace OpenTokSDK
{
    /**
     * Defines values for the mediaMode parameter of the CreateSession() method of the
     * OpenTok class.
     */
    public enum MediaMode
    {
        /**
         * The session will transmit streams using the OpenTok Media Router.
         */
        ROUTED,
        /**
         * The session will attempt to transmit streams directly between clients. If two clients
         * cannot send and receive each others' streams, due to firewalls on the clients' networks,
         * their streams will be relayed using the OpenTok TURN Server.
         */
        RELAYED
    }

    /**
     * Defines values for the archiveMode property of the Session object. You also use these values
     * for the archiveMode parameter of the OpenTok.CreateSession() method.
     */
    public enum ArchiveMode
    {
        /**
         * The session is not archived automatically. To archive the session, you can call the
         * OpenTok.StartArchive() method.
         */
        MANUAL,
        /**
         * The session is archived automatically (as soon as there are clients publishing streams
         * to the session).
         */
        ALWAYS
    }

    /**
    * Represents an OpenTok session. Use the CreateSession() method of the OpenTok class to create
    * an OpenTok session. Use the Id property of the Session object to get the session ID.
    */
    public class Session
    {
        /**
         * The session ID, which uniquely identifies the session.
         */
        public string Id { get; set; }

        /**
         * Your OpenTok API key.
         */
        public int ApiKey { get; private set; }

        /**
         * Your OpenTok API secret.
         */
        public string ApiSecret { get; private set; }

        /**
         * The location hint IP address.
         */
        public string Location { get; set; }

        /**
         * Defines whether the session will transmit streams using the OpenTok Media Router
         * (<code>MediaMode.ROUTED</code>) or attempt to transmit streams directly between clients
         * (<code>MediaMode.RELAYED</code>).
         */
        public MediaMode MediaMode { get; private set; }

        /**
         * Defines whether the session is automatically archived (<code>ArchiveMode.ALWAYS</code>)
         * or not (<code>ArchiveMode.MANUAL</code>).
         */
        public ArchiveMode ArchiveMode { get; private set; }

        private const int MAX_CONNECTION_DATA_LENGTH = 1000;

        internal Session(string sessionId, int apiKey, string apiSecret)
        {
            this.Id = sessionId;
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;
        }

        internal Session(string sessionId, int apiKey, string apiSecret, string location, MediaMode mediaMode, ArchiveMode archiveMode)
        {
            this.Id = sessionId;
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;
            this.Location = location;
            this.MediaMode = mediaMode;
            this.ArchiveMode = archiveMode;
        }


        /**
         * Creates a token for connecting to an OpenTok session. In order to authenticate a user
         * connecting to an OpenTok session that user must pass an authentication token along with
         * the API key.
         *
         * @param role The role for the token. Valid values are defined in the Role enum:
         * <ul>
         *   <li> <code>Role.SUBSCRIBER</code> &mdash; A subscriber can only subscribe to
         *     streams.</li>
         *
         *   <li> <code>Role.PUBLISHER</code> &mdash; A publisher can publish streams, subscribe to
         *      streams, and signal. (This is the default value if you do not specify a role.)</li>
         *
         *   <li> <code>Role.MODERATOR</code> &mdash; In addition to the privileges granted to a
         *     publisher, in clients using the OpenTok.js library, a moderator can call the
         *     <code>forceUnpublish()</code> and <code>forceDisconnect()</code> method of the
         *     Session object.</li>
         * </ul>
         *
         * @param expireTime The expiration time of the token, in seconds since the UNIX epoch.
         * Pass in 0 to use the default expiration time of 24 hours after the token creation time.
         * The maximum expiration time is 30 days after the creation time.
         *
         * @param data A string containing connection metadata describing the end-user. For example,
         * you can pass the user ID, name, or other data describing the end-user. The length of the
         * string is limited to 1000 characters. This data cannot be updated once it is set.
         *
         * @return The token string.
         */
        public string GenerateToken(Role role = Role.PUBLISHER, double expireTime = 0, string data = null, List <string> initialLayoutClassList = null)
        {
            double createTime = OpenTokUtils.GetCurrentUnixTimeStamp();
            int nonce = OpenTokUtils.GetRandomNumber();

            string dataString = BuildDataString(role, expireTime, data, createTime, nonce, initialLayoutClassList);
            return BuildTokenString(dataString);
        }

        private string BuildTokenString(string dataString)
        {
            string signature = OpenTokUtils.EncodeHMAC(dataString, this.ApiSecret);

            StringBuilder innerBuilder = new StringBuilder();
            innerBuilder.Append(string.Format("partner_id={0}", this.ApiKey));
            innerBuilder.Append(string.Format("&sig={0}:{1}", signature, dataString));

            byte[] innerBuilderBytes = Encoding.UTF8.GetBytes(innerBuilder.ToString());
            return "T1==" + Convert.ToBase64String(innerBuilderBytes);
        }

        private string BuildDataString(Role role, double expireTime, string connectionData, double createTime, int nonce, List<string> initialLayoutClassList)
        {
            StringBuilder dataStringBuilder = new StringBuilder();

            dataStringBuilder.Append(string.Format("session_id={0}", this.Id));
            dataStringBuilder.Append(string.Format("&create_time={0}", (long)createTime));
            dataStringBuilder.Append(string.Format("&nonce={0}", nonce));
            dataStringBuilder.Append(string.Format("&role={0}", role.ToString()));

            if (initialLayoutClassList != null)
            {
                dataStringBuilder.Append(string.Format("&initial_layout_class_list={0}", String.Join(" ", initialLayoutClassList)));
            }

            if (CheckExpireTime(expireTime, createTime))
            {
                dataStringBuilder.Append(string.Format("&expire_time={0}", (long)expireTime));
            }

            if (CheckConnectionData(connectionData))
            {
                dataStringBuilder.Append(string.Format("&connection_data={0}", HttpUtility.UrlEncode(connectionData)));
            }

            return dataStringBuilder.ToString();
        }

        private bool CheckExpireTime(double expireTime, double createTime)
        {
            if (expireTime == 0)
            {
                return false;
            }
            else if (expireTime > createTime && expireTime <= OpenTokUtils.GetCurrentUnixTimeStamp() + 2592000)
            {
                return true;
            }
            else
            {
                throw new OpenTokArgumentException("Invalid expiration time for token " + expireTime + ". Expiration time " +
                                                        " has to be positive and less than 30 days");
            }
        }

        private bool CheckConnectionData(string connectionData)
        {
            if (String.IsNullOrEmpty(connectionData))
            {
                return false;
            }
            else if (connectionData.Length <= MAX_CONNECTION_DATA_LENGTH)
            {
                return true;
            }
            else
            {
                throw new OpenTokArgumentException("Invalid connection data, it cannot be longer than 1000 characters");
            }
        }
    }
}
