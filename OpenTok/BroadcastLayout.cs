using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;


namespace OpenTokSDK
{
    /**
    * Represents a broadcast layout of an OpenTok session.
    */
    public class BroadcastLayout
    {
        /**
        * Defines values for the role parameter of the GenerateToken method of the OpenTok class.
        */
        public enum LayoutType
        {

            /**
             * Picture-in-Picture
             */
            Pip,
            /**
             * Best Fit
             */
            BestFit,
            /**
             * Vertical Presentation
             */
            VerticalPresentation,
            /**
             * Horizontal Presentation
             */
            HorizontalPresentation,
            /**
             * Custom Layout
             */
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

        /**
         * The Layout type
        */
        [JsonConverter(typeof(StringEnumConverter), true)]
        [JsonProperty("type")]
        public LayoutType Type { get; set; }

        /**
         * The Stylesheet for the Custom Layout
        */
        [JsonProperty("stylesheet")]
        public string Stylesheet { get; set; }
    }
}
