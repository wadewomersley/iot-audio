namespace IOT_Audio.Audio
{
    using System;
    using Windows.Media.Playback;
    using Windows.Storage;

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
            MediaPlayer.SetFileSource(file);
            MediaPlayer.Play();
        }

        internal async void SetFileName(string filename)
        {
            try
            {
                var music = await KnownFolders.MusicLibrary.GetFileAsync(filename);
                SetFile(music);
            }
            catch(Exception)
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
