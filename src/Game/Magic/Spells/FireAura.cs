#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class FireAura : UnitSpell
{
    // UnitSpells.pas:616
    public FireAura(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FireAuraText);
        SpellId = 40;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Fire;
        TargetRange = 12;
        UnitDispelResistance = 4;
        SpellControlTypes = SpellControlTypes.Global | SpellControlTypes.Combat;
    }

    // UnitSpells.pas:629
    public override void PlayDefaultCastEffects(PlayerId? player, int X, int Y, int layer)
    {
        base.PlayDefaultCastEffects(player, X, Y, layer);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (player == strategicMap.SeatedPlayer || strategicMap.Watching(X, Y, L: layer, radius: 0))
        {
            _ = GetEngine().GamePack.SpellControl[SpellId].CastSounds.Play(1);
        }
    }

    // UnitSpells.pas:655
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && (!target.GetAbilityEnabled(7) || !target.GetAbilityEnabled(43)) && !target.GetAbilityEnabled(163);
    }

    // UnitSpells.pas:636
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(163))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(163).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = UnitDispelResistance.TruncateToSByte();
            }

            // { Original succeeds even when expansion fails. }
            return true;
        }
        else
        {
            return false;
        }
    }
}
