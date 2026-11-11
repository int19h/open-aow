// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class FireMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1733
    public FireMastery(ECustomNode owner) : base(owner)
    {
        FSpellType = SpellType.Global;
        FSpellName = AbilityRules.TranslateRStr(GameText.FireMasteryRStr);
        FSpellID = 63;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Fire;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1792
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        bool Result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        if (ExecuteCount == 0)
        {
            Source.GetLocation(ref X, ref Y, ref L);
            SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Fire);
            AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            var animation = new Animation(null);
            animation.FAnimationLevel = 27;
            animation.FOnShow = ShowAnimation;
            animation.FAnimationMap = AoWHSMap.Current;
            animation.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
            animation.Activate();

            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
        }

        if (ExecuteCount == 20)
        {
            base.ExecuteTE(Event, Source, 0);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1746
    public override GlobalEnchantment CreateEnchantment() => new FireMasteryEnchantment(null);

    public void ShowAnimation(Animation anim, int xhpSurface, int yhpSurface, in LockedSurface surface)
    {
        int chance = GameTables.AoWEngine.FAnimationDetail switch { 0 => 2, 2 => 5, _ => 3 };
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
                if (field.TerrainType is 7 or 8 or 15 || map.FSeatedPlayer == -1 || map.FExplorationEnabled && !field.GetExplored(map.FSeatedPlayer)) continue;
                RandSeed = field.Xhx + field.Yhx * 8 + ((field.FFieldFlags >> 16) & 255) * 65536;
                if (Random(100) >= chance) continue;
                int delay = Random(40);
                FCastImages.Show(0, xhp, yhp, anim.FFrameCounter - delay, surface);
                if (anim.FFrameCounter == delay && GameTables.AoWEngine.FAnimationDetail != 0)
                    AudioPlayback.PlaySample(AbilityRules.ExplosionSFX[x % 2]);
            }
        }
        if (anim.FFrameCounter == 90) anim.Dispose();
    }
}
