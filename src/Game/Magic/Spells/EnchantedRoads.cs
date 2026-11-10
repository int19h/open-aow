// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class EnchantedRoads : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:571
    public EnchantedRoads(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.EnchantedRoadsRStr);
        FSpellID = 44;
        FSpellType = SpellType.Global;
        FBaseManaCost = 1;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Earth;
        FCastDuration = 20;
        FAIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:613
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Animation Anim = default;
        TwinkleAnimation Twinkle = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Anim = new Animation(null);
        Anim.FAnimationLevel = 27;
        Anim.FOnShow = ShowAnimation;
        Anim.FAnimationMap = AoWHSMap.Current;
        Anim.Activate();
        Twinkle = new TwinkleAnimation(null);
        Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(44).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
        AoWHSMap.Current.PlaceHX(Twinkle, X, Y, L);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:636
    public override GlobalEnchantment CreateEnchantment() => new EnchantedRoadsEnchantment(null);

    public void ShowAnimation(Animation anim, int xhpSurface, int yhpSurface, in SurfaceDesc surface)
    {
        var map = AoWHSMap.Current;
        var level = map.MapContainer.GetMapLevel(map.ViewL);
        int maxX = map.ViewXhx + map.ViewWidthHX - 1, maxY = map.ViewYhx + map.ViewHeightHX - 1;
        for (int x = map.ViewXhx; x <= maxX; x++)
        {
            int xhp = 0, yhp = 0;
            HexGeometry.HXtoHP(x, map.ViewYhx, ref xhp, ref yhp);
            xhp = xhp - xhpSurface + 16;
            yhp = yhp - yhpSurface + 16;
            for (int y = map.ViewYhx; y <= maxY; y++, yhp += 32)
            {
                var field = (AoWMapField)level.GetField(x, y);
                if (field.OverlayType == 4 && map.FSeatedPlayer != -1 && field.GetExplored(map.FSeatedPlayer))
                    FCastImages.Show(0, xhp, yhp, anim.FFrameCounter - ((field.Xhx + field.Yhx * 8 + ((field.FFieldFlags >> 16) & 255)) & 15), surface);
            }
        }
        if (anim.FFrameCounter == 50) anim.Dispose();
    }
}
