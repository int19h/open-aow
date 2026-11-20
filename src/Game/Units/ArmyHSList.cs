// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.Units;

public sealed class ArmyHSList(CustomNode owner) : ObservableEventList(owner)
{

    // Army.PAS:3562
    // The list stores army sprites. A child of another class throws, the same way the old cast did.
    public ArmyHexSprite this[int index] => GetChild<ArmyHexSprite>(index);
}
