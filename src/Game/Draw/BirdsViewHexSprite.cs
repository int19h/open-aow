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
    public sbyte FX;
    public sbyte FY;
    public sbyte FLayer;
    public Int32List FHexagons;
    public int FFrame;

    // GlobalSpells.pas:412
    public override void Dispose()
    {
        FHexagons?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:406
    public BirdsViewHexSprite(ECustomNode owner) : base(owner)
    {
        FHexagons = new Int32List();
    }

    // GlobalSpells.pas:418
    public override int GetShowPriority() => 2013265920;

    // GlobalSpells.pas:451
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
            Intensity = 100 - FFrame * 2;
            HexGeometry.HXtoHP(FX, FY, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp -= YhpSurf;
            for (int I = 0, last = FHexagons.Count - 1; I <= last; I++)
            {
                HexGeometry.HNtoHP(FHexagons[I], ref HX, ref HY);
                HX += Xhp;
                HY += Yhp;
                AoWHSSet.Current.FGeneralImages[5].ShowBlended(HX, HY, SurfaceDesc, GraphicsState.AlphaBlendTable.LookupTables[Intensity]);
            }

            RandSeed = AoWHSMap.Current.FInitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                Radius = Random(448);
                Direction = Random(360);
                Delay = Random(30);
                if (FFrame - Delay >= 0)
                {
                    AbilityRules.DegreesRadToCo(Direction, Radius, ref X, ref Y);
                    AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages.Show(0, Xhp + X, Yhp + Y, FFrame - Delay, SurfaceDesc);
                }
            }

            if (FFrame == 50)
            {
                StopAnimation();
            }
        }
    }

    // GlobalSpells.pas:421
    public void StartAnimation(sbyte X, sbyte Y, sbyte Layer, sbyte Radius)
    {
        HexNumberWalkData Walk = default;
        MapLevel Level;
        FX = X;
        FY = Y;
        FLayer = Layer;
        FFrame = 0;
        AoWHSSet.Current.FSpellControl.GetSpell(2).FCastSounds.Play(0);
        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
        Level = AoWHSMap.Current.MapContainer.MapLevels[Layer];
        while (Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                if (!((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetExplored(AoWHSMap.Current.FSeatedPlayer))
                {
                    FHexagons.Add(Walk.HN);
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        AoWHSMap.Current.DynamicFixedVisibleHS.AddChild(this);
    }

    // GlobalSpells.pas:444
    public void StopAnimation()
    {
        AoWHSMap.Current.DynamicFixedVisibleHS.RemoveChild(this);
        AoWHSMap.Current.DynamicVisibleHS.RemoveChild(this);
        Dispose();
    }

    // GlobalSpells.pas:67

}
