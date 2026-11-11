// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class WaterMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:806
    public WaterMastery(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.WaterMasteryRStr);
        FSpellID = 45;
        FSpellType = SpellType.Global;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Water;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:819
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        // { Original ignores the event step. }
        base.ExecuteTE(Event, Source, 0);
        Source.GetLocation(ref X, ref Y, ref L);
        SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Water);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        FCastSounds.Play(0);
        AoWHSMap.Current.AddEffect(WorldEffectType.WaterMastery);
        CastingDone(Source, FBaseManaCost);
        return false;
    }

    // GlobalEnchantmentSpells.pas:801
    public override GlobalEnchantment CreateEnchantment() => new WaterMasteryEnchantment(null);
}
