namespace IOT_Audio.Server.Model.JsonObjects
{
    internal sealed class PlaylistData : ApiKeyBase
    {
        public FileInformation[] Files { get; set; }
    }
}
