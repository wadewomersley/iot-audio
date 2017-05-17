namespace IOT_Audio.Server.Model.JsonObjects
{
    /// <summary>
    /// List of files available to play
    /// </summary>
    internal sealed class PlaylistData : ApiKeyBase
    {
        /// <summary>
        /// List of <see cref="FileInformation"/> objects
        /// </summary>
        public FileInformation[] Files { get; set; }
    }
}
