#nullable enable
using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

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
