#nullable disable warnings

using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Game.Draw;

public sealed partial class DynamicIsometricHex
{
    private IsometricHex? _hex;

    private MapField? Field => _hex.Owner;

    // IsoHex.pas:109. The TEObject body runs with a null owner (the hexagon owns the sprite directly,
    // not as a child), then the sprite is attached and the field of the hexagon shows it.
    public DynamicIsometricHex(IsometricHex owner) : base(null)
    {
        _hex = owner;
        _ = Field.AddHexSprite(this, hexNumber: 0);
    }

    public override GameEngine? GetEngine() => _hex.GetEngine();

    public override byte Level => ((IsometricHexResource)_hex.Resource).DynamicLevel;
    public override sbyte HexX => Field.HexX;
    public override sbyte HexY => Field.HexY;
    public override sbyte HexLevel => Field.HexLevel;

    public override int GetShowPriority()
    {
        int x = (Field.HexX * 32) + 8;
        int y = ((Field.HexX & 1) * 16) + (Field.HexY * 32);
        return (Level << 26) + ((y + 32) << 13) + x + 8;
    }

    public override void Show(int x, int y, in LockedSurface surface) => _hex.ShowDynamic(x, y, lockedSurface: surface);

    public override void Dispose()
    {
        if (_hex is not null)
        {
            if (_hex.Visible)
            {
                _ = Field.Owner.Map.DynamicVisibleHexSprite.RemoveChild(this);
            }

            _ = Field.RemoveHexSprite(this);
            _hex.Dynamic = null;
            _hex = null;
        }
        SetOwnerWithoutNotify(null);
        base.Dispose();
    }
}
