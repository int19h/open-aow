#nullable enable
using Aow.Game.Commands;

using Aow.Game.Session;

namespace Aow.Game.Host;

public static class HeadlessHooks
{
    public static Action<PlayerControl>? AfterNewDay { get; set; }
    public static Action? BeforeAIPlayerProcess { get; set; }
    public static Action<int, int>? ProcessCost { get; set; }
    public static Action<MoveTokenEvent>? BeforeMove { get; set; }
    public static Action<int, int, int, int>? MoveLine { get; set; }
    public static int TacticalBattles { get; set; }
    public static bool Tactical { get; set; }
    public static int TacticalSelections { get; set; }
    public static int TacticalExecutions { get; set; }
}
