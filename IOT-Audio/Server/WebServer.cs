namespace IOT_Audio.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Foundation;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Diagnostics;
    using System.IO;
    using Audio;
    using System.Net;
    using Windows.ApplicationModel;
    using Windows.Storage;
    using Restup.Webserver.Rest;
    using Restup.Webserver.Attributes;
    using Restup.Webserver.Models.Schemas;
    using Restup.Webserver.Models.Contracts;
    using Restup.Webserver.Http;
    using Restup.Webserver.File;

    [RestController(InstanceCreationType.Singleton)]
    internal class RequestController
    {
        public static Player Player;
        public static Settings Settings;

        [UriFormat("/settings")]
        public IGetResponse GetSettings()
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, Settings);
        }
        
        [UriFormat("/volume")]
        public IPutResponse SetVolume([FromContent] SetVolumeData data)
        {
            Player.SetVolume(data.Volume);
            ApplicationData.Current.LocalSettings.Values["volume"] = data.Volume;
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/play")]
        public IPutResponse SetFileToPlay([FromContent] SetFileData data)
        {
            Player.SetFileName(data.Filename);
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/startupFile")]
        public IPutResponse SetStartupFile([FromContent] SetFileData data)
        {
            ApplicationData.Current.LocalSettings.Values["startupFile"] = data.Filename;
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/playlist")]
        public IGetResponse GetFileList()
        {
            var task = Task.Run(async () => {
                var music = await KnownFolders.MusicLibrary.GetFilesAsync();
                var files = new List<FileInfo>(music.Count);
                foreach (var file in music)
                {
                    files.Add(new FileInfo() { FileName = file.Name, DisplayName = file.DisplayName });
                }

                var response = new GetResponse(GetResponse.ResponseStatus.OK, new PlaylistData() { Files = files.ToArray() });

                return response;
            });
            task.Wait();
            return task.Result;
        }
    }

    internal sealed class Settings
    {
        public int Volume { get; set; }

        public string StartupFilename { get; set; }
    }

    internal sealed class FileInfo
    {
        public string FileName { get; set; }

        public string DisplayName { get; set; }
    }

    internal sealed class PlaylistData
    {
        public FileInfo[] Files { get; set; }
    }

    internal sealed class SetVolumeData
    {
        public int Volume { get; set; }
    }

    internal sealed class SetFileData
    {
        public string Filename { get; set; }
    }

    internal class WebServer
    {
        private int Port = 16000;
        private Player Player;
        private HttpServer Server;

        internal WebServer(Player player, int volume, string startupFile)
        {
            RequestController.Settings = new Settings() {
                StartupFilename = startupFile,
                Volume = volume
            };

            Player = player;
            RequestController.Player = player;

            var routeHandler = new RestRouteHandler();
            routeHandler.RegisterController<RequestController>();

            var config = new HttpServerConfiguration();

            config.ListenOnPort(Port)
                .RegisterRoute("api", routeHandler)
                .RegisterRoute(new StaticFileRouteHandler(@"Assets"));

            Server = new HttpServer(config);
        }

        internal void Initialize()
        {
            var task = Server.StartServerAsync();
            task.Wait();
        }
    }
}
