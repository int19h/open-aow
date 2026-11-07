using Aow.Units.Engine;

namespace Aow.Units.AoWE;

public partial class TItemHS
{
    public TItemHS(TECustomNode owner) : base(owner)
    {
        FItems = new TItemHSList(null);
        FItems.Sprite = this;
        FItems.FOnChanged = ItemsChanged;
    }
}

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
