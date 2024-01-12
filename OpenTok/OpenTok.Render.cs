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
        ///     Retrieves an Experience Composer renderer.
        /// </summary>
        /// <param name="renderId">The Id of the rendering.</param>
        public async Task<RenderItem> GetRenderAsync(string renderId)
		{
			var url = this.BuildUrlWithRouteParameter(RenderEndpoint, renderId);
			var response = await this.Client.GetAsync(url);
			return JsonConvert.DeserializeObject<RenderItem>(response);
		}

        /// <summary>
        ///     Retrieves all Experience Composer renderers matching the provided request.
        /// </summary>
        /// <param name="request">The request containing filtering options.</param>
        /// <returns>The list of rendering.</returns>
        public async Task<ListRendersResponse> ListRendersAsync(ListRendersRequest request)
		{
			var url = this.BuildUrlWithQueryParameter(RenderEndpoint, request.ToQueryParameters());
			var response = await this.Client.GetAsync(url);
			return JsonConvert.DeserializeObject<ListRendersResponse>(response);
		}

        /// <summary>
        ///     Starts a new Experience Composer renderer for an OpenTok session.
        /// </summary>
        /// <para>
        ///     For more information, see the .
        ///     <a href="https://tokbox.com/developer/guides/experience-composer/">Experience Composer developer guide</a>.
        /// </para>
        /// <param name="request">The rendering request.</param>
        /// <returns>The generated rendering.</returns>
        public async Task<RenderItem> StartRenderAsync(StartRenderRequest request)
		{
			var response = await this.Client.PostAsync(
				this.BuildUrl(RenderEndpoint),
				GetHeaderDictionary("application/json"),
				request.ToDataDictionary());
			return JsonConvert.DeserializeObject<RenderItem>(response);
		}

        /// <summary>
        ///     Stops an Experience Composer renderer.
        /// </summary>
        /// <param name="renderId">The Id of the rendering.</param>
        public async Task StopRenderAsync(string renderId) =>
			await this.Client.DeleteAsync(
				this.BuildUrlWithRouteParameter(RenderEndpoint, renderId),
				new Dictionary<string, string>());

		private string BuildUrl(string endpoint) => $"v2/project/{this.ApiKey}{endpoint}";

		private string BuildUrlWithQueryParameter(string endpoint, string queryParameter) =>
			$"{this.BuildUrl(endpoint)}?{queryParameter}";

		private string BuildUrlWithRouteParameter(string endpoint, string routeParameter) =>
			$"{this.BuildUrl(endpoint)}/{routeParameter}";

		private static Dictionary<string, string> GetHeaderDictionary(string contentType) =>
			new Dictionary<string, string> {{"Content-Type", contentType}};
	}
}