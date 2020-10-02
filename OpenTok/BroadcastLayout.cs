using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;


namespace OpenTokSDK
{
    /// <summary>
    /// Represents a broadcast layout of an OpenTok session.
    /// </summary>
    public class BroadcastLayout
    {
        /// <summary>
        /// Defines values for the role parameter of the GenerateToken method of the OpenTok class.
        /// </summary>
        public enum LayoutType
        {
            /// <summary>
            /// Picture-in-Picture
            /// </summary>
            Pip,
            /// <summary>
            /// /Best Fit
            /// </summary>
            BestFit,
            /// <summary>
            /// Vertical Presentation
            /// </summary>
            VerticalPresentation,
            /// <summary>
            /// Horizontal Presentation
            /// </summary>
            HorizontalPresentation,
            /// <summary>
            /// Custom Layout
            /// </summary>
            Custom
        }

        public BroadcastLayout(LayoutType Type)
        {
            this.Type = Type;
        }

        public BroadcastLayout(LayoutType Type, string Stylesheet)
        {
            this.Type = Type;
            this.Stylesheet = Stylesheet;
        }

        /// <summary>
        /// The Layout type
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter), true)]
        [JsonProperty("type")]
        public LayoutType Type { get; set; }

        /// <summary>
        /// The Stylesheet for the Custom Layout
        /// </summary>
        [JsonProperty("stylesheet")]
        public string Stylesheet { get; set; }
    }
}
