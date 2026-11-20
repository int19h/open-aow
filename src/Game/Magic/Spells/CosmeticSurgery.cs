#nullable disable
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class CosmeticSurgery : UnitSpell
{
    // UnitSpells.pas:901
    public CosmeticSurgery(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_deformFaceText);
        SpellId = 59;
        SpellType = SpellType.Unit;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Secret;
        AllowMultipleTargets = false;
        UnitDispelResistance = 10;
        TargetRange = -1;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
    }

    // UnitSpells.pas:968
    public override void GetCombatInfo(Troop source, ref SpellCombatInfo info)
    {
        // { The original saves Self and Source as well as Info; this compiler saves only Info. }
        base.GetCombatInfo(source, ref info);
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // UnitSpells.pas:940
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(target.GetUnitType());
    }

    // UnitSpells.pas:946
    public override bool CastSpell(Troop source, UnitUserList targets)
    {
        if (targets.GetCount() == 1)
        {
            CosmeticSurgerySpellCaster caster = new(null)
            {
                Holder = CreationHolder(),
                CasterSourceId = source.UnitId,
                CasterSpellId = SpellId,
                TargetUnitId = targets.GetUnit(0).UnitId,
                FaceIndex = targets.GetUnit(0) is Hero faceTarget ? faceTarget.FaceIndex : 0
            };

            StrategicMap.Current.CosmeticSurgeryEventsStore.Raise(caster);
            _ = caster.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // UnitSpells.pas:915
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            bool expanded = target.GetAbilityEnabled(126) || target.ExpandAbility(126);

            if (expanded)
            {
                // Ability 126 stores the enchantment its ability builds. Another enchantment fails.
                // Null stays null and the upkeep write fails on it.
                CosmeticSurgeryEnchantment enchantment = (CosmeticSurgeryEnchantment)target.GetAbilityData<UnitEnchantmentAbilityData>(126).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.FaceIndex = data[0];
                enchantment.SetSource(source.Player);
            }

            // { Original succeeds even when expansion fails. }
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_deformFaceText = "Cosmagic Surgery";
}
