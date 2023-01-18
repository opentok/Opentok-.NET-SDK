using System;
using EnumsNET;
using Newtonsoft.Json;

namespace OpenTokSDK.Render
{
    /// <summary>
    ///     Custom converter for serializing RenderResolution.
    /// </summary>
    class RenderResolutionConverter : JsonConverter<RenderResolution>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, RenderResolution value, JsonSerializer serializer) =>
            writer.WriteValue(value.AsString(EnumFormat.Description));

        /// <inheritdoc />
        public override RenderResolution ReadJson(JsonReader reader, Type objectType, RenderResolution existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) =>
            reader.Value != null
                ? Enums.Parse<RenderResolution>(reader.Value.ToString(), false, EnumFormat.Description)
                : existingValue;
    }
}