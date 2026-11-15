// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Units;

public partial class ArmyList : ObservableEventList
{
    public ArmyList(CustomNode owner) : base(owner)
    { }

    public Player Player { get; set; }

    // Player.PAS:6
    public Army GetArmy(int index) => (Army)GetChild(index);

    // Engine.pas:708

}
