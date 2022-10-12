using System.Collections.Generic;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// 
    /// </summary>
    public class ListRendersResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ListRenderItem> Items { get; set; }
    }
}