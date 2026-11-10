// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Session;

public partial class TurnInfo : ENode
{
    public TurnInfo(ECustomNode owner) : base(owner)
    { }

    // Player.PAS:177
    public TurnInfoItem GetTurnInfo(int Index) => (TurnInfoItem)FChildren.List[Index];

    // Engine.pas:631

}
