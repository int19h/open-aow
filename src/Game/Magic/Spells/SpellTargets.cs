
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

internal static class SpellTargets
{

    internal static void NotifySpellSphere(sbyte player, MagicSphere sphere)
    {
        EngineMessage message = default;
        EngineMessage.InitializeMessage(ref message, 537002242, null);
        message.Param[0] = player;
        message.Param[1] = (int)sphere;
        AoWHSMap.Current.SendMessage(ref message, null);
    }

}
