// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class PowerLeak : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1036
    public PowerLeak(ECustomNode owner) : base(owner)
    {
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GameText.PowerLeakText);
        SpellId = 48;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Cosmos;
        CastDuration = 20;
        AIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:1049
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TwinkleAnimation animation;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        animation = new TwinkleAnimation(null);
        animation.Setup(AoWHSSet.Current.SpellControl.GetSpell(48).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Implode), 100, 64, 50, 30);
        AoWHSMap.Current.PlaceHex(animation, X, Y, L);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1031
    public override GlobalEnchantment CreateEnchantment() => new PowerLeakEnchantment(null);
}
