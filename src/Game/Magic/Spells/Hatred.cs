// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class Hatred : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:889
    public Hatred(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.HatredRStr);
        FSpellID = 46;
        FSpellType = SpellType.Global;
        FBaseManaCost = 20;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Death;
    }

    // GlobalEnchantmentSpells.pas:900
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        AoWHSMap.Current.AddEffect(TworldFXType.Hatred);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:884
    public override GlobalEnchantment CreateEnchantment() => new HatredEnchantment(null);
}
