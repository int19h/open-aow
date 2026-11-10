using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Units;

namespace Aow.Game.Draw
{

    public partial class ItemHS
    {
        public ItemHS(ECustomNode owner) : base(owner)
        {
            FItems = new ItemHSList(null);
            FItems.Sprite = this;
            FItems.FOnChanged = ItemsChanged;
        }
    }
}

namespace Aow.Game.Units
{

    public partial class ItemHSList
    {
        internal ItemHS Sprite;

        public override GameEngine GetEngine() => Sprite != null ? Sprite.GetEngine() : base.GetEngine();

        public override void GetLocation(ref sbyte x, ref sbyte y, ref sbyte level)
        {
            x = Sprite.GetXhx();
            y = Sprite.GetYhx();
            level = Sprite.GetLhx();
        }
    }
}
