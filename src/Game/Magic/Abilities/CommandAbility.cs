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

// (commanded ability ID)
public partial class CommandAbility : TouchAbility
{
    // AoWAb.PAS:5088
    public CommandAbility(CustomNode owner) : base(owner) => SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);

    public int CommandedAbilityId { get; protected set; }

    // AoWAb.PAS:5190
    public override AbilityType GetAbilityType() => AbilityType.Combat;

    // AoWAb.PAS:5288
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        AbilityDamageValues values = default;
        int distance = commands.Combat.GetRoundDistance();
        if (commands.Source.GetMoves() >= 32)
        {
            distance += 2;
        }

        for (int i = 0, last = commands.TargetSide.GetCount() - 1; i <= last; i++)
        {
            CombatObject target = commands.TargetSide.GetObjects(i);
            if (target.GetEnabled() && target.FastBattleRoundDistance() <= distance && CanTouch(commands.Source, target))
            {
                FastBattleGetDamageValues(commands.Source, target, WallSituation.None, ref values);
                if (values.SourceDamage != 0)
                {
                    FastBattleCommand command = new(AbilityId)
                    {
                        Target = commands.TargetSide.GetObjects(i),
                        ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, commands.Source, target),
                        MoveCost = 0
                    };
                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:5266
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        if (source is CombatUnit sourceUnit && target is CombatUnit targetUnit)
        {
            CommandCombatAction action = CreateCommandCombatAction(sourceUnit, targetUnit);
            source.Combat.ExecuteCombatAction(action);
            if (!action.Successful)
            {
                StrikeCombatAction strike = CombatObject.CreateStrikeCombatAction(target, source, true);
                if (strike is not null)
                {
                    source.Combat.ExecuteCombatAction(strike);
                    _ = strike.Release();
                }
            }

            _ = action.Release();
        }

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
        if (CanTouchUnit(attacker, defender))
        {
            float probability = TouchRoleProbability(attacker, defender);
            return Round((double)(defender.GetHitPoints() * 200) * probability);
        }
        else
        {
            return 0;
        }
    }

    // AoWAb.PAS:5213
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && defender.GetUnitType() != UnitType.Machine)
        {
            float probability = TouchRoleProbability(attacker, defender);
            sbyte hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)(hitPoints * 200) * probability);
            sbyte defenderHitPoints = defender.GetHitPoints();
            damageValues.SourceCost = defenderHitPoints * 2;
            int unitStrikeDamageValue = Troop.UnitStrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round(unitStrikeDamageValue * ((double)1 - probability));
            sbyte damage = defender.GetDamage();
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
            strength = Round(GetTouchAttack(GetLevel(troop)) * (troop.GetAttack() * 5) * 0.1);
            types = Set<DamageType>.Of(DamageType.None);
        }
    }

    // AoWAb.PAS:5237
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (CanTouch(attacker, defender) && attacker is CombatUnit attackerUnit && defender is CombatUnit defenderUnit && defenderUnit.Troop is not Leader)
        {
            float probability = CombatTouchRoleProbability(attackerUnit, defender);
            sbyte hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)(hitPoints * 200) * probability);
            sbyte defenderHitPoints = defender.GetHitPoints();
            damageValues.SourceCost = defenderHitPoints * 2;
            int strikeDamageValue = CombatObject.StrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round(strikeDamageValue * ((double)1 - probability));
            sbyte damage = defender.GetDamage();
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
        if (combatObject is CombatUnit combatUnit)
        {
            ResetCommandedUnits(combatUnit);
        }
    }

    // AoWAb.PAS:5102
    public override void CombatDone(CombatObject combatObject) => combatObject.GetAbilityOwner().RemoveAbilityDataId(AbilityId);

    // AoWAb.PAS:5176
    public override bool CanTouch(CombatObject source, CombatObject target) => base.CanTouch(source, target) && source is CombatUnit && target is CombatUnit targetUnit && targetUnit.Troop.GetUnitType() != UnitType.Machine && !target.GetAbilityEnabled(107);

    // AoWAb.PAS:5184
    public override bool CanTouchUnit(Troop source, Troop target) => base.CanTouchUnit(source, target) && target.GetUnitType() != UnitType.Machine;

    // AoWAb.PAS:5110
    public virtual void Command(CombatUnit source, CombatUnit target)
    {
        // { The returned class is consumed by its constructor. }
        Uncommand(target);
        // { Inline the returned class receiver. }
        CommandedAbilityData targetData = GamePack.Current.AbilityControl.GetAbility<CommandedAbility>(CommandedAbilityId).AbilityDataClass().Create<CommandedAbilityData>(null, holder: CreationHolder());
        targetData.AbilityId = CommandedAbilityId;
        targetData.CommandAbilityId = AbilityId;
        targetData.SourceId = source.Id;
        targetData.OriginalCombatPlayer = (byte)target.CombatPlayer;
        SetAbility(target.Troop, CommandedAbilityId, 1);
        target.Troop.AddAbilityData(targetData);
        target.SetCombatPlayer((byte)source.CombatPlayer);
        CommandAbilityData sourceData = GetData(source.Troop);
        if (sourceData is null)
        {
            sourceData = new CommandAbilityData(null)
            {
                Holder = CreationHolder(),
                AbilityId = AbilityId
            };
            source.Troop.AddAbilityData(sourceData);
        }

        _ = sourceData.CommandedUnits.Add(target.Id);
    }

    // AoWAb.PAS:5259. Draws the random value the touch event used to draw, then builds the action.
    public override CombatAction CreateTouchMoveCombatAction(CombatUnit source, CombatUnit target)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        return CreateCommandCombatAction(source, target: target);
    }

    public virtual CommandCombatAction CreateCommandCombatAction(CombatUnit source, CombatUnit target)
    {
        CommandCombatAction result = new(null) { Holder = CreationHolder() };
        result.Initialize(successful: target.Troop is not Leader && CombatTouchRole(source, target), sourceId: source.Id, targetId: target.Id);
        result.AbilityId = AbilityId;
        return result;
    }

    // AoWAb.PAS:5157
    public virtual void ResetCommandedUnits(CombatUnit source)
    {
        Troop troopOwner = source.Troop;
        if (troopOwner.GetAbilityEnabled(AbilityId))
        {
            CommandAbilityData data = GetData(source.Troop);
            if (data is not null)
            {
                for (int i = data.CommandedUnits.Count - 1; i >= 0; i--)
                {
                    CombatUnit target = source.Combat.CombatData.FindId<CombatUnit>(data.CommandedUnits.List[i]);
                    if (target is not null)
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
        Troop troopOwner = target.Troop;
        if (troopOwner.GetAbilityEnabled(CommandedAbilityId))
        {
            CommandedAbilityData data = GamePack.Current.AbilityControl.GetAbility<CommandedAbility>(CommandedAbilityId).GetData(troopOwner);
            // { Original reads Data before checking it for nil. }
            CombatUnit source = target.Combat.CombatData.FindId<CombatUnit>(data.SourceId);
            if (data is not null)
            {
                target.SetCombatPlayer(data.OriginalCombatPlayer);
            }

            _ = GamePack.Current.AbilityControl.GetAbility(CommandedAbilityId).Remove(target.Troop);
            if (source is not null)
            {
                CommandAbilityData sourceData = GetData(source.Troop);
                _ = (sourceData?.CommandedUnits.Remove(target.Id));
            }
        }
    }

    // AoWAb.PAS:5093
    public override EngineClass AbilityDataClass() => CommandDataClass;

    public CommandAbilityData GetData(AbilityOwner owner) => owner.GetAbilityData<CommandAbilityData>(AbilityId);

    internal static EngineClass CommandDataClass { get; } = EngineClass.Of<CommandAbilityData>();

}
