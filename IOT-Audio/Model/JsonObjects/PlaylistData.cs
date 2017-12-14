using System.Text.RegularExpressions;

namespace IOT_Audio.Server.Model.JsonObjects
{
    /// <summary>
    /// List of files available to play
    /// </summary>
    internal sealed class PlaylistData : ApiKeyBase
    {
        /// <summary>
        /// List of <see cref="FileInformation"/> objects
        /// </summary>
        public FileInformation[] Files { get; set; }

        public FileInformation FindMatchingFile(string term)
        {
            for (var i = 0; i < Files.Length; i++)
            {
                if(Files[i].Matches(term))
                {
                    return Files[i];
                }
            }

            return null;
        }
    }
}
