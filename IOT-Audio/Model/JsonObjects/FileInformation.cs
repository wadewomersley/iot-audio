namespace IOT_Audio.Server.Model.JsonObjects
{
    using System;
    using System.Linq;
    using Windows.Storage;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Information about a specific file so it can be viewed but also gives the disk filename
    /// </summary>
    internal sealed class FileInformation : ApiKeyBase
    {
        public readonly static Regex TidyTerm = new Regex(@"[^\w.' \-]", RegexOptions.Compiled);

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
        /// Check if a search term matches this item
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        internal bool Matches(string term)
        {
            term = TidyTerm.Replace(term, "").ToLowerInvariant();

            for (var j = 0; j < SearchTerms.Length; j++)
            {
                if (SearchTerms[j].Equals(term))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create an object for a <see cref="StorageFile"/> object
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal async static Task<FileInformation> FromStorage(StorageFile file)
        {
            var musicProperties = await file.Properties.GetMusicPropertiesAsync();

            var terms = new List<string>(musicProperties.Title.Split(new char[] { ',' }).Select(item => TidyTerm.Replace(item, "").ToLowerInvariant()));
            terms.Add(file.DisplayName);
            
            terms.Remove("");
            
            return new FileInformation()
            {
                FileName = file.Name,
                DisplayName = file.DisplayName,
                SearchTerms = terms.ToArray()
            };
        }
    }
}
