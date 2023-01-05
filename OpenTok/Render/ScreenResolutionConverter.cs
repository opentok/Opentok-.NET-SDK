using System;
using EnumsNET;
using Newtonsoft.Json;

namespace OpenTokSDK.Render
{
    /// <summary>
    ///     Custom converter for serializing ScreenResolution.
    /// </summary>
    class ScreenResolutionConverter : JsonConverter<ScreenResolution>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, ScreenResolution value, JsonSerializer serializer) =>
            writer.WriteValue(value.AsString(EnumFormat.Description));

        /// <inheritdoc />
        public override ScreenResolution ReadJson(JsonReader reader, Type objectType, ScreenResolution existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) =>
            reader.Value != null
                ? Enums.Parse<ScreenResolution>(reader.Value.ToString(), false, EnumFormat.Description)
                : existingValue;
    }
}