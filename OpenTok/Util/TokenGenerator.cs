#region

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.IdentityModel.Tokens;
using OpenTokSDK.Exception;
using Vonage;
using Vonage.Request;
using Vonage.Video.Authentication;

#endregion

namespace OpenTokSDK.Util
{
    internal class TokenGenerator
    {
        public string GenerateSessionToken(TokenData data)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = data.GenerateCredentials(),
                Claims = data.GeneratePayload(),
                Expires = data.GetExpireTime().UtcDateTime
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateSessionToken(string applicationId, string privateKey, string sessionId)
        {
            return new VideoTokenGenerator().GenerateToken(
                Credentials.FromAppIdAndPrivateKey(applicationId, privateKey),
                TokenAdditionalClaims.Parse(sessionId)).GetSuccessUnsafe().Token;
        }

        public string GenerateLegacyToken(int key, string secret, int expiryPeriod = 300)
        {
            var now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var expiry = now + expiryPeriod;
            var payload = new Dictionary<string, object>
            {
                { "iss", Convert.ToString(key) },
                { "ist", "project" },
                { "iat", now },
                { "exp", expiry }
            };
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secret);
            return token;
        }

        public string GenerateToken(string applicationId, string privateKey)
        {
            return new Jwt().GenerateToken(Credentials.FromAppIdAndPrivateKey(applicationId, privateKey))
                .GetSuccessUnsafe();
        }
    }

    internal struct TokenData
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string SessionId { get; set; }
        public Role Role { get; set; }
        public double ExpireTime { get; set; }
        public string Data { get; set; }
        public IEnumerable<string> InitialLayoutClasses { get; set; }

        public DateTimeOffset GetExpireTime()
        {
            return CheckExpireTime(ExpireTime, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds())
                ? DateTimeOffset.FromUnixTimeSeconds((long)ExpireTime)
                : new DateTimeOffset(DateTime.UtcNow.AddSeconds(300));
        }

        internal Dictionary<string, object> GeneratePayload()
        {
            var creationTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var payload = new Dictionary<string, object>
            {
                { "iss", ApiKey },
                { "ist", "project" },
                { "role", Role.ToString().ToLowerInvariant() },
                { "session_id", SessionId },
                { "scope", "session.connect" },
                { "iat", creationTime },
                { "jti", GenerateTokenId() },
                { "initial_layout_list", string.Join(" ", InitialLayoutClasses) },
                { "nonce", OpenTokUtils.GetRandomNumber() },
                { "data", this.Data },
            };

            return payload;
        }

        private static bool CheckExpireTime(double expireTime, double createTime)
        {
            if (expireTime == 0) return false;
            if (expireTime > createTime && expireTime <= OpenTokUtils.GetCurrentUnixTimeStamp() + 2592000) return true;
            throw new OpenTokArgumentException(
                $"Invalid expiration time for token {expireTime}. Expiration time  has to be positive and less than 30 days");
        }

        private static string GenerateTokenId()
        {
            var tokenData = new byte[64];
            RandomNumberGenerator.Create().GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }

        internal SigningCredentials GenerateCredentials()
        {
            var key = Encoding.ASCII.GetBytes(ApiSecret);
            return new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        }
    }
}