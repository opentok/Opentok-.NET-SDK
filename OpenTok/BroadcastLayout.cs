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

        /// <summary>
        /// Initalizes a Broadcast layout with the given <see cref="ScreenShareLayoutType"/>, automatically
        /// sets the Type to bestFit
        /// </summary>
        /// <param name="type"></param>
        public BroadcastLayout(ScreenShareLayoutType type)
        {
            Type = LayoutType.BestFit;
            ScreenShareType = type;
        }

        /// <summary>
        /// Initalizes a BroadcastLayout with the given <see cref="LayoutType"/>
        /// </summary>
        /// <param name="Type"></param>
        public BroadcastLayout(LayoutType Type)
        {
            this.Type = Type;
        }

        /// <summary>
        /// Initalizes a BroadcastLayout with the given <see cref="LayoutType"/> and stylesheet - note Type must be <see cref="LayoutType.Custom"/>
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Stylesheet"></param>
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
        public string Stylesheet { get; set; } = null;

        /// <summary>
        /// The <see cref="LayoutType"/>to use when there is a screen-sharing 
        /// stream in the session. Note that to use this property, 
        /// you must set the <see cref="Type"/> property to <see cref="LayoutType.BestFit"/> 
        /// and leave the <see cref="Stylesheet"/> property unset. 
        /// For more information, see Layout types for screen sharing.
        /// NOTE: <see cref="LayoutType.Custom"/> is not valid for this property
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter), true)]
        [JsonProperty("screensharetype", NullValueHandling = NullValueHandling.Ignore)]
        public ScreenShareLayoutType? ScreenShareType { get; set; }
    }
}
