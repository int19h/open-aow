#nullable disable warnings
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class Harmony : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:969
    public Harmony(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.HarmonyText);
        SpellId = 47;
        SpellType = SpellType.Global;
        BaseManaCost = 15;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Life;
    }

    // GlobalEnchantmentSpells.pas:980
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        MapCoordinate coordinate71 = source.GetLocation();
        int x = coordinate71.X;
        int y = coordinate71.Y;
        int l = coordinate71.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, radius: 0);
        _ = StrategicMap.Current.AddEffect(WorldEffectType.Tranquility);
        _ = CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:964
    public override GlobalEnchantment CreateEnchantment() => new HarmonyEnchantment(null, holder: CreationHolder());
}
