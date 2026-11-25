using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Headless;

/// <summary>One player at the end of a run.</summary>
internal sealed record PlayerEnd(int Id, string Type, string Status, string GameOver);

/// <summary>
/// The map's end state. <see cref="How"/> is win, loss, turn-limit or allied-victory when the
/// game ended, read from the game result, the day limit and the player end states.
/// </summary>
internal sealed record RunEnd(string GameResult, int[] ActivePlayers, PlayerEnd[] Players, string? How)
{
    public static RunEnd Capture(StrategicMap map, bool ended)
    {
        int won = 0;
        int lost = 0;
        PlayerEnd[] players = new PlayerEnd[map.Players.Count];
        for (int i = 0; i < map.Players.Count; i++)
        {
            Player player = map.Players.GetPlayers(i) ?? throw new InvalidOperationException("The seat is missing.");
            if (player.GameOverStatus == GameOverStatus.Won)
            {
                won++;
            }

            if (player.GameOverStatus == GameOverStatus.Lost)
            {
                lost++;
            }

            players[i] = new PlayerEnd(
                Id: PlayerIdExtensions.ToOriginal(player.PlayerIndex),
                Type: player.PlayerType.ToString(),
                Status: player.Status.ToString(),
                GameOver: player.GameOverStatus.ToString());
        }

        int[] active = new int[map.Players.ActivePlayers.Count];
        for (int i = 0; i < active.Length; i++)
        {
            active[i] = map.Players.ActivePlayers[i];
        }

        return new RunEnd(
            GameResult: map.GameResult.ToString(),
            ActivePlayers: active,
            Players: players,
            How: Describe(ended, result: map.GameResult, day: map.Day, dayLimit: map.DayLimit, won: won, lost: lost));
    }

    /// <summary>
    /// Allied victory is two or more winners. A turn limit with no winner is the day limit.
    /// One winner, or a campaign success, is a win. A campaign failure or a loss with no winner is a loss.
    /// </summary>
    public static string? Describe(bool ended, StrategicMapGameResult result, int day, int dayLimit, int won, int lost)
        => !ended ? null
        : won >= 2 ? "allied-victory"
        : dayLimit != 0 && day >= dayLimit && won == 0 ? "turn-limit"
        : result == StrategicMapGameResult.Succeeded || won == 1 ? "win"
        : result == StrategicMapGameResult.Failed || lost > 0 ? "loss"
        : "ended";
}
