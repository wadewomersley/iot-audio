namespace IOT_Audio.Server.Model.JsonObjects
{
    using Windows.Storage;

    /// <summary>
    /// Includes API key in the JSON object for requests
    /// </summary>
    internal class ApiKeyBase
    {
        /// <summary>
        /// API Key for web requests
        /// </summary>
        public string ApiKey { get; set; }
    }
}
