// Translated from GlobalSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// X/Y/layer unexplored hexagons, animation frame
public partial class BirdsViewHexSprite : FixedVisibleHexSprite
{
    private sbyte _x;
    private sbyte _y;
    private sbyte _layer;
    private readonly Int32List _hexes;
    private int _frame;

    // GlobalSpells.pas:412
    public override void Dispose()
    {
        _hexes?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:406
    public BirdsViewHexSprite(ECustomNode owner) : base(owner)
    {
        _hexes = new Int32List();
    }

    // GlobalSpells.pas:418
    public override int GetShowPriority() => 2013265920;

    // GlobalSpells.pas:451
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        int hex = default;
        int hy = default;
        int intensity;
        int radius;
        int direction;
        int X = default;
        int Y = default;
        int delay;
        if (StrategicMap.Current.ViewMapLevel == _layer)
        {
            _frame += 1;
            intensity = 100 - _frame * 2;
            HexGeometry.HexToPixel(this._x, this._y, out pixelX, out pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY -= pixelYSurface;
            for (int I = 0, last = _hexes.Count - 1; I <= last; I++)
            {
                HexGeometry.HexNumberToPixel(_hexes[I], ref hex, ref hy);
                hex += pixelX;
                hy += pixelY;
                GamePack.Current.GeneralImages[5].ShowBlended(hex, hy, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[intensity]);
            }

            RandSeed = StrategicMap.Current.InitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                radius = Random(448);
                direction = Random(360);
                delay = Random(30);
                if (_frame - delay >= 0)
                {
                    AbilityRules.PolarToCartesian(direction, radius, out X, out Y);
                    GamePack.Current.SpellControl.GetSpell(57).CastImages.Show(0, pixelX + X, pixelY + Y, _frame - delay, lockedSurface);
                }
            }

            if (_frame == 50)
            {
                StopAnimation();
            }
        }
    }

    // GlobalSpells.pas:421
    public void StartAnimation(sbyte X, sbyte Y, sbyte layer, sbyte radius)
    {
        HexNumberWalkData walk = default;
        MapLevel level;
        this._x = X;
        this._y = Y;
        _layer = layer;
        _frame = 0;
        GamePack.Current.SpellControl.GetSpell(2).CastSounds.Play(0);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        level = StrategicMap.Current.MapContainer.MapLevels[layer];
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                if (!level.GetField(walk.X, walk.Y).GetExplored(StrategicMap.Current.SeatedPlayer))
                {
                    _hexes.Add(walk.HexNumber);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        StrategicMap.Current.DynamicFixedVisibleHexSprite.AddChild(this);
    }

    // GlobalSpells.pas:444
    public void StopAnimation()
    {
        StrategicMap.Current.DynamicFixedVisibleHexSprite.RemoveChild(this);
        StrategicMap.Current.DynamicVisibleHexSprite.RemoveChild(this);
        Dispose();
    }

    // GlobalSpells.pas:67

}
