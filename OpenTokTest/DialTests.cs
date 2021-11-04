using OpenTokSDK;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokTest
{
    public class DialTests
    {
        private const int ApiKey = 123456;
        private const string ApiSecret = "1234567890abcdef1234567890abcdef1234567890";

        [Fact]
        public void DialCorrectData()
        {
            // arrange
            var expectedJson = "﻿﻿{\"sessionId\":\"SESSIONID\",\"token\":\"1234567890\",\"spi\":{\"uri\":\"SIPURI\"}}";

            // act
            string sessionId = "SESSIONID";
            string token = "1234567890";
            string sipUri = "SIPURI";

            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            opentok.Dial(sessionId, token, sipUri);

            // assert
            var requestJson = opentok.Client.LastRequest.Trim();

            Assert.True(expectedJson.Equals(requestJson, System.StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
