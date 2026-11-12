// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class CosmeticSurgery : UnitSpell
{
    // UnitSpells.pas:901
    public CosmeticSurgery(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_deformFaceText);
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
    public override bool CastSpell(Troop source, UnitList targets)
    {
        bool result;
        CosmeticSurgerySpellCaster caster;
        if (targets.GetCount() == 1)
        {
            caster = new CosmeticSurgerySpellCaster(null);
            caster.CasterSourceId = source.UnitId;
            caster.CasterSpellId = SpellId;
            caster.TargetUnitId = targets.GetUnit(0).UnitId;
            if (targets.GetUnit(0) is Hero)
            {
                caster.FaceIndex = ((Hero)targets.GetUnit(0)).FaceIndex;
            }
            else
            {
                caster.FaceIndex = 0;
            }

            AoWHSMap.Current.CosmeticSurgeryEventsStore.Raise(caster);
            caster.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // UnitSpells.pas:915
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        CosmeticSurgeryEnchantment enchantment;
        bool expanded;
        if (ValidTargetUnit(source, target))
        {
            if (!target.GetAbilityEnabled(126))
            {
                expanded = target.ExpandAbility(126);
            }
            else
            {
                expanded = true;
            }

            if (expanded)
            {
                enchantment = (CosmeticSurgeryEnchantment)((UnitEnchantmentAbilityData)target.GetAbilityData(126)).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.FaceIndex = data[0];
                enchantment.SetSource(source.Player);
            }

            // { Original succeeds even when expansion fails. }
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_deformFaceText = "Cosmagic Surgery";
}
