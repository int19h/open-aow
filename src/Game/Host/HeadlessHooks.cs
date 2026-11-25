#nullable enable
using Aow.Game.Commands;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Host;

public static class HeadlessHooks
{
    /// <summary>
    /// The host drives each human seat with the AI for this run. The seat stays Human, so the
    /// seated player, the local human list, the turn timer and the campaign result stay on the
    /// paths the game already uses for a human. Turn and simultaneous player control hand that
    /// seat to the AI executor when its turn or day starts, and take it back when the turn ends,
    /// the same two calls a CPU seat gets. The host sets this before the map activates and clears
    /// it when the run ends. The handoff lives inside those methods, on the same stack as the turn
    /// change, so the host frame loop cannot do it on its own. A seat left in the executor keeps
    /// the executor's current player on the finished human.
    /// </summary>
    public static bool DriveHumanSeats { get; set; }

    /// <summary>
    /// True when this seat is handed to the AI executor. A CPU seat always is. A human seat is
    /// only while <see cref="DriveHumanSeats"/> is set.
    /// </summary>
    public static bool HandSeatToAI(PlayerType type) => type != PlayerType.Human || DriveHumanSeats;

    public static Action<PlayerControl>? AfterNewDay { get; set; }
    public static Action? BeforeAIPlayerProcess { get; set; }
    public static Action<int, int>? ProcessCost { get; set; }
    public static Action<MoveTokenEvent>? BeforeMove { get; set; }
    public static Action<MapCoordinate, int, int, int>? MoveLine { get; set; }
    public static int TacticalBattles { get; set; }
    public static bool Tactical { get; set; }
    public static int TacticalSelections { get; set; }
    public static int TacticalExecutions { get; set; }
}
