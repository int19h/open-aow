
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Game.Draw;

public partial class DynamicIsometricHex
{
    private IsometricHex hexagon;

    private MapField Field => (MapField)hexagon.Owner;

    // IsoHex.pas:109. The TEObject body runs with a null owner (the hexagon owns the sprite directly,
    // not as a child), then the sprite is attached and the field of the hexagon shows it.
    public DynamicIsometricHex(IsometricHex owner) : base(null)
    {
        hexagon = owner;
        Field.AddHS(this, 0);
    }

    public override GameEngine GetEngine() => hexagon.GetEngine();
    public override byte GetLevel() => ((IsometricHexResource)hexagon.FResource).FDynamicLevel;
    public override sbyte GetXhx() => Field.Xhx;
    public override sbyte GetYhx() => Field.Yhx;
    public override sbyte GetLhx() => Field.Lhx;

    public override int GetShowPriority()
    {
        int x = Field.Xhx * 32 + 8;
        int y = (Field.Xhx & 1) * 16 + Field.Yhx * 32;
        return (GetLevel() << 26) + ((y + 32) << 13) + x + 8;
    }

    public override void Show(int x, int y, in LockedSurface surface) => hexagon.ShowDynamic(x, y, surface);

    public override void Dispose()
    {
        if (hexagon != null)
        {
            if (hexagon.GetVisible()) ((MapLevel)Field.Owner).Map.DynamicVisibleHS.RemoveChild(this);
            Field.RemoveHS(this);
            hexagon.FDynamic = null;
            hexagon = null;
        }
        FOwner = null;
        base.Dispose();
    }
}
