using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace OpenTokSDKTest
{
    public abstract class TestBase
    {
        protected string ApiSecret = "1234567890abcdef1234567890abcdef1234567890";
        protected int ApiKey = 123456;
        protected string SessionId = "1_MX4xMjM0NTZ-flNhdCBNYXIgMTUgMTQ6NDI6MjMgUERUIDIwMTR-MC40OTAxMzAyNX4";

#if NETCOREAPP2_0_OR_GREATER
        private static readonly Assembly ThisAssembly = typeof(TestBase).GetTypeInfo().Assembly;
#else
        private static readonly Assembly ThisAssembly = typeof(TestBase).Assembly;
#endif

        private static readonly string TestAssemblyName = ThisAssembly.GetName().Name;

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = ThisAssembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        protected string GetResponseJson([CallerMemberName] string name = null)
        {
            name = $"{name}-response";

            var type = GetType().Name;
            var ns = GetType().Namespace;
            if (ns != null)
            {
                var path = Path.Combine(AssemblyDirectory, "Data", type, name + ".json");

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"File not found at {path}.");
                }

                var jsonContent = File.ReadAllText(path);
                jsonContent = Regex.Replace(jsonContent, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
                return jsonContent;
            }

            return string.Empty;
        }
    }
}
