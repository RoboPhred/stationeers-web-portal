
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Networking;
using WebAPI.Payloads;

namespace WebAPI.Models
{
    public static class PlayersModel
    {
        public static PlayerPayload GetPlayer(ulong steamId)
        {
            var player = NetworkManagerOverride.PlayerConnections.Single(x => x.Key.SteamId == steamId).Key;
            if (player == null)
            {
                return null;
            }
            return PlayerPayload.FromPlayerConnection(player);

        }
        public static IList<PlayerPayload> GetPlayers()
        {
            return NetworkManagerOverride.PlayerConnections.Select(x => PlayerPayload.FromPlayerConnection(x.Key)).ToArray();
        }

        public static PlayerPayload KickPlayer(ulong steamId, string reason)
        {
            var player = NetworkManagerOverride.PlayerConnections.Single(x => x.Key.SteamId == steamId).Key;
            var playerPayload = PlayerPayload.FromPlayerConnection(player);
            NetworkManagerHudOverride.Instance.KickPlayer(player.SteamName, reason.Length > 0 ? reason : "");
            return playerPayload;
        }
    }
}