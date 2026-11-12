// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Units;

// inherited children, update lock, change event
public partial class ItemHSList : ItemList
{
    public ItemHSList(ECustomNode owner) : base(owner)
    { }

    // Army.PAS:5202
    public override void SetOwner(ECustomNode owner)
    {
        _owner = owner;
    }

    // intf_P2_04.inc:118

}
