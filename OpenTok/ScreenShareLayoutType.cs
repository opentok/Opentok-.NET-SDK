namespace OpenTokSDK
{

    /// <summary>
    /// For archive and broadcast layouts, the layout type to use with screen shares
    /// If this enum is being used, the Type property should be set to BestFit
    /// </summary>
    public enum ScreenShareLayoutType
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
        HorizontalPresentation        
    }
}
