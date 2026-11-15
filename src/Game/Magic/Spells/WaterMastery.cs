#nullable disable
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
    public WaterMastery(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.WaterMasteryText);
        SpellId = 45;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Water;
        CastDuration = 40;
        AIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:819
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        // { Original ignores the event step. }
        base.ExecuteTokenEvent(eventValue, source, 0);
        source.GetLocation(out x, out y, out l);
        SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Water);
        StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
        CastSounds.Play(0);
        StrategicMap.Current.AddEffect(WorldEffectType.WaterMastery);
        CastingDone(source, BaseManaCost);
        return false;
    }

    // GlobalEnchantmentSpells.pas:801
    public override GlobalEnchantment CreateEnchantment() => new WaterMasteryEnchantment(null, holder: CreationHolder());
}
