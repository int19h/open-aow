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
    public SpellWard(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.SpellWardText);
        SpellId = 65;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
    }

    // GlobalEnchantmentSpells.pas:1960
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        source.GetLocation(out x, out y, out l);
        StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
        StrategicMap.Current.AddEffect(WorldEffectType.SpellWard);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1973
    public override GlobalEnchantment CreateEnchantment() => new SpellWardEnchantment(null, holder: CreationHolder());
}
