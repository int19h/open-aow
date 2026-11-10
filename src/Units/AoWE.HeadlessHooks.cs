using Aow.Game.Commands;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
#nullable enable

namespace Aow.Game.Engine
{

    public static class HeadlessHooks
    {
        public static Action<PlayerControl>? AfterNewDay { get; set; }
        public static Action? BeforeAIPlayerProcess { get; set; }
        public static Action<int, int>? ProcessCost { get; set; }
        public static Action<MoveTE>? BeforeMove { get; set; }
        public static Action<int, int, int, int>? MoveLine { get; set; }
        public static int TacticalBattles { get; set; }
        public static bool Tactical { get; set; }
        public static int TacticalSelections { get; set; }
        public static int TacticalExecutions { get; set; }
    }
}

namespace Aow.Game.Commands
{

    public partial class MoveTE
    {
        public virtual void InitializeMove()
        {
            FMoveIndex = FPath.Count - 1;
            HeadlessHooks.BeforeMove?.Invoke(this);
        }
    }
}

namespace Aow.Game.Session
{

    public partial class TurnPlayerControl
    {
        public override void NewDay()
        {
            var map = AoWHSMap.Current;
            int player = map.FSeatedPlayer;
            if (map.FRequestCombatType) map.FSeatedPlayer = -1;
            base.NewDay();
            if (map.FRequestCombatType) map.FSeatedPlayer = (sbyte)player;
            HeadlessHooks.AfterNewDay?.Invoke(this);
        }
    }
}

namespace Aow.Game.Ai
{

    public partial class AIExecuter
    {
        // AI.pas:612. The callback records the source work charge.
        public void UseProcessTime(int time)
        {
            HeadlessHooks.ProcessCost?.Invoke(time, FProcessTimeUsed);
            if (FStatus.Contains(AIExecuterStatusElement.Executing)) FProcessTimeUsed += time;
        }
    }
}
