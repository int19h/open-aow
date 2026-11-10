// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// Inherited world-map field layout
public partial class AoWCombatMapField : AoWMapField
{
    public AoWCombatMapField(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:2158
    public new const int ClassID = 2228481;

    // AoWTC.pas:2163
    public sbyte GetDefenseValue()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 135169;
        Msg.Result = -1;
        Msg.Param[0] = 0;
        SendMapFieldMsg(ref Msg);
        return (sbyte)Msg.Param[0].Int;
    }

}
