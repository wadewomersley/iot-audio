namespace IOT_Audio.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Audio;
    using Windows.Storage;
    using System.Linq;
    using Restup.Webserver.Attributes;
    using Restup.Webserver.Models.Schemas;
    using Restup.Webserver.Models.Contracts;
    using Server.Model.JsonObjects;

    [RestController(InstanceCreationType.Singleton)]
    internal class RequestController
    {
        private Player Player;
        private Manager Manager;

        public RequestController(Player player, Manager manager)
        {
            Player = player;
            Manager = manager;
        }

        [UriFormat("/settings")]
        public IGetResponse GetSettings()
        {
            var settings = new Settings()
            {
                StartupFilename = Manager.GetStartupFile(),
                Volume = Manager.GetStartupVolume()
            };

            return new GetResponse(GetResponse.ResponseStatus.OK, settings);
        }
        
        [UriFormat("/volume")]
        public IPutResponse SetVolume([FromContent] SetVolumeData data)
        {
            Player.SetVolume(data.Volume);
            Manager.SaveStartupVolume(data.Volume);
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
            Manager.SaveStartupFile(data.FileName);
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/playlist")]
        public IGetResponse GetFileList()
        {
            var fileList = Manager.GetFileList().GetAwaiter().GetResult();
            var files = fileList.Select(f => FileInformation.FromStorage(f)).ToList();
            var body = new PlaylistData() { Files = files.ToArray() };

            var response = new GetResponse(GetResponse.ResponseStatus.OK, body);
            return response;
        }
    }
}
