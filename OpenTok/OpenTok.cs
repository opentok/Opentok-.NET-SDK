using System;
using System.Collections.Generic;
using System.Linq;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace OpenTokSDK
{
    /// <summary>
    /// Contains methods for creating OpenTok sessions, generating tokens, and working with archives.
    /// <para>
    /// To create a new OpenTok object, call the OpenTok() constructor with your OpenTok API key
    /// and the API secret for your <a href="https://tokbox.com/account">OpenTok project</a>.
    /// Do not publicly share your API secret. You will use it with the OpenTok constructor
    /// (only on your web server) to create OpenTok sessions.
    /// </para>
    /// </summary>
    public class OpenTok
    {
        /// <summary>
        /// The OpenTok API key passed into the OpenTok() constructor.
        /// </summary>
        public int ApiKey { get; private set; }
        /// <summary>
        /// The OpenTok API secret passed into the OpenTok() constructor.
        /// </summary>
        public string ApiSecret { get; private set; }
        private string OpenTokServer { get; set; }
        /// <summary>
        /// For internal use
        /// </summary>
        public HttpClient Client { private get; set; }

        private bool _debug;
        /// <summary>
        /// Enables writing request/response details to console.
        /// Don't use in a production environment.
        /// </summary>
        public bool Debug
        {
            get { return _debug; }
            set
            {
                _debug = value;
                Client.debug = _debug;
            }
        }

        /// <summary>
        /// Creates an OpenTok object.
        /// </summary>
        /// <param name="apiKey">Your OpenTok API key. (See the <a href="https://tokbox.com/account" > TokBox account page</a></param>
        /// <param name="apiSecret">Your OpenTok API secret. (See the <a href="https://tokbox.com/account" > TokBox account page</a></param>
        public OpenTok(int apiKey, string apiSecret)
        {
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;
            this.OpenTokServer = "https://api.opentok.com";
            Client = new HttpClient(apiKey, apiSecret, this.OpenTokServer);
            this.Debug = false;
        }

        /// <summary>
        /// For TokBox internal use.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="apiUrl"></param>
        public OpenTok(int apiKey, string apiSecret, string apiUrl)
        {
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;
            this.OpenTokServer = apiUrl;
            Client = new HttpClient(apiKey, apiSecret, this.OpenTokServer);
            this.Debug = false;
        }

        /// <summary>
        /// Creates a new OpenTok session.
        /// <para>
        /// OpenTok sessions do not expire. However, authentication tokens do expire (see the
        /// generateToken() method). Also note that sessions cannot explicitly be destroyed.
        /// </para>
        /// <para>
        /// A session ID string can be up to 255 characters long.
        /// </para>
        /// <para>
        /// Calling this method results in an OpenTokException in the event of an error.
        /// Check the error message for details.
        /// 
        /// You can also create a session using the
        /// <a href="http://www.tokbox.com/opentok/api/#session_id_production">OpenTok
        /// REST API</a> or by logging in to your
        /// <a href="https://tokbox.com/account">TokBox account</a>.
        /// </para>
        /// </summary>
        /// <param name="location">
        /// An IP address that the OpenTok servers will use to situate the session in its
        /// global network. If you do not set a location hint, the OpenTok servers will be
        /// based on the first client connecting to the session.
        /// </param>
        /// <param name="mediaMode">
        /// Whether the session will transmit streams using the OpenTok Media Router
        /// (<see cref="MediaMode.ROUTED"/>) or not (<see cref="MediaMode.RELAYED"/>).
        /// By default, the setting is <see cref="MediaMode.RELAYED"/>.
        /// <para>
        /// With the parameter set to <see cref="MediaMode.RELAYED"/>, the session will
        /// attempt to transmit streams directly between clients. If clients cannot connect
        /// due to firewall restrictions, the session uses the OpenTok TURN server to relay streams.
        /// </para>
        /// <para>
        /// The <a href="https://tokbox.com/opentok/tutorials/create-session/#media-mode">
        /// OpenTok Media Router</a> provides the following benefits:
        /// - The OpenTok Media Router can decrease bandwidth usage in multiparty sessions.
        ///   (When the <paramref name="mediaMode"/> parameter is set to <see cref="MediaMode.ROUTED"/>,
        ///   each client must send a separate audio-video stream to each client subscribing to it.)
        /// - The OpenTok Media Router can improve the quality of the user experience through
        ///   <a href="https://tokbox.com/platform/fallback">audio fallback and video recovery</a>
        ///   With these features, if a client's connectivity degrades to a degree that it does not
        ///   support video for a stream it's subscribing to, the video is dropped on that client
        ///   (without affecting other clients), and the client receives audio only. If the client's
        ///   connectivity improves, the video returns.
        /// - The OpenTok Media Router supports the <a href="http://tokbox.com/opentok/tutorials/archiving">archiving</a>
        ///   feature, which lets you record, save, and retrieve OpenTok sessions.
        /// </para>
        /// </param>
        /// <param name="archiveMode">
        /// Whether the session is automatically archived (<see cref="ArchiveMode.ALWAYS"/>) or not
        /// (<see cref="ArchiveMode.MANUAL"/>). By default, the setting is <see cref="ArchiveMode.MANUAL"/>
        /// and you must call the <see cref="StartArchive"/> method of the OpenTok object to start archiving.
        /// To archive the session (either automatically or not), you must set the mediaMode parameter to
        /// <see cref="MediaMode.ROUTED"/>
        /// </param>
        /// <returns>
        /// A Session object representing the new session. The <see cref="Session.Id"/> property of the
        /// <see cref="Session"/> is the session ID, which uniquely identifies the session. You will use
        /// this session ID in the client SDKs to identify the session. For example, when using the
        /// OpenTok.js library, use the session ID when calling the
        /// <a href="http://tokbox.com/opentok/libraries/client/js/reference/OT.html#initSession">OT.initSession()</a>
        /// method (to initialize an OpenTok session).
        /// </returns>
        public Session CreateSession(string location = "", MediaMode mediaMode = MediaMode.RELAYED, ArchiveMode archiveMode = ArchiveMode.MANUAL)
        {
            try
            {
                var task = CreateSessionAsync(location, mediaMode, archiveMode);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Creates a new OpenTok session.
        /// <para>
        /// OpenTok sessions do not expire. However, authentication tokens do expire (see the
        /// generateToken() method). Also note that sessions cannot explicitly be destroyed.
        /// </para>
        /// <para>
        /// A session ID string can be up to 255 characters long.
        /// </para>
        /// <para>
        /// Calling this method results in an OpenTokException in the event of an error.
        /// Check the error message for details.
        /// 
        /// You can also create a session using the
        /// <a href="http://www.tokbox.com/opentok/api/#session_id_production">OpenTok
        /// REST API</a> or by logging in to your
        /// <a href="https://tokbox.com/account">TokBox account</a>.
        /// </para>
        /// </summary>
        /// <param name="location">
        /// An IP address that the OpenTok servers will use to situate the session in its
        /// global network. If you do not set a location hint, the OpenTok servers will be
        /// based on the first client connecting to the session.
        /// </param>
        /// <param name="mediaMode">
        /// Whether the session will transmit streams using the OpenTok Media Router
        /// (<see cref="MediaMode.ROUTED"/>) or not (<see cref="MediaMode.RELAYED"/>).
        /// By default, the setting is <see cref="MediaMode.RELAYED"/>.
        /// <para>
        /// With the parameter set to <see cref="MediaMode.RELAYED"/>, the session will
        /// attempt to transmit streams directly between clients. If clients cannot connect
        /// due to firewall restrictions, the session uses the OpenTok TURN server to relay streams.
        /// </para>
        /// <para>
        /// The <a href="https://tokbox.com/opentok/tutorials/create-session/#media-mode">
        /// OpenTok Media Router</a> provides the following benefits:
        /// - The OpenTok Media Router can decrease bandwidth usage in multiparty sessions.
        ///   (When the <paramref name="mediaMode"/> parameter is set to <see cref="MediaMode.ROUTED"/>,
        ///   each client must send a separate audio-video stream to each client subscribing to it.)
        /// - The OpenTok Media Router can improve the quality of the user experience through
        ///   <a href="https://tokbox.com/platform/fallback">audio fallback and video recovery</a>
        ///   With these features, if a client's connectivity degrades to a degree that it does not
        ///   support video for a stream it's subscribing to, the video is dropped on that client
        ///   (without affecting other clients), and the client receives audio only. If the client's
        ///   connectivity improves, the video returns.
        /// - The OpenTok Media Router supports the <a href="http://tokbox.com/opentok/tutorials/archiving">archiving</a>
        ///   feature, which lets you record, save, and retrieve OpenTok sessions.
        /// </para>
        /// </param>
        /// <param name="archiveMode">
        /// Whether the session is automatically archived (<see cref="ArchiveMode.ALWAYS"/>) or not
        /// (<see cref="ArchiveMode.MANUAL"/>). By default, the setting is <see cref="ArchiveMode.MANUAL"/>
        /// and you must call the <see cref="StartArchive"/> method of the OpenTok object to start archiving.
        /// To archive the session (either automatically or not), you must set the mediaMode parameter to
        /// <see cref="MediaMode.ROUTED"/>
        /// </param>
        /// <returns>
        /// A Session object representing the new session. The <see cref="Session.Id"/> property of the
        /// <see cref="Session"/> is the session ID, which uniquely identifies the session. You will use
        /// this session ID in the client SDKs to identify the session. For example, when using the
        /// OpenTok.js library, use the session ID when calling the
        /// <a href="http://tokbox.com/opentok/libraries/client/js/reference/OT.html#initSession">OT.initSession()</a>
        /// method (to initialize an OpenTok session).
        /// </returns>
        public async Task<Session> CreateSessionAsync(string location = "", MediaMode mediaMode = MediaMode.RELAYED, ArchiveMode archiveMode = ArchiveMode.MANUAL)
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

            var response = await Client.PostAsync("session/create", headers, data);
            var xmlDoc = Client.ReadXmlResponse(response);

            if (xmlDoc.GetElementsByTagName("session_id").Count == 0)
            {
                throw new OpenTokWebException("Session could not be provided. Are ApiKey and ApiSecret correctly set?");
            }
            var sessionId = xmlDoc.GetElementsByTagName("session_id")[0].ChildNodes[0].Value;
            var apiKey = Convert.ToInt32(xmlDoc.GetElementsByTagName("partner_id")[0].ChildNodes[0].Value);
            return new Session(sessionId, apiKey, ApiSecret, location, mediaMode, archiveMode);
        }

        /// <summary>
        /// Creates a token for connecting to an OpenTok session. In order to authenticate a user
        /// connecting to an OpenTok session, the client passes a token when connecting to the session.
        /// <para>
        /// For testing, you can also generate test tokens by logging in to your
        /// <a href="https://tokbox.com/account">TokBox account</a>.
        /// </para>
        /// </summary>
        /// <param name="sessionId">
        /// The session ID corresponding to the session to which the user will connect.
        /// </param>
        /// <param name="role">
        /// The role for the token. Valid values are defined in the Role enum:
        /// - <see cref="Role.SUBSCRIBER"/> (A subscriber can only subscribe to streams)
        /// - <see cref="Role.PUBLISHER"/> (A publisher can publish streams, subscribe to streams, and signal.
        ///   (This is the default value if you do not specify a role.))
        /// - <see cref="Role.MODERATOR"/> (In addition to the privileges granted to a publisher, in clients
        ///   using the OpenTok.js library, a moderator can call the forceUnpublish() and forceDisconnect()
        ///   method of the <see cref="Session"/> object.
        /// </param>
        /// <param name="expireTime">
        /// The expiration time of the token, in seconds since the UNIX epoch. Pass in 0 to use the default
        /// expiration time of 24 hours after the token creation time. The maximum expiration time is 30 days
        /// after the creation time.
        /// </param>
        /// <param name="data">
        /// A string containing connection metadata describing the end-user. For example, you can pass the
        /// user ID, name, or other data describing the end-user. The length of the string is limited to 1000
        /// characters. This data cannot be updated once it is set.
        /// </param>
        /// <param name="initialLayoutClassList">
        /// A list of strings values containing the initial layout for the stream.
        /// </param>
        /// <returns></returns>
        public string GenerateToken(string sessionId, Role role = Role.PUBLISHER, double expireTime = 0, string data = null, List<string> initialLayoutClassList = null)
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
            return session.GenerateToken(role, expireTime, data, initialLayoutClassList);
        }

        /// <summary>
        /// Starts archiving an OpenTok session.
        /// <para>
        /// Clients must be actively connected to the OpenTok session for you to successfully start
        /// recording an archive.
        /// </para>
        /// <para>
        /// You can only record one archive at a time for a given session. You can only record
        /// archives of sessions that uses the OpenTok Media Router (sessions with the media mode set
        /// to routed); you cannot archive sessions with the media mode set to relayed.
        /// </para>
        /// <para>
        /// Note that you can have the session be automatically archived by setting the archiveMode
        /// parameter of the <see cref="CreateSession"/> method to <see cref="ArchiveMode.ALWAYS"/>.
        /// </para>
        /// </summary>
        /// <param name="sessionId">
        /// The session ID of the OpenTok session to archive.
        /// </param>
        /// <param name="name">
        /// The name of the archive. You can use this name to identify the archive. It is a property
        /// of the Archive object, and it is a property of archive-related events in the OpenTok client
        /// libraries.
        /// </param>
        /// <param name="hasVideo">
        /// Whether the archive will record video (true) or not (false). The default value is true
        /// (video is recorded). If you set both <paramref name="hasAudio"/> and <paramref name="hasVideo"/>
        /// to false, the call to the <see cref="StartArchive"/> method results in an error.
        /// </param>
        /// <param name="hasAudio">
        /// Whether the archive will record audio (true) or not (false). The default value is true
        /// (audio is recorded). If you set both <paramref name="hasAudio"/> and <paramref name="hasVideo"/>
        /// to false, the call to the <see cref="StartArchive"/> method results in an error.
        /// </param>
        /// <param name="outputMode">
        /// Whether all streams in the archive are recorded to a single file (<see cref="OutputMode.COMPOSED"/>,
        /// the default) or to individual files (<see cref="OutputMode.INDIVIDUAL"/>).
        /// </param>
        /// <param name="resolution">
        /// The resolution for the archive. The default for <see cref="OutputMode.COMPOSED"/> is "640x480".
        /// You cannot specify the resolution for <see cref="OutputMode.INDIVIDUAL"/>.
        /// </param>
        /// <param name="layout">
        /// The layout that you want to use for your archive. If type is set to <see cref="LayoutType.custom"/>
        /// you must provide a StyleSheet string to Vonage how to layout your archive.
        /// </param>
        /// <returns>
        /// The Archive object. This object includes properties defining the archive, including the archive ID.
        /// </returns>
        /// <summary>
        /// Starts archiving an OpenTok session.
        /// <para>
        /// Clients must be actively connected to the OpenTok session for you to successfully start
        /// recording an archive.
        /// </para>
        /// <para>
        /// You can only record one archive at a time for a given session. You can only record
        /// archives of sessions that uses the OpenTok Media Router (sessions with the media mode set
        /// to routed); you cannot archive sessions with the media mode set to relayed.
        /// </para>
        /// <para>
        /// Note that you can have the session be automatically archived by setting the archiveMode
        /// parameter of the <see cref="CreateSession"/> method to <see cref="ArchiveMode.ALWAYS"/>.
        /// </para>
        /// </summary>
        /// <param name="sessionId">
        /// The session ID of the OpenTok session to archive.
        /// </param>
        /// <param name="name">
        /// The name of the archive. You can use this name to identify the archive. It is a property
        /// of the Archive object, and it is a property of archive-related events in the OpenTok client
        /// libraries.
        /// </param>
        /// <param name="hasVideo">
        /// Whether the archive will record video (true) or not (false). The default value is true
        /// (video is recorded). If you set both <paramref name="hasAudio"/> and <paramref name="hasVideo"/>
        /// to false, the call to the <see cref="StartArchive"/> method results in an error.
        /// </param>
        /// <param name="hasAudio">
        /// Whether the archive will record audio (true) or not (false). The default value is true
        /// (audio is recorded). If you set both <paramref name="hasAudio"/> and <paramref name="hasVideo"/>
        /// to false, the call to the <see cref="StartArchive"/> method results in an error.
        /// </param>
        /// <param name="outputMode">
        /// Whether all streams in the archive are recorded to a single file (<see cref="OutputMode.COMPOSED"/>,
        /// the default) or to individual files (<see cref="OutputMode.INDIVIDUAL"/>).
        /// </param>
        /// <param name="resolution">
        /// The resolution for the archive. The default for <see cref="OutputMode.COMPOSED"/> is "640x480".
        /// You cannot specify the resolution for <see cref="OutputMode.INDIVIDUAL"/>.
        /// </param>
        /// <param name="layout">
        /// The layout that you want to use for your archive. If type is set to <see cref="LayoutType.custom"/>
        /// you must provide a StyleSheet string to Vonage how to layout your archive.
        /// </param>
        /// <returns>
        /// The Archive object. This object includes properties defining the archive, including the archive ID.
        /// </returns>
        public Archive StartArchive(string sessionId, string name = "", bool hasVideo = true, bool hasAudio = true, OutputMode outputMode = OutputMode.COMPOSED, string resolution = null, ArchiveLayout layout = null)
        {
            try
            {
                var task = StartArchiveAsync(sessionId, name, hasVideo, hasAudio, outputMode, resolution, layout);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Starts archiving an OpenTok session.
        /// <para>
        /// Clients must be actively connected to the OpenTok session for you to successfully start
        /// recording an archive.
        /// </para>
        /// <para>
        /// You can only record one archive at a time for a given session. You can only record
        /// archives of sessions that uses the OpenTok Media Router (sessions with the media mode set
        /// to routed); you cannot archive sessions with the media mode set to relayed.
        /// </para>
        /// <para>
        /// Note that you can have the session be automatically archived by setting the archiveMode
        /// parameter of the <see cref="CreateSession"/> method to <see cref="ArchiveMode.ALWAYS"/>.
        /// </para>
        /// </summary>
        /// <param name="sessionId">
        /// The session ID of the OpenTok session to archive.
        /// </param>
        /// <param name="name">
        /// The name of the archive. You can use this name to identify the archive. It is a property
        /// of the Archive object, and it is a property of archive-related events in the OpenTok client
        /// libraries.
        /// </param>
        /// <param name="hasVideo">
        /// Whether the archive will record video (true) or not (false). The default value is true
        /// (video is recorded). If you set both <paramref name="hasAudio"/> and <paramref name="hasVideo"/>
        /// to false, the call to the <see cref="StartArchive"/> method results in an error.
        /// </param>
        /// <param name="hasAudio">
        /// Whether the archive will record audio (true) or not (false). The default value is true
        /// (audio is recorded). If you set both <paramref name="hasAudio"/> and <paramref name="hasVideo"/>
        /// to false, the call to the <see cref="StartArchive"/> method results in an error.
        /// </param>
        /// <param name="outputMode">
        /// Whether all streams in the archive are recorded to a single file (<see cref="OutputMode.COMPOSED"/>,
        /// the default) or to individual files (<see cref="OutputMode.INDIVIDUAL"/>).
        /// </param>
        /// <param name="resolution">
        /// The resolution for the archive. The default for <see cref="OutputMode.COMPOSED"/> is "640x480".
        /// You cannot specify the resolution for <see cref="OutputMode.INDIVIDUAL"/>.
        /// </param>
        /// <param name="layout">
        /// The layout that you want to use for your archive. If type is set to <see cref="LayoutType.custom"/>
        /// you must provide a StyleSheet string to Vonage how to layout your archive.
        /// </param>
        /// <returns>
        /// The Archive object. This object includes properties defining the archive, including the archive ID.
        /// </returns>
        /// <summary>
        /// Starts archiving an OpenTok session.
        /// <para>
        /// Clients must be actively connected to the OpenTok session for you to successfully start
        /// recording an archive.
        /// </para>
        /// <para>
        /// You can only record one archive at a time for a given session. You can only record
        /// archives of sessions that uses the OpenTok Media Router (sessions with the media mode set
        /// to routed); you cannot archive sessions with the media mode set to relayed.
        /// </para>
        /// <para>
        /// Note that you can have the session be automatically archived by setting the archiveMode
        /// parameter of the <see cref="CreateSession"/> method to <see cref="ArchiveMode.ALWAYS"/>.
        /// </para>
        /// </summary>
        /// <param name="sessionId">
        /// The session ID of the OpenTok session to archive.
        /// </param>
        /// <param name="name">
        /// The name of the archive. You can use this name to identify the archive. It is a property
        /// of the Archive object, and it is a property of archive-related events in the OpenTok client
        /// libraries.
        /// </param>
        /// <param name="hasVideo">
        /// Whether the archive will record video (true) or not (false). The default value is true
        /// (video is recorded). If you set both <paramref name="hasAudio"/> and <paramref name="hasVideo"/>
        /// to false, the call to the <see cref="StartArchive"/> method results in an error.
        /// </param>
        /// <param name="hasAudio">
        /// Whether the archive will record audio (true) or not (false). The default value is true
        /// (audio is recorded). If you set both <paramref name="hasAudio"/> and <paramref name="hasVideo"/>
        /// to false, the call to the <see cref="StartArchive"/> method results in an error.
        /// </param>
        /// <param name="outputMode">
        /// Whether all streams in the archive are recorded to a single file (<see cref="OutputMode.COMPOSED"/>,
        /// the default) or to individual files (<see cref="OutputMode.INDIVIDUAL"/>).
        /// </param>
        /// <param name="resolution">
        /// The resolution for the archive. The default for <see cref="OutputMode.COMPOSED"/> is "640x480".
        /// You cannot specify the resolution for <see cref="OutputMode.INDIVIDUAL"/>.
        /// </param>
        /// <param name="layout">
        /// The layout that you want to use for your archive. If type is set to <see cref="LayoutType.custom"/>
        /// you must provide a StyleSheet string to Vonage how to layout your archive.
        /// </param>
        /// <returns>
        /// The Archive object. This object includes properties defining the archive, including the archive ID.
        /// </returns>
        public async Task<Archive> StartArchiveAsync(string sessionId, string name = "", bool hasVideo = true, bool hasAudio = true, OutputMode outputMode = OutputMode.COMPOSED, string resolution = null, ArchiveLayout layout = null)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("Session not valid");
            }
            string url = string.Format("v2/project/{0}/archive", this.ApiKey);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            var data = new Dictionary<string, object>() { { "sessionId", sessionId }, { "name", name }, { "hasVideo", hasVideo }, { "hasAudio", hasAudio }, { "outputMode", outputMode.ToString().ToLowerInvariant() } };

            if (!String.IsNullOrEmpty(resolution) && outputMode.Equals(OutputMode.INDIVIDUAL))
            {
                throw new OpenTokArgumentException("Resolution can't be specified for Individual Archives");
            }
            else if (!String.IsNullOrEmpty(resolution) && outputMode.Equals(OutputMode.COMPOSED))
            {
                data.Add("resolution", resolution);
            }
            if (layout != null)
            {
                if (layout?.Type == LayoutType.custom && string.IsNullOrEmpty(layout?.StyleSheet) ||
                    layout?.Type != LayoutType.custom && !string.IsNullOrEmpty(layout?.StyleSheet))
                {
                    throw new OpenTokArgumentException("Could not set layout, stylesheet must be set if and only if type is custom");
                }
                data.Add("layout", layout);
            }

            string response = await Client.PostAsync(url, headers, data);
            return OpenTokUtils.GenerateArchive(response, ApiKey, ApiSecret, OpenTokServer);
        }

        /// <summary>
        /// Stops an OpenTok archive that is being recorded.
        /// <para>
        /// Archives automatically stop recording after 120 minutes or when all clients have
        /// disconnected from the session being archived.
        /// </para>
        /// </summary>
        /// <param name="archiveId">The archive ID of the archive you want to stop recording.</param>
        /// <returns>The Archive object corresponding to the archive being STOPPED.</returns>
        public Archive StopArchive(string archiveId)
        {
            try
            {
                var task = StopArchiveAsync(archiveId);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Stops an OpenTok archive that is being recorded.
        /// <para>
        /// Archives automatically stop recording after 120 minutes or when all clients have
        /// disconnected from the session being archived.
        /// </para>
        /// </summary>
        /// <param name="archiveId">The archive ID of the archive you want to stop recording.</param>
        /// <returns>The Archive object corresponding to the archive being STOPPED.</returns>
        public async Task<Archive> StopArchiveAsync(string archiveId)
        {
            string url = string.Format("v2/project/{0}/archive/{1}/stop", this.ApiKey, archiveId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };

            string response = await Client.PostAsync(url, headers, new Dictionary<string, object>());
            return JsonConvert.DeserializeObject<Archive>(response);
        }

        /// <summary>
        /// Sets the layout type for the broadcast. For a description of layout types, see
        /// <a href="https://tokbox.com/developer/guides/archiving/layout-control.html#dynamically-changing-the-layout-type-during-an-archive-recording">Configuring the video layout for OpenTok archive</a>.
        /// </summary>
        /// <param name="archiveId">The broadcast ID of the broadcasting session.</param>
        /// <param name="layout">The BroadcastLayout that defines layout options for the broadcast.</param>
        public void SetArchiveLayout(string archiveId, ArchiveLayout layout)
        {
            try
            {
                var task = SetArchiveLayoutAsync(archiveId, layout);
                task.Wait();
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Sets the layout type for the broadcast. For a description of layout types, see
        /// <a href="https://tokbox.com/developer/guides/archiving/layout-control.html#dynamically-changing-the-layout-type-during-an-archive-recording">Configuring the video layout for OpenTok archive</a>.
        /// </summary>
        /// <param name="archiveId">The broadcast ID of the broadcasting session.</param>
        /// <param name="layout">The BroadcastLayout that defines layout options for the broadcast.</param>
        public Task SetArchiveLayoutAsync(string archiveId, ArchiveLayout layout)
        {
            string url = string.Format("v2/project/{0}/archive/{1}/layout", this.ApiKey, archiveId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            var data = new Dictionary<string, object>();
            if (layout != null)
            {
                if ((layout.Type.Equals(LayoutType.custom) && String.IsNullOrEmpty(layout.StyleSheet)) ||
                    (!layout.Type.Equals(LayoutType.custom) && !String.IsNullOrEmpty(layout.StyleSheet)))
                {
                    throw new OpenTokArgumentException("Could not set the layout. Either an invalid JSON or an invalid layout options.");
                }
                else
                {
                    data.Add("type", OpenTokUtils.convertToCamelCase(layout.Type.ToString()));
                    if (layout.Type.Equals(LayoutType.custom))
                    {
                        data.Add("stylesheet", layout.StyleSheet);
                    }
                }
            }

            return Client.PutAsync(url, headers, data);
        }

        /// <summary>
        /// Returns a List of <see cref="Archive"/> objects, representing archives that are both
        /// both completed and in-progress, for your API key.
        /// </summary>
        /// <param name="offset">
        /// The index offset of the first archive. 0 is offset of the most recently started archive.
        /// 1 is the offset of the archive that started prior to the most recent archive.
        /// </param>
        /// <param name="count">
        /// The number of archives to be returned. The maximum number of archives returned is 1000.
        /// </param>
        /// <returns>A List of <see cref="Archive"/> objects.</returns>
        public ArchiveList ListArchives(int offset = 0, int count = 0, string sessionId = "")
        {
            try
            {
                var task = ListArchivesAsync(offset, count, sessionId);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Returns a List of <see cref="Archive"/> objects, representing archives that are both
        /// both completed and in-progress, for your API key.
        /// </summary>
        /// <param name="offset">
        /// The index offset of the first archive. 0 is offset of the most recently started archive.
        /// 1 is the offset of the archive that started prior to the most recent archive.
        /// </param>
        /// <param name="count">
        /// The number of archives to be returned. The maximum number of archives returned is 1000.
        /// </param>
        /// <returns>A List of <see cref="Archive"/> objects.</returns>
        public async Task<ArchiveList> ListArchivesAsync(int offset = 0, int count = 0, string sessionId = "")
        {
            if (count < 0)
            {
                throw new OpenTokArgumentException("count cannot be smaller than 0");
            }
            string url = string.Format("v2/project/{0}/archive?offset={1}", this.ApiKey, offset);
            if (count > 0)
            {
                url = string.Format("{0}&count={1}", url, count);
            }
            if (!string.IsNullOrEmpty(sessionId))
            {
                if (!OpenTokUtils.ValidateSession(sessionId))
                {
                    throw new OpenTokArgumentException("Session Id is not valid");
                }
                url = $"{url}&sessionId={sessionId}";
            }
            string response = await Client.GetAsync(url);
            JObject archives = JObject.Parse(response);
            JArray archiveArray = (JArray)archives["items"];
            ArchiveList archiveList = new ArchiveList(archiveArray.ToObject<List<Archive>>(), (int)archives["count"]);
            return archiveList;
        }

        /// <summary>
        /// Gets an Archive object for the given archive ID.
        /// </summary>
        /// <param name="archiveId">The archive ID.</param>
        /// <returns>The <see cref="Archive"/> object.</returns>
        public Archive GetArchive(string archiveId)
        {
            try
            {
                var task = GetArchiveAsync(archiveId);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Gets an Archive object for the given archive ID.
        /// </summary>
        /// <param name="archiveId">The archive ID.</param>
        /// <returns>The <see cref="Archive"/> object.</returns>
        public async Task<Archive> GetArchiveAsync(string archiveId)
        {
            string url = string.Format("v2/project/{0}/archive/{1}", this.ApiKey, archiveId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            string response = await Client.GetAsync(url);
            return JsonConvert.DeserializeObject<Archive>(response);
        }

        /// <summary>
        /// Deletes an OpenTok archive.
        /// <para>
        /// You can only delete an archive which has a status of "available" or "uploaded". Deleting
        /// an archive removes its record from the list of archives. For an "available" archive, it
        /// also removes the archive file, making it unavailable for download.
        /// </para>
        /// </summary>
        /// <param name="archiveId">The archive ID of the archive you want to delete.</param>
        public void DeleteArchive(string archiveId)
        {
            try
            {
                DeleteArchiveAsync(archiveId)
                    .Wait();
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Deletes an OpenTok archive.
        /// <para>
        /// You can only delete an archive which has a status of "available" or "uploaded". Deleting
        /// an archive removes its record from the list of archives. For an "available" archive, it
        /// also removes the archive file, making it unavailable for download.
        /// </para>
        /// </summary>
        /// <param name="archiveId">The archive ID of the archive you want to delete.</param>
        public Task DeleteArchiveAsync(string archiveId)
        {
            string url = string.Format("v2/project/{0}/archive/{1}", this.ApiKey, archiveId);
            var headers = new Dictionary<string, string>();
            return Client.DeleteAsync(url, headers);
        }

        /// <summary>
        /// Gets a Stream object for the given stream ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the OpenTok session.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <returns>The <see cref="Stream"/> object.</returns>
        public Stream GetStream(string sessionId, string streamId)
        {
            try
            {
                var task = GetStreamAsync(sessionId, streamId);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Gets a Stream object for the given stream ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the OpenTok session.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <returns>The <see cref="Stream"/> object.</returns>
        public async Task<Stream> GetStreamAsync(string sessionId, string streamId)
        {
            if (String.IsNullOrEmpty(sessionId) || String.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The sessionId or streamId cannot be null or empty");
            }
            string url = string.Format("v2/project/{0}/session/{1}/stream/{2}", this.ApiKey, sessionId, streamId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            string response = await Client.GetAsync(url);
            Stream stream = JsonConvert.DeserializeObject<Stream>(response);
            Stream streamCopy = new Stream();
            streamCopy.CopyStream(stream);
            return streamCopy;
        }

        /// <summary>
        /// Returns a List of <see cref="Stream"/> objects, representing streams that are in-progress,
        /// for the Session Id.
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session.</param>
        /// <returns>A List of <see cref="Stream"/> objects.</returns>
        public StreamList ListStreams(string sessionId)
        {
            try
            {
                var task = ListStreamsAsync(sessionId);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Returns a List of <see cref="Stream"/> objects, representing streams that are in-progress,
        /// for the Session Id.
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session.</param>
        /// <returns>A List of <see cref="Stream"/> objects.</returns>
        public async Task<StreamList> ListStreamsAsync(string sessionId)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be null or empty");
            }
            string url = string.Format("v2/project/{0}/session/{1}/stream", this.ApiKey, sessionId);
            string response = await Client.GetAsync(url);
            JObject streams = JObject.Parse(response);
            JArray streamsArray = (JArray)streams["items"];
            StreamList streamList = new StreamList(streamsArray.ToObject<List<Stream>>(), (int)streams["count"]);
            return streamList;
        }

        /// <summary>
        /// Force disconnects a specific client connected to an OpenTok session.
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session.</param>
        /// <param name="connectionId">The connectionId of the connection in a session.</param>
        public void ForceDisconnect(string sessionId, string connectionId)
        {
            try
            {
                ForceDisconnectAsync(sessionId, connectionId)
                    .Wait();
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Force disconnects a specific client connected to an OpenTok session.
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session.</param>
        /// <param name="connectionId">The connectionId of the connection in a session.</param>
        public Task ForceDisconnectAsync(string sessionId, string connectionId)
        {
            if (String.IsNullOrEmpty(sessionId) || String.IsNullOrEmpty(connectionId))
            {
                throw new OpenTokArgumentException("The sessionId or connectionId cannot be null or empty");
            }

            if (!OpenTokUtils.ValidateSession(sessionId))
            {
                throw new OpenTokArgumentException("Invalid session Id");
            }
            string url = string.Format("v2/project/{0}/session/{1}/connection/{2}", this.ApiKey, sessionId, connectionId);
            var headers = new Dictionary<string, string>();
            return Client.DeleteAsync(url, headers);
        }

        /// <summary>
        /// Use this method to start a live streaming for an OpenTok session.
        /// This broadcasts the session to an HLS (HTTP live streaming) or to RTMP streams.
        /// <para>
        /// To successfully start broadcasting a session, at least one client must be connected to the session.
        /// </para>
        /// <para>
        /// You can only have one active live streaming broadcast at a time for a session
        /// (however, having more than one would not be useful).
        /// The live streaming broadcast can target one HLS endpoint and up to five RTMP servers simultaneously for a session.
        /// You can only start live streaming for sessions that use the OpenTok Media Router (with the media mode set to routed);
        /// you cannot use live streaming with sessions that have the media mode set to relayed OpenTok Media Router. See
        /// <a href="https://tokbox.com/developer/guides/create-session/#media-mode">The OpenTok Media Router and media modes.</a>
        /// </para>
        /// <para>
        /// For more information on broadcasting, see the
        /// <a href="https://tokbox.com/developer/guides/broadcast/">Broadcast developer guide.</a>
        /// </para>
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session.</param>
        /// <param name="hls"></param>
        /// <param name="rtmpList"></param>
        /// <param name="resolution"></param>
        /// <param name="maxDuration"></param>
        /// <param name="layout"></param>
        /// <returns>The Broadcast object. This object includes properties defining the archive, including the archive ID.</returns>
        public Broadcast StartBroadcast(string sessionId, Boolean hls = true, List<Rtmp> rtmpList = null, string resolution = null, int maxDuration = 7200, BroadcastLayout layout = null)
        {
            try
            {
                var task = StartBroadcastAsync(sessionId, hls, rtmpList, resolution, maxDuration, layout);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Use this method to start a live streaming for an OpenTok session.
        /// This broadcasts the session to an HLS (HTTP live streaming) or to RTMP streams.
        /// <para>
        /// To successfully start broadcasting a session, at least one client must be connected to the session.
        /// </para>
        /// <para>
        /// You can only have one active live streaming broadcast at a time for a session
        /// (however, having more than one would not be useful).
        /// The live streaming broadcast can target one HLS endpoint and up to five RTMP servers simultaneously for a session.
        /// You can only start live streaming for sessions that use the OpenTok Media Router (with the media mode set to routed);
        /// you cannot use live streaming with sessions that have the media mode set to relayed OpenTok Media Router. See
        /// <a href="https://tokbox.com/developer/guides/create-session/#media-mode">The OpenTok Media Router and media modes.</a>
        /// </para>
        /// <para>
        /// For more information on broadcasting, see the
        /// <a href="https://tokbox.com/developer/guides/broadcast/">Broadcast developer guide.</a>
        /// </para>
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session.</param>
        /// <param name="hls"></param>
        /// <param name="rtmpList"></param>
        /// <param name="resolution"></param>
        /// <param name="maxDuration"></param>
        /// <param name="layout"></param>
        /// <returns>The Broadcast object. This object includes properties defining the archive, including the archive ID.</returns>
        public async Task<Broadcast> StartBroadcastAsync(string sessionId, Boolean hls = true, List<Rtmp> rtmpList = null, string resolution = null, int maxDuration = 7200, BroadcastLayout layout = null)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("Session not valid");
            }

            if (!String.IsNullOrEmpty(resolution) && resolution != "640x480" && resolution != "1280x720")
            {
                throw new OpenTokArgumentException("Resolution value must be either 640x480 (SD) or 1280x720 (HD).");
            }

            if (maxDuration < 60 || maxDuration > 36000)
            {
                throw new OpenTokArgumentException("MaxDuration value must be between 60 and 36000 (inclusive).");
            }

            if (rtmpList != null && rtmpList.Count() >= 5)
            {
                throw new OpenTokArgumentException("Cannot add more than 5 RTMP properties");
            }

            string url = string.Format("v2/project/{0}/broadcast", this.ApiKey);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            var outputs = new Dictionary<string, object>();

            if (hls)
            {
                outputs.Add("hls", new Object());
            }

            if (rtmpList != null)
            {
                outputs.Add("rtmp", rtmpList);
            }

            var data = new Dictionary<string, object>() {
                { "sessionId", sessionId },
                { "maxDuration", maxDuration },
                { "outputs", outputs }
            };

            if (!String.IsNullOrEmpty(resolution))
            {
                data.Add("resolution", resolution);
            }

            if (layout != null)
            {
                if ((layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && String.IsNullOrEmpty(layout.Stylesheet)) ||
                    (!layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && !String.IsNullOrEmpty(layout.Stylesheet)))
                {
                    throw new OpenTokArgumentException("Could not set the layout. Either an invalid JSON or an invalid layout options.");
                }
                else
                {
                    if (layout.Type.Equals(BroadcastLayout.LayoutType.Custom))
                    {
                        data.Add("layout", layout);
                    }
                    else
                    {
                        data.Add("layout", new { type = OpenTokUtils.convertToCamelCase(layout.Type.ToString()) });
                    }
                }
            }

            string response = await Client.PostAsync(url, headers, data);
            return OpenTokUtils.GenerateBroadcast(response, ApiKey, ApiSecret, OpenTokServer);
        }

        /// <summary>
        /// Use this method to stop a live broadcast of an OpenTok session.
        /// Note that broadcasts automatically stop 120 minutes after they are started.
        /// <para>
        /// For more information on broadcasting, see the <a href="https://tokbox.com/developer/guides/broadcast/">Broadcast developer guide.</a>
        /// </para>
        /// </summary>
        /// <param name="broadcastId">The broadcast ID of the broadcasting session</param>
        /// <returns>
        /// The <see cref="Broadcast"/> object. This object includes properties defining the broadcast,
        /// including the broadcast ID.
        /// </returns>
        public Broadcast StopBroadcast(string broadcastId)
        {
            try
            {
                var task = StopBroadcastAsync(broadcastId);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Use this method to stop a live broadcast of an OpenTok session.
        /// Note that broadcasts automatically stop 120 minutes after they are started.
        /// <para>
        /// For more information on broadcasting, see the <a href="https://tokbox.com/developer/guides/broadcast/">Broadcast developer guide.</a>
        /// </para>
        /// </summary>
        /// <param name="broadcastId">The broadcast ID of the broadcasting session</param>
        /// <returns>
        /// The <see cref="Broadcast"/> object. This object includes properties defining the broadcast,
        /// including the broadcast ID.
        /// </returns>
        public async Task<Broadcast> StopBroadcastAsync(string broadcastId)
        {
            string url = string.Format("v2/project/{0}/broadcast/{1}/stop", this.ApiKey, broadcastId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };

            string response = await Client.PostAsync(url, headers, new Dictionary<string, object>());
            return JsonConvert.DeserializeObject<Broadcast>(response);
        }

        /// <summary>
        /// Use this method to get a live streaming broadcast object of an OpenTok session.
        /// </summary>
        /// <para>
        /// For more information on broadcasting, see the <a href="https://tokbox.com/developer/guides/broadcast/">Broadcast developer guide.</a>
        /// </para>
        /// <param name="broadcastId">The broadcast ID of the broadcasting session</param>
        /// <returns>
        /// The <see cref="Broadcast"/> object. This object includes properties defining the broadcast,
        /// including the broadcast ID.
        /// </returns>
        public Broadcast GetBroadcast(string broadcastId)
        {
            try
            {
                var task = GetBroadcastAsync(broadcastId);
                task.Wait();
                return task.Result;
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }            
        }

        /// <summary>
        /// Use this method to get a live streaming broadcast object of an OpenTok session.
        /// </summary>
        /// <para>
        /// For more information on broadcasting, see the <a href="https://tokbox.com/developer/guides/broadcast/">Broadcast developer guide.</a>
        /// </para>
        /// <param name="broadcastId">The broadcast ID of the broadcasting session</param>
        /// <returns>
        /// The <see cref="Broadcast"/> object. This object includes properties defining the broadcast,
        /// including the broadcast ID.
        /// </returns>
        public async Task<Broadcast> GetBroadcastAsync(string broadcastId)
        {
            string url = string.Format("v2/project/{0}/broadcast/{1}", this.ApiKey, broadcastId);
            string response = await Client.GetAsync(url);
            return OpenTokUtils.GenerateBroadcast(response, ApiKey, ApiSecret, OpenTokServer);
        }

        /// <summary>
        /// Sets the layout type for the broadcast. For a description of layout types, see
        /// <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#configuring-video-layout-for-opentok-live-streaming-broadcasts">Configuring the video layout for OpenTok live streaming broadcasts</a>.
        /// </summary>
        /// <param name="broadcastId">The broadcast ID of the broadcasting session.</param>
        /// <param name="layout">The BroadcastLayout that defines layout options for the broadcast.</param>
        public void SetBroadcastLayout(string broadcastId, BroadcastLayout layout)
        {
            try
            {
                SetBroadcastLayoutAsync(broadcastId, layout)
                    .Wait();
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }
        }

        /// <summary>
        /// Sets the layout type for the broadcast. For a description of layout types, see
        /// <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#configuring-video-layout-for-opentok-live-streaming-broadcasts">Configuring the video layout for OpenTok live streaming broadcasts</a>.
        /// </summary>
        /// <param name="broadcastId">The broadcast ID of the broadcasting session.</param>
        /// <param name="layout">The BroadcastLayout that defines layout options for the broadcast.</param>
        public Task SetBroadcastLayoutAsync(string broadcastId, BroadcastLayout layout)
        {
            string url = string.Format("v2/project/{0}/broadcast/{1}/layout", this.ApiKey, broadcastId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            var data = new Dictionary<string, object>();
            if (layout != null)
            {
                if ((layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && String.IsNullOrEmpty(layout.Stylesheet)) ||
                    (!layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && !String.IsNullOrEmpty(layout.Stylesheet)))
                {
                    throw new OpenTokArgumentException("Could not set the layout. Either an invalid JSON or an invalid layout options.");
                }
                else
                {
                    data.Add("type", OpenTokUtils.convertToCamelCase(layout.Type.ToString()));
                    if (layout.Type.Equals(BroadcastLayout.LayoutType.Custom))
                    {
                        data.Add("stylesheet", layout.Stylesheet);
                    }
                }
            }

            return Client.PutAsync(url, headers, data);
        }

        /// <summary>
        /// Sets the layout class list for streams in a session. Layout classes are used in
        /// the layout for composed archives and live streaming broadcasts. For more information, see
        /// <a href="https://tokbox.com/developer/guides/archiving/layout-control.html">Customizing the video layout for composed archives</a> and
        /// <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#configuring-video-layout-for-opentok-live-streaming-broadcasts" > Configuring video layout for OpenTok live streaming broadcasts</a>.
        /// <para>
        /// You can set the initial layout class list for streams published by a client when you generate
        /// used by the client. See the <see cref="GenerateToken"/> method.
        /// </para>
        /// </summary>
        /// <param name="sessionId">The sessionId</param>
        /// <param name="streams">A list of StreamsProperties that defines class lists for one or more streams in the session.</param>
        public void SetStreamClassLists(string sessionId, List<StreamProperties> streams)
        {
            try
            {
                SetStreamClassListsAsync(sessionId, streams)
                    .Wait();
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }
        }

        /// <summary>
        /// Sets the layout class list for streams in a session. Layout classes are used in
        /// the layout for composed archives and live streaming broadcasts. For more information, see
        /// <a href="https://tokbox.com/developer/guides/archiving/layout-control.html">Customizing the video layout for composed archives</a> and
        /// <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#configuring-video-layout-for-opentok-live-streaming-broadcasts" > Configuring video layout for OpenTok live streaming broadcasts</a>.
        /// <para>
        /// You can set the initial layout class list for streams published by a client when you generate
        /// used by the client. See the <see cref="GenerateToken"/> method.
        /// </para>
        /// </summary>
        /// <param name="sessionId">The sessionId</param>
        /// <param name="streams">A list of StreamsProperties that defines class lists for one or more streams in the session.</param>
        public Task SetStreamClassListsAsync(string sessionId, List<StreamProperties> streams)
        {
            string url = string.Format("v2/project/{0}/session/{1}/stream", this.ApiKey, sessionId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            var items = new List<object>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (streams == null || streams.Count() == 0)
            {
                throw new OpenTokArgumentException("The stream list must include at least one item.");
            }
            else
            {
                foreach (StreamProperties stream in streams)
                {
                    items.Add(
                        new
                        {
                            id = stream.Id,
                            layoutClassList = stream.LayoutClassList
                        }
                    );
                }
            }
            data.Add("items", items);

            return Client.PutAsync(url, headers, data);
        }

        /// <summary>
        /// Sends a signal to clients (or a specific client) connected to an OpenTok session.
        /// </summary>
        /// <param name="sessionId">The OpenTok sessionId where the signal will be sent.</param>
        /// <param name="signalProperties">This signalProperties defines the payload for the signal.</param>
        /// <param name="connectionId">An optional parameter used to send the signal to a specific connection in a session.</param>
        public void Signal(string sessionId, SignalProperties signalProperties, string connectionId = null)
        {
            try
            {
                SignalAsync(sessionId, signalProperties, connectionId)
                    .Wait();
            }
            catch (AggregateException aex)
            {
                throw aex.InnerException;
            }
        }

        /// <summary>
        /// Sends a signal to clients (or a specific client) connected to an OpenTok session.
        /// </summary>
        /// <param name="sessionId">The OpenTok sessionId where the signal will be sent.</param>
        /// <param name="signalProperties">This signalProperties defines the payload for the signal.</param>
        /// <param name="connectionId">An optional parameter used to send the signal to a specific connection in a session.</param>
        public Task SignalAsync(string sessionId, SignalProperties signalProperties, string connectionId = null)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.");
            }
            string url = String.IsNullOrEmpty(connectionId) ?
                            string.Format("v2/project/{0}/session/{1}/signal", this.ApiKey, sessionId) :
                            string.Format("v2/project/{0}/session/{1}/connection/{2}/signal", this.ApiKey, sessionId, connectionId);
            var headers = new Dictionary<string, string> { { "Content-type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                { "data", signalProperties.data },
                { "type", signalProperties.type }
            };
            return Client.PostAsync(url, headers, data);
        }

        /// <summary>
        /// Set's the default request timeout (in milliseconds) for all WebRequest's sent by the SDK
        /// </summary>
        /// <param name="timeout"></param>
        public void SetDefaultRequestTimeout(int timeout)
        {
            Client.RequestTimeout = timeout;
        }
    }
}
