namespace OpenTokSDK
{
    /// <summary>
    ///     Represents an Audio Connector.
    /// </summary>
    public struct AudioConnector
    {
        /// <summary>
        ///     The OpenTok connection ID for the Audio Connector WebSocket connection in the OpenTok session.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        ///     A unique ID identifying the Audio Connector WebSocket connection.
        /// </summary>
        public string Id { get; set; }
        
        
    }
}