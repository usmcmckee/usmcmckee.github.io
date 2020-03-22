namespace SalesforceObjectDetails
{
    public struct Field
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value> The description. </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        /// <value> The external identifier. </value>
        public bool? ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value> The formula. </value>
        public string Formula { get; set; }

        /// <summary>
        /// Gets or sets the full name API.
        /// </summary>
        /// <value> The full name API. </value>
        public string FullNameAPI { get; set; }

        /// <summary>
        /// Gets or sets the inline help text.
        /// </summary>
        /// <value> The inline help text. </value>
        public string InlineHelpText { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value> The length. </value>
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value> The name. </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the required.
        /// </summary>
        /// <value> The required. </value>
        public bool? Required { get; set; }

        /// <summary>
        /// Gets or sets the track feed history.
        /// </summary>
        /// <value> The track feed history. </value>
        public bool? TrackFeedHistory { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value> The type. </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the unique.
        /// </summary>
        /// <value> The unique. </value>
        public bool? Unique { get; set; }
    }
}