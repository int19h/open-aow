#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// (melee round)
public partial class StrikeAbility : AdjustableAbility
{

    // AoWAb.PAS:1210
    public StrikeAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem);
        AbilityType = AbilityType.Combat;
        AbilityId = 21;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_strikeText);
        MeleeRound = new MeleeRound(this);
    }

    public MeleeRound MeleeRound { get; }

    // AoWAb.PAS:1220
    public override void Dispose()
    {
        MeleeRound?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:1349
    public override CombatMode GetCombatMode(AbilityOwner owner) => CombatMode.Melee;

    // AoWAb.PAS:1226
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of();

    // AoWAb.PAS:1445
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        CombatObject target;
        FastBattleCommand command;
        AbilityDamageValues values = default;
        for (int i = 0, last = commands.TargetSide.GetCount() - 1; i <= last; i++)
        {
            target = commands.TargetSide.GetObjects(i);
            if (target.GetEnabled() && FastBattleValidRoundDistance(commands.Source, target) && CanExecuteMelee(commands.Source, target))
            {
                FastBattleGetDamageValues(commands.Source, target, WallSituation.None, ref values);
                if (values.SourceDamage != 0)
                {
                    command = new FastBattleCommand(AbilityId);
                    command.Target = target;
                    command.SourceDamageValue = values.SourceDamage;
                    command.TargetDamageValue = values.TargetDamage;
                    command.ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, commands.Source, target);
                    // { Both original branches assign the same move cost. }
                    command.MoveCost = values.TargetDamage == 0 ? 2 : 2;

                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:1399
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        MeleeRound round;
        int i;
        bool stop;
        CombatObject target;
        CombatAction action;
        MoveWallCombatAction moveCombatAction;
        MeleeRound.Strike strike = default;
        target = command.Target;
        round = new MeleeRound(this);
        round.Calculate(source, target);
        stop = false;
        i = 0;
        if (source is FastBattleUnit)
        {
            ((FastBattleUnit)source).State = ((FastBattleUnit)source).State + Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.Vulnerable);
            if (source.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position) && !target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                moveCombatAction = new MoveWallCombatAction(null) { Holder = CreationHolder() };
                moveCombatAction.CombatObjectId = source.Id;

                moveCombatAction.InsideWall = 0;

                source.Combat.ExecuteCombatAction(moveCombatAction);
                moveCombatAction.Release();
            }
        }

        while (i < round.Count && !stop)
        {
            action = round.CreateStrikeCombatAction(i);
            if (action != null)
            {
                source.Combat.ExecuteCombatAction(action);
                round.GetMeleeStrike(i, out strike);
                switch (strike.Side)
                {
                    case CombatSideIndex.Attacker:
                        stop = !target.GetEnabled();
                        break;
                    case CombatSideIndex.Defender:
                        stop = !source.GetEnabled();
                        break;
                }

                action.Release();
            }

            i += 1;
        }

        bool result = round.Count != 0;
        round?.Dispose();
        return result;
    }

    // AoWAb.PAS:1297
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        if (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1))
        {
            MeleeRound.CalculateUnit(attacker, defender);
            result = MeleeRound.GetAttackerDamageValue();
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:1308
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
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
            Set<DamageType> immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = MeleeRound.AttackerDamageTypes - immunityTypes;
        }
        else
        {
            // { Original leaves the cost fields untouched on this path. }
            damageValues.SourceDamage = 0;
            damageValues.TargetDamage = 0;
            damageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:1274
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        int attack;
        int damage;
        int repeats;
        if (owner is Troop troop)
        {
            repeats = 2;
            attack = troop.GetAttack();
            damage = troop.GetDamage();
            if (troop.GetAbilityEnabled(115))
            {
                repeats += 1;
            }

            strength = AbilityRules.LimitedDamage(attack, damage, repeats);
            types = GetDamageTypes(owner);
        }
        else
        {
            strength = 0;
            types = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:1328
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
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
            Set<DamageType> immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = MeleeRound.AttackerDamageTypes - immunityTypes;
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

    // AoWAb.PAS:1378
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        if (owner is Troop troop)
        {
            sbyte attack = troop.GetAttack();
            info.Attack = attack;
            sbyte damage = troop.GetDamage();
            info.Damage = damage;

            Set<DamageType> damageTypes = GetDamageTypes(owner);
            info.DamageTypes = damageTypes;
            info.RepeatCount = 2;
        }
        else
        {
            info.Attack = -1;
            info.Damage = -1;
            info.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            info.RepeatCount = 2;
        }
    }

    // AoWAb.PAS:1248
    public bool CanExecuteMelee(CombatObject source, CombatObject target)
    {
        bool result = source.GetSide() == CombatSideIndex.Defender
            ? !source.FastBattleBehindWall() || !source.FastBattleWallInBetween(target.Position) || target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing)
            : !target.FastBattleBehindWall() || !source.FastBattleWallInBetween(target.Position) || source.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
        return result && (!target.GetAbilityEnabled(1) || source.GetAbilityEnabled(1));
    }

    // AoWAb.PAS:1354
    public Set<DamageType> GetDamageTypes(AbilityOwner owner)
    {
        Set<DamageType> result;
        if (owner is Troop troop && troop.GetAbilityEnabled(21))
        {
            result = Set<DamageType>.Of(DamageType.Physical);
            if (troop.GetAbilityEnabled(14) || troop.GetAbilityEnabled(163))
            {
                result = result + Set<DamageType>.Of(DamageType.Fire);
            }

            if (troop.GetAbilityEnabled(15))
            {
                result = result + Set<DamageType>.Of(DamageType.Cold);
            }

            if (troop.GetAbilityEnabled(16))
            {
                result = result + Set<DamageType>.Of(DamageType.Lightning);
            }

            if (troop.GetAbilityEnabled(17))
            {
                result = result + Set<DamageType>.Of(DamageType.Poison);
            }

            if (troop.GetAbilityEnabled(18) || troop.GetAbilityEnabled(169))
            {
                result = result + Set<DamageType>.Of(DamageType.Death);
            }

            if (troop.GetAbilityEnabled(19))
            {
                result = result + Set<DamageType>.Of(DamageType.Holy);
            }

            if (troop.GetAbilityEnabled(20) || troop.GetAbilityEnabled(154))
            {
                result = result + Set<DamageType>.Of(DamageType.Magic);
            }
        }
        else
        {
            result = Set<DamageType>.Of();
        }

        return result;
    }

    public override bool Remove(AbilityOwner owner)
    {
        if (!owner.GetAbEnabled(21))
        {
            return false;
        }

        base.Remove(owner);
        for (int i = owner.GetAbCount() - 1; i >= 0; --i)
        {
            if (owner.GetAbEnabled(i) && GamePack.Current.AbilityControl.GetAbility(i) is StrikeEnhancementAbility)
            {
                while (GamePack.Current.AbilityControl.GetAbility(i).Remove(owner)) { }
            }
        }

        return true;
    }

    private static readonly string s_strikeText = "Strike";
}
