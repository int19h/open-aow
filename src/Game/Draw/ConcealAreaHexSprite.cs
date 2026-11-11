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
    public sbyte FX;
    public sbyte FY;
    public sbyte FLayer;
    public Int32List FHexagons;
    public int FFrame;

    // GlobalSpells.pas:205
    public override void Dispose()
    {
        FHexagons?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:199
    public ConcealAreaHexSprite(ECustomNode owner) : base(owner)
    {
        FHexagons = new Int32List();
    }

    // GlobalSpells.pas:211
    public override int GetShowPriority() => 2013265920;

    // GlobalSpells.pas:244
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        int HX = default;
        int HY = default;
        int Intensity;
        int Radius;
        int Direction;
        int X = default;
        int Y = default;
        int Delay;
        if (AoWHSMap.Current.ViewL == FLayer)
        {
            FFrame += 1;
            if (GameTables.AoWEngine.FAnimationDetail == 2 && AoWHSMap.Current.ViewL == FLayer)
            {
                Intensity = 60 - Math.Abs(FFrame * 3 - 60);
                HexGeometry.HXtoHP(FX, FY, ref Xhp, ref Yhp);
                Xhp = Xhp - XhpSurf - 8;
                Yhp -= YhpSurf;
                for (int I = FHexagons.Count - 1; I >= 0; I--)
                {
                    HexGeometry.HNtoHP(FHexagons[I], ref HX, ref HY);
                    HX += Xhp;
                    HY += Yhp;
                    AoWHSSet.Current.FGeneralImages[5].ShowBlended(HX, HY, SurfaceDesc, GraphicsState.AlphaBlendTable.LookupTables[Intensity]);
                }
            }

            // { Original bug: Xhp/Yhp are uninitialized outside high detail. }
            RandSeed = AoWHSMap.Current.FInitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                Radius = Random(448);
                Direction = Random(360);
                Delay = Random(20);
                if (FFrame - Delay >= 0)
                {
                    AbilityRules.DegreesRadToCo(Direction, Radius, ref X, ref Y);
                    AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages.Show(0, Xhp + X, Yhp + Y, FFrame - Delay, SurfaceDesc);
                }
            }

            if (FFrame == 40)
            {
                StopAnimation();
            }
        }
    }

    // GlobalSpells.pas:214
    public void StartAnimation(sbyte X, sbyte Y, sbyte Layer, sbyte Radius)
    {
        HexNumberWalkData Walk = default;
        MapLevel Level;
        FX = X;
        FY = Y;
        FLayer = Layer;
        FFrame = 0;
        AoWHSSet.Current.FSpellControl.GetSpell(1).FCastSounds.Play(0);
        AoWHSMap.Current.DynamicFixedVisibleHS.AddChild(this);
        HexNumberWalkData.InitHNWalk(ref Walk, FX, FY, 0);
        Level = AoWHSMap.Current.MapContainer.MapLevels[FLayer];
        while (Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                if (((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetExplored(AoWHSMap.Current.FSeatedPlayer))
                {
                    FHexagons.Add(Walk.HN);
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // GlobalSpells.pas:237
    public void StopAnimation()
    {
        AoWHSMap.Current.DynamicFixedVisibleHS.RemoveChild(this);
        AoWHSMap.Current.DynamicVisibleHS.RemoveChild(this);
        Dispose();
    }

    // GlobalSpells.pas:39

}
