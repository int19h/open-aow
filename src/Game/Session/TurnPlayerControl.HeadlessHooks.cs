#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

public partial class TurnPlayerControl
{
    public override void NewDay()
    {
        StrategicMap map = StrategicMap.Current;
        int player = PlayerIdExtensions.ToOriginal(map.SeatedPlayer);
        if (map.RequestCombatType)
        {
            map.SeatedPlayer = null;
        }

        base.NewDay();
        if (map.RequestCombatType)
        {
            map.SeatedPlayer = PlayerId.FromOriginal((sbyte)player);
        }

        HeadlessHooks.AfterNewDay?.Invoke(this);
    }
}
