using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Draw;

public partial class ItemHS
{
    public ItemHS(ECustomNode owner) : base(owner)
    {
        FItems = new ItemHSList(null);
        FItems.Sprite = this;
        FItems.FOnChanged = ItemsChanged;
    }
}
