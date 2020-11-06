using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK
{
    /// <summary>
    ///  A class for accessing an array of Stream objects.
    /// </summary>
    public class StreamList : List<Stream>
    {
        /// <summary>
        /// The total number of streams (associated with the sessionId).
        /// </summary>
        public int TotalCount { get; private set; }

        internal StreamList(List<Stream> items, int totalCount)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}

