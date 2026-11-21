#nullable disable
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Draw;

public partial class ItemHexSprite
{
    public ItemHexSprite(CustomNode owner) : base(owner)
    {
        Items = new ItemHSList(null)
        {
            Holder = CreationHolder(),
            Sprite = this
        };
        Items.SetChangedHandler(ItemsChanged);
    }
}
