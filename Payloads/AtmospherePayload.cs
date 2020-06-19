using Assets.Scripts.Atmospherics;

namespace WebAPI.Payloads
{
    public class AtmospherePayload : IAtmosphericContentPayload
    {
        public float? roomId { get; set; }
        public float? pipeNetworkId { get; set; }
        public bool isGlobal { get; set; }

        public float oxygen { get; set; }
        public float volatiles { get; set; }
        public float water { get; set; }
        public float carbonDioxide { get; set; }
        public float nitrogen { get; set; }
        public float nitrousOxide { get; set; }
        public float pollutant { get; set; }
        public float volume { get; set; }
        public float energy { get; set; }
        public float temperature { get; set; }
        public float pressure { get; set; }

        public static AtmospherePayload FromAtmosphere(Atmosphere atmosphere)
        {
            var payload = new AtmospherePayload()
            {
                roomId = atmosphere.Room?.RoomId,
                pipeNetworkId = atmosphere.PipeNetwork?.NetworkId,
                isGlobal = atmosphere.IsGlobalAtmosphere,
            };
            payload.CopyFromAtmosphere(atmosphere);
            return payload;
        }
    }
}