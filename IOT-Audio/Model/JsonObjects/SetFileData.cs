namespace IOT_Audio.Server.Model.JsonObjects
{
    /// <summary>
    /// Passes a filename back to the service (e.g. to set a file to play or a startupfile)
    /// </summary>
    internal sealed class SetFileData : ApiKeyBase
    {
        /// <summary>
        /// Filename to act on
        /// </summary>
        public string FileName { get; set; }
    }
}
