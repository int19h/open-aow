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
        FSpellType = SpellType.Global;
        FSpellName = AbilityRules.TranslateRStr(GameText.PowerLeakRStr);
        FSpellID = 48;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Cosmos;
        FCastDuration = 20;
        FAIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:1049
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TwinkleAnimation Anim;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Anim = new TwinkleAnimation(null);
        Anim.Setup(AoWHSSet.Current.FSpellControl.GetSpell(48).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Implode), 100, 64, 50, 30);
        AoWHSMap.Current.PlaceHX(Anim, X, Y, L);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1031
    public override GlobalEnchantment CreateEnchantment() => new PowerLeakEnchantment(null);
}
