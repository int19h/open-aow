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
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GameText.s_lifeMasteryText);
        SpellId = 61;
        BaseManaCost = 1;
        SpellLevel = 4;
        // { Original assigns Cosmos to Life Mastery. }
        MagicSphere = MagicSphere.Cosmos;
        CastDuration = 40;
        AIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1457
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TwinkleAnimation twinkle;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Life);
        source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        twinkle = new TwinkleAnimation(null);
        twinkle.Setup(AoWHSSet.Current.SpellControl.GetSpell(61).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 100, 64, 50, 30);
        AoWHSMap.Current.PlaceHex(twinkle, X, Y, L);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1454
    public override GlobalEnchantment CreateEnchantment() => new LifeMasteryEnchantment(null);
}
