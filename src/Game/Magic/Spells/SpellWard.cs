#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class SpellWard : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1949
    public SpellWard(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SpellWardText);
        SpellId = 65;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
    }

    // GlobalEnchantmentSpells.pas:1960
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        MapCoordinate coordinate79 = source.GetLocation();
        sbyte x = coordinate79.X;
        sbyte y = coordinate79.Y;
        sbyte l = coordinate79.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, radius: 0);
        _ = StrategicMap.Current.AddEffect(WorldEffectType.SpellWard);
        _ = CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1973
    public override GlobalEnchantment CreateEnchantment() => new SpellWardEnchantment(null, holder: CreationHolder());
}
