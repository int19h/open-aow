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
        SpellName = AbilityRules.TranslateResourceString(GameText.SpellWardText);
        SpellId = 65;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
    }

    // GlobalEnchantmentSpells.pas:1960
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        source.GetLocation(ref X, ref Y, ref L);
        StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        StrategicMap.Current.AddEffect(WorldEffectType.SpellWard);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1973
    public override GlobalEnchantment CreateEnchantment() => new SpellWardEnchantment(null);
}
