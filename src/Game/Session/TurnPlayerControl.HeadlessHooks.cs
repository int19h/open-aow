#nullable enable
using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

public partial class TurnPlayerControl
{
    public override void NewDay()
    {
        var map = StrategicMap.Current;
        int player = map.SeatedPlayer;
        if (map.RequestCombatType) map.SeatedPlayer = -1;
        base.NewDay();
        if (map.RequestCombatType) map.SeatedPlayer = (sbyte)player;
        HeadlessHooks.AfterNewDay?.Invoke(this);
    }
}
