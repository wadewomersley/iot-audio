namespace IOT_Audio.Server.Model.JsonObjects
{
    using Windows.Storage;

    internal sealed class FileInformation
    {
        public string FileName { get; set; }

        public string DisplayName { get; set; }

        internal static FileInformation FromStorage(StorageFile file)
        {
            return new FileInformation() {
                FileName = file.Name,
                DisplayName = file.DisplayName
            };
        }
    }
}
