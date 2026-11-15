
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

internal static class SpellTargets
{

    internal static void NotifySpellSphere(sbyte player, MagicSphere sphere)
    {
        EngineMessage message = default;
        EngineMessage.InitializeMessage(out message, 537002242, null);
        var param = message.Param;
        param[0] = player;
        message.Param = param;
        var param2 = message.Param;
        param2[1] = (int)sphere;
        message.Param = param2;
        StrategicMap.Current.SendMessage(ref message, null);
    }

}
