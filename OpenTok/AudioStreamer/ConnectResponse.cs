namespace OpenTokSDK.AudioStreamer
{
    /// <summary>
    ///     Response of the Connect action.
    /// </summary>
    public struct ConnectResponse
    {
        /// <summary>
        ///     Identifier of the outgoing WebSocket connect call that can be used for debugging purposes and for other requests in
        ///     the future.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Opentok client connectionId that has been created. This connection will subscribe and forward the streams defined
        ///     in the payload to the WebSocket, as any other participant, will produce a connectionCreated event on the session.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        ///     Creates a new instance of ConnectResponse.
        /// </summary>
        /// <param name="id">
        ///     Identifier of the outgoing WebSocket connect call that can be used for debugging purposes and for
        ///     other requests in the future.
        /// </param>
        /// <param name="connectionId">
        ///     Opentok client connectionId that has been created. This connection will subscribe and
        ///     forward the streams defined in the payload to the WebSocket, as any other participant, will produce a
        ///     connectionCreated event on the session.
        /// </param>
        public ConnectResponse(string id, string connectionId)
        {
            this.Id = id;
            this.ConnectionId = connectionId;
        }
    }
}