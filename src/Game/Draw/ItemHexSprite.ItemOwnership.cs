using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Draw;

public partial class ItemHexSprite
{
    public ItemHexSprite(ECustomNode owner) : base(owner)
    {
        Items = new ItemHSList(null) { Holder = CreationHolder() };
        Items.Sprite = this;
        Items.OnChanged += ItemsChanged;
    }
}
