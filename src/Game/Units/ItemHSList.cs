#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Units;

// inherited children, update lock, change event
public sealed partial class ItemHSList(CustomNode owner) : ItemList(owner)
{

    // Army.PAS:5202
    public override void SetOwner(CustomNode owner) => SetOwnerWithoutNotify(owner);
}
