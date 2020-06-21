

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Ceen;
using Ceen.Httpd;

namespace WebAPI.Server
{
    public class WebServer : IDisposable
    {
        private readonly HttpHandlerDelegate requestHandler;
        private CancellationTokenSource _cancellationTokenSource;

        public WebServer(HttpHandlerDelegate requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        public void Start(int port)
        {
            Logging.Log("Starting web server");

            var tcs = new CancellationTokenSource();
            var config = new ServerConfig()
                .AddLogger(OnLogMessage)
                .AddRoute(this.requestHandler);

            this._cancellationTokenSource = new CancellationTokenSource();
            HttpServer.ListenAsync(
                new IPEndPoint(IPAddress.Any, port),
                false,
                config,
                _cancellationTokenSource.Token
            );

            Logging.Log(string.Format("Server started on port {0}", port));
        }

        public void Dispose()
        {
            this._cancellationTokenSource.Cancel();
            Logging.Log("Server stopped");
        }

        private Task OnLogMessage(IHttpContext context, Exception exception, DateTime started, TimeSpan duration)
        {
            if (exception != null)
            {
                Logging.Log(
                    new Dictionary<string, string>() {
                    {"RequestMethod", context.Request.Method},
                    {"RequestPath", context.Request.Path},
                    {"RemoteEndpoint", context.Request.RemoteEndPoint.ToString()}
                    },
                    exception.ToString()
                );
            }
            return Task.CompletedTask;
        }
    }
}