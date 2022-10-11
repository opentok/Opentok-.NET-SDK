using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenTokSDK.Render;

namespace OpenTokSDK
{
    public partial class OpenTok
    {
        private const string RenderEndpoint = "/render";

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="request">TODO</param>
        /// <returns>TODO</returns>
        public async Task<StartRenderResponse> StartRenderAsync(StartRenderRequest request)
        {
            var response = await this.Client.PostAsync(
                this.BuildUrl(RenderEndpoint),
                GetHeaderDictionary("application/json"),
                request.ToDataDictionary());
            return JsonConvert.DeserializeObject<StartRenderResponse>(response);
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="renderId">TODO</param>
        public async Task StopRenderAsync(string renderId) =>
            await this.Client.DeleteAsync(
                this.BuildUrlWithQuery(RenderEndpoint, renderId),
                new Dictionary<string, string>());

        private string BuildUrl(string endpoint) => $"v2/project/{ApiKey}{endpoint}";

        private string BuildUrlWithQuery(string endpoint, string query) => $"{this.BuildUrl(endpoint)}/{query}";

        private static Dictionary<string, string> GetHeaderDictionary(string contentType) =>
            new Dictionary<string, string> {{"Content-Type", contentType}};
    }
}