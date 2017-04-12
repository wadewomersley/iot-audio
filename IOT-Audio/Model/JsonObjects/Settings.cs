namespace IOT_Audio.Server.Model.JsonObjects
{
    internal sealed class Settings : ApiKeyBase
    {
        public int Volume { get; set; }

        public string StartupFilename { get; set; }

        public bool ApiKeySaved { get; set; }
    }
}
