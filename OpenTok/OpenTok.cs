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
        public HttpClient Client { internal get; set; }

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
            if (!OpenTokUtils.TestIpAddress(location))
            {
                throw new OpenTokArgumentException($"Location {location} is not a valid IP address");
            }

            if (archiveMode == ArchiveMode.ALWAYS && mediaMode != MediaMode.ROUTED)
            {
                throw new OpenTokArgumentException("A session with always archive mode must also have the routed media mode.");
            }

            string preference = (mediaMode == MediaMode.RELAYED) ? "enabled" : "disabled";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/x-www-form-urlencoded" } };
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
                throw new OpenTokArgumentException($"Location {location} is not a valid IP address");
            }

            if (archiveMode == ArchiveMode.ALWAYS && mediaMode != MediaMode.ROUTED)
            {
                throw new OpenTokArgumentException("A session with always archive mode must also have the routed media mode.");
            }

            string preference = mediaMode == MediaMode.RELAYED
                ? "enabled"
                : "disabled";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/x-www-form-urlencoded" } };
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
        /// - <see cref="Role.MODERATOR"/> (In addition to the privileges granted to a publisher, 
        ///   a moderator can perform moderation functions, such as forcing clients
        ///   to disconnect, to stop publishing streams, or to mute audio in published streams. See the
        ///   <a href="https://tokbox.com/developer/guides/moderation/">Moderation developer guide</a>.
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
        /// <param name="streamMode">
        /// Whether streams included in the archive are selected automatically (StreamMode.Auto,
        /// the default) or manually (StreamMode.Manual). With StreamMode.Manual, you will
        /// specify streams to be included in the archive using the
        /// <see cref="OpenTok.AddStreamToArchive"/> and
        /// <see cref="OpenTok.RemoveStreamFromArchive"/> methods (or the
        /// <see cref="OpenTok.AddStreamToArchiveAsync"/> and
        /// <see cref="OpenTok.RemoveStreamFromArchiveAsync"/> methods).
        /// </param>
        /// <returns>
        /// The Archive object. This object includes properties defining the archive, including the archive ID.
        /// </returns>
        public Archive StartArchive(string sessionId, string name = "", bool hasVideo = true, bool hasAudio = true, OutputMode outputMode = OutputMode.COMPOSED, string resolution = null, ArchiveLayout layout = null, StreamMode? streamMode = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("Session not valid");
            }
            string url = $"v2/project/{ApiKey}/archive";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "sessionId", sessionId }, { "name", name }, { "hasVideo", hasVideo }, { "hasAudio", hasAudio }, { "outputMode", outputMode.ToString().ToLowerInvariant() } };

            if (!string.IsNullOrEmpty(resolution) && outputMode.Equals(OutputMode.INDIVIDUAL))
            {
                throw new OpenTokArgumentException("Resolution can't be specified for Individual Archives");
            }

            if (!string.IsNullOrEmpty(resolution) && outputMode.Equals(OutputMode.COMPOSED))
            {
                data.Add("resolution", resolution);
            }

            if (layout != null)
            {
                if (layout.Type == LayoutType.custom && string.IsNullOrEmpty(layout.StyleSheet) ||
                    layout.Type != LayoutType.custom && !string.IsNullOrEmpty(layout.StyleSheet))
                {
                    throw new OpenTokArgumentException("Could not set layout, stylesheet must be set if and only if type is custom");
                }

                if (layout.ScreenShareType != null && layout.Type != LayoutType.bestFit)
                {
                    throw new OpenTokArgumentException($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}");
                }
                data.Add("layout", layout);
            }

            if (streamMode.HasValue)
            {
                data.Add("streamMode", streamMode.Value.ToString().ToLower());
            }

            string response = Client.Post(url, headers, data);
            return OpenTokUtils.GenerateArchive(response, ApiKey, ApiSecret, OpenTokServer);
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
        /// <param name="streamMode">
        /// Whether streams included in the archive are selected automatically (StreamMode.Auto,
        /// the default) or manually (StreamMode.Manual). With StreamMode.Manual, you will
        /// specify streams to be included in the archive using the
        /// <see cref="OpenTok.AddStreamToArchive"/> and
        /// <see cref="OpenTok.RemoveStreamFromArchive"/> methods (or the
        /// <see cref="OpenTok.AddStreamToArchiveAsync"/> and
        /// <see cref="OpenTok.RemoveStreamFromArchiveAsync"/> methods).
        /// </param>
        /// <returns>
        /// The Archive object. This object includes properties defining the archive, including the archive ID.
        /// </returns>
        public async Task<Archive> StartArchiveAsync(string sessionId, string name = "", bool hasVideo = true, bool hasAudio = true, OutputMode outputMode = OutputMode.COMPOSED, string resolution = null, ArchiveLayout layout = null, StreamMode? streamMode = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("Session not valid");
            }
            string url = $"v2/project/{ApiKey}/archive";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                { "sessionId", sessionId }, 
                { "name", name }, 
                { "hasVideo", hasVideo }, 
                { "hasAudio", hasAudio }, 
                { "outputMode", outputMode.ToString().ToLowerInvariant() }
            };

            if (!string.IsNullOrEmpty(resolution) && outputMode.Equals(OutputMode.INDIVIDUAL))
            {
                throw new OpenTokArgumentException("Resolution can't be specified for Individual Archives");
            }

            if (!string.IsNullOrEmpty(resolution) && outputMode.Equals(OutputMode.COMPOSED))
            {
                data.Add("resolution", resolution);
            }

            if (layout != null)
            {
                if (layout.Type == LayoutType.custom && string.IsNullOrEmpty(layout.StyleSheet) ||
                    layout.Type != LayoutType.custom && !string.IsNullOrEmpty(layout.StyleSheet))
                {
                    throw new OpenTokArgumentException("Could not set layout, stylesheet must be set if and only if type is custom");
                }

                if (layout.ScreenShareType != null && layout.Type != LayoutType.bestFit)
                {
                    throw new OpenTokArgumentException($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}");
                }
                data.Add("layout", layout);
            }

            if (streamMode.HasValue)
            {
                data.Add("streamMode", streamMode.Value.ToString().ToLower());
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
            string url = string.Format("v2/project/{0}/archive/{1}/stop", this.ApiKey, archiveId);
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };

            string response = Client.Post(url, headers, new Dictionary<string, object>());
            return JsonConvert.DeserializeObject<Archive>(response);
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
        /// <param name="sessionId">
        /// The session ID.
        /// </param>
        /// <returns>A List of <see cref="Archive"/> objects.</returns>
        public ArchiveList ListArchives(int offset = 0, int count = 0, string sessionId = "")
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
            string response = Client.Get(url);
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
            string url = $"v2/project/{ApiKey}/archive/{archiveId}";
            string response = Client.Get(url);
            return JsonConvert.DeserializeObject<Archive>(response);
        }

        /// <summary>
        /// Gets an Archive object for the given archive ID.
        /// </summary>
        /// <param name="archiveId">The archive ID.</param>
        /// <returns>The <see cref="Archive"/> object.</returns>
        public async Task<Archive> GetArchiveAsync(string archiveId)
        {
            string url = $"v2/project/{ApiKey}/archive/{archiveId}";
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
            string url = $"v2/project/{ApiKey}/archive/{archiveId}";
            var headers = new Dictionary<string, string>();
            Client.Delete(url, headers);
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
            string url = $"v2/project/{ApiKey}/archive/{archiveId}";
            var headers = new Dictionary<string, string>();
            return Client.DeleteAsync(url, headers);
        }

        /// <summary>
        /// Adds a stream to a currently running composed archive that was started with the
        /// <c>streamMode</c> set to StreamMode.Manual. You can call the method repeatedly
        /// with the same stream ID, to toggle the stream's audio or video in the archive.
        /// </summary>
        /// <param name="archiveId">The archive ID.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <param name="hasAudio">Whether the composed archive should include the stream's audio
        /// (true, the default) or not (false).</param>
        /// <param name="hasVideo">Whether the composed archive should include the stream's video
        /// (true, the default) or not (false).</param>
        public void AddStreamToArchive(string archiveId, string streamId, bool hasAudio = true, bool hasVideo = true)
        {
            if (string.IsNullOrEmpty(archiveId))
            {
                throw new OpenTokArgumentException("The archiveId cannot be null or empty");
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be null or empty");
            }

            string url = $"v2/project/{ApiKey}/archive/{archiveId}/streams";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                {"addStream", streamId},
                {"hasAudio", hasAudio},
                {"hasVideo", hasVideo}
            };

            Client.Patch(url, headers, data);
        }

        /// <summary>
        /// Adds a stream to a currently running composed archive that was started with the
        /// <c>streamMode</c> set to StreamMode.Manual. You can call the method repeatedly
        /// with the same stream ID, to toggle the stream's audio or video in the archive.
        /// </summary>
        /// <param name="archiveId">The archive ID.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <param name="hasAudio">Whether the composed archive should include the stream's audio
        /// (true, the default) or not (false).</param>
        /// <param name="hasVideo">Whether the composed archive should include the stream's video
        /// (true, the default) or not (false).</param>
        public Task AddStreamToArchiveAsync(string archiveId, string streamId, bool hasAudio = true, bool hasVideo = true)
        {
            if (string.IsNullOrEmpty(archiveId))
            {
                throw new OpenTokArgumentException("The archiveId cannot be null or empty");
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be null or empty");
            }

            string url = $"v2/project/{ApiKey}/archive/{archiveId}/streams";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                {"addStream", streamId},
                {"hasAudio", hasAudio},
                {"hasVideo", hasVideo}
            };

            return Client.PatchAsync(url, headers, data);
        }

        /// <summary>
        /// Removes a stream from a composed archive that was started with the
        /// <c>streamMode</c> set to StreamMode.Manual.
        /// </summary>
        /// <param name="archiveId">The archive ID.</param>
        /// <param name="streamId">The stream ID.</param>
        public void RemoveStreamFromArchive(string archiveId, string streamId)
        {
            if (string.IsNullOrEmpty(archiveId))
            {
                throw new OpenTokArgumentException("The archiveId cannot be null or empty");
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be null or empty");
            }

            string url = $"v2/project/{ApiKey}/archive/{archiveId}/streams";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "removeStream", streamId } };

            Client.Patch(url, headers, data);
        }

        /// <summary>
        /// Removes a stream from a composed archive that was started with the
        /// <c>streamMode</c> set to StreamMode.Manual.
        /// </summary>
        /// <param name="archiveId">The archive ID.</param>
        /// <param name="streamId">The stream ID.</param>
        public Task RemoveStreamFromArchiveAsync(string archiveId, string streamId)
        {
            if (string.IsNullOrEmpty(archiveId))
            {
                throw new OpenTokArgumentException("The archiveId cannot be null or empty");
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be null or empty");
            }

            string url = $"v2/project/{ApiKey}/archive/{archiveId}/streams";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "removeStream", streamId } };

            return Client.PatchAsync(url, headers, data);
        }

        /// <summary>
        /// Gets a Stream object for the given stream ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the OpenTok session.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <returns>The <see cref="Stream"/> object.</returns>
        public Stream GetStream(string sessionId, string streamId)
        {
            if (String.IsNullOrEmpty(sessionId) || String.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The sessionId or streamId cannot be null or empty");
            }
            string url = string.Format("v2/project/{0}/session/{1}/stream/{2}", this.ApiKey, sessionId, streamId);
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            string response = Client.Get(url);
            Stream stream = JsonConvert.DeserializeObject<Stream>(response);
            Stream streamCopy = new Stream();
            streamCopy.CopyStream(stream);
            return streamCopy;
        }

        /// <summary>
        /// Returns a List of <see cref="Stream"/> objects, representing streams that are in-progress,
        /// for the session ID.
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session.</param>
        /// <returns>A List of <see cref="Stream"/> objects.</returns>
        public StreamList ListStreams(string sessionId)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be null or empty");
            }
            string url = string.Format("v2/project/{0}/session/{1}/stream", this.ApiKey, sessionId);
            string response = Client.Get(url);
            JObject streams = JObject.Parse(response);
            JArray streamsArray = (JArray)streams["items"];
            StreamList streamList = new StreamList(streamsArray.ToObject<List<Stream>>(), (int)streams["count"]);
            return streamList;
        }

        /// <summary>
        /// Force a specific client to disconnect from an OpenTok session.
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session.</param>
        /// <param name="connectionId">The connectionId of the connection in a session.</param>
        public void ForceDisconnect(string sessionId, string connectionId)
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
            Client.Delete(url, headers);
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
        /// <param name="hls">Whether to include an HLS broadcast.</param>
        /// <param name="rtmpList">
        /// A list of <see cref="Rtmp"/> objects, defining RTMP streams to be broadcast (up to five).
        /// </param>
        /// <param name="resolution">
        /// The resolution of the broadcast video. This can be set to either "640x480" or "1280x720".
        /// </param>
        /// <param name="maxDuration">
        /// The maximum duration for the broadcast, in seconds. The broadcast will automatically
        /// stop when the maximum duration is reached. You can set the maximum duration to a value
        /// from 60 (60 seconds) to 36000 (10 hours). The default maximum duration is 2 hours
        /// (7,200 seconds).
        /// </param>
        /// <param name="layout">
        /// Specify this BroadcastLayout object to assign the initial layout type for
        /// the broadcast.
        /// </param>
        /// <param name="streamMode">
        /// Whether streams included in the broadcast are selected automatically (StreamMode.Auto,
        /// the default) or manually (StreamMode.Manual). With StreamMode.Manual, you will
        /// specify streams to be included in the broadcast using the
        /// <see cref="OpenTok.AddStreamToBroadcast"/> and
        /// <see cref="OpenTok.RemoveStreamFromBroadcast"/> methods (or the
        /// <see cref="OpenTok.AddStreamToBroadcastAsync"/> and
        /// <see cref="OpenTok.RemoveStreamFromBroadcastAsync"/> methods).
        /// </param>
        /// <returns>The Broadcast object. This object includes properties defining the archive, including the archive ID.</returns>
        public Broadcast StartBroadcast(string sessionId, Boolean hls = true, List<Rtmp> rtmpList = null, string resolution = null,
            int maxDuration = 7200, BroadcastLayout layout = null, StreamMode? streamMode = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("Session not valid");
            }

            if (!string.IsNullOrEmpty(resolution) && resolution != "640x480" && resolution != "1280x720")
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

            string url = $"v2/project/{ApiKey}/broadcast";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var outputs = new Dictionary<string, object>();

            if (hls)
            {
                outputs.Add("hls", new object());
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

            if (!string.IsNullOrEmpty(resolution))
            {
                data.Add("resolution", resolution);
            }

            if (layout != null)
            {
                if (layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && string.IsNullOrEmpty(layout.Stylesheet) ||
                    !layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && !string.IsNullOrEmpty(layout.Stylesheet))
                {
                    throw new OpenTokArgumentException("Could not set the layout. Either an invalid JSON or an invalid layout options.");
                }

                if (layout.ScreenShareType != null && layout.Type != BroadcastLayout.LayoutType.BestFit)
                {
                    throw new OpenTokArgumentException($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}");
                }

                if (layout.Type.Equals(BroadcastLayout.LayoutType.Custom))
                {
                    data.Add("layout", layout);
                }
                else
                {
                    data.Add("layout", layout);
                }
            }

            if (streamMode.HasValue)
            {
                data.Add("streamMode", streamMode.Value.ToString().ToLower());
            }

            string response = Client.Post(url, headers, data);
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
            string url = string.Format("v2/project/{0}/broadcast/{1}/stop", this.ApiKey, broadcastId);
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };

            string response = Client.Post(url, headers, new Dictionary<string, object>());
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
            string url = string.Format("v2/project/{0}/broadcast/{1}", this.ApiKey, broadcastId);
            string response = Client.Get(url);
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
            string url = $"v2/project/{ApiKey}/broadcast/{broadcastId}/layout";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>();
            if (layout != null)
            {
                if (layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && string.IsNullOrEmpty(layout.Stylesheet) ||
                    !layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && !string.IsNullOrEmpty(layout.Stylesheet))
                {
                    throw new OpenTokArgumentException("Could not set the layout. Either an invalid JSON or an invalid layout options.", nameof(layout));
                }

                if (layout.ScreenShareType != null && layout.Type != BroadcastLayout.LayoutType.BestFit)
                {
                    throw new OpenTokArgumentException($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}");
                }

                data.Add("type", OpenTokUtils.convertToCamelCase(layout.Type.ToString()));
                
                if (layout.Type.Equals(BroadcastLayout.LayoutType.Custom))
                {
                    data.Add("stylesheet", layout.Stylesheet);
                }

                if (layout.ScreenShareType != null)
                {
                    data.Add("screenShareType", OpenTokUtils.convertToCamelCase(layout.ScreenShareType.ToString()));
                }
            }

            Client.Put(url, headers, data);
        }

        /// <summary>
        /// Sets the layout type for the broadcast. For a description of layout types, see
        /// <a href="https://tokbox.com/developer/guides/broadcast/live-streaming/#configuring-video-layout-for-opentok-live-streaming-broadcasts">Configuring the video layout for OpenTok live streaming broadcasts</a>.
        /// </summary>
        /// <param name="broadcastId">The broadcast ID of the broadcasting session.</param>
        /// <param name="layout">The BroadcastLayout that defines layout options for the broadcast.</param>
        public async Task SetBroadcastLayoutAsync(string broadcastId, BroadcastLayout layout)
        {
            string url = $"v2/project/{ApiKey}/broadcast/{broadcastId}/layout";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>();
            if (layout != null)
            {
                if (layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && string.IsNullOrEmpty(layout.Stylesheet) ||
                    !layout.Type.Equals(BroadcastLayout.LayoutType.Custom) && !string.IsNullOrEmpty(layout.Stylesheet))
                {
                    throw new OpenTokArgumentException("Could not set the layout. Either an invalid JSON or an invalid layout options.", nameof(layout));
                }

                if (layout.ScreenShareType != null && layout.Type != BroadcastLayout.LayoutType.BestFit)
                {
                    throw new OpenTokArgumentException($"Could not set screenShareLayout. When screenShareType is set, layout.Type must be bestFit, was {layout.Type}");
                }

                data.Add("type", OpenTokUtils.convertToCamelCase(layout.Type.ToString()));

                if (layout.Type.Equals(BroadcastLayout.LayoutType.Custom))
                {
                    data.Add("stylesheet", layout.Stylesheet);
                }
                if (layout.ScreenShareType != null)
                {
                    data.Add("screenShareType", OpenTokUtils.convertToCamelCase(layout.ScreenShareType.ToString()));
                }
            }

            await Client.PutAsync(url, headers, data);
        }

        /// <summary>
        /// Allows you to Dynamically change the layout of a composed archive while it's being recorded
        /// see <a href="https://tokbox.com/developer/guides/archiving/layout-control.html">Customizing the video layout for composed archives</a>
        /// for details regarding customizing a layout.
        /// </summary>
        /// <param name="archiveId"></param>
        /// <param name="layout"></param>
        /// <returns></returns>
        public bool SetArchiveLayout(string archiveId, ArchiveLayout layout)
        {
            string url = $"v2/project/{ApiKey}/archive/{archiveId}/layout";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>();
            
            if (layout != null)
            {
                if (layout.Type == LayoutType.custom && string.IsNullOrEmpty(layout.StyleSheet))
                {
                    throw new OpenTokArgumentException("Invalid layout, layout is custom but no stylesheet provided", nameof(layout));
                }

                if (layout.Type != LayoutType.custom && !string.IsNullOrEmpty(layout.StyleSheet))
                {
                    throw new OpenTokArgumentException("Invalid layout, layout is not custom, but stylesheet is set", nameof(layout));
                }

                data.Add("type", layout.Type.ToString());
                if (!string.IsNullOrEmpty(layout.StyleSheet))
                {
                    data.Add("stylesheet", layout.StyleSheet);
                }

                if (layout.ScreenShareType != null)
                {
                    if (layout.Type != LayoutType.bestFit)
                    {
                        throw new OpenTokArgumentException("Invalid layout, when ScreenShareType is set, Type must be bestFit", nameof(layout));
                    }
                    data.Add("screenshareType", OpenTokUtils.convertToCamelCase(layout.ScreenShareType.ToString()));
                }
            }

            Client.Put(url, headers, data);
            return true;
        }

        /// <summary>
        /// Allows you to Dynamically change the layout of a composed archive while it's being recorded
        /// see <a href="https://tokbox.com/developer/guides/archiving/layout-control.html">Customizing the video layout for composed archives</a>
        /// for details regarding customizing a layout.
        /// </summary>
        /// <param name="archiveId"></param>
        /// <param name="layout"></param>
        /// <returns></returns>
        public async Task<bool> SetArchiveLayoutAsync(string archiveId, ArchiveLayout layout)
        {
            string url = $"v2/project/{ApiKey}/archive/{archiveId}/layout";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>();

            if (layout != null)
            {
                if (layout.Type == LayoutType.custom && string.IsNullOrEmpty(layout.StyleSheet))
                {
                    throw new OpenTokArgumentException("Invalid layout, layout is custom but no stylesheet provided", nameof(layout));
                }

                if (layout.Type != LayoutType.custom && !string.IsNullOrEmpty(layout.StyleSheet))
                {
                    throw new OpenTokArgumentException("Invalid layout, layout is not custom, but stylesheet is set", nameof(layout));
                }

                data.Add("type", layout.Type.ToString());
                if (!string.IsNullOrEmpty(layout.StyleSheet))
                {
                    data.Add("stylesheet", layout.StyleSheet);
                }

                if (layout.ScreenShareType != null)
                {
                    if (layout.Type != LayoutType.bestFit)
                    {
                        throw new OpenTokArgumentException("Invalid layout, when ScreenShareType is set, Type must be bestFit", nameof(layout));
                    }
                    data.Add("screenshareType", OpenTokUtils.convertToCamelCase(layout.ScreenShareType.ToString()));
                }
            }

            await Client.PutAsync(url, headers, data);
            return true;
        }


        /// <summary>
        /// Adds a stream to a currently running broadcast that was started with the
        /// the <c>streamMode</c> set to StreamMode.Manual. You can call the method repeatedly
        /// with the same stream ID, to toggle the stream's audio or video in the broadcast.
        /// </summary>
        /// <param name="broadcastId">The broadcast ID.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <param name="hasAudio">Whether the broadcast should include the stream's audio (true, the default)
        /// or not (false).</param>
        /// <param name="hasVideo">Whether the broadcast should include the stream's video (true, the default)
        /// or not (false).</param>
        /// <exception cref="OpenTokArgumentException"></exception>
        public void AddStreamToBroadcast(string broadcastId, string streamId, bool hasAudio = true, bool hasVideo = true)
        {
            if (string.IsNullOrEmpty(broadcastId))
            {
                throw new OpenTokArgumentException("The broadcastId cannot be null or empty");
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be null or empty");
            }

            string url = $"v2/project/{ApiKey}/broadcast/{broadcastId}/streams";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                {"addStream", streamId},
                {"hasAudio", hasAudio},
                {"hasVideo", hasVideo}
            };

            Client.Patch(url, headers, data);
        }

        /// <summary>
        /// Adds a stream to a currently running broadcast that was started with the
        /// the <c>streamMode</c> set to StreamMode.Manual. You can call the method repeatedly
        /// with the same stream ID, to toggle the stream's audio or video in the broadcast.
        /// </summary>
        /// <param name="broadcastId">The broadcast ID.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <param name="hasAudio">Whether the broadcast should include the stream's audio (true, the default)
        /// or not (false).</param>
        /// <param name="hasVideo">Whether the broadcast should include the stream's video (true, the default)
        /// or not (false).</param>
        /// <exception cref="OpenTokArgumentException"></exception>
        public Task AddStreamToBroadcastAsync(string broadcastId, string streamId, bool hasAudio = true, bool hasVideo = true)
        {
            if (string.IsNullOrEmpty(broadcastId))
            {
                throw new OpenTokArgumentException("The broadcastId cannot be null or empty");
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be null or empty");
            }

            string url = $"v2/project/{ApiKey}/broadcast/{broadcastId}/streams";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                {"addStream", streamId},
                {"hasAudio", hasAudio},
                {"hasVideo", hasVideo}
            };

            return Client.PatchAsync(url, headers, data);
        }

        /// <summary>
        /// Removes a stream from a broadcast that was started with the
        /// the <c>streamMode</c> set to StreamMode.Manual.
        /// </summary>
        /// <param name="broadcastId">The broadcast ID.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <exception cref="OpenTokArgumentException"></exception>
        public void RemoveStreamFromBroadcast(string broadcastId, string streamId)
        {
            if (string.IsNullOrEmpty(broadcastId))
            {
                throw new OpenTokArgumentException("The broadcastId cannot be null or empty");
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be null or empty");
            }

            string url = $"v2/project/{ApiKey}/broadcast/{broadcastId}/streams";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                {"removeStream", streamId}
            };

            Client.Patch(url, headers, data);
        }

        /// <summary>
        /// Removes a stream from a broadcast that was started with the
        /// the <c>streamMode</c> set to StreamMode.Manual.
        /// </summary>
        /// <param name="broadcastId">The broadcast ID.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <exception cref="OpenTokArgumentException"></exception>
        public Task RemoveStreamFromBroadcastAsync(string broadcastId, string streamId)
        {
            if (string.IsNullOrEmpty(broadcastId))
            {
                throw new OpenTokArgumentException("The broadcastId cannot be null or empty");
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be null or empty");
            }

            string url = $"v2/project/{ApiKey}/broadcast/{broadcastId}/streams";
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                {"removeStream", streamId}
            };

            return Client.PatchAsync(url, headers, data);
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
            string url = string.Format("v2/project/{0}/session/{1}/stream", this.ApiKey, sessionId);
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
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

            Client.Put(url, headers, data);
        }

        /// <summary>
        /// Sends a signal to clients (or a specific client) connected to an OpenTok session.
        /// </summary>
        /// <param name="sessionId">The OpenTok sessionId where the signal will be sent.</param>
        /// <param name="signalProperties">This signalProperties defines the payload for the signal.</param>
        /// <param name="connectionId">An optional parameter used to send the signal to a specific connection in a session.</param>
        public void Signal(string sessionId, SignalProperties signalProperties, string connectionId = null)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.");
            }
            string url = String.IsNullOrEmpty(connectionId) ?
                            string.Format("v2/project/{0}/session/{1}/signal", this.ApiKey, sessionId) :
                            string.Format("v2/project/{0}/session/{1}/connection/{2}/signal", this.ApiKey, sessionId, connectionId);
            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                { "data", signalProperties.data },
                { "type", signalProperties.type }
            };
            Client.Post(url, headers, data);
        }

        /// <summary>
        /// Set's the default request timeout (in milliseconds) for all WebRequest's sent by the SDK
        /// </summary>
        /// <param name="timeout"></param>
        public void SetDefaultRequestTimeout(int timeout)
        {
            Client.RequestTimeout = timeout;
        }

        /// <summary>
        /// Send DTMF digits to all participants in an active OpenTok session or to a specific client connected to that session.
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session that will receive the DTMF string.</param>
        /// <param name="connectionId">The connection connection ID of the client you are sending the DTMF signal to. Leave this empty to send a DTMF signal to all clients connected to the session.</param>
        /// <param name="digits">This is the string of DTMF digits to send. This can include 0-9, '*', '#', and 'p'. A p indicates a pause of 500ms (if you need to add a delay in sending the digits).</param>
        public void PlayDTMF(string sessionId, string digits, string connectionId = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.");
            }

            string url = string.IsNullOrEmpty(connectionId)
                ? $"v2/project/{ApiKey}/session/{sessionId}/play-dtmf"
                : $"v2/project/{ApiKey}/session/{sessionId}/connection/{connectionId}/play-dtmf";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "digits", digits } };
            Client.Post(url, headers, data);
        }

        /// <summary>
        /// Send DTMF digits to all participants in an active OpenTok session or to a specific client connected to that session.
        /// </summary>
        /// <param name="sessionId">The session ID corresponding to the session that will receive the DTMF string.</param>
        /// <param name="connectionId">The connection connection ID of the client you are sending the DTMF signal to. Leave this empty to send a DTMF signal to all clients connected to the session.</param>
        /// <param name="digits">This is the string of DTMF digits to send. This can include 0-9, '*', '#', and 'p'. A p indicates a pause of 500ms (if you need to add a delay in sending the digits).</param>
        public Task PlayDTMFAsync(string sessionId, string digits, string connectionId = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.");
            }

            string url = string.IsNullOrEmpty(connectionId)
                ? $"v2/project/{ApiKey}/session/{sessionId}/play-dtmf"
                : $"v2/project/{ApiKey}/session/{sessionId}/connection/{connectionId}/play-dtmf";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "digits", digits } };
            return Client.PostAsync(url, headers, data);
        }

        /// <summary>
        /// Connects a SIP platform to an OpenTok session.
        /// </summary>
        /// <remarks>
        /// For more information, including technical details and security considerations, see the 
        /// the <a href="https://tokbox.com/developer/guides/sip/">OpenTok SIP interconnect developer guide</a>.
        /// </remarks>
        /// <param name="sessionId">The session ID corresponding to the session to which the user will connect.</param>
        /// <param name="token">The token for the session ID with which the SIP user will use to connect.</param>
        /// <param name="sipUri">The SIP URI to be used as destination of the SIP call initiated from
        /// OpenTok to your SIP platform. If the SIP URI contains a ​transport=tls​ header,
        /// the negotiation between OpenTok and the SIP endpoint will be done securely. Note that
        /// this will only apply to the negotiation itself, and not to the transmission of audio.
        /// If you also audio transmission to be encrypted, set the <c>Secure</c> property of the
        /// of the DialOptions object passed into the options parameter to <c>​true​</c>.
        /// This is an example of setting <c>sipUri</c> for a secure call negotiation:
        /// <c>"sip:user@sip.partner.com;transport=tls"</c>. This is an example of insecure call negotiation:
        /// <c>"sip:user@sip.partner.com"</c>.</param>
        /// <param name="options">Optional parameters for SIP dialing.</param>
        public void Dial(string sessionId, string token, string sipUri, DialOptions options = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.");
            }

            if (!OpenTokUtils.ValidateSession(sessionId))
            {
                throw new OpenTokArgumentException("Session Id is not valid");
            }

            string url = $"v2/project/{this.ApiKey}/dial";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                { "sessionId", sessionId },
                { "token", token },
                { "sip", new {
                        uri = sipUri,
                        from = options?.From,
                        headers = options?.Headers,
                        auth = options?.Auth,
                        secure = options?.Secure,
                        video = options?.Video,
                        observeForceMute = options?.ObserveForceMute
                    }
                }
            };
            Client.Post(url, headers, data);
        }

        /// <summary>
        /// Connects a SIP platform to an OpenTok session.
        /// </summary>
        /// <remarks>
        /// <p>
        /// For more information, including technical details and security considerations, see the 
        /// the <a href="https://tokbox.com/developer/guides/sip/">OpenTok SIP interconnect developer guide</a>.
        /// </p>
        /// <p>
        /// Also see OpenTok.Dial.
        /// </p>
        /// </remarks>
        /// <param name="sessionId">The session ID corresponding to the session to which the user will connect.</param>
        /// <param name="token">The token for the session ID with which the SIP user will use to connect.</param>
        /// <param name="sipUri">The SIP URI to be used as destination of the SIP call initiated from
        /// OpenTok to your SIP platform. If the SIP URI contains a ​transport=tls​ header,
        /// the negotiation between OpenTok and the SIP endpoint will be done securely. Note that
        /// this will only apply to the negotiation itself, and not to the transmission of audio.
        /// If you also audio transmission to be encrypted, set the <c>Secure</c> property of the
        /// of the DialOptions object passed into the options parameter to <c>​true​</c>.
        /// This is an example of setting <c>sipUri</c> for a secure call negotiation:
        /// <c>"sip:user@sip.partner.com;transport=tls"</c>. This is an example of insecure call negotiation:
        /// <c>"sip:user@sip.partner.com"</c>.</param>
        /// <param name="options">Optional parameters for SIP dialing.</param>
        public Task DialAsync(string sessionId, string token, string sipUri, DialOptions options = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.");
            }

            if (!OpenTokUtils.ValidateSession(sessionId))
            {
                throw new OpenTokArgumentException("Session Id is not valid");
            }

            string url = $"v2/project/{this.ApiKey}/dial";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object>
            {
                { "sessionId", sessionId },
                { "token", token },
                { "sip", new {
                        uri = sipUri,
                        from = options?.From,
                        headers = options?.Headers,
                        auth = options?.Auth,
                        secure = options?.Secure,
                        video = options?.Video,
                        observeForceMute = options?.ObserveForceMute
                    }
                }
            };
            return Client.PostAsync(url, headers, data);
        }

        /// <summary>
        /// Force the publisher of a specific stream to mute its published audio.
        /// </summary>
        /// <para>
        /// Also see the <see cref="ForceMuteAll"/> and <see cref="ForceMuteStreamAsync"/> methods.
        /// </para>
        /// <param name="sessionId">The session ID of the session that includes the stream.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <exception cref="OpenTokArgumentException">Thrown when session or stream ID is invalid.</exception>
        /// <exception cref="OpenTokWebException">Thrown when an HTTP error has occurred.</exception>
        public void ForceMuteStream(string sessionId, string streamId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.", nameof(sessionId));
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be empty.", nameof(streamId));
            }

            string url = $"v2/project/{this.ApiKey}/session/{sessionId}/stream/{streamId}/mute";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            Client.Post(url, headers, null);
        }

        /// <summary>
        /// Force the publisher of a specific stream to mute its published audio.
        /// </summary>
        /// <para>
        /// Also see the <see cref="ForceMuteAll"/> and <see cref="ForceMuteStream"/> methods.
        /// </para>
        /// <param name="sessionId">The session ID of the session that includes the stream.</param>
        /// <param name="streamId">The stream ID.</param>
        /// <exception cref="OpenTokArgumentException">Thrown when session or stream ID is invalid.</exception>
        /// <exception cref="OpenTokWebException">Thrown when an HTTP error has occurred.</exception>
        public async Task ForceMuteStreamAsync(string sessionId, string streamId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.", nameof(sessionId));
            }

            if (string.IsNullOrEmpty(streamId))
            {
                throw new OpenTokArgumentException("The streamId cannot be empty.", nameof(streamId));
            }

            string url = $"v2/project/{this.ApiKey}/session/{sessionId}/stream/{streamId}/mute";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            await Client.PostAsync(url, headers, null);
        }

        /// <summary>
        /// Forces all streams (except for an optional list of streams) in a session to mute
        /// published audio.
        /// </summary>
        /// <para>
        /// In addition to existing streams, any streams that are published after the call to
        /// this method are published with audio muted. You can remove the mute state of a session
        /// by calling the <see cref="DisableForceMute"/> method.
        /// </para>
        /// <para>
        /// Also see the <see cref="ForceMuteAllAsync"/> and <see cref="ForceMuteStream"/> methods.
        /// </para>
        /// <param name="sessionId">The ID of session.</param>
        /// <param name="excludedStreamIds">The stream IDs of streams that will not be muted.</param>
        /// <exception cref="OpenTokArgumentException">Thrown when the session ID is invalid.</exception>
        /// <exception cref="OpenTokWebException">Thrown when an HTTP error has occurred.</exception>
        public void ForceMuteAll(string sessionId, string[] excludedStreamIds)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.", nameof(sessionId));
            }

            string url = $"v2/project/{ApiKey}/session/{sessionId}/mute";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "active", true }, { "excludedStreamIds", excludedStreamIds } };
            Client.Post(url, headers, data);
        }

        /// <summary>
        /// Forces all streams (except for an optional list of streams) in a session to mute
        /// published audio.
        /// </summary>
        /// <para>
        /// In addition to existing streams, any streams that are published after the call to
        /// this method are published with audio muted. You can remove the mute state of a session
        /// by calling the <see cref="DisableForceMuteAsync"/> method.
        /// </para>
        /// <para>
        /// Also see the <see cref="ForceMuteAll"/> and <see cref="ForceMuteStreamAsync"/> methods.
        /// </para>
        /// <param name="sessionId">The ID of session.</param>
        /// <param name="excludedStreamIds">The stream IDs of streams that will not be muted.</param>
        /// <exception cref="OpenTokArgumentException">Thrown when the session ID is invalid.</exception>
        /// <exception cref="OpenTokWebException">Thrown when an HTTP error has occurred.</exception>
        public async Task ForceMuteAllAsync(string sessionId, string[] excludedStreamIds)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.", nameof(sessionId));
            }

            string url = $"v2/project/{this.ApiKey}/session/{sessionId}/mute";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "active", true }, { "excludedStreamIds", excludedStreamIds } };
            await Client.PostAsync(url, headers, data);
        }

        /// <summary>
        /// Disables the active mute state of the session. After you call this method, new streams
        /// published to the session will no longer have audio muted.
        /// </summary>
        /// <para>
        /// After you call the <see cref="ForceMuteAll"/> method, any streams published after
        /// the call are published with audio muted. Call the <c>DisableForceMute()</c> method
        ///  to remove the mute state of a session, so that new published streams are not
        /// automatically muted.
        /// </para>
        /// <para>
        /// Also see the <see cref="DisableForceMuteAsync"/> method.
        /// </para>
        /// <param name="sessionId">The session ID.</param>
        /// <exception cref="OpenTokArgumentException">Thrown when the session ID is invalid.</exception>
        /// <exception cref="OpenTokWebException">Thrown when an HTTP error has occurred.</exception>
        public void DisableForceMute(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.", nameof(sessionId));
            }

            string url = $"v2/project/{ApiKey}/session/{sessionId}/mute";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "active", false } };
            Client.Post(url, headers, data);
        }

        /// <summary>
        /// Disables the active mute state of the session. After you call this method, new streams
        /// published to the session will no longer have audio muted.
        /// </summary>
        /// <para>
        /// After you call the <see cref="ForceMuteAllAsync"/> method, any streams published after
        /// the call are published with audio muted. Call the <c>DisableForceMuteAsync()</c> method
        //  to remove the mute state of a session, so that new published streams are not
        /// automatically muted.
        /// </para>
        /// <para>
        /// Also see the <see cref="DisableForceMutec"/> method.
        /// </para>
        /// <param name="sessionId">The session ID.</param>
        /// <exception cref="OpenTokArgumentException">Thrown when the session ID is invalid.</exception>
        /// <exception cref="OpenTokWebException">Thrown when an HTTP error has occurred.</exception>
        public async Task DisableForceMuteAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new OpenTokArgumentException("The sessionId cannot be empty.", nameof(sessionId));
            }

            string url = $"v2/project/{ApiKey}/session/{sessionId}/mute";

            var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var data = new Dictionary<string, object> { { "active", false } };
            await Client.PostAsync(url, headers, data);
        }
    }
}
