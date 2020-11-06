namespace OpenTokSDK.Constants
{
    /// <summary>
    /// For internal use.
    /// </summary>
    internal class OpenTokVersion
    {
        private static string Version = "Opentok-DotNet-SDK/" + typeof(OpenTokVersion).Assembly.GetName().Version;

        public static string GetVersion()
        {
            return Version;
        }
    }
}
