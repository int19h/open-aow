#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class LiquidForm : UnitSpell
{
    // UnitSpells.pas:741
    public LiquidForm(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.LiquidFormText);
        SpellId = 49;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Water;
        AIValue = 300;
        TargetRange = 12;
        UnitDispelResistance = 5;
        SpellControlTypes = SpellControlTypes.Global | SpellControlTypes.Combat;
    }

    // UnitSpells.pas:783
    public override void PlayDefaultCastEffects(sbyte player, int X, int Y, sbyte layer)
    {
        base.PlayDefaultCastEffects(player, X, Y, layer);
        if (player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(X, Y, L: layer, radius: 0))
        {
            _ = GamePack.Current.SpellControl[SpellId].CastSounds.Play(1);
        }
    }

    // UnitSpells.pas:774
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        result = result && !target.GetAbilityEnabled(166);
        return result && ((!target.GetAbilityEnabled(13) && !target.GetAbilityEnabled(77)) || (!target.GetAbilityEnabled(2) && !target.GetAbilityEnabled(1) && !target.GetAbilityEnabled(59)));
    }

    // UnitSpells.pas:755
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(166))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(166).Enchantment;
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
