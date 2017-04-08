namespace IOT_Audio.Server.Model.JsonObjects
{
    internal sealed class Settings : ApiKeyAbstract
    {
        public int Volume { get; set; }

        public string StartupFilename { get; set; }
    }
}
