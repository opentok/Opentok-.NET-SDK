using OpenTokSDK.Exception;

namespace OpenTokSDK.Render
{
    /// <summary>
    ///     Represents a request for retrieving renders.
    /// </summary>
    public class ListRendersRequest
    {
        private const int DefaultCount = 50;
        private const int MaximumCount = 1000;
        private const int MinimumCount = 0;
        private const int MinimumOffset = 0;

        /// <summary>
        ///     Indicates the maximum count has been exceeded.
        /// </summary>
        public const string CountExceeded = "Count cannot be higher than 1000.";

        /// <summary>
        ///     Indicates the provided count is negative.
        /// </summary>
        public const string NegativeCount = "Count cannot be negative.";

        /// <summary>
        ///     Indicates the provided offset is negative.
        /// </summary>
        public const string NegativeOffset = "Offset cannot be negative.";

        /// <summary>
        ///     Initializes a ListRendersRequest with default values. Count will be 50.
        /// </summary>
        public ListRendersRequest()
        {
            this.Count = DefaultCount;
        }

        /// <summary>
        ///     Initializes a ListRendersRequest.
        /// </summary>
        /// <param name="count">Number of Renders to retrieve starting at offset. Default is 50, maximum is 1000.</param>
        public ListRendersRequest(int count)
            : this()
        {
            ValidateCount(count);
            this.Count = count;
        }

        /// <summary>
        ///     Initializes a ListRendersRequest.
        /// </summary>
        /// <param name="offset">Start offset in the list of existing Renders.</param>
        /// <param name="count">Number of Renders to retrieve starting at offset. Default is 50, maximum is 1000.</param>
        public ListRendersRequest(int offset, int count)
            : this(count)
        {
            ValidateOffset(offset);
            this.Offset = offset;
            this.Count = count;
        }

        /// <summary>
        ///     Start offset in the list of existing Renders.
        /// </summary>
        public int? Offset { get; }

        /// <summary>
        ///     Number of Renders to retrieve starting at offset. Default is 50, maximum is 1000.
        /// </summary>
        public int Count { get; }

        private static void ValidateCount(int count)
        {
            if (count > MaximumCount)
            {
                throw new OpenTokException(CountExceeded);
            }

            if (count < MinimumCount)
            {
                throw new OpenTokException(NegativeCount);
            }
        }

        private static void ValidateOffset(int offset)
        {
            if (offset < MinimumOffset)
            {
                throw new OpenTokException(NegativeOffset);
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <returns>TODO</returns>
        public string ToQueryParameters()
        {
            var parameters = $"count={this.Count}";
            if (this.Offset.HasValue)
            {
                parameters += $"&offset={this.Offset}";
            }

            return parameters;
        }
    }
}