using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTok.Constants
{
    /**
     * For internal use.
     */
    class OpenTokVersion
    {
        private static string Version = "Opentok-DotNet-SDK/" + typeof(OpenTokVersion).Assembly.GetName().Version + "-development";

        public static string GetVersion()
        {
            return Version;
        }
    }
}
