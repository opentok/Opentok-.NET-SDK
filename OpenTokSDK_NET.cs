    ///Author: Robert Phan
    ///Date Submitted: 11/17/2010 9:34 Central
    ///OpenTok Community Member Name - trebor
    ///
    ///
    ///web.config
    ///<configuration>
    ///	<appSettings>
    ///    <add key="opentok_server_staging" value="https://staging.tokbox.com/hl"/>
    ///    <add key="opentok_server_production" value="https://api.opentok.com/hl"/>
    ///    <add key="opentok_token_sentinel" value="T1=="/>
    ///    <add key="opentok_sdk_version" value="tbdotnet"/>
    ///    <add key="opentok_key" value="***API key***"/>
    ///    <add key="opentok_secret" value="***API secret***"/>
    ///	</appSettings>
    ///
    ///
    ///code-behind
    ///OpenTokSDK_NET openTok = new OpenTokSDK_NET();
    ///string ipAdd = Request.ServerVariables["REMOTE_ADDR"];
    ///string session_id = openTok.CreateSession(ipAdd);
    ///string token = openTok.GenerateToken(session_id, null, null);
    ///
    ///
    ///ASP.NET MVC2
    ///public ActionResult Index()
    ///{
    ///	OpenTokSDK_NET openTok = new OpenTokSDK_NET();
    ///	string ipAdd = Request.ServerVariables["REMOTE_ADDR"];
    ///	string sessionId = openTok.CreateSession(ipAdd);
    ///	string token = openTok.GenerateToken(sessionId, null, null);
    ///
    ///	ViewData["opentok_session"] = session_id;
    ///	ViewData["opentok_token"] = token;
    ///}
    
    public class OpenTokSDK_NET
    {
        public string CreateSession(string location)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("location", location);
            dict.Add("partner_id", appSettings["opentok_key"]);

            XmlDocument xmlDoc = CreateSessionId(string.Format("{0}/session/create", appSettings["opentok_server_staging"]), dict);

            string session_id = xmlDoc.GetElementsByTagName("session_id")[0].ChildNodes[0].Value;

            return session_id;
        }

        public string GenerateToken(string sessionId, string[] permissions, DateTime? expireTime)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            long createTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            int nonce = RandomNumber(0, 999999);

            string dataString = string.Format("session_id={0}&create_time={1}&permissions={2}&nonce={3}", sessionId, createTime, permissions, nonce);
            string sig = SignString(dataString, appSettings["opentok_secret"].Trim());
            string token = string.Format("{0}{1}", appSettings["opentok_token_sentinel"], EncodeTo64(string.Format("partner_id={0}&sdk_version={1}&sig={2}:{3}", appSettings["opentok_key"], appSettings["opentok_sdk_version"], sig, dataString)));
            
            return token;
        }

        static public string EncodeTo64(string data)
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

        protected string SignString(string message, string key)
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

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                ///Hex format
                sbinary += buff[i].ToString("X2");
            }
            return (sbinary);
        }

        protected XmlDocument CreateSessionId(string uri, Dictionary<string, string> dict)
        {
            XmlDocument xmlDoc = new XmlDocument();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            string postData = string.Empty;
            foreach (KeyValuePair<string, string> pair in dict)
            {
                postData += pair.Key + "=" + HttpUtility.UrlEncode(pair.Value) + "&";
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