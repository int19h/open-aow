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

// (commanded ability ID)
public partial class CommandAbility : TouchAbility
{
    // AoWAb.PAS:5088
    public CommandAbility(ECustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
    }

    public int CommandedAbilityId { get; protected set; }

    // AoWAb.PAS:5190
    public override AbilityType GetAbilityType() => AbilityType.Combat;

    // AoWAb.PAS:5288
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        int distance;
        CombatObject target;
        AbilityDamageValues values = default;
        FastBattleCommand command;
        distance = commands.Combat.GetRoundDistance();
        if (commands.Source.GetMoves() >= 32)
        {
            distance += 2;
        }

        for (int I = 0, last = commands.TargetSide.GetCount() - 1; I <= last; I++)
        {
            target = commands.TargetSide.GetObjects(I);
            if (target.GetEnabled() && target.FastBattleRoundDistance() <= distance && CanTouch(commands.Source, target))
            {
                FastBattleGetDamageValues(commands.Source, target, WallSituation.None, ref values);
                if (values.SourceDamage != 0)
                {
                    command = new FastBattleCommand(AbilityId);
                    command.Target = commands.TargetSide.GetObjects(I);
                    command.ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, commands.Source, target);
                    command.MoveCost = 0;
                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:5266
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        CommandCombatAction action;
        StrikeCombatAction strike;
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        action = CreateCommandCombatAction((CombatUnit)source, (CombatUnit)target);
        source.Combat.ExecuteCombatAction(action);
        if (!action.Successful)
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

    // AoWAb.PAS:5097
    public override bool Remove(AbilityOwner owner)
    {
        bool result = base.Remove(owner);
        if (result)
        {
            owner.RemoveAbilityDataId(AbilityId);
        }

        return result;
    }

    // AoWAb.PAS:5194
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        float probability;
        if (CanTouchUnit(attacker, defender))
        {
            probability = TouchRoleProbability(attacker, defender);
            result = Round((double)(defender.GetHitPoints() * 200) * probability);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:5213
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        if (wallSituation == WallSituation.None && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && defender.GetUnitType() != UnitType.Machine)
        {
            probability = TouchRoleProbability(attacker, defender);
            var hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)(hitPoints * 200) * probability);
            var defenderHitPoints = defender.GetHitPoints();
            damageValues.SourceCost = defenderHitPoints * 2;
            var unitStrikeDamageValue = Troop.UnitStrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round((double)unitStrikeDamageValue * ((double)1 - probability));
            var damage = defender.GetDamage();
            damageValues.TargetCost = damage;
        }
        else
        {
            // { Original leaves DamageTypes unchanged on this path. }
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:5205
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        if (owner is Troop troop)
        {
            strength = Round((double)(GetTouchAttack(GetLevel(troop)) * (troop.GetAttack() * 5)) * 0.1);
            types = Set<DamageType>.Of(DamageType.None);
        }
    }

    // AoWAb.PAS:5237
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        if (CanTouch(attacker, defender) && !(((CombatUnit)defender).Troop is Leader))
        {
            probability = CombatTouchRoleProbability((CombatUnit)attacker, defender);
            var hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)(hitPoints * 200) * probability);
            var defenderHitPoints = defender.GetHitPoints();
            damageValues.SourceCost = defenderHitPoints * 2;
            var strikeDamageValue = CombatObject.StrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round((double)strikeDamageValue * ((double)1 - probability));
            var damage = defender.GetDamage();
            damageValues.TargetCost = damage;
        }
        else
        {
            // { Original leaves DamageTypes unchanged on this path. }
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:5106
    public override void CombatObjectDestroyed(CombatObject combatObject)
    {
        ResetCommandedUnits((CombatUnit)combatObject);
    }

    // AoWAb.PAS:5102
    public override void CombatDone(CombatObject combatObject)
    {
        combatObject.GetAbilityOwner().RemoveAbilityDataId(AbilityId);
    }

    // AoWAb.PAS:5176
    public override bool CanTouch(CombatObject source, CombatObject target) => base.CanTouch(source, target) && (source is CombatUnit && target is CombatUnit && ((CombatUnit)target).Troop.GetUnitType() != UnitType.Machine && !target.GetAbilityEnabled(107));

    // AoWAb.PAS:5184
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result;
        if (base.CanTouchUnit(source, target))
        {
            result = target.GetUnitType() != UnitType.Machine;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:5110
    public virtual void Command(CombatUnit source, CombatUnit target)
    {
        CommandedAbilityData targetData;
        CommandAbilityData sourceData;
        // { The returned class is consumed by its constructor. }
        Uncommand(target);
        // { Inline the returned class receiver. }
        targetData = (CommandedAbilityData)GamePack.Current.AbilityControl.GetAbility(CommandedAbilityId).AbilityDataClass().CreateInstance(null, holder: CreationHolder());
        targetData.AbilityId = CommandedAbilityId;
        targetData.CommandAbilityId = AbilityId;
        targetData.SourceId = source.Id;
        targetData.OriginalCombatPlayer = (byte)target.CombatPlayer;
        SetAbility(target.Troop, CommandedAbilityId, 1);
        target.Troop.AddAbilityData(targetData);
        target.SetCombatPlayer((byte)source.CombatPlayer);
        sourceData = (CommandAbilityData)source.Troop.GetAbilityData(AbilityId);
        if (sourceData == null)
        {
            sourceData = new CommandAbilityData(null) { Holder = CreationHolder() };
            sourceData.AbilityId = AbilityId;
            source.Troop.AddAbilityData(sourceData);
        }

        sourceData.CommandedUnits.Add(target.Id);
    }

    // AoWAb.PAS:5259
    public virtual CommandCombatAction CreateCommandCombatAction(CombatUnit source, CombatUnit target)
    {
        CommandCombatAction result = new CommandCombatAction(null) { Holder = CreationHolder() };
        bool condition = !(target.Troop is Leader) && CombatTouchRole(source, target);
        result.Initialize(source.Id, target.Id, condition);
        result.AbilityId = AbilityId;
        return result;
    }

    // AoWAb.PAS:5157
    public virtual void ResetCommandedUnits(CombatUnit source)
    {
        Troop troopOwner;
        CommandAbilityData data;
        CombatUnit target;
        troopOwner = source.Troop;
        if (troopOwner.GetAbilityEnabled(AbilityId))
        {
            data = (CommandAbilityData)source.Troop.GetAbilityData(AbilityId);
            if (data != null)
            {
                for (int I = data.CommandedUnits.Count - 1; I >= 0; I--)
                {
                    target = (CombatUnit)source.Combat.CombatData.FindId(data.CommandedUnits.List[I]);
                    if (target != null)
                    {
                        Uncommand(target);
                    }
                }
            }
        }
    }

    // AoWAb.PAS:5135
    public virtual void Uncommand(CombatUnit target)
    {
        Troop troopOwner;
        CommandedAbilityData data;
        CombatUnit source;
        CommandAbilityData sourceData;
        troopOwner = target.Troop;
        if (troopOwner.GetAbilityEnabled(CommandedAbilityId))
        {
            data = (CommandedAbilityData)troopOwner.GetAbilityData(CommandedAbilityId);
            // { Original reads Data before checking it for nil. }
            source = (CombatUnit)target.Combat.CombatData.FindId(data.SourceId);
            if (data != null)
            {
                target.SetCombatPlayer(data.OriginalCombatPlayer);
            }

            GamePack.Current.AbilityControl.GetAbility(CommandedAbilityId).Remove(target.Troop);
            if (source != null)
            {
                sourceData = (CommandAbilityData)source.Troop.GetAbilityData(AbilityId);
                if (sourceData != null)
                {
                    sourceData.CommandedUnits.Remove(target.Id);
                }
            }
        }
    }

    // AoWAb.PAS:5093
    public override EngineClass AbilityDataClass() => CommandDataClass;

    internal static EngineClass CommandDataClass { get; } = EngineClass.Of<CommandAbilityData>();

}
