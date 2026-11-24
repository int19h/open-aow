#nullable disable warnings

using Aow.Engine.Nodes;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

internal static class SpellTargets
{

    internal static void NotifySpellSphere(PlayerId? player, MagicSphere sphere)
    {
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537002242, sender: null);
        EngineMessageParameters param = message.Param;
        param[0] = Session.MsgArgIds.FromPlayer(player);
        message.Param = param;
        EngineMessageParameters param2 = message.Param;
        param2[1] = (int)sphere;
        message.Param = param2;
        StrategicMap.Current.SendMessage(ref message, sender: null);
    }
}
