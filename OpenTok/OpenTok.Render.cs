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
        public async Task<RenderItem> StartRenderAsync(StartRenderRequest request)
        {
            var response = await this.Client.PostAsync(
                this.BuildUrl(RenderEndpoint),
                GetHeaderDictionary("application/json"),
                request.ToDataDictionary());
            return JsonConvert.DeserializeObject<RenderItem>(response);
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="renderId">TODO</param>
        public async Task StopRenderAsync(string renderId) =>
            await this.Client.DeleteAsync(
                this.BuildUrlWithRouteParameter(RenderEndpoint, renderId),
                new Dictionary<string, string>());

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="request">TODO</param>
        /// <returns>TODO</returns>
        public async Task<ListRendersResponse> ListRendersAsync(ListRendersRequest request)
        {
            var url = this.BuildUrlWithQueryParameter(RenderEndpoint, request.ToQueryParameters());
            var response = await this.Client.GetAsync(url);
            return JsonConvert.DeserializeObject<ListRendersResponse>(response);
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="renderId">TODO</param>
        public async Task<RenderItem> GetRenderAsync(string renderId)
        {
            var url = this.BuildUrlWithRouteParameter(RenderEndpoint, renderId);
            var response = await this.Client.GetAsync(url);
            return JsonConvert.DeserializeObject<RenderItem>(response);
        }

        private string BuildUrl(string endpoint) => $"v2/project/{this.ApiKey}{endpoint}";

        private string BuildUrlWithRouteParameter(string endpoint, string routeParameter) =>
            $"{this.BuildUrl(endpoint)}/{routeParameter}";

        private string BuildUrlWithQueryParameter(string endpoint, string queryParameter) =>
            $"{this.BuildUrl(endpoint)}?{queryParameter}";

        private static Dictionary<string, string> GetHeaderDictionary(string contentType) =>
            new Dictionary<string, string> {{"Content-Type", contentType}};
    }
}