namespace IOT_Audio.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Audio;
    using Windows.Storage;
    using Restup.Webserver.Attributes;
    using Restup.Webserver.Models.Schemas;
    using Restup.Webserver.Models.Contracts;
    using Server.Model.JsonObjects;

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
            Player.SetFileName(data.FileName);
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/startupFile")]
        public IPutResponse SetStartupFile([FromContent] SetFileData data)
        {
            ApplicationData.Current.LocalSettings.Values["startupFile"] = data.FileName;
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/playlist")]
        public IGetResponse GetFileList()
        {
            var task = Task.Run(async () => {
                var music = await KnownFolders.MusicLibrary.GetFilesAsync();
                var files = new List<FileInformation>(music.Count);
                foreach (var file in music)
                {
                    files.Add(new FileInformation() { FileName = file.Name, DisplayName = file.DisplayName });
                }

                var response = new GetResponse(GetResponse.ResponseStatus.OK, new PlaylistData() { Files = files.ToArray() });

                return response;
            });
            task.Wait();
            return task.Result;
        }
    }
}
