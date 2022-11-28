using System.Collections.Generic;

namespace OpenTokSDK.Render
{
    /// <summary>
    /// Represents a response from a ListRenders request.
    /// </summary>
    public struct ListRendersResponse
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="count">Total number of Renders.</param>
        /// <param name="items">The list of renderings items.</param>
        public ListRendersResponse(int count, IEnumerable<RenderItem> items)
        {
            this.Count = count;
            this.Items = items;
        }

        /// <summary>
        /// Number of rendering items.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Rendering items contained in the response.
        /// </summary>
        public IEnumerable<RenderItem> Items { get; set; }
    }
}