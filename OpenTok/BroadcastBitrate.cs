using System;
using Newtonsoft.Json;
using OpenTokSDK.Exception;

namespace OpenTokSDK
{
    /// <summary>
    ///     Represents a BitRate for broadcasts.
    /// </summary>
    public class BroadcastBitrate
    {
        private const int MaxBitrate = 2000000;
        private const int MinBitrate = 400000;

        /// <summary>
        ///     The maximum BitRate allowed for the broadcast composing.
        /// </summary>
        public long Bitrate { get; }

        /// <summary>
        ///     Creates a BroadcastBitrate with the maximum BitRate.
        /// </summary>
        public BroadcastBitrate() => this.Bitrate = MaxBitrate;

        /// <summary>
        ///     Creates a BroadcastBitrate with a specific Bitrate.
        /// </summary>
        /// <param name="bitrate">The Bitrate.</param>
        /// <exception cref="OpenTokArgumentException">
        ///     When specified bitrate is lower than the minimum value, or higher than the
        ///     maximum value.
        /// </exception>
        public BroadcastBitrate(long bitrate) =>
            this.Bitrate = ValidateBitrate(bitrate)
                ? bitrate
                : throw new OpenTokArgumentException($"Bitrate value must be between {MinBitrate} and {MaxBitrate}.");

        private static bool ValidateBitrate(long bitrate) => bitrate <= MaxBitrate && bitrate >= MinBitrate;
    }

    internal class BroadcastBitrateConverter : JsonConverter<BroadcastBitrate>
    {
        public override void WriteJson(JsonWriter writer, BroadcastBitrate value, JsonSerializer serializer) => 
            writer.WriteValue(value?.Bitrate);

        public override BroadcastBitrate ReadJson(JsonReader reader, Type objectType, BroadcastBitrate existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = (long?)reader.Value;
            return value.HasValue ? new BroadcastBitrate(value.Value) : null;
        }
    }
}