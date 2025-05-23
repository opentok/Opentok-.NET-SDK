#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using JWT;
using Newtonsoft.Json;
using OpenTokSDK.Constants;
using OpenTokSDK.Exception;
using Vonage;
using Vonage.Request;

#endregion

#pragma warning disable CS1591

namespace OpenTokSDK.Util
{
    /// <summary>
    ///     For internal use.
    /// </summary>
    public class HttpClient
    {
        private readonly int _apiKey;
        private readonly string _apiSecret;
        private readonly string _apiUrl;

        private readonly DateTime _unixEpoch = new DateTime(
            1970, 1, 1, 0, 0, 0, DateTimeKind.Utc
        );

        private readonly string appId;
        private readonly string privateKey;

        internal HttpClient()
        {
        }

        internal HttpClient(int apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public HttpClient(int apiKey, string apiSecret, string apiUrl = "")
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _apiUrl = apiUrl;
        }

        public HttpClient(string appId, string privateKey)
        {
            this.appId = appId;
            this.privateKey = privateKey;
            _apiUrl = "https://video.api.vonage.com";
        }

        private bool IsShim => !string.IsNullOrEmpty(appId);

        /// <summary>
        ///     The custom user-agent value. The HttpClient will append this value, if it exists, to the initial user-agent value.
        /// </summary>
        public string CustomUserAgent { get; internal set; }

        /// <summary>
        ///     Turns on and off debug logging
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        ///     Timeout in milliseconds for the HttpWebRequests sent by the client.
        /// </summary>
        public int? RequestTimeout { get; set; }

        internal string LastRequest { get; private set; }

        public virtual string Get(string url)
        {
            return Get(url, new Dictionary<string, string>());
        }

        public virtual string Get(string url, Dictionary<string, string> headers)
        {
            headers.Add("Method", "GET");
            return DoRequest(url, headers, null);
        }

        public virtual Task<string> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            if (headers == null) headers = new Dictionary<string, string>();
            headers.Add("Method", "GET");
            return DoRequestAsync(url, headers, null);
        }

        public virtual string Post(string url, Dictionary<string, string> headers, Dictionary<string, object> data)
        {
            headers.Add("Method", "POST");
            return DoRequest(url, headers, data);
        }

        public virtual Task<string> PostAsync(string url, Dictionary<string, string> headers,
            Dictionary<string, object> data)
        {
            headers.Add("Method", "POST");
            return DoRequestAsync(url, headers, data);
        }

        public virtual string Put(string url, Dictionary<string, string> headers, Dictionary<string, object> data)
        {
            headers.Add("Method", "PUT");
            return DoRequest(url, headers, data);
        }

        public virtual async Task<string> PutAsync(string url, Dictionary<string, string> headers,
            Dictionary<string, object> data)
        {
            headers.Add("Method", "PUT");
            return await DoRequestAsync(url, headers, data);
        }

        public virtual string Patch(string url, Dictionary<string, string> headers, Dictionary<string, object> data)
        {
            headers.Add("Method", "PATCH");
            return DoRequest(url, headers, data);
        }

        public virtual Task<string> PatchAsync(string url, Dictionary<string, string> headers,
            Dictionary<string, object> data)
        {
            headers.Add("Method", "PATCH");
            return DoRequestAsync(url, headers, data);
        }

        public virtual string Delete(string url, Dictionary<string, string> headers)
        {
            headers.Add("Method", "DELETE");
            return DoRequest(url, headers, null);
        }

        public virtual Task<string> DeleteAsync(string url, Dictionary<string, string> headers)
        {
            headers.Add("Method", "DELETE");
            return DoRequestAsync(url, headers, null);
        }

        public string DoRequest(string url, Dictionary<string, string> specificHeaders,
            Dictionary<string, object> bodyData)
        {
            var data = GetRequestPostData(bodyData, specificHeaders);
            var headers = GetRequestHeaders(specificHeaders);
            var request = CreateRequest(url, headers, data);
            DebugLog("Request Method: " + request.Method);
            DebugLog("Request URI: " + request.RequestUri);
            DebugLogHeaders(request.Headers, "Request");
            HttpWebResponse response;
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    DebugLog("Request Body: " + data);
                    SendData(request, data);
                }

                using (response = (HttpWebResponse)request.GetResponse())
                {
                    DebugLog("Response Status Code: " + response.StatusCode);
                    DebugLog("Response Status Description: " + response.StatusDescription);
                    DebugLogHeaders(response.Headers, "Response");
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        case HttpStatusCode.Accepted:
                            using (var stream = new StreamReader(response.GetResponseStream() ??
                                                                 throw new InvalidOperationException(
                                                                     "Response stream null")))
                            {
                                return stream.ReadToEnd();
                            }
                        case HttpStatusCode.NoContent:
                            return "";
                        default:
                            throw new OpenTokWebException(
                                $"Response returned with unexpected status code {response.StatusCode}");
                    }
                }
            }
            catch (WebException e)
            {
                DebugLog("WebException Status: " + e.Status + ", Message: " + e.Message);
                response = (HttpWebResponse)e.Response;
                if (response != null)
                {
                    DebugLog("Response Status Code: " + response.StatusCode);
                    DebugLog("Response Status Description: " + response.StatusDescription);
                    DebugLogHeaders(response.Headers, "Response");
                    if (Debug)
                        using (var stream = new StreamReader(response.GetResponseStream() ??
                                                             throw new InvalidOperationException(
                                                                 "Response stream null")))
                        {
                            DebugLog("Response Body: " + stream.ReadToEnd());
                        }
                }
                else if (e.Status == WebExceptionStatus.SendFailure)
                {
                    throw new OpenTokWebException(
                        "Error with request submission (TLS1.1 or other network/protocol issue)", e);
                }

                OpenTokUtils.ValidateTlsVersion(e);
                throw new OpenTokWebException("Error with request submission", e);
            }
        }

        public async Task<string> DoRequestAsync(string url, Dictionary<string, string> specificHeaders,
            Dictionary<string, object> bodyData)
        {
            var data = GetRequestPostData(bodyData, specificHeaders);
            var headers = GetRequestHeaders(specificHeaders);
            var request = CreateRequest(url, headers, data);
            DebugLog("Request Method: " + request.Method);
            DebugLog("Request URI: " + request.RequestUri);
            DebugLogHeaders(request.Headers, "Request");
            HttpWebResponse response;
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    DebugLog("Request Body: " + data);
                    await SendDataAsync(request, data);
                }

                using (response = await request.GetResponseAsync().ConfigureAwait(false) as HttpWebResponse)
                {
                    DebugLog("Response Status Code: " + response.StatusCode);
                    DebugLog("Response Status Description: " + response.StatusDescription);
                    DebugLogHeaders(response.Headers, "Response");
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        case HttpStatusCode.Accepted:
                            using (var stream = new StreamReader(response.GetResponseStream() ??
                                                                 throw new InvalidOperationException(
                                                                     "Response stream null")))
                            {
                                return await stream.ReadToEndAsync();
                            }
                        case HttpStatusCode.NoContent:
                            return "";
                        default:
                            throw new OpenTokWebException(
                                $"Response returned with unexpected status code {response.StatusCode}");
                    }
                }
            }
            catch (WebException e)
            {
                DebugLog("WebException Status: " + e.Status + ", Message: " + e.Message);
                response = (HttpWebResponse)e.Response;
                if (response != null)
                {
                    DebugLog("Response Status Code: " + response.StatusCode);
                    DebugLog("Response Status Description: " + response.StatusDescription);
                    DebugLogHeaders(response.Headers, "Response");
                    if (Debug)
                        using (var stream = new StreamReader(response.GetResponseStream() ??
                                                             throw new InvalidOperationException(
                                                                 "Response stream null")))
                        {
                            var body = await stream.ReadToEndAsync();
                            DebugLog($"Response Body: {body}");
                        }
                }

                OpenTokUtils.ValidateTlsVersion(e);
                throw new OpenTokWebException("Error with request submission", e);
            }
        }

        public XmlDocument ReadXmlResponse(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }

        private void SendData(HttpWebRequest request, string data)
        {
            LastRequest = data;
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(data);
            }
        }

        private async Task SendDataAsync(HttpWebRequest request, string data)
        {
            using (var stream = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
            {
                await stream.WriteAsync(data).ConfigureAwait(false);
            }
        }

        private HttpWebRequest CreateRequest(string url, Dictionary<string, string> headers, string data)
        {
            var uri = new Uri($"{_apiUrl}/{url}");
            var request = (HttpWebRequest)WebRequest.Create(uri);
            if (RequestTimeout != null) request.Timeout = (int)RequestTimeout;

            request.ContentLength = data.Length;
            request.UserAgent = CustomUserAgent != null
                ? $"{OpenTokVersion.GetVersion()}/{CustomUserAgent}"
                : OpenTokVersion.GetVersion();
            if (headers.ContainsKey("Content-Type"))
            {
                request.ContentType = headers["Content-Type"];
                request.Expect = headers["Content-Type"];
                headers.Remove("Content-Type");
            }

            if (headers.ContainsKey("Method"))
            {
                request.Method = headers["Method"];
                headers.Remove("Method");
            }

            foreach (var entry in headers) request.Headers.Add(entry.Key, entry.Value);

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
            if (data != null && headers.ContainsKey("Content-Type"))
            {
                if (headers["Content-Type"] == "application/json")
                    return JsonConvert.SerializeObject(data,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                if (headers["Content-Type"] == "application/x-www-form-urlencoded") return ProcessParameters(data);
            }
            else if (data != null || headers.ContainsKey("Content-Type"))
            {
                throw new OpenTokArgumentException(
                    "If Content-Type is set in the headers data in the body is expected");
            }

            return "";
        }

        private string ProcessParameters(Dictionary<string, object> parameters)
        {
            var data = string.Empty;
            foreach (var pair in parameters)
                data += pair.Key + "=" + HttpUtility.UrlEncode(pair.Value.ToString()) + "&";

            return data.Substring(0, data.Length - 1);
        }

        private int CurrentTime()
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();
            var secondsSinceEpoch = (int)Math.Round((now - _unixEpoch).TotalSeconds);
            return secondsSinceEpoch;
        }

        private string GenerateJwt()
        {
            return IsShim
                ? new TokenGenerator().GenerateToken(appId, privateKey)
                : new TokenGenerator().GenerateLegacyToken(_apiKey, _apiSecret);
        }

        private Dictionary<string, string> GetCommonHeaders()
        {
            return new Dictionary<string, string>
            {
                {
                    IsShim ? "Authorization" : "X-OPENTOK-AUTH",
                    IsShim ? "Bearer " + GenerateJwt() : GenerateJwt()
                },
                { "X-TB-VERSION", "1" }
            };
        }

        private void DebugLog(string message)
        {
            if (Debug)
            {
                var now = Convert.ToString(CurrentTime());
                Console.WriteLine("[{0}] {1}", now, message);
            }
        }

        private void DebugLogHeaders(WebHeaderCollection headers, string label)
        {
            if (Debug)
                for (var i = 0; i < headers.Count; ++i)
                {
                    var header = headers.GetKey(i);
                    foreach (var value in headers.GetValues(i)) DebugLog(label + " Header: " + header + " = " + value);
                }
        }
    }
}