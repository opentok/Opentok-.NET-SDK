using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using OpenTokSDK;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
	public class OpenTokCaptionsTests
	{
		private readonly int apiKey;
		private readonly Mock<HttpClient> mockClient;
		private readonly OpenTok sut;

		public OpenTokCaptionsTests()
		{
			var fixture = new Fixture();
			this.apiKey = fixture.Create<int>();
			this.mockClient = new Mock<HttpClient>();
			this.sut = new OpenTok(this.apiKey, fixture.Create<string>())
			{
				Client = this.mockClient.Object,
			};
		}

		[Fact]
		public async Task StartLiveCaptionsAsync_ShouldReturnResponse()
		{
			const string contentTypeKey = "Content-Type";
			const string contentType = "application/json";
			var expectedUrl = $"v2/project/{this.apiKey}/captions";
			var expectedResponse = new Caption {CaptionsId = Guid.NewGuid()};
			var serializedResponse = JsonConvert.SerializeObject(expectedResponse);
			var request = CaptionOptions.Build("sessionId", "token");
			this.mockClient.Setup(httpClient => httpClient.PostAsync(
					expectedUrl,
					It.Is<Dictionary<string, string>>(dictionary =>
						dictionary.ContainsKey(contentTypeKey) && dictionary[contentTypeKey] == contentType),
					It.Is<Dictionary<string, object>>(dictionary =>
						dictionary.SequenceEqual(request.ToDataDictionary()))))
				.ReturnsAsync(serializedResponse);
			var response = await this.sut.StartLiveCaptionsAsync(request);
			Assert.Equal(expectedResponse, response);
		}

		[Fact]
		public async Task StopLiveCaptionsAsync_ShouldReturnResponse()
		{
			const string contentTypeKey = "Content-Type";
			const string contentType = "application/json";
			var captionId = Guid.NewGuid();
			var expectedUrl = $"v2/project/{this.apiKey}/captions/{captionId}/stop";
			await this.sut.StopLiveCaptionsAsync(captionId);
			this.mockClient.Verify(httpClient => httpClient.PostAsync(
				expectedUrl,
				It.Is<Dictionary<string, string>>(dictionary =>
					dictionary.ContainsKey(contentTypeKey) && dictionary[contentTypeKey] == contentType),
				It.Is<Dictionary<string, object>>(dictionary => !dictionary.Any())), Times.Once);
		}
	}
}