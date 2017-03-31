namespace IOT_Audio
{
    using Audio;
    using Server;
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static Uri AudioUri = new Uri(@"http://localhost:16000");
        private WebServer WebServer;

        public MainPage()
        {
            InitializeComponent();

            var mediaPlayer = new Player();
            var manager = new Manager();

            mediaPlayer.SetVolume(manager.GetStartupVolume());
            mediaPlayer.SetFileName(manager.GetStartupFile());

            WebServer = new WebServer(mediaPlayer, manager);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WebServer.Initialize();
            webView.Source = AudioUri;
        }
    }
}
