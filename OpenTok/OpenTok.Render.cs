using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenTokSDK.Render;

namespace OpenTokSDK
{
    public partial class OpenTok
    {
        /// <summary>
        ///     Indicates the StartRender endpoint (/render)
        /// </summary>
        public const string StartRenderEndpoint = "/render";

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="request">TODO</param>
        /// <returns>TODO</returns>
        public async Task<StartRenderResponse> StartRenderAsync(StartRenderRequest request)
        {
            var response = await this.Client.PostAsync(
                this.BuildUrl(StartRenderEndpoint),
                GetHeaderDictionary("application/json"),
                request.ToDataDictionary());
            return JsonConvert.DeserializeObject<StartRenderResponse>(response);
        }

        private string BuildUrl(string endpoint) => $"v2/project/{ApiKey}{endpoint}";

        private static Dictionary<string, string> GetHeaderDictionary(string contentType) =>
            new Dictionary<string, string> {{"Content-Type", contentType}};
    }
}