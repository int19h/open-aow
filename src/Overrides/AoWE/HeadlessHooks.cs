#nullable enable
using Aow.Rtl;

namespace Aow.Units.AoWE;

public static class HeadlessHooks
{
    public static Action<TPlayerControl>? AfterNewDay { get; set; }
    public static bool Tactical { get; set; }
    public static int TacticalSelections { get; set; }
    public static int TacticalExecutions { get; set; }
}

public partial class TTurnPlayerControl
{
    public override void NewDay()
    {
        var map = AoWE.AoWHSMap;
        int player = map.FSeatedPlayer;
        if (map.FRequestCombatType) map.FSeatedPlayer = -1;
        base.NewDay();
        if (map.FRequestCombatType) map.FSeatedPlayer = unchecked((sbyte)player);
        HeadlessHooks.AfterNewDay?.Invoke(this);
    }
}
