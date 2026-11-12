// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.Units;

public partial class ArmyHSList : EChangeNotifyEventList
{
    public ArmyHSList(ECustomNode owner) : base(owner)
    { }

    // Army.PAS:3562
    public ArmyHexSprite GetArmyHexSprite(int index) => (ArmyHexSprite)_children.List[index];

    // Engine.pas:708

}
