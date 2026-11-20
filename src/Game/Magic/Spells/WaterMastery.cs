#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class WaterMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:806
    public WaterMastery(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WaterMasteryText);
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
        // { Original ignores the event step. }
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount: 0);
        MapCoordinate coordinate86 = source.GetLocation();
        sbyte x = coordinate86.X;
        sbyte y = coordinate86.Y;
        sbyte l = coordinate86.MapLevel;
        SpellTargets.NotifySpellSphere(source.Player, sphere: MagicSphere.Water);
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, radius: 0);
        _ = CastSounds.Play(0);
        _ = StrategicMap.Current.AddEffect(WorldEffectType.WaterMastery);
        CastingDone(source, mana: BaseManaCost);
        return false;
    }

    // GlobalEnchantmentSpells.pas:801
    public override GlobalEnchantment CreateEnchantment() => new WaterMasteryEnchantment(null, holder: CreationHolder());
}
