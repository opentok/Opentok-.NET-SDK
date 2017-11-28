using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTokSDK.Exception;
using OpenTokSDK.Util;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenTokSDK
{
    /**
    * Contains methods for creating OpenTok sessions, generating tokens, and working with archives.
    * <p>
    * To create a new OpenTok object, call the OpenTok() constructor with your OpenTok API key
    * and the API secret for your <a href="https://tokbox.com/account">OpenTok project</a>.
    * Do not publicly share your API secret. You will use it with the OpenTok constructor
    * (only on your web server) to create OpenTok sessions.
    */
    public class OpenTok
    {

        /** The OpenTok API key passed into the OpenTok() constructor. */
        public int ApiKey { get; private set; }
        /** The OpenTok API secret passed into the OpenTok() constructor. */
        public string ApiSecret { get; private set; }
        private string OpenTokServer { get; set; }
        /** For internal use. */
        public HttpClient Client { private get; set; }

        /**
         * Enables writing request/response details to console.
         * Don't use in a production environment.
         */
        private bool _debug;
        public bool Debug {
          get { return _debug; }
          set
          {
            _debug = value;
            Client.debug = _debug;
          }
        }

        /**
        * Creates an OpenTok object.
        *
        * @param apiKey Your OpenTok API key. (See the
        * <a href="https://tokbox.com/account">TokBox account page</a>.)
        * @param apiSecret Your OpenTok API secret. (See the
        * <a href="https://tokbox.com/account">TokBox account page</a>.)
        */
        public OpenTok(int apiKey, string apiSecret)
        {
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;
            this.OpenTokServer = "https://api.opentok.com";
            Client = new HttpClient(apiKey, apiSecret, this.OpenTokServer);
            this.Debug = false;
        }

        /**
         * For TokBox internal use.
         */
        public OpenTok(int apiKey, string apiSecret, string apiUrl)
        {
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;
            this.OpenTokServer = apiUrl;
            Client = new HttpClient(apiKey, apiSecret, this.OpenTokServer);
            this.Debug = false;
        }

        /**
         * Creates a new OpenTok session.
         * <p>
         * OpenTok sessions do not expire. However, authentication tokens do expire (see the
         * generateToken() method). Also note that sessions cannot explicitly be destroyed.
         * <p>
         * A session ID string can be up to 255 characters long.
         * <p>
         * Calling this method results in an OpenTokException in the event of an error.
         * Check the error message for details.
         *
         * You can also create a session using the
         * <a href="http://www.tokbox.com/opentok/api/#session_id_production">OpenTok
         * REST API</a> or by logging in to your
         * <a href="https://tokbox.com/account">TokBox account</a>.
         *
         * @param location (String) An IP address that the OpenTok servers will use to
         * situate the session in its global network. If you do not set a location hint,
         * the OpenTok servers will be based on the first client connecting to the session.
         *
         * @param mediaMode Whether the session will transmit streams using the
         * OpenTok Media Router (<code>MediaMode.ROUTED</code>) or not
         * (<code>MediaMode.RELAYED</code>). By default, the setting is
         * <code>MediaMode.RELAYED</code>.
         * <p>
         * With the <code>mediaMode</code> parameter set to <code>MediaMode.RELAYED</code>, the
         * session will attempt to transmit streams directly between clients. If clients cannot
         * connect due to firewall restrictions, the session uses the OpenTok TURN server to relay
         * streams.
         * <p>
         * The <a href="https://tokbox.com/opentok/tutorials/create-session/#media-mode"
         * target="_top">OpenTok Media Router</a> provides the following benefits:
         *
         * <ul>
         *   <li>The OpenTok Media Router can decrease bandwidth usage in multiparty sessions.
         *       (When the <code>mediaMode</code> parameter is set to
         *       <code>MediaMode.ROUTED</code>, each client must send a separate audio-video stream
         *      to each client subscribing to it.)</li>
         *   <li>The OpenTok Media Router can improve the quality of the user experience through
         *     <a href="https://tokbox.com/platform/fallback" target="_top">audio fallback and video
         *     recovery</a>. With these features, if a client's connectivity degrades to a degree
         *     that it does not support video for a stream it's subscribing to, the video is dropped
         *     on that client (without affecting other clients), and the client receives audio only.
         *     If the client's connectivity improves, the video returns.</li>
         *   <li>The OpenTok Media Router supports the
         *     <a href="http://tokbox.com/opentok/tutorials/archiving" target="_top">archiving</a>
         *     feature, which lets you record, save, and retrieve OpenTok sessions.</li>
         * </ul>
         *
         * @param archiveMode Whether the session is automatically archived
         * (<code>ArchiveMode.ALWAYS</code>) or not (<code>ArchiveMode.MANUAL</code>). By default,
         * the setting is <code>ArchiveMode.MANUAL</code>, and you must call the
         * StartArchive() method of the OpenTok object to start archiving. To archive the session
         * (either automatically or not), you must set the mediaMode parameter to
         * <code>MediaMode.ROUTED</code>.
         *
         * @return A Session object representing the new session. The <code>Id</code> property of
         * the Session is the session ID, which uniquely identifies the session. You will use
         * this session ID in the client SDKs to identify the session. For example, when using the
         * OpenTok.js library, use the session ID when calling the
         * <a href="http://tokbox.com/opentok/libraries/client/js/reference/OT.html#initSession">
         * OT.initSession()</a> method (to initialize an OpenTok session).
         */
        public Session CreateSession(string location = "", MediaMode mediaMode = MediaMode.RELAYED, ArchiveMode archiveMode = ArchiveMode.MANUAL)
        {

            if (!OpenTokUtils.TestIpAddress(location))
            {
                throw new OpenTokArgumentException(string.Format("Location {0} is not a valid IP address", location));
            }

            if (archiveMode == ArchiveMode.ALWAYS && mediaMode != MediaMode.ROUTED)
            {
                throw new OpenTokArgumentException("A session with always archive mode must also have the routed media mode.");
            }

            string preference = (mediaMode == MediaMode.RELAYED) ? "enabled" : "disabled";

            var headers = new Dictionary<string, string> { { "Content-type", "application/x-www-form-urlencoded" } };
            var data = new Dictionary<string, object>
            {
                {"location", location},
                {"p2p.preference", preference},
                {"archiveMode", archiveMode.ToString().ToLowerInvariant()}
            };

            var response = Client.Post("session/create", headers, data);
            var xmlDoc = Client.ReadXmlResponse(response);

            if (xmlDoc.GetElementsByTagName("session_id").Count == 0)
            {
                throw new OpenTokWebException("Session could not be provided. Are ApiKey and ApiSecret correctly set?");
            }
            var sessionId = xmlDoc.GetElementsByTagName("session_id")[0].ChildNodes[0].Value;
            var apiKey = Convert.ToInt32(xmlDoc.GetElementsByTagName("partner_id")[0].ChildNodes[0].Value);
            return new Session(sessionId, apiKey, ApiSecret, location, mediaMode, archiveMode);
        }

        /**
         * Creates a token for connecting to an OpenTok session. In order to authenticate a user
         * connecting to an OpenTok session, the client passes a token when connecting to the
         * session.
         * <p>
         * For testing, you can also generate test tokens by logging in to your
         * <a href="https://tokbox.com/account">TokBox account</a>.
         *
         * @param sessionId The session ID corresponding to the session to which the user will
         * connect.
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
        public string GenerateToken(string sessionId, Role role = Role.PUBLISHER, double expireTime = 0, string data = null)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("Session id cannot be empty or null");
            }

            if (!OpenTokUtils.ValidateSession(sessionId))
            {
                throw new OpenTokArgumentException("Invalid Session id " + sessionId);
            }

            Session session = new Session(sessionId, this.ApiKey, this.ApiSecret);
            return session.GenerateToken(role, expireTime, data);
        }

        /**
         * Starts archiving an OpenTok session.
         *
         * <p>
         * Clients must be actively connected to the OpenTok session for you to successfully start
         * recording an archive.
         * <p>
         * You can only record one archive at a time for a given session. You can only record
         * archives of sessions that uses the OpenTok Media Router (sessions with the media mode set
         * to routed); you cannot archive sessions with the media mode set to relayed.
         * <p>
         * Note that you can have the session be automatically archived by setting the archiveMode
         * parameter of the OpenTok.CreateSession() method to ArchiveMode.ALWAYS.
         *
         * @param sessionId The session ID of the OpenTok session to archive.
         *
         * @param name The name of the archive. You can use this name to identify the archive. It is
         * a property of the Archive object, and it is a property of archive-related events in the
         * OpenTok client libraries.
         *
         * @param hasVideo Whether the archive will record video (true) or not (false). The default
         * value is true (video is recorded). If you set both <code>hasAudio</code> and
         * <code>hasVideo</code> to false, the call to the <code>StartArchive()</code> method
         * results in an error.
         *
         * @param hasAudio Whether the archive will record audio (true) or not (false). The default
         * value is true (audio is recorded). If you set both <code>hasAudio</code> and
         * <code>hasVideo</code> to false, the call to the <code>StartArchive()</code> method
         * results in an error.
         *
         * @param outputMode Whether all streams in the archive are recorded to a single file
         * (<code>OutputMode.COMPOSED</code>, the default) or to individual files
         * (<code>OutputMode.INDIVIDUAL</code>).
         *
         * @return The Archive object. This object includes properties defining the archive,
         * including the archive ID.
         */
        public Archive StartArchive(string sessionId, string name = "", bool hasVideo = true, bool hasAudio = true, OutputMode outputMode = OutputMode.COMPOSED)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("Session not valid");
            }
            string url = string.Format("v2/project/{0}/archive", this.ApiKey);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            var data = new Dictionary<string, object>() { { "sessionId", sessionId }, { "name", name }, { "hasVideo", hasVideo }, { "hasAudio", hasAudio }, { "outputMode", outputMode.ToString().ToLowerInvariant() } };
            string response = Client.Post(url, headers, data);
            return OpenTokUtils.GenerateArchive(response, ApiKey, ApiSecret, OpenTokServer);
        }

        /**
         * Stops an OpenTok archive that is being recorded.
         * <p>
         * Archives automatically stop recording after 120 minutes or when all clients have
         * disconnected from the session being archived.
         *
         * @param archiveId The archive ID of the archive you want to stop recording.
         * @return The Archive object corresponding to the archive being STOPPED.
         */
        public Archive StopArchive(string archiveId)
        {
            string url = string.Format("v2/project/{0}/archive/{1}/stop", this.ApiKey, archiveId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };

            string response = Client.Post(url, headers, new Dictionary<string, object>());
            return JsonConvert.DeserializeObject<Archive>(response);
        }

        /**
         * Returns a List of Archive objects, representing archives that are both
         * both completed and in-progress, for your API key. This list is limited to 1000 archives
         * starting with the first archive recorded. For a specific range of archives, call
         * listArchives(int offset, int count).
         *
         * @return A List of Archive objects.
         */
        public ArchiveList ListArchives()
        {
            return ListArchives(0, 0);
        }

        /**
         * Returns a List of Archive objects, representing archives that are both
         * both completed and in-progress, for your API key.
         *
         * @param offset The index offset of the first archive. 0 is offset of the most recently
         * started archive. 1 is the offset of the archive that started prior to the most recent
         * archive.
         *
         * @param count The number of archives to be returned. The maximum number of archives
         * returned is 1000.
         *
         * @return A List of Archive objects.
         */
        public ArchiveList ListArchives(int offset, int count)
        {
            if (count < 0)
            {
                throw new OpenTokArgumentException("count cannot be smaller than 1");
            }
            string url = string.Format("v2/project/{0}/archive?offset={1}", this.ApiKey, offset);
            if (count > 0)
            {
                url = string.Format("{0}&count={1}", url, count);
            }
            string response = Client.Get(url);
            JObject archives = JObject.Parse(response);
            JArray archiveArray = (JArray)archives["items"];
            ArchiveList archiveList = new ArchiveList(archiveArray.ToObject<List<Archive>>(), (int)archives["count"]);
            return archiveList;
        }

        /**
         * Gets an Archive object for the given archive ID.
         *
         * @param archiveId The archive ID.
         * @return The Archive object.
         */
        public Archive GetArchive(string archiveId)
        {
            string url = string.Format("v2/project/{0}/archive/{1}", this.ApiKey, archiveId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            string response = Client.Get(url); ;
            return JsonConvert.DeserializeObject<Archive>(response);
        }

        /**
         * Deletes an OpenTok archive.
         * <p>
         * You can only delete an archive which has a status of "available" or "uploaded". Deleting
         * an archive removes its record from the list of archives. For an "available" archive, it
         * also removes the archive file, making it unavailable for download.
         *
         * @param archiveId The archive ID of the archive you want to delete.
         */
        public void DeleteArchive(string archiveId)
        {
            string url = string.Format("v2/project/{0}/archive/{1}", this.ApiKey, archiveId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            Client.Delete(url, headers, new Dictionary<string, object>());
        }
    }
}
