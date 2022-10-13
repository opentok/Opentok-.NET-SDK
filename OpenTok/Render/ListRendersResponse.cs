using System.Collections.Generic;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// TODO
    /// </summary>
    public struct ListRendersResponse
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="count">TODO</param>
        /// <param name="items">TODO</param>
        public ListRendersResponse(int count, IEnumerable<ListRenderItem> items)
        {
            this.Count = count;
            this.Items = items;
        }

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