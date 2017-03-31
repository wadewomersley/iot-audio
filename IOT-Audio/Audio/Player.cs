namespace IOT_Audio.Audio
{
    using System;
    using Windows.Media.Core;
    using Windows.Media.Playback;
    using Windows.Storage;
    using System.Diagnostics.Contracts;

    class Player
    {
        private MediaPlayer MediaPlayer;

        internal Player()
        {
            MediaPlayer = BackgroundMediaPlayer.Current;
            MediaPlayer.IsLoopingEnabled = true;
            MediaPlayer.AutoPlay = true;
        }

        internal void SetFile(StorageFile file)
        {
            MediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
            MediaPlayer.Play();
        }

        internal async void SetFileName(string filename)
        {
            if (filename == null)
            {
                return;
            }

            try
            {
                var music = await KnownFolders.MusicLibrary.GetFileAsync(filename);
                SetFile(music);
            }
            catch(Exception)
            {
            }
        }

        /// <summary>
        /// Sets volume between 0 and 100 (%).
        /// </summary>
        /// <param name="vol"></param>
        internal void SetVolume(int vol)
        {
            Contract.Requires(vol >=0 && vol <= 100, "Volume must be between 0 and 100");

            MediaPlayer.Volume = (double)vol / 100;
        }
    }
}
