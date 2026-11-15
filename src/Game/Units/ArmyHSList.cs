// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.Units;

public partial class ArmyHSList : ObservableEventList
{
    public ArmyHSList(CustomNode owner) : base(owner)
    { }

    // Army.PAS:3562
    public ArmyHexSprite GetArmyHexSprite(int index) => (ArmyHexSprite)GetChild(index);

    // Engine.pas:708

}
