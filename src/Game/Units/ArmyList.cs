// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Units;

public partial class ArmyList : EChangeNotifyEventList
{
    public ArmyList(ECustomNode owner) : base(owner)
    { }

    public Player FPlayer;

    // Player.PAS:6
    public Army GetArmy(int Index) => (Army)FChildren.List[Index];

    // Engine.pas:708

}
