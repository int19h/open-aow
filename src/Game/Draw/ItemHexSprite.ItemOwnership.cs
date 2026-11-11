using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Draw;

public partial class ItemHexSprite
{
    public ItemHexSprite(ECustomNode owner) : base(owner)
    {
        FItems = new ItemHSList(null);
        FItems.Sprite = this;
        FItems.FOnChanged = ItemsChanged;
    }
}
