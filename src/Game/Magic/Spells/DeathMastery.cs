// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class DeathMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1577
    public DeathMastery(ECustomNode owner) : base(owner)
    {
        FSpellType = SpellType.Global;
        FSpellName = AbilityRules.TranslateRStr(GameText.DeathMasteryRStr);
        FSpellID = 62;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        // { Original assigns Cosmos to Death Mastery. }
        FMagicSphere = MagicSphere.Cosmos;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1618
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Death);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        var animation = new Animation(null);
        animation.FAnimationLevel = 28;
        animation.FOnShow = ShowAnimation;
        animation.FAnimationMap = AoWHSMap.Current;
        animation.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
        animation.Activate();

        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1590
    public override GlobalEnchantment CreateEnchantment() => new DeathMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:1593
    public void ShowAnimation(Animation Anim, int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Count;
        int X;
        int Y;
        int Speed;
        int D;
        int Image;
        RandSeed = AoWHSMap.Current.Frame - Anim.FFrameCounter;
        switch (GameTables.AoWEngine.FAnimationDetail)
        {
            case 0:
                Count = 5;
                break;
            case 2:
                Count = 20;
                break;
            default:
                Count = 10;
                break;
        }

        for (int I = 0; I <= Count - 1; I++)
        {
            X = Random(Anim.FTag & 65535);
            Y = Random((int)(((uint)(int)(uint)Anim.FTag) >> 16));
            Speed = Random(15) + 5;
            Image = Random(3);
            // { Preserve the original image/displacement register lifetimes. }
            D = (Anim.FFrameCounter - 45) * Speed;
            X -= D * 2;
            Y += D;
            FCastImages.Show(Image, SurfaceDesc.ClipRect.Left + X, SurfaceDesc.ClipRect.Top + Y, 0, SurfaceDesc);
        }

        if (Anim.FFrameCounter == 90)
        {
            Anim.Dispose();
        }
    }
}
