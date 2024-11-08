#region

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OpenTokSDK.Exception;

#endregion

namespace OpenTokSDK.Util
{
    internal class TokenGenerator
    {
        public string GenerateToken(TokenData data)
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
                { "nonce", OpenTokUtils.GetRandomNumber() }
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