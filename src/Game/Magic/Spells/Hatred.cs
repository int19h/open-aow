#nullable disable warnings
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class Hatred : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:889
    public Hatred(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.HatredText);
        SpellId = 46;
        SpellType = SpellType.Global;
        BaseManaCost = 20;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Death;
    }

    // GlobalEnchantmentSpells.pas:900
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        MapCoordinate coordinate72 = source.GetLocation();
        int x = coordinate72.X;
        int y = coordinate72.Y;
        int l = coordinate72.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, radius: 0);
        _ = StrategicMap.Current.AddEffect(WorldEffectType.Hatred);
        _ = CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:884
    public override GlobalEnchantment CreateEnchantment() => new HatredEnchantment(null, holder: CreationHolder());
}
