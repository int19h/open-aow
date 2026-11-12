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

public partial class HealingAbility : TouchAbility
{
    // AoWAb.PAS:3616
    public HealingAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 47;
        AbilityName = AbilityRules.TranslateResourceString(s_healingText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem, AbilitySelectionType.Editor);
        AbilityType = AbilityType.Enhancement;
    }

    // AoWAb.PAS:3663
    public override string GetName(AbilityOwner owner)
    {
        string result;
        if (owner == null || GetEnabled(owner))
        {
            result = AbilityName;
        }
        else
        {
            var used = AbilityRules.TranslateResourceString(GameText.s_usedText);
            result = AbilityName + " (" + LowerCase(used) + ")";
        }

        return result;
    }

    // AoWAb.PAS:3640
    public override bool GetEnabled(AbilityOwner owner)
    {
        HealingAbilityData data;
        bool result = owner.GetAbilitySetAt(AbilityId);
        if (result)
        {
            data = (HealingAbilityData)owner.GetAbilityData(AbilityId);
            if (data != null)
            {
                result = data.Enabled;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    // AoWAb.PAS:3624
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action, AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:3801
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        CombatObject target;
        FastBattleCommand command;
        AbilityDamageValues values = default;
        for (int I = 0, last = commands.SourceSide.GetCount() - 1; I <= last; I++)
        {
            target = commands.SourceSide.GetObjects(I);
            if (target.GetEnabled() && CanTouch(commands.Source, target))
            {
                FastBattleGetDamageValues(commands.Source, target, WallSituation.None, ref values);
                if (values.SourceDamage != 0)
                {
                    command = new FastBattleCommand(AbilityId);
                    command.Target = target;
                    command.SourceDamageValue = values.SourceDamage;
                    command.TargetDamageValue = 0;
                    command.ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, 0, commands.Source, target);
                    command.MoveCost = 0;
                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:3789
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        HealingCombatAction action;
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        action = CreateHealingCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        action.Release();
        return true;
    }

    // AoWAb.PAS:3654
    public override void NewTurn(AbilityOwner owner, sbyte player)
    {
        if (owner is Troop && ((Troop)owner).Player == player)
        {
            SetEnabled(owner, true);
        }
    }

    // AoWAb.PAS:3827
    public override bool Activate(AbilityOwner owner, ref string reason)
    {
        bool result;
        UnitSelectionActivation info;
        HealingUnitSelector selector;
        if (CanActivate(owner, ref reason))
        {
            var troop = owner as Troop;
            info = new UnitSelectionActivation(null);
            info.AbilityId = AbilityId;
            if (troop != null)
            {
                info.UnitId = troop.UnitId;
            }
            else
            {
                info.UnitId = 0;
            }

            selector = new HealingUnitSelector();
            var healing = AbilityRules.TranslateResourceString(s_healingText);
            selector.Caption = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_selectUnitFormat), [healing]);
            if (troop != null)
            {
                if (troop.Owner is HeroList)
                {
                    var empty = new UnitUserList(null);
                    selector.Setup(troop, empty);
                    empty.Dispose();
                }
                else
                {
                    selector.Setup(troop, troop.UnitListOwner);
                }
            }

            info.SetUnitSelector(selector);
            // { Original omits Selector.Release here. }
            AoWHSMap.Current._onActivateAbilityStore.Raise(info);
            info.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:3670
    public override bool Remove(AbilityOwner owner)
    {
        bool result = base.Remove(owner);
        if (result)
        {
            owner.RemoveAbilityDataId(AbilityId);
        }

        return result;
    }

    // AoWAb.PAS:3707
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        if (!defender.GetImmunityTypes().Contains(DamageType.None))
        {
            result = defender.GetHits() - defender.GetHitPoints();
            if (result > 5)
            {
                result = 5;
            }

            return result * 100;
        }

        return 0;
    }

    // AoWAb.PAS:3718
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && !defender.GetImmunityTypes().Contains(DamageType.None))
        {
            var hits = defender.GetHits();
            var hitPoints = defender.GetHitPoints();
            damageValues.SourceCost = hits - hitPoints;
            if (damageValues.SourceCost > 5)
            {
                damageValues.SourceCost = 5;
            }

            damageValues.SourceDamage = damageValues.SourceCost * 100;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = Set<DamageType>.Of(DamageType.None);
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

    // AoWAb.PAS:3738
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        int hitPoints;
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            hitPoints = GetHealHitPoints(defender);
            damageValues.SourceCost = hitPoints;
            damageValues.SourceDamage = hitPoints * 100;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = Set<DamageType>.Of(DamageType.None);
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

    // AoWAb.PAS:3702
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        base.GetCombatInfo(owner, ref info);
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend);
    }

    // AoWAb.PAS:3693
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

            result = condition && (target.GetHits() != target.GetHitPoints() || target.GetAbilityEnabled(96) || target.GetAbilityEnabled(53) || target.GetAbilityEnabled(108));
        }

        return result;
    }

    // AoWAb.PAS:3685
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result = base.CanTouchUnit(source, target);
        if (result)
        {
            var unitType = target.GetUnitType();
            result = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && (target.GetHits() != target.GetHitPoints() || target.GetAbilityEnabled(96) || target.GetAbilityEnabled(53) || target.GetAbilityEnabled(108));
        }

        return result;
    }

    // AoWAb.PAS:3628
    public void SetEnabled(AbilityOwner owner, bool enabled)
    {
        HealingAbilityData data;
        data = (HealingAbilityData)owner.GetAbilityData(AbilityId);
        if (data == null)
        {
            data = new HealingAbilityData(null);
            owner.AddAbilityData(data);
        }

        data.Enabled = enabled;
    }

    // AoWAb.PAS:3675
    public int GetHealHitPoints(CombatObject target)
    {
        int result = target.GetHits() - target.GetHitPoints();
        if (result > 5)
        {
            result = 5;
        }

        return result;
    }

    // AoWAb.PAS:3680
    public HealingCombatAction CreateHealingCombatAction(CombatObject source, CombatObject target)
    {
        HealingCombatAction result = new HealingCombatAction(null);
        result.Initialize(source.Id, target.Id);
        return result;
    }

    // AoWAb.PAS:3760
    public bool HealUnit(Troop troop)
    {
        int hitPoints;
        bool result = false;
        if (troop.GetHits() != troop.GetHitPoints())
        {
            hitPoints = troop.GetHits() - troop.GetHitPoints();
            if (hitPoints > 5)
            {
                hitPoints = 5;
            }

            troop.SetHitPoints((sbyte)(troop.GetHitPoints() + (sbyte)hitPoints));
            result = true;
        }

        if (troop.GetAbilityEnabled(96))
        {
            troop.RemoveAbility(96);
            result = true;
        }

        if (troop.GetAbilityEnabled(53))
        {
            troop.RemoveAbility(53);
            result = true;
        }

        if (troop.GetAbilityEnabled(108))
        {
            troop.RemoveAbility(108);
            result = true;
        }

        troop.Changed();
        return result;
    }

    // AoWAb.PAS:3659
    public override EngineClass AbilityDataClass() => s_healingDataClass;

    private static EngineClass s_healingDataClass = EngineClass.Of<HealingAbilityData>();
    private static readonly string s_healingText = "Healing";

}
