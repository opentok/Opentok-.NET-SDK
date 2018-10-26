using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;
using System.Xml;

using System.Web;

using Newtonsoft.Json;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

using OpenTokSDK.Constants;
using OpenTokSDK.Exception;

namespace OpenTokSDK.Util
{
    /**
     * For internal use.
     */
    public class HttpClient
    {
        private string userAgent;
        private int apiKey;
        private string apiSecret;
        private string server;
        public bool debug = false;
        private readonly DateTime unixEpoch = new DateTime(
          1970, 1, 1, 0, 0, 0, DateTimeKind.Utc
        );

        public HttpClient()
        {
            // This is only for testing purposes
        }

        public HttpClient(int apiKey, string apiSecret, string apiUrl = "")
        {
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
            this.server = apiUrl;
            this.userAgent = OpenTokVersion.GetVersion();
        }

        public virtual string Get(string url)
        {
            return Get(url, new Dictionary<string, string>());
        }

        public virtual string Get(string url, Dictionary<string, string> headers)
        {
            headers.Add("Method", "GET");
            return DoRequest(url, headers, null);
        }

        public virtual string Post(string url, Dictionary<string, string> headers, Dictionary<string, object> data)
        {
            headers.Add("Method", "POST");
            return DoRequest(url, headers, data);
        }

        public virtual string Put(string url, Dictionary<string, string> headers, Dictionary<string, object> data)
        {
            headers.Add("Method", "PUT");
            return DoRequest(url, headers, data);
        }

        public virtual string Delete(string url, Dictionary<string, string> headers)
        {
            headers.Add("Method", "DELETE");
            return DoRequest(url, headers, null);
        }

        public string DoRequest(string url, Dictionary<string, string> specificHeaders,
                                        Dictionary<string, object> bodyData)
        {
            string data = GetRequestPostData(bodyData, specificHeaders);
            var headers = GetRequestHeaders(specificHeaders);
            HttpWebRequest request = CreateRequest(url, headers, data);

            DebugLog("Request Method: " + request.Method);
            DebugLog("Request URI: " + request.RequestUri);
            DebugLogHeaders(request.Headers, "Request");

            HttpWebResponse response;

            try
            {
                if (!String.IsNullOrEmpty(data))
                {
                    DebugLog("Request Body: " + data);
                    SendData(request, data);
                }
                using (response = (HttpWebResponse) request.GetResponse())
                {
                    DebugLog("Response Status Code: " + response.StatusCode);
                    DebugLog("Response Status Description: " + response.StatusDescription);
                    DebugLogHeaders(response.Headers, "Response");

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            using (var stream = new StreamReader(response.GetResponseStream()))
                            {
                                return stream.ReadToEnd();
                            }
                        case HttpStatusCode.NoContent:
                            return "";
                        default:
                            throw new OpenTokWebException("Response returned with unexpected status code " +
                                                          response.StatusCode.ToString());
                    }
                }
            }
            catch (WebException e)
            {
                DebugLog("WebException Status: " + e.Status + ", Message: " + e.Message);

                response = (HttpWebResponse)e.Response;

                DebugLog("Response Status Code: " + response.StatusCode);
                DebugLog("Response Status Description: " + response.StatusDescription);
                DebugLogHeaders(response.Headers, "Response");

                if (this.debug)
                {
                    using (var stream = new StreamReader(response.GetResponseStream()))
                    {
                        DebugLog("Response Body: " + stream.ReadToEnd());
                    }
                }

                throw new OpenTokWebException("Error with request submission", e);
            }

        }

        public XmlDocument ReadXmlResponse(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }

        private void SendData(HttpWebRequest request, object data)
        {
            using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(data);
            }
        }

        private HttpWebRequest CreateRequest(string url, Dictionary<string, string> headers, string data)
        {
            Uri uri = new Uri(string.Format("{0}/{1}", server, url));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentLength = data.Length;
            request.UserAgent = userAgent;

            if (headers.ContainsKey("Content-type"))
            {
                request.ContentType = headers["Content-type"];
                request.Expect = headers["Content-type"];
                headers.Remove("Content-type");
            }
            if (headers.ContainsKey("Method"))
            {
                request.Method = headers["Method"];
                headers.Remove("Method");
            }

            foreach (KeyValuePair<string, string> entry in headers)
            {
                request.Headers.Add(entry.Key, entry.Value);
            }

            return request;
        }
        private Dictionary<string, string> GetRequestHeaders(Dictionary<string, string> headers)
        {
            var requestHeaders = GetCommonHeaders();
            requestHeaders = requestHeaders.Concat(headers).GroupBy(d => d.Key)
                                .ToDictionary(d => d.Key, d => d.First().Value);
            return requestHeaders;
        }

        private string GetRequestPostData(Dictionary<string, object> data, Dictionary<string, string> headers)
        {
            if (data != null && headers.ContainsKey("Content-type"))
            {
                if (headers["Content-type"] == "application/json")
                {
                    return JsonConvert.SerializeObject(data);
                }
                else if (headers["Content-type"] == "application/x-www-form-urlencoded")
                {
                    return ProcessParameters(data);
                }
            }
            else if (data != null || headers.ContainsKey("Content-type"))
            {
                throw new OpenTokArgumentException("If Content-type is set in the headers data in the body is expected");
            }
            return "";
        }

        private string ProcessParameters(Dictionary<string, object> parameters)
        {
            string data = string.Empty;

            foreach (KeyValuePair<string, object> pair in parameters)
            {
                data += pair.Key + "=" + HttpUtility.UrlEncode(pair.Value.ToString()) + "&";
            }
            return data.Substring(0, data.Length - 1);
        }

        private int CurrentTime()
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();

            int secondsSinceEpoch = (int) Math.Round((now - unixEpoch).TotalSeconds);
            return secondsSinceEpoch;
        }

        private string GenerateJwt(int key, string secret, int expiryPeriod = 300)
        {
            int now = CurrentTime();
            int expiry = now + expiryPeriod;

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

        private Dictionary<string, string> GetCommonHeaders()
        {
            return new Dictionary<string, string>
            {   { "X-OPENTOK-AUTH", GenerateJwt(apiKey, apiSecret) },
                { "X-TB-VERSION", "1" },
            };
        }

        private void DebugLog(string message)
        {
            if (this.debug)
            {
                var now = Convert.ToString(CurrentTime());
                Console.WriteLine("[{0}] {1}", now, message);
            }
        }

        private void DebugLogHeaders(WebHeaderCollection headers, string label)
        {
            if (this.debug)
            {
                for(int i = 0; i < headers.Count; ++i)
                {
                    string header = headers.GetKey(i);
                    foreach(string value in headers.GetValues(i))
                    {
                        DebugLog(label + " Header: " + header + " = " + value);
                    }
                }
            }
        }
    }
}
