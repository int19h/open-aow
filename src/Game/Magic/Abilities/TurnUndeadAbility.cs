#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public partial class TurnUndeadAbility : TouchAbility
{
    // AoWAb.PAS:3121
    public TurnUndeadAbility(CustomNode owner, EngineObject holder) : base(owner)
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
        if (owner is not null)
        {
            data = GetData(owner);
            if (data is not null)
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
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
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
            if (strike is not null)
            {
                source.Combat.ExecuteCombatAction(strike);
                _ = strike.Release();
            }
        }

        _ = action.Release();
        return true;
    }

    // AoWAb.PAS:3204
    public override bool CanExpand(AbilityOwner owner) => (owner.GetAbilitySelectionTypes() & SelectionTypes) != Set<AbilitySelectionType>.Of() && GetLevel(owner) < 4;

    // AoWAb.PAS:3199
    public override int ExpandCost(AbilityOwner owner)
    {
        int result = GetLevel(owner);
        result = result < 4 ? 5 : 0;

        return result;
    }

    // AoWAb.PAS:3195
    public override string ExpandName(AbilityOwner owner) => GetLevelName(GetLevel(owner) + 1);

    // AoWAb.PAS:3217
    public override bool Expand(AbilityOwner owner)
    {
        MultiLevelAbilityData data;
        bool result = false;
        data = GetData(owner);
        if (data is null)
        {
            data = new MultiLevelAbilityData(null)
            {
                Holder = CreationHolder(),
                LevelAbilityId = (ushort)AbilityId,
                Level = 1
            };
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
        data = GetData(owner);
        if (data is not null)
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
            int unitStrikeDamageValue = Troop.UnitStrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round(unitStrikeDamageValue * ((double)1 - probability));
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: attacker.GetProtectionTypes(), damageTypes: GamePack.Current.AbilityControl.GetAbility<StrikeAbility>(21).GetDamageTypes(defender), immunityTypes: attacker.GetImmunityTypes(), damage: defender.GetDamage());
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

            strength = Round(troop.GetAttack() * 4 * damage * 0.1);
            // { Original reports death damage here, although the attack deals holy damage. }
            types = Set<DamageType>.Of(DamageType.Death);
        }
    }

    // AoWAb.PAS:3331
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        int hitPoints;
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender) && attacker is CombatUnit attackerUnit)
        {
            probability = CombatTouchRoleProbability(attackerUnit, defender);
            hitPoints = defender.GetHitPoints();
            damageValues.SourceCost = hitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            int strikeDamageValue = CombatObject.StrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round(strikeDamageValue * ((double)1 - probability));
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: attacker.GetProtectionTypes(), immunityTypes: attacker.GetImmunityTypes(), damageTypes: CombatObject.MeleeDamageTypes(defender), damage: defender.GetDamage());
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
        sbyte touchAttack = GetTouchAttack(level);
        info.Attack = touchAttack;
        sbyte turnUndeadDamage = GetTurnUndeadDamage(level);
        info.Damage = turnUndeadDamage;
        info.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
        info.RepeatCount = 1;
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:3260
    public override sbyte GetTouchAttack(int level)
    {
        sbyte result = level switch
        {
            1 => 4,
            2 => 5,
            3 => 6,
            4 => 7,
            _ => 4,
        };
        return result;
    }

    // AoWAb.PAS:3137
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = base.CanTouch(source, target);
        if (result)
        {
            result = source is CombatUnit && target is CombatUnit targetUnit && Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(targetUnit.Troop.GetUnitType()) && ((ushort)targetUnit.Troop.GetRace() == 11 || targetUnit.Troop.GetAbilityEnabled(129));
        }

        return result;
    }

    // AoWAb.PAS:3130
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result = base.CanTouchUnit(source, target);
        if (result)
        {
            UnitType unitType = target.GetUnitType();
            result = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && ((ushort)target.GetRace() == 11 || target.GetAbilityEnabled(129));
        }

        return result;
    }

    // AoWAb.PAS:3181
    public string GetLevelName(int level)
    {
        string result = level switch
        {
            0 => AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText),
            1 => $"{AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText)} I",
            2 => $"{AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText)} II",
            3 => $"{AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText)} III",
            4 => $"{AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText)} IV",
            _ => "",
        };

        return result;
    }

    // AoWAb.PAS:3271
    public sbyte GetTurnUndeadDamage(int level)
    {
        sbyte result = level switch
        {
            1 => 4,
            2 => 5,
            3 => 6,
            4 => 7,
            _ => 0,
        };
        return result;
    }

    // AoWAb.PAS:3356. Draws the random value the touch event used to draw, then builds the action.
    public override CombatAction CreateTouchMoveCombatAction(CombatUnit source, CombatUnit target)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        return CreateTurnUndeadCombatAction(source, target: target);
    }

    public TurnUndeadCombatAction CreateTurnUndeadCombatAction(CombatObject source, CombatObject target)
    {
        TurnUndeadCombatAction action;
        int level;
        // { These eliminated copies preserve the original saved registers. }
        action = new TurnUndeadCombatAction(null) { Holder = CreationHolder() };
        if (HitChance.HitRole(source.GetAttack() - target.GetDefense()))
        {
            level = source.GetAbilityLevel(AbilityId);
            action.Generate(damage: GetTurnUndeadDamage(level), source: source, target: target, attack: GetTouchAttack(level), damageTypes: Set<DamageType>.Of(DamageType.Holy), damageMode: 1);
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

    public MultiLevelAbilityData GetData(AbilityOwner owner) => owner.GetAbilityData<MultiLevelAbilityData>(AbilityId);

}
