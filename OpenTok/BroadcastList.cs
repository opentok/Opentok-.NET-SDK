using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTokSDK
{
    /**
     * A class for accessing an array of Broadcast objects.
     */
    public class BroadcastList : List<Broadcast>
    {
        /**
         * The total number of boradcasts.
         */
        public int TotalCount { get; private set; }

        internal BroadcastList(List<Broadcast> items, int totalCount)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}
