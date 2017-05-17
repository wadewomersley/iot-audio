namespace IOT_Audio.Server.Model.JsonObjects
{
    using Windows.Storage;

    /// <summary>
    /// Information about a specific file so it can be viewed but also gives the disk filename
    /// </summary>
    internal sealed class FileInformation : ApiKeyBase
    {
        /// <summary>
        /// Filename on the filesystem
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// Friendly display name for the file
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Create an object for a <see cref="StorageFile"/> object
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal static FileInformation FromStorage(StorageFile file)
        {
            return new FileInformation() {
                FileName = file.Name,
                DisplayName = file.DisplayName
            };
        }
    }
}
