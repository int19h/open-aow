
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Game.Draw;

public partial class DynamicIsometricHex
{
    private IsometricHex _hex;

    private MapField Field => (MapField)_hex.Owner;

    // IsoHex.pas:109. The TEObject body runs with a null owner (the hexagon owns the sprite directly,
    // not as a child), then the sprite is attached and the field of the hexagon shows it.
    public DynamicIsometricHex(IsometricHex owner) : base(null)
    {
        _hex = owner;
        Field.AddHexSprite(this, 0);
    }

    public override GameEngine GetEngine() => _hex.GetEngine();
    public override byte GetLevel() => ((IsometricHexResource)_hex._resource).DynamicLevel;
    public override sbyte GetHexX() => Field.HexX;
    public override sbyte GetHexY() => Field.HexY;
    public override sbyte GetHexLevel() => Field.HexLevel;

    public override int GetShowPriority()
    {
        int x = Field.HexX * 32 + 8;
        int y = (Field.HexX & 1) * 16 + Field.HexY * 32;
        return (GetLevel() << 26) + ((y + 32) << 13) + x + 8;
    }

    public override void Show(int x, int y, in LockedSurface surface) => _hex.ShowDynamic(x, y, surface);

    public override void Dispose()
    {
        if (_hex != null)
        {
            if (_hex.GetVisible()) ((MapLevel)Field.Owner).Map.DynamicVisibleHexSprite.RemoveChild(this);
            Field.RemoveHexSprite(this);
            _hex.Dynamic = null;
            _hex = null;
        }
        SetOwnerWithoutNotify(null);
        base.Dispose();
    }
}
