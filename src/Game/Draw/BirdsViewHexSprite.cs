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
    public sbyte X;
    public sbyte Y;
    public sbyte Layer;
    public Int32List Hexes;
    public int Frame;

    // GlobalSpells.pas:412
    public override void Dispose()
    {
        Hexes?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:406
    public BirdsViewHexSprite(ECustomNode owner) : base(owner)
    {
        Hexes = new Int32List();
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
        if (AoWHSMap.Current.ViewL == Layer)
        {
            Frame += 1;
            intensity = 100 - Frame * 2;
            HexGeometry.HexToPixel(this.X, this.Y, ref pixelX, ref pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY -= pixelYSurface;
            for (int I = 0, last = Hexes.Count - 1; I <= last; I++)
            {
                HexGeometry.HexNumberToPixel(Hexes[I], ref hex, ref hy);
                hex += pixelX;
                hy += pixelY;
                AoWHSSet.Current.GeneralImages[5].ShowBlended(hex, hy, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[intensity]);
            }

            RandSeed = AoWHSMap.Current.InitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                radius = Random(448);
                direction = Random(360);
                delay = Random(30);
                if (Frame - delay >= 0)
                {
                    AbilityRules.PolarToCartesian(direction, radius, ref X, ref Y);
                    AoWHSSet.Current.SpellControl.GetSpell(57).CastImages.Show(0, pixelX + X, pixelY + Y, Frame - delay, lockedSurface);
                }
            }

            if (Frame == 50)
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
        this.X = X;
        this.Y = Y;
        Layer = layer;
        Frame = 0;
        AoWHSSet.Current.SpellControl.GetSpell(2).CastSounds.Play(0);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        level = AoWHSMap.Current.MapContainer.MapLevels[layer];
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                if (!((AoWMapField)level.GetField(walk.X, walk.Y)).GetExplored(AoWHSMap.Current.SeatedPlayer))
                {
                    Hexes.Add(walk.HexNumber);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        AoWHSMap.Current.DynamicFixedVisibleHexSprite.AddChild(this);
    }

    // GlobalSpells.pas:444
    public void StopAnimation()
    {
        AoWHSMap.Current.DynamicFixedVisibleHexSprite.RemoveChild(this);
        AoWHSMap.Current.DynamicVisibleHexSprite.RemoveChild(this);
        Dispose();
    }

    // GlobalSpells.pas:67

}
