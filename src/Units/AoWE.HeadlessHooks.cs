#nullable enable

namespace Aow.Units.AoWE;

public static class HeadlessHooks
{
    public static Action<TPlayerControl>? AfterNewDay { get; set; }
    public static Action? BeforeAIPlayerProcess { get; set; }
    public static Action<int, int>? ProcessCost { get; set; }
    public static Action<TMoveTE>? BeforeMove { get; set; }
    public static Action<int, int, int, int>? MoveLine { get; set; }
    public static int TacticalBattles { get; set; }
    public static bool Tactical { get; set; }
    public static int TacticalSelections { get; set; }
    public static int TacticalExecutions { get; set; }
}

public partial class TMoveTE
{
    public virtual void InitializeMove()
    {
        FMoveIndex = FPath.Count - 1;
        HeadlessHooks.BeforeMove?.Invoke(this);
    }
}

public partial class TTurnPlayerControl
{
    public override void NewDay()
    {
        var map = AoWEUnit.AoWHSMap;
        int player = map.FSeatedPlayer;
        if (map.FRequestCombatType) map.FSeatedPlayer = -1;
        base.NewDay();
        if (map.FRequestCombatType) map.FSeatedPlayer = (sbyte)player;
        HeadlessHooks.AfterNewDay?.Invoke(this);
    }
}

public partial class TAIExecuter
{
    // AI.pas:612. The callback records the source work charge.
    public void UseProcessTime(int time)
    {
        HeadlessHooks.ProcessCost?.Invoke(time, FProcessTimeUsed);
        if (FStatus.Contains(TAIExecuterStatusElement.aesExecuting)) FProcessTimeUsed += time;
    }
}
