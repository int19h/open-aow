#nullable disable warnings
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class CosmagicSurgery : UnitSpell
{
    // UnitSpells.pas:901
    public CosmagicSurgery(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_deformFaceText);
        SpellId = 59;
        SpellType = SpellType.Unit;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Secret;
        AllowMultipleTargets = false;
        UnitDispelResistance = 10;
        TargetRange = -1;
        SpellControlTypes = SpellControlTypes.Global;
    }

    // UnitSpells.pas:968
    public override void GetCombatInfo(Troop? source, ref SpellCombatInfo info)
    {
        // { The original saves Self and Source as well as Info; this compiler saves only Info. }
        base.GetCombatInfo(source, ref info);
        info.TargetTypes = CombatTargetTypes.Enemy;
    }

    // UnitSpells.pas:940
    public override bool ValidTargetUnit(Troop? source, Troop? target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && target.GetUnitType() is UnitType.Humanoid or UnitType.Creature;
    }

    // UnitSpells.pas:946
    public override bool CastSpell(Troop source, UnitUserList targets)
    {
        if (targets.GetCount() == 1)
        {
            CosmagicSurgerySpellCaster caster = new(null)
            {
                Holder = CreationHolder(),
                CasterSourceId = source.UnitId,
                CasterSpellId = SpellId,
                TargetUnitId = targets.GetUnit(0).UnitId,
                FaceIndex = targets.GetUnit(0) is Hero faceTarget ? faceTarget.FaceIndex : 0
            };

            StrategicMap.Current.CosmagicSurgeryEventsStore.Raise(caster);
            _ = caster.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // UnitSpells.pas:915
    public override bool ExecuteSpell(Troop? source, Troop? target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            bool expanded = target.GetAbilityEnabled(126) || target.ExpandAbility(126);

            if (expanded)
            {
                // Ability 126 stores the enchantment its ability builds. Another enchantment fails.
                // Null stays null and the upkeep write fails on it.
                CosmagicSurgeryEnchantment? enchantment = (CosmagicSurgeryEnchantment?)target.GetAbilityData<UnitEnchantmentAbilityData>(126).Enchantment;
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
