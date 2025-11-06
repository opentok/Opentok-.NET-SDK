using System;
using System.Collections.Generic;
using System.Linq;
using OpenTokSDK;
using OpenTokSDK.Exception;
using Xunit;

namespace OpenTokSDKTest
{
	public class CaptionOptionsTest
	{
		[Fact]
		public void Build_ShouldDisablePartialCaptionsByDefault() =>
			Assert.True(CaptionOptions.Build("sessionId", "token").PartialCaptions);
		
		[Fact]
		public void Build_ShouldHaveNoCallbackUri() =>
			Assert.Null(CaptionOptions.Build("sessionId", "token").StatusCallbackUrl);

		[Fact]
		public void Build_ShouldSetDefaultLanguageCode() => Assert.Equal(CaptionOptions.LanguageCodeValue.EnUs,
			CaptionOptions.Build("sessionId", "token").LanguageCode);

		[Fact]
		public void Build_ShouldSetDefaultMaxDuration() => Assert.Equal(new TimeSpan(4, 0, 0),
			CaptionOptions.Build("sessionId", "token").MaxDuration);

		[Fact]
		public void Build_ShouldSetSessionId() =>
			Assert.Equal("sessionId", CaptionOptions.Build("sessionId", "token").SessionId);

		[Fact]
		public void Build_ShouldSetToken() => Assert.Equal("token",
			CaptionOptions.Build("sessionId", "token").Token);

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(null)]
		public void Build_ShouldThrowException_GivenSessionIdIsNullOrEmpty(string invalidSessionId)
		{
			void Act() => CaptionOptions.Build(invalidSessionId, "token");
			var exception = Assert.Throws<OpenTokException>(Act);
			Assert.Equal("SessionId cannot be null or empty.", exception.Message);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(null)]
		public void Build_ShouldThrowException_GivenTokenIdIsNullOrEmpty(string invalidToken)
		{
			void Act() => CaptionOptions.Build("sessionId", invalidToken);
			var exception = Assert.Throws<OpenTokException>(Act);
			Assert.Equal("Token cannot be null or empty.", exception.Message);
		}

		[Fact]
		public void DisablePartialCaptions_ShouldDisablePartialCaptions() => Assert.False(
			CaptionOptions.Build("sessionId", "token").DisablePartialCaptions().PartialCaptions);

		[Fact]
		public void WithLanguageCode_ShouldSetLanguageCode() => Assert.Equal(CaptionOptions.LanguageCodeValue.FrFr,
			CaptionOptions.Build("sessionId", "token").WithLanguageCode(CaptionOptions.LanguageCodeValue.FrFr)
				.LanguageCode);

		[Fact]
		public void WithMaxDuration_ShouldSetMaxDuration() => Assert.Equal(new TimeSpan(1, 0, 0),
			CaptionOptions.Build("sessionId", "token").WithMaxDuration(new TimeSpan(1, 0, 0))
				.MaxDuration);

		[Fact]
		public void WithMaxDuration_ShouldThrowException_GivenValueIsExceededMaximum()
		{
			void Act() => CaptionOptions.Build("sessionId", "token")
				.WithMaxDuration(new TimeSpan(4, 0, 1));

			var exception = Assert.Throws<OpenTokException>(Act);
			Assert.Equal("Max duration cannot exceed 4 hours.", exception.Message);
		}
		
		[Fact]
		public void WithMaxDuration_ShouldThrowException_GivenValueIsLowerThanMinimum()
		{
			void Act() => CaptionOptions.Build("sessionId", "token")
				.WithMaxDuration(new TimeSpan(0, 4, 0));

			var exception = Assert.Throws<OpenTokException>(Act);
			Assert.Equal("Max duration cannot be lower than 5 minutes.", exception.Message);
		}
		
		[Fact]
		public void WithCallbackUrl_ShouldSetCallbackUrl() => Assert.Equal(new Uri("http://example.com"),
			CaptionOptions.Build("sessionId", "token").WithCallbackUrl(new Uri("http://example.com"))
				.StatusCallbackUrl);

		[Fact]
		public void ToDataDictionary_ShouldReturnDataDictionary()
		{
			var expectedData = new Dictionary<string, object>
			{
				{"sessionId", "sessionId"},
				{"token", "token"},
				{"languageCode", "en-AU"},
				{"maxDuration", (double)3600},
				{"partialCaptions", false},
				{"statusCallbackUrl", "http://example.com/"},
			};
			var data = CaptionOptions.Build("sessionId", "token")
				.WithLanguageCode(CaptionOptions.LanguageCodeValue.EnAu)
				.WithMaxDuration(new TimeSpan(1, 0, 0))
				.WithCallbackUrl(new Uri("http://example.com"))
				.DisablePartialCaptions()
				.ToDataDictionary();
			Assert.Equal(data, expectedData);
		}
		
		[Fact]
		public void ToDataDictionary_ShouldExcludeStatusCallbackUrl_GivenUrlIsNull()
		{
			var expectedData = new Dictionary<string, object>
			{
				{"sessionId", "sessionId"},
				{"token", "token"},
				{"languageCode", "en-AU"},
				{"maxDuration", (double)300},
				{"partialCaptions", false},
			};
			var data = CaptionOptions.Build("sessionId", "token")
				.WithLanguageCode(CaptionOptions.LanguageCodeValue.EnAu)
				.WithMaxDuration(new TimeSpan(0, 5, 0))
				.DisablePartialCaptions()
				.ToDataDictionary();
			Assert.Equal(data, expectedData);
		}
	}
}