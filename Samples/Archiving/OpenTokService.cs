using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using OpenTokSDK;

namespace Archiving
{
    public class OpenTokService
    {
        public Session Session { get; protected set; }
        public OpenTok OpenTok { get; protected set; }

        public OpenTokService()
        {
            int apiKey = 0;
            string apiSecret = null;
            try
            {
                string apiKeyString = ConfigurationManager.AppSettings["API_KEY"];
                apiSecret = ConfigurationManager.AppSettings["API_SECRET"];
                apiKey = Convert.ToInt32(apiKeyString);
            }

            catch (Exception ex)
            {
                if (!(ex is ConfigurationErrorsException || ex is FormatException || ex is OverflowException))
                {
                    throw ex;
                }
            }

            finally
            {
                if (apiKey == 0 || apiSecret == null)
                {
                    Console.WriteLine(
                        "The OpenTok API Key and API Secret were not set in the application configuration. " +
                        "Set the values in App.config and try again. (apiKey = {0}, apiSecret = {1})", apiKey, apiSecret);
                    Console.ReadLine();
                    Environment.Exit(-1);
                }
            }

            this.OpenTok = new OpenTok(apiKey, apiSecret);

            this.Session = this.OpenTok.CreateSession(mediaMode: MediaMode.ROUTED);
        }
    }
}
