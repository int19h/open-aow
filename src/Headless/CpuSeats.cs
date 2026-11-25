using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Headless;

/// <summary>
/// Hands human seats to the AI without changing their type. A turn game hands the current
/// playing human, which is the open turn. A simultaneous game hands every playing human,
/// because their day starts together. Later turns are handed by the player control while
/// <see cref="Game.Host.HeadlessHooks.DriveHumanSeats"/> is set, and that flag takes the
/// seat back when the turn ends. The seated player, the local human list, the turn timer and
/// the campaign result stay on the human paths. Applying this after activation is safe when
/// the player control already handed the same seats over. The executor refuses a second entry.
/// </summary>
internal static class CpuSeats
{
    public static void Apply(StrategicMap map)
    {
        if (map.PlayMode == PlayMode.Simultaneous)
        {
            for (int i = 0; i < map.Players.Count; i++)
            {
                Player? player = map.Players.GetPlayers(i);
                if (player is { PlayerType: PlayerType.Human, Status: PlayerStatus.Playing })
                {
                    _ = map.AIExecutor.ActivatePlayer(PlayerId.FromOriginal((sbyte)i));
                }
            }

            return;
        }

        Player? current = map.Players.GetPlayers(map.CurrentPlayer);
        if (current is { PlayerType: PlayerType.Human, Status: PlayerStatus.Playing })
        {
            _ = map.AIExecutor.ActivatePlayer(map.CurrentPlayer);
        }
    }
}
