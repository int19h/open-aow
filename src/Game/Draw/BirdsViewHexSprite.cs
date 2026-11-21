#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Draw;

// X/Y/layer unexplored hexagons, animation frame
public sealed class BirdsViewHexSprite(CustomNode owner) : FixedVisibleHexSprite(owner)
{

    private sbyte _x;
    private sbyte _y;
    private sbyte _layer;
    private readonly Int32List _hexes = new();
    private int _frame;

    // GlobalSpells.pas:412
    public override void Dispose()
    {
        _hexes?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:418
    public override int GetShowPriority() => 2_013_265_920;

    // GlobalSpells.pas:451
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (StrategicMap.Current.ViewMapLevel == _layer)
        {
            _frame += 1;
            int intensity = 100 - (_frame * 2);
            HexGeometry.HexToPixel(_x, _y, out int pixelX, out int pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY -= pixelYSurface;
            int hex = default;
            int hy = default;
            for (int i = 0, last = _hexes.Count - 1; i <= last; i++)
            {
                HexGeometry.HexNumberToPixel(_hexes[i], ref hex, ref hy);
                hex += pixelX;
                hy += pixelY;
                GamePack.Current.GeneralImages[5].ShowBlended(hex, hy, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[intensity]);
            }

            RandSeed = StrategicMap.Current.InitialRandomSeed;
            for (int i = 0; i <= 200; i++)
            {
                int radius = Random(448);
                int direction = Random(360);
                int delay = Random(30);
                if (_frame - delay >= 0)
                {
                    AbilityRules.PolarToCartesian(direction, radius, out int x, out int y);
                    _ = GamePack.Current.SpellControl[57].CastImages.Show(0, pixelX + x, pixelY + y, frame: _frame - delay, lockedSurface);
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
        _x = X;
        _y = Y;
        _layer = layer;
        _frame = 0;
        _ = GamePack.Current.SpellControl[2].CastSounds.Play(0);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[layer];
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                if (!level.GetField(walk.X, walk.Y).GetExplored(StrategicMap.Current.SeatedPlayer))
                {
                    _ = _hexes.Add(walk.HexNumber);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        _ = StrategicMap.Current.DynamicFixedVisibleHexSprite.AddChild(this);
    }

    // GlobalSpells.pas:444
    public void StopAnimation()
    {
        _ = StrategicMap.Current.DynamicFixedVisibleHexSprite.RemoveChild(this);
        _ = StrategicMap.Current.DynamicVisibleHexSprite.RemoveChild(this);
        Dispose();
    }
}
