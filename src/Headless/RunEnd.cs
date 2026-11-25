using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Headless;

/// <summary>One player at the end of a run.</summary>
internal sealed record PlayerEnd(int Id, string Type, string Status, string GameOver);

/// <summary>
/// The map's end state. <see cref="How"/> is win, loss, turn-limit or allied-victory when the
/// game ended, read from the game result, the day limit and the player end states.
/// </summary>
internal sealed record RunEnd(
    string GameResult,
    IReadOnlyList<int> ActivePlayers,
    IReadOnlyList<PlayerEnd> Players,
    string? How)
{
    public static RunEnd Capture(StrategicMap map, bool ended)
    {
        int wonCount = 0;
        int lostCount = 0;
        List<PlayerEnd> players = [];
        for (int i = 0; i < map.Players.Count; i++)
        {
            Player player = map.Players.GetPlayers(i) ?? throw new InvalidOperationException("The seat is missing.");
            if (player.GameOverStatus == GameOverStatus.Won)
            {
                wonCount++;
            }

            if (player.GameOverStatus == GameOverStatus.Lost)
            {
                lostCount++;
            }

            players.Add(new PlayerEnd(
                Id: PlayerIdExtensions.ToOriginal(player.PlayerIndex),
                Type: player.PlayerType.ToString(),
                Status: player.Status.ToString(),
                GameOver: player.GameOverStatus.ToString()));
        }

        List<int> activePlayers = [];
        for (int i = 0; i < map.Players.ActivePlayers.Count; i++)
        {
            activePlayers.Add(map.Players.ActivePlayers[i]);
        }

        return new RunEnd(
            GameResult: map.GameResult.ToString(),
            ActivePlayers: activePlayers.AsReadOnly(),
            Players: players.AsReadOnly(),
            How: Describe(ended, result: map.GameResult, day: map.Day, dayLimit: map.DayLimit, wonCount: wonCount, lostCount: lostCount));
    }

    /// <summary>
    /// Allied victory is two or more winners. A campaign success is a win, including a success
    /// on the limit day with no winner. A turn limit with no winner and no success is the day
    /// limit. One winner is a win. A campaign failure or a loss with no winner is a loss.
    /// </summary>
    public static string? Describe(bool ended, StrategicMapGameResult result, int day, int dayLimit, int wonCount, int lostCount)
        => !ended ? null
        : wonCount >= 2 ? "allied-victory"
        : result == StrategicMapGameResult.Succeeded ? "win"
        : dayLimit != 0 && day >= dayLimit && wonCount == 0 ? "turn-limit"
        : wonCount == 1 ? "win"
        : result == StrategicMapGameResult.Failed || lostCount > 0 ? "loss"
        : "ended";
}
