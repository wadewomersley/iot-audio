namespace IOT_Audio.Server.Model.JsonObjects
{
    /// <summary>
    /// Volume settings
    /// </summary>
    internal sealed class SetVolumeData : ApiKeyBase
    {
        /// <summary>
        /// Volume to set to
        /// </summary>
        public int Volume { get; set; }
    }
}
