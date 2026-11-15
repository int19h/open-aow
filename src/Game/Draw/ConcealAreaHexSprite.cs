// Translated from GlobalSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// X/Y/layer explored hexagons, animation frame
public partial class ConcealAreaHexSprite : FixedVisibleHexSprite
{
    private sbyte _x;
    private sbyte _y;
    private sbyte _layer;
    private readonly Int32List _hexes;
    private int _frame;

    // GlobalSpells.pas:205
    public override void Dispose()
    {
        _hexes?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:199
    public ConcealAreaHexSprite(CustomNode owner) : base(owner)
    {
        _hexes = new Int32List();
    }

    // GlobalSpells.pas:211
    public override int GetShowPriority() => 2013265920;

    // GlobalSpells.pas:244
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        int hex = default;
        int hy = default;
        int intensity;
        int radius;
        int direction;
        int x = default;
        int y = default;
        int delay;
        if (StrategicMap.Current.ViewMapLevel == _layer)
        {
            _frame += 1;
            if (GetEngine().AnimationDetail == 2 && StrategicMap.Current.ViewMapLevel == _layer)
            {
                intensity = 60 - Math.Abs(_frame * 3 - 60);
                HexGeometry.HexToPixel(_x, _y, out pixelX, out pixelY);
                pixelX = pixelX - pixelXSurface - 8;
                pixelY -= pixelYSurface;
                for (int i = _hexes.Count - 1; i >= 0; i--)
                {
                    HexGeometry.HexNumberToPixel(_hexes[i], ref hex, ref hy);
                    hex += pixelX;
                    hy += pixelY;
                    GamePack.Current.GeneralImages[5].ShowBlended(hex, hy, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[intensity]);
                }
            }

            // { Original bug: Xhp/Yhp are uninitialized outside high detail. }
            RandSeed = StrategicMap.Current.InitialRandomSeed;
            for (int i = 0; i <= 200; i++)
            {
                radius = Random(448);
                direction = Random(360);
                delay = Random(20);
                if (_frame - delay >= 0)
                {
                    AbilityRules.PolarToCartesian(direction, radius, out x, out y);
                    GamePack.Current.SpellControl.GetSpell(57).CastImages.Show(0, pixelX + x, pixelY + y, _frame - delay, lockedSurface);
                }
            }

            if (_frame == 40)
            {
                StopAnimation();
            }
        }
    }

    // GlobalSpells.pas:214
    public void StartAnimation(sbyte X, sbyte Y, sbyte layer, sbyte radius)
    {
        HexNumberWalkData walk = default;
        MapLevel level;
        _x = X;
        _y = Y;
        _layer = layer;
        _frame = 0;
        GamePack.Current.SpellControl.GetSpell(1).CastSounds.Play(0);
        StrategicMap.Current.DynamicFixedVisibleHexSprite.AddChild(this);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, _x, _y, 0);
        level = StrategicMap.Current.MapContainer.MapLevels[_layer];
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                if (level.GetField(walk.X, walk.Y).GetExplored(StrategicMap.Current.SeatedPlayer))
                {
                    _hexes.Add(walk.HexNumber);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // GlobalSpells.pas:237
    public void StopAnimation()
    {
        StrategicMap.Current.DynamicFixedVisibleHexSprite.RemoveChild(this);
        StrategicMap.Current.DynamicVisibleHexSprite.RemoveChild(this);
        Dispose();
    }

    // GlobalSpells.pas:39

}
