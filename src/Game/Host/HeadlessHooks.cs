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

    /// <summary>
    /// Called first in <see cref="Player.SetGameOverStatus"/> with the player and the requested
    /// status, before the game tests or changes anything (Player.PAS:425). The oracle's campaign
    /// victory run (issue 344) turns the campaign seat human here at its victory, so the game's
    /// own campaign branch runs FinishScenario and the transfer Retrieve. The native harness
    /// does the same at a breakpoint on the entry of TPlayer.SetGameOverStatus.
    /// </summary>
    public static Action<Player, GameOverStatus>? BeforeSetGameOverStatus { get; set; }

    /// <summary>
    /// Called first in <see cref="StrategicMap.StartLoadedMap"/>, after activation rebuilt the
    /// player lists and before the player control seats the first local human (AoWHSMap.PAS:2615,
    /// PlayerC.PAS:779). The oracle's campaign victory run (issue 344) makes the campaign seat a CPU
    /// player here, so the seat is still seated as the local human and plays its turn as the AI. The
    /// native harness does the same at a breakpoint on the entry of TAoWHSMap.StartLoadedMap.
    /// </summary>
    public static Action<StrategicMap>? BeforeStartLoadedMap { get; set; }

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
