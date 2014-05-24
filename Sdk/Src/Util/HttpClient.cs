using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTokSDK.Constants;
using OpenTokSDK.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

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

        public virtual string Delete(string url, Dictionary<string, string> headers, Dictionary<string, object> data)
        {
            headers.Add("Method", "DELETE");
            return DoRequest(url, headers, data);
        }
      
        public string DoRequest(string url, Dictionary<string, string> specificHeaders, 
                                        Dictionary<string, object> bodyData)
        {
            string data = GetRequestPostData(bodyData, specificHeaders);
            var headers = GetRequestHeaders(specificHeaders);
            HttpWebRequest request = CreateRequest(url, headers, data);
            HttpWebResponse response;

            try
            {
                if (!String.IsNullOrEmpty(data))
                {
                    SendData(request, data);
                }
                response = (HttpWebResponse)request.GetResponse();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        StreamReader stream = new StreamReader(response.GetResponseStream());
                        return stream.ReadToEnd();
                    case HttpStatusCode.NoContent:
                        return "";
                    default:
                        throw new OpenTokWebException("Response returned with unexpected status code " + response.StatusCode.ToString());
                }
            }
            catch(WebException e)
            {
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
        private Dictionary<string, string> GetCommonHeaders()
        {
            return new Dictionary<string, string> 
            {   { "X-TB-PARTNER-AUTH", String.Format("{0}:{1}", apiKey, apiSecret) },            
                { "X-TB-VERSION", "1" },
            };
        }
    }
}
