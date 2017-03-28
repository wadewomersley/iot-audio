namespace IOT_Audio.Server
{
    using Audio;
    using Controllers;
    using Model.JsonObjects;
    using Restup.Webserver.File;
    using Restup.Webserver.Http;
    using Restup.Webserver.Rest;

    internal class WebServer
    {
        private int Port = 16000;
        private HttpServer Server;

        internal WebServer(Player player, Settings startupSettings)
        {
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
