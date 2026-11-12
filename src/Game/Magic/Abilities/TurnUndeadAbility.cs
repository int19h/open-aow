// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class TurnUndeadAbility : TouchAbility
{
    // AoWAb.PAS:3121
    public TurnUndeadAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 38;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.UseItem, AbilitySelectionType.Editor);
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText);
        AbilityType = AbilityType.Combat;
    }

    // AoWAb.PAS:3191
    public override string GetName(AbilityOwner owner) => GetLevelName(GetLevel(owner));

    // AoWAb.PAS:3146
    public override int GetLevel(AbilityOwner owner)
    {
        int result;
        MultiLevelAbilityData data;
        if (owner != null)
        {
            data = (MultiLevelAbilityData)owner.GetAbilityData(AbilityId);
            if (data != null)
            {
                result = data.Level;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:3213
    public override int GetSkillPoints(AbilityOwner owner)
    {
        int result = GetLevel(owner);
        return result * 5;
    }

    // AoWAb.PAS:3374
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        TurnUndeadCombatAction action;
        StrikeCombatAction strike;
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        action = CreateTurnUndeadCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        if (target.GetEnabled())
        {
            strike = CombatObject.CreateStrikeCombatAction(target, source, true);
            if (strike != null)
            {
                source.Combat.ExecuteCombatAction(strike);
                strike.Release();
            }
        }

        action.Release();
        return true;
    }

    // AoWAb.PAS:3204
    public override bool CanExpand(AbilityOwner owner)
    {
        if ((owner.GetAbilitySelectionTypes() & SelectionTypes) == Set<AbilitySelectionType>.Of() || GetLevel(owner) >= 4)
        {
            return false;
        }

        return true;
    }

    // AoWAb.PAS:3199
    public override int ExpandCost(AbilityOwner owner)
    {
        int result = GetLevel(owner);
        if (result < 4)
        {
            result = 5;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:3195
    public override string ExpandName(AbilityOwner owner) => GetLevelName(GetLevel(owner) + 1);

    // AoWAb.PAS:3217
    public override bool Expand(AbilityOwner owner)
    {
        MultiLevelAbilityData data;
        bool result = false;
        data = (MultiLevelAbilityData)owner.GetAbilityData(AbilityId);
        if (data == null)
        {
            data = new MultiLevelAbilityData(null) { Holder = CreationHolder() };
            data.LevelAbilityId = (ushort)AbilityId;
            data.Level = 1;
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            result = true;
        }
        else
        {
            if (data.Level != 4)
            {
                result = true;
                data.Level = (byte)(data.Level + 1);
            }
        }

        return result;
    }

    // AoWAb.PAS:3238
    public override bool Remove(AbilityOwner owner)
    {
        MultiLevelAbilityData data;
        bool result = true;
        data = (MultiLevelAbilityData)owner.GetAbilityData(AbilityId);
        if (data != null)
        {
            if (data.Level == 1)
            {
                SetAbility(owner, AbilityId, 0);
                owner.RemoveAbilityData(data);
            }
            else
            {
                data.Level = (byte)(data.Level - 1);
            }
        }
        else
        {
            SetAbility(owner, AbilityId, 0);
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:3293
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        float probability;
        if (CanTouchUnit(attacker, defender) && !defender.GetImmunityTypes().Contains(DamageType.None))
        {
            probability = TouchRoleProbability(attacker, defender);
            result = Round((double)((double)defender.GetHitPoints() * probability) * 100);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:3305
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        int hitPoints;
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, defender) && !defender.GetImmunityTypes().Contains(DamageType.None))
        {
            probability = TouchRoleProbability(attacker, defender);
            hitPoints = defender.GetHitPoints();
            damageValues.SourceCost = hitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            var unitStrikeDamageValue = Troop.UnitStrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round((double)unitStrikeDamageValue * ((double)1 - probability));
            var protectionTypes = attacker.GetProtectionTypes();
            var damageTypes = ((StrikeAbility)GamePack.Current.AbilityControl.GetAbility(21)).GetDamageTypes(defender);
            var immunityTypes = attacker.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(defender.GetDamage(), damageTypes, immunityTypes, protectionTypes);
            damageValues.TargetCost = statisticsToMaxDamage;
            damageValues.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
        }
        else
        {
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:3168
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        int damage;
        if (owner is Troop troop)
        {
            damage = GetTurnUndeadDamage(GetLevel(troop));
            if (damage > 5)
            {
                damage = 5;
            }

            strength = Round((double)(troop.GetAttack() * 4 * damage) * 0.1);
            // { Original reports death damage here, although the attack deals holy damage. }
            types = Set<DamageType>.Of(DamageType.Death);
        }
    }

    // AoWAb.PAS:3331
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        int hitPoints;
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            probability = CombatTouchRoleProbability((CombatUnit)attacker, defender);
            hitPoints = defender.GetHitPoints();
            damageValues.SourceCost = hitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            var strikeDamageValue = CombatObject.StrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round((double)strikeDamageValue * ((double)1 - probability));
            var protectionTypes = attacker.GetProtectionTypes();
            var immunityTypes = attacker.GetImmunityTypes();
            var meleeDamageTypes = CombatObject.MeleeDamageTypes(defender);
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(defender.GetDamage(), meleeDamageTypes, immunityTypes, protectionTypes);
            damageValues.TargetCost = statisticsToMaxDamage;
            damageValues.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
        }
        else
        {
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:3282
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        int level;
        level = GetLevel(owner);
        var touchAttack = GetTouchAttack(level);
        info.Attack = touchAttack;
        var turnUndeadDamage = GetTurnUndeadDamage(level);
        info.Damage = turnUndeadDamage;
        info.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
        info.RepeatCount = 1;
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:3260
    public override sbyte GetTouchAttack(int level)
    {
        sbyte result;
        switch (level)
        {
            case 1:
                result = 4;
                break;
            case 2:
                result = 5;
                break;
            case 3:
                result = 6;
                break;
            case 4:
                result = 7;
                break;
            default:
                result = 4;
                break;
        }

        return result;
    }

    // AoWAb.PAS:3137
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = base.CanTouch(source, target);
        if (result)
        {
            bool condition = source is CombatUnit && target is CombatUnit;
            if (condition)
            {
                var unitType = ((CombatUnit)target).Troop.GetUnitType();
                condition = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType);
            }

            result = condition && ((ushort)((CombatUnit)target).Troop.GetRace() == 11 || ((CombatUnit)target).Troop.GetAbilityEnabled(129));
        }

        return result;
    }

    // AoWAb.PAS:3130
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result = base.CanTouchUnit(source, target);
        if (result)
        {
            var unitType = target.GetUnitType();
            result = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && ((ushort)target.GetRace() == 11 || target.GetAbilityEnabled(129));
        }

        return result;
    }

    // AoWAb.PAS:3181
    public string GetLevelName(int level)
    {
        string result = "";
        switch (level)
        {
            case 0:
                result = AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText);
                break;
            case 1:
                result = AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText) + " I";
                break;
            case 2:
                result = AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText) + " II";
                break;
            case 3:
                result = AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText) + " III";
                break;
            case 4:
                result = AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText) + " IV";
                break;
        }

        return result;
    }

    // AoWAb.PAS:3271
    public sbyte GetTurnUndeadDamage(int level)
    {
        sbyte result;
        switch (level)
        {
            case 1:
                result = 4;
                break;
            case 2:
                result = 5;
                break;
            case 3:
                result = 6;
                break;
            case 4:
                result = 7;
                break;
            default:
                result = 0;
                break;
        }

        return result;
    }

    // AoWAb.PAS:3356
    public TurnUndeadCombatAction CreateTurnUndeadCombatAction(CombatObject source, CombatObject target)
    {
        TurnUndeadCombatAction action;
        int level;
        // { These eliminated copies preserve the original saved registers. }
        action = new TurnUndeadCombatAction(null) { Holder = CreationHolder() };
        if (HitChance.HitRole(source.GetAttack() - target.GetDefense()))
        {
            level = source.GetAbilityLevel(AbilityId);
            var turnUndeadDamage = GetTurnUndeadDamage(level);
            action.Generate(source, target, GetTouchAttack(level), turnUndeadDamage, Set<DamageType>.Of(DamageType.Holy), 1);
        }
        else
        {
            action.SourceId = source.Id;
            action.TargetId = target.Id;
        }

        return action;
    }

    // AoWAb.PAS:3164
    public override EngineClass AbilityDataClass() => AbilityRules.MultiLevelDataClass;

}
