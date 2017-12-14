namespace IOT_Audio.Server.Model.JsonObjects
{
    /// <summary>
    /// Passes a search term to trigger playing an item.
    /// </summary>
    internal sealed class SetPlayItem : ApiKeyBase
    {
        /// <summary>
        /// Filename to act on
        /// </summary>
        public string Term { get; set; }
    }
}
