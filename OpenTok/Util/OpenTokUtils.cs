using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.Net;

using Newtonsoft.Json;

namespace OpenTokSDK.Util
{
    public class OpenTokUtils
    {
        public static string[] SplitString(string toBeSplitted, char separator, int numberOfFields)
        {
            char[] sep = { separator };
            string[] fields = toBeSplitted.Split(sep);

            if (numberOfFields > 0 && fields.Length != numberOfFields)
            {
                return null;
            }
            return fields;
        }

        public static string Decode64(string encodedString)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    byte[] data = Convert.FromBase64String(encodedString);
                    return Encoding.UTF8.GetString(data);
                }
                catch (FormatException)
                {
                    // We don't do anything here because we need to try to 
                    // decode the string again
                }
                encodedString = encodedString + "=";
            }
            throw new FormatException("String cannot be decoded");
        }

        public static string EncodeHMAC(string input, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            HMACSHA1 hmac = new HMACSHA1(keyBytes);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedValue = hmac.ComputeHash(inputBytes);

            // iterates over bytes and converts them each to a 2 digit hexidecimal string representation,
            // concatenates, and converts to lower case
            string encodedInput = string.Concat(hashedValue.Select(b => string.Format("{0:X2}", b)).ToArray());
            return encodedInput.ToLowerInvariant();
        }

        public static string Convert64(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes);
        }
        public static double GetUnixTimeStampForDate(DateTime date)
        {
            return (date - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public static double GetCurrentUnixTimeStamp()
        {
            return GetUnixTimeStampForDate(DateTime.UtcNow);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(0, 999999);
        }

        public static bool TestIpAddress(string location)
        {
            IPAddress ipAddress;
            if (location == "" || location == "localhost")
            {
                return true;
            }
            else if (IPAddress.TryParse(location, out ipAddress))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidateSession(string sessionId)
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

        internal static Archive GenerateArchive(string response, int apiKey, string apiSecret, string apiUrl)
        {
            Archive archive = JsonConvert.DeserializeObject<Archive>(response);
            Archive archiveCopy = new Archive(new OpenTok(apiKey, apiSecret, apiUrl));
            archiveCopy.CopyArchive(archive);
            return archiveCopy;
        }

        internal static Broadcast GenerateBroadcast(string response, int apiKey, string apiSecret, string apiUrl)
        {
            Broadcast broadcast = JsonConvert.DeserializeObject<Broadcast>(response);
            Broadcast broadcastCopy = new Broadcast(new OpenTok(apiKey, apiSecret, apiUrl));
            broadcastCopy.CopyBroadcast(broadcast);
            return broadcastCopy;
        }

        public static string convertToCamelCase(string text)
        {
            return Char.ToLowerInvariant(text[0]) + text.Substring(1);
        }

        public static int GetPartnerIdFromSessionId(string sessionId)
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
    }
}
