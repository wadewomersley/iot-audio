namespace IOT_Audio.Server
{
    using Audio;
    using Controllers;
    using Restup.Webserver.File;
    using Restup.Webserver.Http;
    using Restup.Webserver.Rest;
    using System;

    internal class WebServer : IDisposable
    {
        private HttpServer Server;

        internal WebServer(Player player, Manager manager)
        {
            var routeHandler = new RestRouteHandler();
            routeHandler.RegisterController<RequestController>(player, manager);

            var config = new HttpServerConfiguration();
            
            config.ListenOnPort((int)manager.GetPublicPort())
                .RegisterRoute("api", routeHandler)
                .RegisterRoute(new StaticFileRouteHandler(@"Assets"));

            Server = new HttpServer(config);
        }

        public void Dispose()
        {
            Server.StopServer();
        }

        internal void Initialize()
        {
            var task = Server.StartServerAsync();
            task.Wait();
        }
    }
}
