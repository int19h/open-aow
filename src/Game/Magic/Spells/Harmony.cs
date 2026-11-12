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
        SpellName = AbilityRules.TranslateResourceString(GameText.HarmonyText);
        SpellId = 47;
        SpellType = SpellType.Global;
        BaseManaCost = 15;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Life;
    }

    // GlobalEnchantmentSpells.pas:980
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        source.GetLocation(ref X, ref Y, ref L);
        StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        StrategicMap.Current.AddEffect(WorldEffectType.Tranquility);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:964
    public override GlobalEnchantment CreateEnchantment() => new HarmonyEnchantment(null);
}
