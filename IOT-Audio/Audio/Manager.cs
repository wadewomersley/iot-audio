namespace IOT_Audio.Audio
{
    using System;
    using System.Collections.Generic;
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

        internal bool GetApiKeySaved()
        {
            var apiKeySaved = ApplicationData.Current.LocalSettings.Values["apiKeySaved"];
            return apiKeySaved is bool ? (bool)apiKeySaved : false;
        }

        internal void SetApiKeySaved()
        {
            ApplicationData.Current.LocalSettings.Values["apiKeySaved"] = true;
        }

        internal int GetStartupVolume()
        {
            var volume = ApplicationData.Current.LocalSettings.Values["volume"];
            return volume is int ? (int)volume : 100;
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

        internal void SavePublicPort(uint port)
        {
            ApplicationData.Current.LocalSettings.Values["publicPort"] = port;
        }

        internal uint GetPublicPort()
        {
            var publicPort = ApplicationData.Current.LocalSettings.Values["publicPort"];
            return publicPort is uint ? (uint)publicPort : 16000;
        }

        internal string GetApiKey()
        {
            var key = ApplicationData.Current.LocalSettings.Values["apiKey"]?.ToString();

            if (key == null)
            {
                key = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                key = key.Replace("=", "").Replace("+", "").Replace("/", "");
                SetApiKey(key);
            }

            return key;
        }

        internal void SetApiKey(string apiKey)
        {
            ApplicationData.Current.LocalSettings.Values["apiKey"] = apiKey;
        }
    }
}
