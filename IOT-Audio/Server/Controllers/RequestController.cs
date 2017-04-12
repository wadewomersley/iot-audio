namespace IOT_Audio.Server.Controllers
{
    using System;
    using Audio;
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

        private bool IsValidApiKey(string apiKey)
        {
            return apiKey.Equals(Manager.GetApiKey());
        }

        [UriFormat("/settings?apiKey={apiKey}")]
        public IGetResponse GetSettings(string apiKey)
        {
            if (!IsValidApiKey(apiKey))
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound);
            }

            var settings = new Settings()
            {
                StartupFilename = Manager.GetStartupFile(),
                Volume = Manager.GetStartupVolume(),
                ApiKeySaved = Manager.GetApiKeySaved(),
                ApiKey = Manager.GetApiKey()
            };

            return new GetResponse(GetResponse.ResponseStatus.OK, settings);
        }

        [UriFormat("/apikey/seen")]
        public IPutResponse SeenApiKey()
        {
            Manager.SetApiKeySaved();
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/apikey")]
        public IPutResponse SetApiKey([FromContent] ApiKeyBase data)
        {
            if (!IsValidApiKey(data.ApiKey))
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound);
            }

            Manager.SetApiKey(data.ApiKey);
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/volume")]
        public IPutResponse SetVolume([FromContent] SetVolumeData data)
        {
            if (!IsValidApiKey(data.ApiKey))
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound);
            }

            Player.SetVolume(data.Volume);
            Manager.SaveStartupVolume(data.Volume);
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/play")]
        public IPutResponse SetFileToPlay([FromContent] SetFileData data)
        {
            if (!IsValidApiKey(data.ApiKey))
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound);
            }

            Player.SetFileName(data.FileName);
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/startupFile")]
        public IPutResponse SetStartupFile([FromContent] SetFileData data)
        {
            if (!IsValidApiKey(data.ApiKey))
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound);
            }

            Manager.SaveStartupFile(data.FileName);
            return new PutResponse(PutResponse.ResponseStatus.NoContent);
        }

        [UriFormat("/playlist?apiKey={apiKey}")]
        public IGetResponse GetFileList(string apiKey)
        {
            if (!IsValidApiKey(apiKey))
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound);
            }

            var fileList = Manager.GetFileList().GetAwaiter().GetResult();
            var files = fileList.Select(f => FileInformation.FromStorage(f)).ToList();
            var body = new PlaylistData() { Files = files.ToArray() };
            
            return new GetResponse(GetResponse.ResponseStatus.OK, body); ;
        }
    }
}
