
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

internal static class SpellTargets
{

    internal static void NotifySpellSphere(sbyte player, MagicSphere sphere)
    {
        EngineMessage message = default;
        EngineMessage.InitializeMessage(ref message, 537002242, null);
        message.Param[0] = player;
        message.Param[1] = (int)sphere;
        StrategicMap.Current.SendMessage(ref message, null);
    }

}
