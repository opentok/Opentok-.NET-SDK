///
/// OpenTok .NET Library
/// Last Updated November 16, 2011
/// https://github.com/opentok/Opentok-.NET-SDK
///

using System;
using System.Collections.Generic;
using System.Xml;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.IO;

namespace OpenTok
{
    public class OpenTokSDK
    {
        public string CreateSession(string location)
        {
            Dictionary<string, object> options = new Dictionary<string, object>();
            
            return CreateSession(location, options);
        }

        public string CreateSession(string location, Dictionary<string, object> options)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            options.Add("location", location);
            options.Add("partner_id", appSettings["opentok_key"]);

            XmlDocument xmlDoc = CreateSessionId(string.Format("{0}/session/create", appSettings["opentok_server"]), options);

            string session_id = xmlDoc.GetElementsByTagName("session_id")[0].ChildNodes[0].Value;

            return session_id;
        }

        public string GenerateToken(string sessionId)
        {
            Dictionary<string, object> options = new Dictionary<string, object>();

            return GenerateToken(sessionId, options);
        }

        public string GenerateToken(string sessionId, Dictionary<string, object> options)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            options.Add("session_id", sessionId);
            options.Add("create_time", (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);            
            options.Add("nonce", RandomNumber(0, 999999));
            if (!options.ContainsKey(TokenPropertyConstants.ROLE))
            {
                options.Add(TokenPropertyConstants.ROLE, "publisher");
            }
            // Convert expire time to Unix Timestamp
            if (options.ContainsKey(TokenPropertyConstants.EXPIRE_TIME)) {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0);
                DateTime expireTime = (DateTime) options[TokenPropertyConstants.EXPIRE_TIME];
                TimeSpan diff = expireTime - origin;
                options[TokenPropertyConstants.EXPIRE_TIME] = Math.Floor(diff.TotalSeconds);
            }

            string dataString = string.Empty;
            foreach (KeyValuePair<string, object> pair in options)
            {
                dataString += pair.Key + "=" + HttpUtility.UrlEncode(pair.Value.ToString()) + "&";
            }
            dataString = dataString.TrimEnd('&');

            string sig = SignString(dataString, appSettings["opentok_secret"].Trim());
            string token = string.Format("{0}{1}", appSettings["opentok_token_sentinel"], EncodeTo64(string.Format("partner_id={0}&sdk_version={1}&sig={2}:{3}", appSettings["opentok_key"], appSettings["opentok_sdk_version"], sig, dataString)));

            return token;
        }

        static private string EncodeTo64(string data)
        {
            byte[] encData_byte = new byte[data.Length];
            encData_byte = Encoding.UTF8.GetBytes(data);
            string encodedData = Convert.ToBase64String(encData_byte);

            return encodedData;
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private string SignString(string message, string key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            ///Make sure to utilize ToLower() method, else an exception willl be thrown
            ///Exception: 1006::Connecting to server to fetch session info failed.
            string result = ByteToString(hashmessage).ToLower();

            return result;
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                ///Hex format
                sbinary += buff[i].ToString("X2");
            }
            return (sbinary);
        }

        private XmlDocument CreateSessionId(string uri, Dictionary<string, object> dict)
        {
            XmlDocument xmlDoc = new XmlDocument();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            string postData = string.Empty;
            foreach (KeyValuePair<string, object> pair in dict)
            {
                postData += pair.Key + "=" + HttpUtility.UrlEncode(pair.Value.ToString()) + "&";
            }
            postData = postData.Substring(0, postData.Length - 1);
            byte[] postBytes = Encoding.UTF8.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            request.Headers.Add("X-TB-PARTNER-AUTH", string.Format("{0}:{1}", appSettings["opentok_key"], appSettings["opentok_secret"].Trim()));

            Stream requestStream = request.GetRequestStream();

            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (XmlReader reader = XmlReader.Create(response.GetResponseStream(), new XmlReaderSettings() { CloseInput = true }))
                    {
                        xmlDoc.Load(reader);
                    }
                }
            }

            return xmlDoc;
        }
    }

    public class SessionPropertyConstants
    {
        public const string ECHOSUPRESSION_ENABLED = "echoSuppression.enabled";
        public const string MULTIPLEXER_NUMOUTPUTSTREAMS = "multiplexer.numOutputStreams";
        public const string MULTIPLEXER_SWITCHTYPE = "multiplexer.switchType";
        public const string MULTIPLEXER_SWITCHTIMEOUT = "multiplexer.switchTimeout";
        public const string P2P_PREFERENCE = "p2p.preference";
    }

    public class TokenPropertyConstants
    {
        public const string ROLE = "role";
        public const string EXPIRE_TIME = "expire_time";
        public const string CONNECTION_DATA = "connection_data";
    }

    public class RoleConstants
    {
        public const string SUBSCRIBER = "subscriber";
        public const string PUBLISHER = "publisher";
        public const string MODERATOR = "moderator";    
    }
}
