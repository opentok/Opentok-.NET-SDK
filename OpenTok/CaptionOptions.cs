using System;
using System.Collections.Generic;
using System.ComponentModel;
using EnumsNET;
using OpenTokSDK.Exception;

namespace OpenTokSDK
{
	/// <summary>
	///     Represents options to start live captions.
	/// </summary>
	public class CaptionOptions
	{
		private CaptionOptions(string sessionId, string token, LanguageCodeValue languageCode = LanguageCodeValue.EnUs,
			Uri statusCallbackUrl = null, TimeSpan? maxDuration = null, bool partialCaptions = true)
		{
			this.LanguageCode = languageCode;
			this.PartialCaptions = partialCaptions;
			this.SessionId = sessionId;
			this.StatusCallbackUrl = statusCallbackUrl;
			this.Token = token;
			this.MaxDuration = maxDuration ?? new TimeSpan(4, 0, 0);
		}

		/// <summary>
		///     The BCP-47 code for a spoken language used on this call. The default value is "en-US".
		/// </summary>
		public LanguageCodeValue LanguageCode { get; }

		/// <summary>
		///     The maximum duration for the audio captioning, in seconds. The default value is 14,400 seconds (4 hours).
		/// </summary>
		public TimeSpan MaxDuration { get; }

		/// <summary>
		///     Whether to enable this to faster captioning at the cost of some degree of inaccuracies. The default value is true.
		/// </summary>
		public bool PartialCaptions { get; }

		/// <summary>
		///     The session ID of the OpenTok session. The audio from Publishers publishing into this session will be used to
		///     generate the captions.
		/// </summary>
		public string SessionId { get; }

		/// <summary>
		///     A publicly reachable URL controlled by the customer and capable of generating the content to be rendered without
		///     user intervention. The minimum length of the URL is 15 characters and the maximum length is 2048 characters. For
		///     more information, see
		///     <see href="https://tokbox.com/developer/guides/live-captions/#live-caption-status-updates">
		///         Live Caption status
		///         updates
		///     </see>
		///     .
		/// </summary>
		public Uri StatusCallbackUrl { get; }

		/// <summary>
		///     A valid OpenTok token with role set to Moderator.
		/// </summary>
		public string Token { get; }

		/// <summary>
		///     Initializes a caption options with mandatory values.
		/// </summary>
		/// <param name="sessionId">
		///     The session ID of the OpenTok session. The audio from Publishers publishing into this session
		///     will be used to generate the captions.
		/// </param>
		/// <param name="token">A valid OpenTok token with role set to Moderator.</param>
		/// <returns>The options.</returns>
		public static CaptionOptions Build(string sessionId, string token)
		{
			ValidateSessionId(sessionId);
			ValidateToken(token);
			return new CaptionOptions(sessionId, token);
		}

		/// <summary>
		///     Disables partial captions.
		/// </summary>
		/// <returns>The options.</returns>
		public CaptionOptions DisablePartialCaptions() => new CaptionOptions(this.SessionId, this.Token,
			this.LanguageCode, this.StatusCallbackUrl, this.MaxDuration, false);

		/// <summary>
		///     Specifies callback Url.
		/// </summary>
		/// <param name="uri">
		///     A publicly reachable URL controlled by the customer and capable of generating the content to be
		///     rendered without user intervention. The minimum length of the URL is 15 characters and the maximum length is 2048
		///     characters. For more information, see &lt;see
		///     href="https://tokbox.com/developer/guides/live-captions/#live-caption-status-updates"&gt;Live Caption status
		///     updates&lt;/see&gt;.
		/// </param>
		/// <returns>The options.</returns>
		public CaptionOptions WithCallbackUrl(Uri uri) => new CaptionOptions(this.SessionId, this.Token,
			this.LanguageCode, uri, this.MaxDuration, this.PartialCaptions);

		/// <summary>
		///     Specifies language code.
		/// </summary>
		/// <param name="code">The BCP-47 code for a spoken language used on this call. The default value is "en-US".</param>
		/// <returns>The options.</returns>
		public CaptionOptions WithLanguageCode(LanguageCodeValue code) => new CaptionOptions(this.SessionId, this.Token,
			code, this.StatusCallbackUrl, this.MaxDuration, this.PartialCaptions);

		/// <summary>
		///     Specifies the maximum duration.
		/// </summary>
		/// <param name="maxDuration">
		///     The maximum duration for the audio captioning, in seconds. The default value is 14,400
		///     seconds (4 hours).
		/// </param>
		/// <returns>The options.</returns>
		public CaptionOptions WithMaxDuration(TimeSpan maxDuration)
		{
			ValidateMaxDuration(maxDuration);
			return new CaptionOptions(this.SessionId, this.Token, this.LanguageCode, this.StatusCallbackUrl,
				maxDuration, this.PartialCaptions);
		}

		private static void ValidateMaxDuration(TimeSpan timeSpan)
		{
			if (timeSpan > new TimeSpan(4, 0, 0))
			{
				throw new OpenTokException("Max duration cannot exceed 4 hours.");
			}
		}

		private static void ValidateSessionId(string sessionId)
		{
			if (string.IsNullOrWhiteSpace(sessionId))
			{
				throw new OpenTokException("SessionId cannot be null or empty.");
			}
		}

		private static void ValidateToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
			{
				throw new OpenTokException("Token cannot be null or empty.");
			}
		}

		/// <summary>
		///     Represents the BCP-47 code for a spoken language used on this call.
		/// </summary>
		public enum LanguageCodeValue
		{
			/// <summary>
			///     en-AU (English, Australia)
			/// </summary>
			[Description("en-AU")]
			EnAu,

			/// <summary>
			///     en-US (English, US)
			/// </summary>
			[Description("en-US")]
			EnUs,

			/// <summary>
			///     en-GB (English, UK)
			/// </summary>
			[Description("en-GB")]
			EnGb,

			/// <summary>
			///     zh-CN (Chinese, Simplified)
			/// </summary>
			[Description("zh-CN")]
			ZhCn,

			/// <summary>
			///     fr-FR (French)
			/// </summary>
			[Description("fr-FR")]
			FrFr,

			/// <summary>
			///     fr-CA (French, Canadian)
			/// </summary>
			[Description("fr-CA")]
			FrCa,

			/// <summary>
			///     de-DE (German)
			/// </summary>
			[Description("de-DE")]
			DeDe,

			/// <summary>
			///     hi-IN (Hindi, Indian)
			/// </summary>
			[Description("hi-IN")]
			HiIn,

			/// <summary>
			///     it-IT (Italian)
			/// </summary>
			[Description("it-IT")]
			ItIt,

			/// <summary>
			///     ja-JP (Japanese)
			/// </summary>
			[Description("ja-JP")]
			JaJp,

			/// <summary>
			///     ko-KR (Korean)
			/// </summary>
			[Description("ko-KR")]
			KoKr,

			/// <summary>
			///     pt-BR (Portuguese, Brazilian)
			/// </summary>
			[Description("pt-BR")]
			PrBr,

			/// <summary>
			///     th-TH (Thai)
			/// </summary>
			[Description("th-TH")]
			ThTh,
		}

		internal Dictionary<string, object> ToDataDictionary()
		{
			var dictionary = new Dictionary<string, object>
			{
				{"sessionId", this.SessionId},
				{"token", this.Token},
				{"languageCode", this.LanguageCode.AsString(EnumFormat.Description)},
				{"maxDuration", this.MaxDuration.TotalMinutes},
				{"partialCaptions", this.PartialCaptions},
			};
			if (this.StatusCallbackUrl != null)
			{
				dictionary.Add("statusCallbackUrl",  this.StatusCallbackUrl.AbsoluteUri);
			}
			return dictionary;
		}
	}
}