// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IOT_Audio
{
    using Audio;
    using Server;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.System.Threading;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Navigation;


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
            WebServer = new WebServer(MediaPlayer);
            webView.ScriptNotify += WebView_ScriptNotify;
        }

        private void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var a = 1;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WebServer.Initialize();
            webView.Source = AudioUri;
        }
    }
}
