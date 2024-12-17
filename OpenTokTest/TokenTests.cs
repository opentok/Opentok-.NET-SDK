using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using OpenTokSDK;
using OpenTokSDK.Exception;
using OpenTokSDK.Util;
using Xunit;

namespace OpenTokSDKTest
{
    public class T1TokenTests : TestBase
    {
        [Fact]
        public void GenerateInvalidTokensTest()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            Assert.Throws<OpenTokArgumentException>(() => opentok.GenerateT1Token(null));
            Assert.Throws<OpenTokArgumentException>(() => opentok.GenerateT1Token(""));
            Assert.Throws<OpenTokArgumentException>(() => opentok.GenerateT1Token(string.Empty));
            Assert.Throws<OpenTokArgumentException>(() => opentok.GenerateT1Token("NOT A VALID SESSION ID"));
        }

        [Fact]
        public void GenerateTokenTest()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            var token = opentok.GenerateT1Token(sessionId);
            Assert.NotNull(token);
            var data = CheckToken(token);
            Assert.Equal(data["partner_id"], ApiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], "publisher");
        }

        [Fact]
        public void GenerateTokenWithConnectionDataTest()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var connectionData = "Somedatafortheconnection";
            var sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            var token = opentok.GenerateT1Token(sessionId, data: connectionData);
            Assert.NotNull(token);
            var data = CheckToken(token);
            Assert.Equal(data["partner_id"], ApiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], "publisher");
            Assert.Equal(data["connection_data"], connectionData);
        }

        [Fact]
        public void GenerateTokenWithExpireTimeTest()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var expireTime = OpenTokUtils.GetCurrentUnixTimeStamp() + 10;
            var sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            var token = opentok.GenerateT1Token(sessionId, expireTime: expireTime);
            Assert.NotNull(token);
            var data = CheckToken(token);
            Assert.Equal(data["partner_id"], ApiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], "publisher");
            Assert.Equal(data["expire_time"], ((long) expireTime).ToString());
        }

        [Fact]
        public void GenerateTokenWithInitialLayoutClass()
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            var initalLayoutClassList = new List<string>();
            initalLayoutClassList.Add("focus");
            var token = opentok.GenerateT1Token(sessionId, initialLayoutClassList: initalLayoutClassList);
            Assert.NotNull(token);
            var data = CheckToken(token);
            Assert.Equal(data["partner_id"], ApiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], "publisher");
            Assert.Equal("focus", data["initial_layout_class_list"]);
        }

        [Theory]
        [InlineData(Role.SUBSCRIBER, "subscriber")]
        [InlineData(Role.PUBLISHER, "publisher")]
        [InlineData(Role.MODERATOR, "moderator")]
        [InlineData(Role.PUBLISHERONLY, "publisheronly")]
        public void GenerateTokenWithRoleTest(Role role, string expectedRole)
        {
            var opentok = new OpenTok(ApiKey, ApiSecret);
            var sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            var token = opentok.GenerateT1Token(sessionId, role);
            Assert.NotNull(token);
            var data = CheckToken(token);
            Assert.Equal(data["partner_id"], ApiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], expectedRole);
        }

        private Dictionary<string, string> CheckToken(string token)
        {
            var baseToken = OpenTokUtils.Decode64(token.Substring(4));
            char[] sep = {'&'};
            var tokenFields = baseToken.Split(sep);
            var tokenData = new Dictionary<string, string>();
            foreach (var t in tokenFields)
            {
                tokenData.Add(t.Split('=')[0], t.Split('=')[1]);
            }

            return tokenData;
        }
    }

    public class TokenTests
    {
        private const int ApiKey = 123456;
        private const string ApiSecret = "1234567890abcdef1234567890abcdef1234567890";
        private const string SessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
        private readonly OpenTok sut = new(ApiKey, ApiSecret);

        [Fact(Skip = "Until GA.")]
        public void GenerateToken_ShouldReturnTokenWithDefaultValues()
        {
            var token = sut.GenerateToken(SessionId);
            var claims = ExtractClaims(token);
            claims["iat"].Should().NotBeEmpty();
            claims["exp"].Should().NotBeEmpty();
            claims["jti"].Should().NotBeEmpty();
            claims["nonce"].Should().NotBeEmpty();
            claims.ContainsKey("exp").Should().BeTrue();
            claims["iss"].Should().Be(ApiKey.ToString());
            claims["ist"].Should().Be("project");
            claims["scope"].Should().Be("session.connect");
            claims["session_id"].Should().Be(SessionId);
            claims["role"].Should().Be("publisher");
            claims.ContainsKey("initial_layout_class_list").Should().BeFalse();
        }

        [Fact(Skip = "Until GA.")]
        public void GenerateToken_ShouldSetData()
        {
            var token = sut.GenerateToken(SessionId, data: "Some data.");
            ExtractClaims(token)["connection_data"].Should().Be("Some data.");
        }

        [Fact(Skip = "Until GA.")]
        public void GenerateToken_ShouldSetExpireTime()
        {
            var expireTime = new DateTimeOffset(DateTime.UtcNow.AddSeconds(30)).ToUnixTimeSeconds();
            var token = sut.GenerateToken(SessionId, expireTime: expireTime);
            var claims = ExtractClaims(token);
            claims["exp"].Should().Be(expireTime.ToString(CultureInfo.InvariantCulture));
        }

        [Fact(Skip = "Until GA.")]
        public void GenerateToken_ShouldSetInitialLayoutClass()
        {
            var list = new List<string> {"focus", "hello"};
            var token = sut.GenerateToken(SessionId, initialLayoutClassList: list);
            var claims = ExtractClaims(token);
            claims["initial_layout_list"].Should().Be("focus hello");
        }

        [Theory(Skip = "Until GA.")]
        [InlineData(Role.SUBSCRIBER, "subscriber")]
        [InlineData(Role.PUBLISHER, "publisher")]
        [InlineData(Role.MODERATOR, "moderator")]
        [InlineData(Role.PUBLISHERONLY, "publisheronly")]
        public void GenerateToken_ShouldSetRole(Role role, string expectedRole)
        {
            var token = sut.GenerateToken(SessionId, role);
            var claims = ExtractClaims(token);
            claims["role"].Should().Be(expectedRole);
        }

        private static Dictionary<string, string> ExtractClaims(string token)
        {
            var key = Encoding.ASCII.GetBytes(ApiSecret);
            new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out var validatedToken);
            var jwtToken = (JwtSecurityToken) validatedToken;
            return jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
            ;
        }
    }
}
