using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenTokSDK
{
	public partial class OpenTok
	{
		private const string CaptionsEndpoint = "/captions";
		
		/// <summary>
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public async Task<Caption> StartLiveCaptionsAsync(CaptionOptions options)
		{
			var response = await this.Client.PostAsync(
				this.BuildUrl(CaptionsEndpoint),
				GetHeaderDictionary("application/json"),
				options.ToDataDictionary());
			return JsonConvert.DeserializeObject<Caption>(response);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public Task StopLiveCaptionsAsync(Guid captionId) =>
			this.Client.PostAsync(
				$"{this.BuildUrlWithRouteParameter(CaptionsEndpoint, captionId.ToString())}/stop",
				GetHeaderDictionary("application/json"), new Dictionary<string, object>());
	}
}