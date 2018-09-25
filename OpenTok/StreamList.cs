using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK
{
    /**
     * A class for accessing an array of Stream objects.
     */
    public class StreamList : List<Stream>
    {
        /**
         * The total number of streams (associated with the sessionId).
         */
        public int TotalCount { get; private set; }

        internal StreamList(List<Stream> items, int totalCount)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}

