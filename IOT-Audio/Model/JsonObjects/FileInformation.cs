namespace IOT_Audio.Server.Model.JsonObjects
{
    using System;
    using Windows.Storage;
    using System.Threading.Tasks;
    using System.Collections.Generic;

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
        /// List of things that trigger this item.
        /// </summary>
        public string[] SearchTerms { get; set; }

        /// <summary>
        /// Create an object for a <see cref="StorageFile"/> object
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal async static Task<FileInformation> FromStorage(StorageFile file)
        {
            var musicProperties = await file.Properties.GetMusicPropertiesAsync();

            var terms = new List<string>(musicProperties.Title.Split(new char[] { ',' }));
            terms.Add(file.DisplayName);

            if (terms.Contains("")) {
                terms.Remove("");
            }

            return new FileInformation()
            {
                FileName = file.Name,
                DisplayName = file.DisplayName,
                SearchTerms = terms.ToArray()
            };
        }
    }
}
