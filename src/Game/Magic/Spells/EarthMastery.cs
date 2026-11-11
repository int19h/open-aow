// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Host;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class EarthMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1877
    public EarthMastery(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.EarthMasteryRStr);
        FSpellID = 64;
        FSpellType = SpellType.Global;
        FBaseManaCost = 1;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Earth;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1918
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Earth);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        AoWHSMap.Current.AddEffect(WorldEffectType.EarthMastery);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1932
    public override GlobalEnchantment CreateEnchantment() => new EarthMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:1890
    public void ShowAnimation(Animation Anim, int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int MaxX;
        int MaxY;
        int Xhp = default;
        int Yhp = default;
        MapLevel MapLevel;
        AoWMapField MapF;
        MapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(AoWHSMap.Current.ViewL);
        MaxX = AoWHSMap.Current.ViewXhx + AoWHSMap.Current.ViewWidthHX - 1;
        MaxY = AoWHSMap.Current.ViewYhx + AoWHSMap.Current.ViewHeightHX - 1;
        for (int X = AoWHSMap.Current.ViewXhx; X <= MaxX; X++)
        {
            HexGeometry.HXtoHP(X, AoWHSMap.Current.ViewYhx, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf + 16;
            Yhp = Yhp - YhpSurf + 16;
            MapF = (AoWMapField)MapLevel.GetField(X, AoWHSMap.Current.ViewYhx);
            for (int Y = AoWHSMap.Current.ViewYhx; Y <= MaxY; Y++)
            {
                // The original frame offset uses the field address.
                if (MapF.OverlayType == 4)
                {
                    FCastImages.Show(0, Xhp, Yhp, Anim.FFrameCounter - FrameFixtureInputs.FieldFrameOffset(MapF), SurfaceDesc);
                }

                // The original steps the field pointer by FHNOffsets[0, 4], one row down. The step
                // after the last row is never read there, and the guard keeps it from reading here.
                if (Y < MaxY)
                {
                    MapF = (AoWMapField)MapLevel.GetField(X, Y + 1);
                }

                Yhp += 32;
            }
        }

        if (Anim.FFrameCounter == 50)
        {
            Anim.Dispose();
        }
    }
}
