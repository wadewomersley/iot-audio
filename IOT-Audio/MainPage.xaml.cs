namespace IOT_Audio
{
    using Audio;
    using Server.Model.JsonObjects;
    using Server;
    using System;
    using Windows.Storage;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static Uri AudioUri = new Uri(@"http://localhost:16000");
        private WebServer WebServer;
        private Player MediaPlayer;

        public MainPage()
        {
            InitializeComponent();

            MediaPlayer = new Player();

            var volume = 100;
            var startupFile = (string)null;

            if (ApplicationData.Current.LocalSettings.Values["volume"] is int)
            {
                volume = (int)ApplicationData.Current.LocalSettings.Values["volume"];
                MediaPlayer.SetVolume(volume);
            }

            if (ApplicationData.Current.LocalSettings.Values["startupFile"] != null)
            {
                startupFile = ApplicationData.Current.LocalSettings.Values["startupFile"].ToString();
                MediaPlayer.SetFileName(startupFile);
            }

            var settings = new Settings()
            {
                StartupFilename = startupFile,
                Volume = volume
            };


            WebServer = new WebServer(MediaPlayer, settings);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WebServer.Initialize();
            webView.Source = AudioUri;
        }
    }
}
