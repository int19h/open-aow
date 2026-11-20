#nullable disable

using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

internal static class SpellTargets
{

    internal static void NotifySpellSphere(sbyte player, MagicSphere sphere)
    {
        EngineMessage.InitializeMessage(out EngineMessage message, 537002242, null);
        EngineMessageParameters param = message.Param;
        param[0] = player;
        message.Param = param;
        EngineMessageParameters param2 = message.Param;
        param2[1] = (int)sphere;
        message.Param = param2;
        StrategicMap.Current.SendMessage(ref message, null);
    }
}
