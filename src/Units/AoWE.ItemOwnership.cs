using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Units;

namespace Aow.Game.Draw
{

    public partial class TItemHS
    {
        public TItemHS(TECustomNode owner) : base(owner)
        {
            FItems = new TItemHSList(null);
            FItems.Sprite = this;
            FItems.FOnChanged = ItemsChanged;
        }
    }
}

namespace Aow.Game.Units
{

    public partial class TItemHSList
    {
        internal TItemHS Sprite;

        public override TEngine GetEngine() => Sprite != null ? Sprite.GetEngine() : base.GetEngine();

        public override void GetLocation(ref sbyte x, ref sbyte y, ref sbyte level)
        {
            x = Sprite.GetXhx();
            y = Sprite.GetYhx();
            level = Sprite.GetLhx();
        }
    }
}
