
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

internal static class SpellTargets
{

    internal static Type SpellStructureClass = typeof(Structure);

    internal static readonly string SpellCancelRStr = "Cancel";

    internal static readonly string SpellXCastRStr = "%s cast";

    internal static void NotifySpellSphere(sbyte Player, MagicSphere Sphere)
    {
        EngineMessage Msg = default;
        EngineMessage.InitMsg(ref Msg, 537002242, null);
        Msg.Param[0] = Player;
        Msg.Param[1] = (int)Sphere;
        AoWHSMap.Current.SendMsg(ref Msg, null);
    }

}
