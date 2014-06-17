using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK.Constants
{
    /**
     * For internal use.
     */
    class OpenTokVersion
    {
        private static string Version = "Opentok-DotNet-SDK/" + typeof(OpenTokVersion).Assembly.GetName().Version;

        public static string GetVersion()
        {
            return Version;
        }
    }
}
