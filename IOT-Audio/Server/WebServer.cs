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
        public RequestController()
        {
        }

        [UriFormat("/volume")]
        public IPutResponse SetVolume([FromContent] SetVolumeData data)
        {
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }
    }

    internal sealed class SetVolumeData
    {
        public uint Volume { get; set; }
    }

    internal class WebServer
    {
        private StreamSocketListener Listener;
        private const uint BufferSize = 8192;
        private int Port = 16000;
        private Player Player;
        private string Html;

        internal WebServer()
        {
            Listener = new StreamSocketListener();
            Listener.ConnectionReceived += ProcessRequest;
        }

        internal async void Initialize(Player player)
        {
            this.Player = player;


            var routeHandler = new RestRouteHandler();
            routeHandler.RegisterController<RequestController>();

            var config = new HttpServerConfiguration();

            config.ListenOnPort(Port)
                .RegisterRoute("api", routeHandler)
                .RegisterRoute(new StaticFileRouteHandler(@"Assets"));

            var server = new HttpServer(config);
            await server.StartServerAsync();
        }

        private async void ProcessRequest(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            var socket = args.Socket;

            try
            {
                var request = new StringBuilder();
                using (var input = socket.InputStream)
                {
                    // Convert the request bytes to a string that we understand
                    byte[] data = new byte[BufferSize];
                    IBuffer buffer = data.AsBuffer();
                    uint dataRead = BufferSize;
                    while (dataRead == BufferSize)
                    {
                        await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                        request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                        dataRead = buffer.Length;
                    }
                }

                using (var output = socket.OutputStream)
                {
                    // Parse the request
                    string[] requestParts = request.ToString().Split('\n');
                    string requestMethod = requestParts[0];
                    string[] requestMethodParts = requestMethod.Split(' ');

                    // Process the request and write a response to send back to the browser
                    if (requestMethodParts[0].ToUpper() == "GET")
                    {
                        Debug.WriteLine("request for: {0}", requestMethodParts[1]);
                        await writeResponse(requestMethodParts[1], output, socket.Information);
                    }
                    else if (requestMethodParts[0].ToUpper() == "POST")
                    {
                        string requestUri = string.Format("{0}?{1}", requestMethodParts[1], requestParts[requestParts.Length - 1]);
                        Debug.WriteLine("POST request for: {0} ", requestUri);
                        await writeResponse(requestUri, output, socket.Information);
                    }
                    else
                    {
                        throw new InvalidDataException("HTTP method not supported: "
                                                       + requestMethodParts[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in processRequestAsync(): " + ex.Message);
            }
        }

        private async Task writeResponse(string request, IOutputStream os, StreamSocketInformation socketInfo)
        {
            try
            {
                request = request.TrimEnd('\0'); //remove possible null from POST request

                string[] requestParts = request.Split('/');

                if(requestParts.Length == 2 && requestParts[1].IndexOf("play=") == 0)
                {
                    var file = WebUtility.UrlDecode(requestParts[1].Substring(5));
                    this.Player.SetFileName(file);
                }
                else if (requestParts.Length == 2 && requestParts[1].IndexOf("volume=") == 0)
                {
                    int vol = 0;
                    if (int.TryParse(requestParts[1].Substring(7), out vol))
                    {
                        this.Player.SetVolume(vol);
                    }

                    await SendEmpty(os);
                }

                await SendFileList(os);
            }
            catch (Exception ex)
            {
            }
        }

        private async Task SendEmpty(IOutputStream os)
        {
            await WriteToStreamHelper("", os);
        }

        private async Task SendFileList(IOutputStream os)
        {
            string data = this.Html;

            var music = await Windows.Storage.KnownFolders.MusicLibrary.GetFilesAsync();
            var files = new List<string>(music.Count);
            foreach (var file in music)
            {
                files.Add("<li><a href=\"/api/play/" + file.Name + "\">" + file.DisplayName + "</a></li>");
            }

            data = data.Replace("%PLAYLIST%", String.Join("", files.ToArray()));

            
            await WriteToStreamHelper(data, os);
        }

        private static async Task WriteToStreamHelper(string data, IOutputStream os)
        {
            try
            {
                using (Stream resp = os.AsStreamForWrite())
                {
                    if (resp.CanWrite)
                    {
                        // Look in the Data subdirectory of the app package
                        byte[] bodyArray = Encoding.UTF8.GetBytes(data);
                        MemoryStream stream = new MemoryStream(bodyArray);
                        string header = String.Format("HTTP/1.1 200 OK\r\n" +
                                          "Content-Length: {0}\r\n" +
                                          "Connection: close\r\n\r\n",
                                          stream.Length);
                        byte[] headerArray = Encoding.UTF8.GetBytes(header);
                        await resp.WriteAsync(headerArray, 0, headerArray.Length);
                        await stream.CopyToAsync(resp);
                        await resp.FlushAsync();
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
