namespace OpenTokSDK
{
    /// <summary>
    /// The Layout Type for the an Archive
    /// </summary>
    public enum LayoutType
    {
        /// <summary>
        /// Use best fit layout
        /// </summary>
        bestFit,
        /// <summary>
        /// Use a Custom layout (stylesheet property must be set)
        /// </summary>
        custom,
        /// <summary>
        /// Presents participants horizantally
        /// </summary>
        horizontalPresentation,
        /// <summary>
        /// Presents as picture in picture
        /// </summary>
        pip,
        /// <summary>
        /// presents participants vertically
        /// </summary>
        verticalPresentation
    }
}
