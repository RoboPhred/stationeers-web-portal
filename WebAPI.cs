
using System;
using System.Linq;
using System.Threading.Tasks;
using BepInEx;
using Ceen;
using UnityEngine;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI
{
    [BepInPlugin("net.robophreddev.stationeers.WebAPI", "Web API for Stationeers", "1.0.0.0")]
    public class WebAPIPlugin : BaseUnityPlugin
    {
        public static WebAPIPlugin Instance;

        private readonly WebServer _webServer = new WebServer();
        private readonly WebRouter _router = new WebRouter();

        public void Log(string line)
        {
            Debug.Log("[WebAPI]: " + line);
        }

        void Awake()
        {
            WebAPIPlugin.Instance = this;
            Log("Hello World");

            WebAPI.Config.LoadConfig();
            Dispatcher.Initialize();

            if (WebAPI.Config.Instance.enabled)
            {
                var webRouteType = typeof(IWebRoute);
                var foundTypes = typeof(IWebRoute).Assembly.GetTypes()
                    .Where(p => p.IsClass && p.GetInterfaces().Contains(webRouteType))
                    .ToArray();

                Log("Initializing " + foundTypes.Length + " routes.");
                foreach (var routeType in foundTypes)
                {
                    var instance = (IWebRoute)Activator.CreateInstance(routeType);
                    _router.AddRoute(instance);
                }
                Log("Routes initialized");

                _webServer.Start(OnRequest);
            }
            else
            {
                Log("Not enabled.");
            }
        }

        async Task<bool> OnRequest(IHttpContext context)
        {
            // Does this need to be outside of OPTIONS / apply to all requests?
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (context.Request.Method == "OPTIONS")
            {
                context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                context.Response.AddHeader("Access-Control-Max-Age", "1728000");
                context.Response.StatusCode = HttpStatusCode.OK;
                return true;
            }

            try
            {
                return await _router.HandleRequest(context);
            }
            catch (AuthenticationException e)
            {
                await context.SendResponse(HttpStatusCode.Unauthorized, new ErrorPayload()
                {
                    message = e.Message
                });
                return true;
            }
            catch (Exception e)
            {
                await context.SendResponse(HttpStatusCode.InternalServerError, new ErrorPayload()
                {
                    message = e.ToString()
                });
                return true;
            }
        }
    }
}