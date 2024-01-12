using System;
using System.Collections.Generic;
using Xunit;
using OpenTokSDK;
using OpenTokSDK.Util;
using OpenTokSDK.Exception;

namespace OpenTokSDKTest
{
    public class TokenTests : TestBase
    {
        [Fact]
        public void GenerateTokenTest()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            
            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";          
            string token = opentok.GenerateToken(sessionId);

            Assert.NotNull(token);
            var data = CheckToken(token);

            Assert.Equal(data["partner_id"], ApiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], "publisher");
        }

        [Fact]
        public void GenerateTokenWithRoleTest()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            string token = opentok.GenerateToken(sessionId, role:Role.SUBSCRIBER);

            Assert.NotNull(token);
            var data = CheckToken(token);

            Assert.Equal(data["partner_id"], ApiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"], "subscriber");
        }
        
        [Fact]
        public void GenerateTokenWithExpireTimeTest()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            double expireTime = OpenTokUtils.GetCurrentUnixTimeStamp() + 10;

            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            string token = opentok.GenerateToken(sessionId, expireTime: expireTime);

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
        public void GenerateTokenWithConnectionDataTest()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            string connectionData =  "Somedatafortheconnection";
            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            string token = opentok.GenerateToken(sessionId, data:connectionData);

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
        public void GenerateTokenWithInitialLayoutClass()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);

            String sessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";
            List<string> initalLayoutClassList = new List<string>();
            initalLayoutClassList.Add("focus");
            string token = opentok.GenerateToken(sessionId, initialLayoutClassList: initalLayoutClassList);

            Assert.NotNull(token);
            var data = CheckToken(token);

            Assert.Equal(data["partner_id"], ApiKey.ToString());
            Assert.NotNull(data["sig"]);
            Assert.NotNull(data["create_time"]);
            Assert.NotNull(data["nonce"]);
            Assert.Equal(data["role"],"publisher");
            Assert.Equal("focus", data["initial_layout_class_list"]);
        }

        [Fact]
        public void GenerateInvalidTokensTest()
        {
            OpenTok opentok = new OpenTok(ApiKey, ApiSecret);
            Assert.Throws<OpenTokArgumentException>(() => opentok.GenerateToken(null));
            Assert.Throws<OpenTokArgumentException>(() => opentok.GenerateToken(""));
            Assert.Throws<OpenTokArgumentException>(() => opentok.GenerateToken(string.Empty));
            Assert.Throws<OpenTokArgumentException>(() => opentok.GenerateToken("NOT A VALID SESSION ID"));
        }
        

        private Dictionary<string,string> CheckToken(string token)
        {
            string baseToken = OpenTokUtils.Decode64(token.Substring(4));
            char[] sep = { '&' };
            string[] tokenFields = baseToken.Split(sep);
            var tokenData = new Dictionary<string, string>();

            foreach (var t in tokenFields)
            {
                tokenData.Add(t.Split('=')[0], t.Split('=')[1]);
            }
            return tokenData;
        }
    }
}

