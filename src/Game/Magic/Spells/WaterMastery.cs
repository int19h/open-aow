#nullable disable warnings
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
    public WaterMastery(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WaterMasteryText);
        SpellId = 45;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Water;
        CastDuration = 40;
        AIBasePriority = 1_000;
    }

    // GlobalEnchantmentSpells.pas:819
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        // { Original ignores the event step. }
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount: 0);
        MapCoordinate coordinate86 = source.GetLocation();
        int x = coordinate86.X;
        int y = coordinate86.Y;
        int l = coordinate86.MapLevel;
        SpellTargets.NotifySpellSphere(source.Player, sphere: MagicSphere.Water, engine: GetEngine()!);
        _ = GetEngine().StrategicMap.ValidateObservation(source.Player, x, y, l, radius: 0);
        _ = CastSounds.Play(0);
        _ = GetEngine().StrategicMap.AddEffect(WorldEffectType.WaterMastery);
        CastingDone(source, mana: BaseManaCost);
        return false;
    }

    // GlobalEnchantmentSpells.pas:801
    public override GlobalEnchantment CreateEnchantment() => new WaterMasteryEnchantment(null, holder: CreationHolder());
}
