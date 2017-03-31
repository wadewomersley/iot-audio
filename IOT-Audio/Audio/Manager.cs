namespace IOT_Audio.Audio
{
    using IOT_Audio.Server.Model.JsonObjects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Storage;

    internal class Manager
    {
        public Manager()
        {
        }

        public async Task<IReadOnlyList<StorageFile>> GetFileList()
        {
            var music = await KnownFolders.MusicLibrary.GetFilesAsync();

            return music;
        }

        internal void SaveStartupVolume(int volume)
        {
            ApplicationData.Current.LocalSettings.Values["volume"] = volume;
        }

        internal void SaveStartupFile(string fileName)
        {
            ApplicationData.Current.LocalSettings.Values["startupFile"] = fileName;
        }

        internal string GetStartupFile()
        {
            var file = ApplicationData.Current.LocalSettings.Values["startupFile"];
            return file?.ToString();
        }

        internal int GetStartupVolume()
        {
            var volume = ApplicationData.Current.LocalSettings.Values["volume"];
            return volume is int ? (int)volume : 100;
        }
    }
}
