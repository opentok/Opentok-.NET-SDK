using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK
{
    /// <summary>
    /// A class for accessing an array of Archive objects.
    /// </summary>
    public class ArchiveList : List<Archive>
    {
        /// <summary>
        /// The total number of archives (associated with your OpenTok API key).
        /// </summary>
        public int TotalCount { get; private set; }

        internal ArchiveList(List<Archive> items, int totalCount)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}
