// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class Hatred : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:889
    public Hatred(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.HatredText);
        SpellId = 46;
        SpellType = SpellType.Global;
        BaseManaCost = 20;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Death;
    }

    // GlobalEnchantmentSpells.pas:900
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        source.GetLocation(out X, out Y, out L);
        StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        StrategicMap.Current.AddEffect(WorldEffectType.Hatred);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:884
    public override GlobalEnchantment CreateEnchantment() => new HatredEnchantment(null, holder: CreationHolder());
}
