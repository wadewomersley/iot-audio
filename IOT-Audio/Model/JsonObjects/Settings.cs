namespace IOT_Audio.Server.Model.JsonObjects
{
    /// <summary>
    /// Settings used by the service
    /// </summary>
    internal sealed class Settings : ApiKeyBase
    {
        /// <summary>
        /// Current volume
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// Startup file when app is launched
        /// </summary>
        public string StartupFilename { get; set; }
        
        /// <summary>
        /// Has the user said they have saved the API key
        /// </summary>
        public bool ApiKeySaved { get; set; }
    }
}
