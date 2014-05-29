using Xunit;
using OpenTokSDK;
using System;
using OpenTokSDK.Util;
using OpenTokSDK.Exceptions;

namespace Tests
{
    public class ApiTest
    {
        private int apiKey = 0;
        private string apiSecret = "*** YOUR API SECRET ***";

        [Fact]
        public void OpenTokTest()
        {            
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Assert.Equal(opentok.ApiKey, apiKey);
            Assert.Equal(opentok.ApiSecret, apiSecret);
        }

        [Fact]
        public void CreateSessionTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession();
            Console.Out.WriteLine("SessionId: %s", session.Id);

            Assert.NotNull(session);
            Assert.Equal(session.ApiKey, apiKey);
            Assert.Equal(session.ApiSecret, apiSecret);
            Assert.Equal(session.MediaMode, MediaMode.ROUTED);
            Assert.Equal(session.Location, "");
            Assert.True(ValidateSession(session.Id));
        }

        [Fact]
        public void CreateRelayedSessionTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession(mediaMode: MediaMode.RELAY);

            Assert.NotNull(session);
            Assert.Equal(session.ApiKey, apiKey);
            Assert.Equal(session.ApiSecret, apiSecret);
            Assert.Equal(session.MediaMode, MediaMode.RELAY);
            Assert.Equal(session.Location, "");
            Assert.True(ValidateSession(session.Id));
        }

        [Fact]
        public void CreateSessionWithLocationTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession(location: "0.0.0.0");

            Assert.NotNull(session);
            Assert.Equal(session.ApiKey, apiKey);
            Assert.Equal(session.ApiSecret, apiSecret);
            Assert.Equal(session.MediaMode, MediaMode.ROUTED);
            Assert.Equal(session.Location, "0.0.0.0");
            Assert.True(ValidateSession(session.Id));
        }

        [Fact]
        public void CreateLocationSessionTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session;
            try
            {
                session = opentok.CreateSession(location: "A location");
                Assert.True(false);
            }
            catch(OpenTokArgumentException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void GenerateTokenTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession(mediaMode: MediaMode.RELAY);
            string token = session.GenerateToken();

            CheckToken(token, apiKey);
        }

        [Fact]
        public void GenerateSubscriberTokenTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession(mediaMode: MediaMode.RELAY);
            string token = session.GenerateToken(role: Role.SUBSCRIBER);

            CheckToken(token, apiKey);
        }

        [Fact]
        public void GenerateTokenWithDataTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession(mediaMode: MediaMode.RELAY);
            string token = session.GenerateToken(data: "This is some data");

            CheckToken(token, apiKey);
        }

        [Fact]
        public void GenerateTokenWithExpireTimeTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession(mediaMode: MediaMode.RELAY);
            double expireTime = OpenTokUtils.GetCurrentUnixTimeStamp() + 10;

            string token = session.GenerateToken(expireTime: expireTime);
            CheckToken(token, apiKey);
        }

        [Fact]
        public void GenerateComplexTokenTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession();
            double expireTime = OpenTokUtils.GetCurrentUnixTimeStamp() + 10;

            string token = session.GenerateToken(role: Role.MODERATOR, expireTime: expireTime, data: "Connection data");
            CheckToken(token, apiKey);
        }

        [Fact]
        public void GenerateTokenWithInvalidExpiryTimeTest()
        {
            OpenTok opentok = new OpenTok(apiKey, apiSecret);
            Session session = opentok.CreateSession();
            double expireTime = OpenTokUtils.GetCurrentUnixTimeStamp() - 1;
            string token;
            try
            {
                token = session.GenerateToken(role: Role.MODERATOR, expireTime: expireTime, data: "Connection data");
                Assert.False(true);
            }
            catch(OpenTokArgumentException)
            {
                Assert.True(true);
            }
        }

        private bool ValidateSession(string sessionId)
        {
            try
            {
                return GetPartnerIdFromSessionId(sessionId) > 0;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private int GetPartnerIdFromSessionId(string sessionId)
        {
            if (String.IsNullOrEmpty(sessionId))
            {
                throw new FormatException("SessionId can not be empty");
            }

            string formatedSessionId = sessionId.Replace('-', '+');
            string[] splittedSessionId = OpenTokUtils.SplitString(formatedSessionId, '_', 2);
            if (splittedSessionId == null)
            {
                throw new FormatException("Session id could not be decoded");
            }

            string decodedSessionId = OpenTokUtils.Decode64(splittedSessionId[1]);

            string[] sessionParameters = OpenTokUtils.SplitString(decodedSessionId, '~', 0);
            if (sessionParameters == null)
            {
                throw new FormatException("Session id could not be decoded");
            }

            return Convert.ToInt32(sessionParameters[1]);
        }
        private void IsSessionCorrect(string sessionId)
        {
            Assert.True(ValidateSession(sessionId));
        }

        private void CheckCorrectApiKey(string sessionId, int apiKey)
        {
            int sessionApiKey = GetPartnerIdFromSessionId(sessionId);
            Assert.Equal(sessionApiKey, apiKey);
        }

        private void CheckToken(string token, int apiKey)
        {
            string baseToken = OpenTokUtils.Decode64(token.Substring(4));
            const string partnerId = "partner_id=";
            const string sig = "sig=";


            char[] sep = { '&' };
            string[] tokenFields = baseToken.Split(sep);

            Assert.True(tokenFields[0].StartsWith(partnerId));
            Assert.True(tokenFields[1].StartsWith(sig));
            Assert.Equal(Convert.ToInt32(tokenFields[0].Substring(partnerId.Length)), apiKey);
        }
    }
}
