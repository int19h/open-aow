#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

namespace Aow.Game.Magic.Abilities;

// (melee round)
public class StrikeAbility : AdjustableAbility
{

    // AoWAb.PAS:1210
    public StrikeAbility(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.AttackItem;
        AbilityType = AbilityType.Combat;
        AbilityId = 21;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_strikeText);
        MeleeRound = new MeleeRound(this);
    }

    public MeleeRound MeleeRound { get; }

    // AoWAb.PAS:1220
    public override void Dispose() => base.Dispose();

    // AoWAb.PAS:1349
    public override CombatMode GetCombatMode(AbilityOwner? owner) => CombatMode.Melee;

    // AoWAb.PAS:1226
    public override AbilityControlTypes GetControlType(AbilityOwner? owner) => AbilityControlTypes.None;

    // AoWAb.PAS:1445
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        AbilityDamageValues values = default;
        for (int i = 0, last = commands.TargetSide.GetCount() - 1; i <= last; i++)
        {
            CombatObject target = commands.TargetSide.GetObjects(i);
            if (target.GetEnabled() && FastBattleValidRoundDistance(commands.Source, target) && CanExecuteMelee(commands.Source, target))
            {
                FastBattleGetDamageValues(commands.Source, defender: target, wallSituation: WallSituation.None, damageValues: ref values);
                if (values.SourceDamage != 0)
                {
                    FastBattleCommand command = new(AbilityId)
                    {
                        Target = target,
                        SourceDamageValue = values.SourceDamage,
                        TargetDamageValue = values.TargetDamage,
                        ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, targetDamageValue: values.TargetDamage, commands.Source, target),
                        // { Both original branches assign the same move cost. }
                        MoveCost = values.TargetDamage == 0 ? 2 : 2
                    };

                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:1399
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand? command)
    {
        CombatObject? target = command.Target;
        MeleeRound round = new(this);
        round.Calculate(source, defender: target);
        bool stop = false;
        int i = 0;
        if (source is FastBattleUnit fastUnit)
        {
            fastUnit.State |= FastBattleUnitStates.Vulnerable;
            if (source.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position) && !target.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing))
            {
                MoveWallCombatAction moveCombatAction = new(null)
                {
                    Holder = CreationHolder(),
                    CombatObjectId = source.Id,

                    InsideWall = 0
                };

                source.Combat.ExecuteCombatAction(moveCombatAction);
                _ = moveCombatAction.Release();
            }
        }

        while (i < round.Count && !stop)
        {
            CombatAction? action = round.CreateStrikeCombatAction(i);
            if (action is not null)
            {
                source.Combat.ExecuteCombatAction(action);
                MeleeRound.Strike strike = round.GetMeleeStrike(i);
                switch (strike.Side)
                {
                    case CombatSideIndex.Attacker:
                        stop = !target.GetEnabled();
                        break;
                    case CombatSideIndex.Defender:
                        stop = !source.GetEnabled();
                        break;
                    case CombatSideIndex.Neutral:
                    default:
                        break;
                }

                _ = action.Release();
            }

            i += 1;
        }

        bool result = round.Count != 0;
        return result;
    }

    // AoWAb.PAS:1297
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        if (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1))
        {
            MeleeRound.CalculateUnit(attacker, defender);
            return MeleeRound.GetAttackerDamageValue();
        }
        else
        {
            return 0;
        }
    }

    // AoWAb.PAS:1308
    public override void GetDamageValues(Troop attacker, Troop? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)))
        {
            MeleeRound.CalculateUnit(attacker, defender);
            int attackerDamageValue = MeleeRound.GetAttackerDamageValue();
            damageValues.SourceDamage = attackerDamageValue;
            damageValues.SourceCost = 0;
            int defenderDamageValue = MeleeRound.GetDefenderDamageValue();
            damageValues.TargetDamage = defenderDamageValue;
            damageValues.TargetCost = 0;
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = MeleeRound.AttackerDamageTypes & ~immunityTypes;
        }
        else
        {
            // { Original leaves the cost fields untouched on this path. }
            damageValues.SourceDamage = 0;
            damageValues.TargetDamage = 0;
            damageValues.DamageTypes = DamageTypes.None;
        }
    }

    // AoWAb.PAS:1274
    public override void GetOffensiveStrength(AbilityOwner? owner, ref int strength, ref DamageTypes types)
    {
        if (owner is Troop troop)
        {
            int repeats = 2;
            int attack = troop.GetAttack();
            int damage = troop.GetDamage();
            if (troop.GetAbilityEnabled(115))
            {
                repeats += 1;
            }

            strength = AbilityRules.LimitedDamage(attack, defense: damage, damage: repeats);
            types = GetDamageTypes(owner);
        }
        else
        {
            strength = 0;
            types = DamageTypes.None;
        }
    }

    // AoWAb.PAS:1328
    public override void FastBattleGetDamageValues(CombatObject? attacker, CombatObject? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)))
        {
            MeleeRound.Calculate(attacker, defender);
            int attackerDamageValue = MeleeRound.GetAttackerDamageValue();
            damageValues.SourceDamage = attackerDamageValue;
            damageValues.SourceCost = 0;
            int defenderDamageValue = MeleeRound.GetDefenderDamageValue();
            damageValues.TargetDamage = defenderDamageValue;
            damageValues.TargetCost = 0;
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = MeleeRound.AttackerDamageTypes & ~immunityTypes;
        }
        else
        {
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = DamageTypes.None;
        }
    }

    // AoWAb.PAS:1378
    public override void GetCombatInfo(AbilityOwner? owner, ref AbilityCombatInfo info)
    {
        if (owner is Troop troop)
        {
            int attack = troop.GetAttack();
            info.Attack = (sbyte)attack;
            int damage = troop.GetDamage();
            info.Damage = (sbyte)damage;

            DamageTypes damageTypes = GetDamageTypes(owner);
            info.DamageTypes = damageTypes;
            info.RepeatCount = 2;
        }
        else
        {
            info.Attack = -1;
            info.Damage = -1;
            info.DamageTypes = DamageTypes.Physical;
            info.RepeatCount = 2;
        }
    }

    // AoWAb.PAS:1248
    public bool CanExecuteMelee(CombatObject source, CombatObject target)
    {
        bool result = source.Side == CombatSideIndex.Defender
            ? !source.FastBattleBehindWall() || !source.FastBattleWallInBetween(target.Position) || target.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing)
            : !target.FastBattleBehindWall() || !source.FastBattleWallInBetween(target.Position) || source.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing);
        return result && (!target.GetAbilityEnabled(1) || source.GetAbilityEnabled(1));
    }

    // AoWAb.PAS:1354
    public DamageTypes GetDamageTypes(AbilityOwner? owner)
    {
        DamageTypes result;
        if (owner is Troop troop && troop.GetAbilityEnabled(21))
        {
            result = DamageTypes.Physical;
            if (troop.GetAbilityEnabled(14) || troop.GetAbilityEnabled(163))
            {
                result |= DamageTypes.Fire;
            }

            if (troop.GetAbilityEnabled(15))
            {
                result |= DamageTypes.Cold;
            }

            if (troop.GetAbilityEnabled(16))
            {
                result |= DamageTypes.Lightning;
            }

            if (troop.GetAbilityEnabled(17))
            {
                result |= DamageTypes.Poison;
            }

            if (troop.GetAbilityEnabled(18) || troop.GetAbilityEnabled(169))
            {
                result |= DamageTypes.Death;
            }

            if (troop.GetAbilityEnabled(19))
            {
                result |= DamageTypes.Holy;
            }

            if (troop.GetAbilityEnabled(20) || troop.GetAbilityEnabled(154))
            {
                result |= DamageTypes.Magic;
            }
        }
        else
        {
            return DamageTypes.None;
        }

        return result;
    }

    public override bool Remove(AbilityOwner? owner)
    {
        if (!owner.GetOwnAbilityEnabled(21))
        {
            return false;
        }

        _ = base.Remove(owner);
        for (int i = owner.OwnAbilityCount - 1; i >= 0; --i)
        {
            if (owner.GetOwnAbilityEnabled(i) && owner.GetEngine().GamePack.AbilityControl[i] is StrikeEnhancementAbility)
            {
                while (owner.GetEngine().GamePack.AbilityControl[i].Remove(owner))
                {
                }
            }
        }

        return true;
    }

    private static readonly string s_strikeText = "Strike";
}
