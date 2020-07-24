
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;

namespace WebAPI.Controllers
{
    [WebController(Path = "api/stationpedia")]
    class StationpediasController
    {
        [WebRouteMethod(Method = "GET", Path = "ic/instructions")]
        public async Task GetICInstructions(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);
            await context.SendResponse(HttpStatusCode.OK, ICInstructionPayload.FromGame());
        }

        [WebRouteMethod(Method = "GET", Path = "logic/slottypes")]
        public async Task GetLogicSlotTypes(IHttpContext context)
        {       
            Authenticator.VerifyAuth(context);
            await context.SendResponse(HttpStatusCode.OK, LogicSlotTypesPayload.FromGame());
        }

        [WebRouteMethod(Method = "GET", Path = "logic/types")]
        public async Task GetLogicTypes(IHttpContext context)
        {       
            Authenticator.VerifyAuth(context);
            await context.SendResponse(HttpStatusCode.OK, LogicTypesPayload.FromGame());
        }

        [WebRouteMethod(Method = "GET", Path = "things")]
        public async Task GetThings(IHttpContext context)
        {       
            Authenticator.VerifyAuth(context);
            var things = await Dispatcher.RunOnMainThread(() => ThingPrefabPayload.FromGame());
            await context.SendResponse(HttpStatusCode.OK, things);
        }
    }
}