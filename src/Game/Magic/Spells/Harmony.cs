// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class Harmony : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:969
    public Harmony(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.HarmonyRStr);
        FSpellID = 47;
        FSpellType = SpellType.Global;
        FBaseManaCost = 15;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Life;
    }

    // GlobalEnchantmentSpells.pas:980
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        AoWHSMap.Current.AddEffect(TworldFXType.Tranquility);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:964
    public override GlobalEnchantment CreateEnchantment() => new HarmonyEnchantment(null);
}
