// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class SpellWard : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1949
    public SpellWard(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.SpellWardRStr);
        FSpellID = 65;
        FSpellType = SpellType.Global;
        FBaseManaCost = 1;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Earth;
    }

    // GlobalEnchantmentSpells.pas:1960
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        AoWHSMap.Current.AddEffect(TworldFXType.SpellWard);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1973
    public override GlobalEnchantment CreateEnchantment() => new SpellWardEnchantment(null);
}
