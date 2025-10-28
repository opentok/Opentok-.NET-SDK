namespace OpenTokSDK
{
    /// <summary>
    ///     A connection
    /// </summary>
    public struct Connection
    {
        /// <summary>
        ///     The connection ID.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        ///     The state of the connection. Must be one of Connecting, Connected
        /// </summary>
        public string ConnectionState { get; set; }

        /// <summary>
        ///     The timestamp for when the connection was created, expressed in milliseconds since the Unix epoch (January 1, 1970,
        ///     00:00:00 UTC).
        /// </summary>
        public long CreatedAt { get; set; }
    }

    /// <summary>
    ///     The list of connections.
    /// </summary>
    public struct ConnectionList
    {
        /// <summary>
        ///     The total number of connections in the session.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     Your Vonage Application ID.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        ///     The session ID.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// </summary>
        public Connection[] Items { get; set; }
    }
}