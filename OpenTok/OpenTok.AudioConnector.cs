using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenTokSDK
{
    public partial class OpenTok
    {
        /// <summary>
        /// Sends audio from a Vonage Video API session to a WebSocket. For more information, see the <a href="https://tokbox.com/developer/guides/audio-connector/">Audio Connector developer guide</a>.
        /// </summary>
        /// <param name="request">The request to start the audio connector.</param>
        /// <returns>The response object from the server.</returns>
        public async Task<AudioConnector> StartAudioConnectorAsync(AudioConnectorStartRequest request)
        {
            var response = await this.Client.PostAsync(
                $"v2/project/{this.ApiKey}/connect",
                GetHeaderDictionary("application/json"),
                request.ToDataDictionary());
            return JsonConvert.DeserializeObject<AudioConnector>(response);
        }

        /// <summary>
        /// Stops sending audio for a Vonage Video API session.
        /// </summary>
        /// <param name="connectionId">The OpenTok connection ID for the Audio Connector WebSocket connection in the OpenTok session. See <see cref="AudioConnector.ConnectionId"/>.</param>
        public async Task StopAudioConnectorAsync(string connectionId) =>
            _ = await this.Client.PostAsync(
                $"v2/project/{this.ApiKey}/connect/{connectionId}/stop",
                new Dictionary<string, string>(),
                new Dictionary<string, object>());

        private static Dictionary<string, string> GetHeaderDictionary(string contentType) =>
            new Dictionary<string, string> {{"Content-Type", contentType}};
    }
}