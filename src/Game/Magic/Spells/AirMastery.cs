// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class AirMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:688
    public AirMastery(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.AirMasteryRStr);
        FSpellID = 60;
        FSpellType = SpellType.Global;
        FBaseManaCost = 1;
        FSpellLevel = 3;
        // { Original also assigns Earth to Air Mastery. }
        FMagicSphere = MagicSphere.Earth;
        FCastDuration = 40;
        FAIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:727
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Animation Anim = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Air);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Anim = new Animation(null);
        Anim.FAnimationLevel = 28;
        Anim.FOnShow = ShowAnimation;
        Anim.FAnimationMap = AoWHSMap.Current;
        Anim.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
        Anim.Activate();
        AoWHSSet.Current.FSpellControl.GetSpell(60).FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:747
    public override GlobalEnchantment CreateEnchantment() => new AirMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:701
    public void ShowAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Count;
        int X;
        int Y;
        int Speed;
        int Image;
        if (AoWHSMap.Current.ViewL == 0)
        {
            RandSeed = AoWHSMap.Current.Frame - Anim.FFrameCounter;
            switch ((int)GameTables.AoWEngine.FAnimationDetail)
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
                X += (Anim.FFrameCounter - 45) * Speed * 2;
                Y -= (Anim.FFrameCounter - 45) * Speed;
                FCastImages.Show(Image, SurfaceDesc.ClipRect.Left + X, SurfaceDesc.ClipRect.Top + Y, 0, SurfaceDesc);
            }
        }

        if (Anim.FFrameCounter == 90)
        {
            Anim.Dispose();
        }
    }
}
