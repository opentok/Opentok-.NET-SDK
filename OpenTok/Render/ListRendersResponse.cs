using System.Collections.Generic;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// TODO
    /// </summary>
    public class ListRendersResponse
    {
        /// <summary>
        /// TODO
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public IEnumerable<ListRenderItem> Items { get; set; }
    }
}