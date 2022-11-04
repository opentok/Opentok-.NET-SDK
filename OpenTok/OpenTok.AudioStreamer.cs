using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenTokSDK.AudioStreamer;

namespace OpenTokSDK
{
    public partial class OpenTok
    {
        private const string ConnectEndpoint = "connect";
        private const string ProjectEndpoint = "project";
        private const string StopEndpoint = "stop";

        /// <summary>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ConnectResponse> ConnectAudioStreamerAsync(string apiVersion, ConnectRequest request)
        {
            var response = await this.Client.PostAsync(
                string.Join("/", apiVersion, ProjectEndpoint, this.ApiKey, ConnectEndpoint),
                GetHeaderDictionary("application/json"),
                request.ToDataDictionary());
            return JsonConvert.DeserializeObject<ConnectResponse>(response);
        }

        /// <summary>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="callId"></param>
        /// <returns></returns>
        public async Task StopAudioStreamerAsync(string apiVersion, string callId) =>
            _ = await this.Client.PostAsync(
                string.Join("/", apiVersion, ProjectEndpoint, this.ApiKey, ConnectEndpoint, callId, StopEndpoint),
                new Dictionary<string, string>(),
                new Dictionary<string, object>());

        private static Dictionary<string, string> GetHeaderDictionary(string contentType) =>
            new Dictionary<string, string> {{"Content-Type", contentType}};
    }
}