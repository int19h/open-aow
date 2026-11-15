// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.Units;

public partial class ArmyHSList : ObservableEventList
{
    public ArmyHSList(CustomNode owner) : base(owner)
    { }

    // Army.PAS:3562
    // The list stores army sprites. A child of another class throws, the same way the old cast did.
    public ArmyHexSprite GetArmyHexSprite(int index) => GetChild<ArmyHexSprite>(index);

    // Engine.pas:708

}
