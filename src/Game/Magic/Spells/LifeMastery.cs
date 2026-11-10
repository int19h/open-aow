// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class LifeMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1441
    public LifeMastery(ECustomNode owner) : base(owner)
    {
        FSpellType = SpellType.Global;
        FSpellName = AbilityRules.TranslateRStr(GameText.LifeMasteryRStr);
        FSpellID = 61;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        // { Original assigns Cosmos to Life Mastery. }
        FMagicSphere = MagicSphere.Cosmos;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1457
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TwinkleAnimation Twinkle;
        base.ExecuteTE(Event, Source, ExecuteCount);
        SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Life);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Twinkle = new TwinkleAnimation(null);
        Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(61).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 100, 64, 50, 30);
        AoWHSMap.Current.PlaceHX(Twinkle, X, Y, L);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1454
    public override GlobalEnchantment CreateEnchantment() => new LifeMasteryEnchantment(null);
}
