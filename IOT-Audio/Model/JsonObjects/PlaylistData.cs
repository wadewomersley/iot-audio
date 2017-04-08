namespace IOT_Audio.Server.Model.JsonObjects
{
    internal sealed class PlaylistData : ApiKeyAbstract
    {
        public FileInformation[] Files { get; set; }
    }
}
