namespace IOT_Audio.Audio
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Media.Playback;
    using Windows.Storage;

    class Player
    {
        private StorageFile File;
        private MediaPlayer MediaPlayer;

        internal Player()
        {
            MediaPlayer = BackgroundMediaPlayer.Current;
            MediaPlayer.IsLoopingEnabled = true;
            MediaPlayer.AutoPlay = true;
        }

        internal async void SetFile(StorageFile file)
        {
            MediaPlayer.SetFileSource(file);
            MediaPlayer.Play();
        }

        internal async void SetFileName(string filename)
        {
            try
            {
                var music = await Windows.Storage.KnownFolders.MusicLibrary.GetFileAsync(filename);
                SetFile(music);
            }
            catch(Exception ex)
            {
            }
        }

        internal void SetVolume(int vol)
        {
            var curVol = MediaPlayer.Volume;
            MediaPlayer.Volume = (double)vol / 100;
        }
    }
}
