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
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Magic.Abilities;

public partial class HealingAbility : TouchAbility
{
    // AoWAb.PAS:3616
    public HealingAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 47;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_healingText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem, AbilitySelectionType.Editor);
        AbilityType = AbilityType.Enhancement;
    }

    // AoWAb.PAS:3663
    public override string GetName(AbilityOwner owner)
    {
        if (owner is null || GetEnabled(owner))
        {
            return AbilityName;
        }
        else
        {
            string used = AbilityRules.TranslateResourceString(GetEngine(), GameText.UsedText);
            return $"{AbilityName} ({LowerCase(used)})";
        }
    }

    // AoWAb.PAS:3640
    public override bool GetEnabled(AbilityOwner owner)
    {
        bool result = owner.GetAbilitySetAt(AbilityId);
        if (result)
        {
            HealingAbilityData data = GetData(owner);
            return data is null || data.Enabled;
        }

        return result;
    }

    // AoWAb.PAS:3624
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action, AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:3801
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        AbilityDamageValues values = default;
        for (int i = 0, last = commands.SourceSide.GetCount() - 1; i <= last; i++)
        {
            CombatObject target = commands.SourceSide.GetObjects(i);
            if (target.GetEnabled() && CanTouch(commands.Source, target))
            {
                FastBattleGetDamageValues(commands.Source, target, WallSituation.None, ref values);
                if (values.SourceDamage != 0)
                {
                    FastBattleCommand command = new(AbilityId)
                    {
                        Target = target,
                        SourceDamageValue = values.SourceDamage,
                        TargetDamageValue = 0,
                        ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, 0, commands.Source, target),
                        MoveCost = 0
                    };
                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:3789
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        HealingCombatAction action = CreateHealingCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        _ = action.Release();
        return true;
    }

    // AoWAb.PAS:3654
    public override void NewTurn(AbilityOwner owner, sbyte player)
    {
        if (owner is Troop troop && troop.Player == player)
        {
            SetEnabled(owner, true);
        }
    }

    // AoWAb.PAS:3827
    public override bool Activate(AbilityOwner owner, ref string reason)
    {
        if (CanActivate(owner, ref reason))
        {
            Troop troop = owner as Troop;
            UnitSelectionActivation info = new(null)
            {
                Holder = CreationHolder(),
                AbilityId = AbilityId,
                UnitId = troop is not null ? troop.UnitId : 0
            };

            HealingUnitSelector selector = new(this);
            string healing = AbilityRules.TranslateResourceString(GetEngine(), s_healingText);
            selector.Caption = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.SelectUnitFormat), [healing]);
            if (troop is not null)
            {
                if (troop.Owner is IHeroHolder)
                {
                    using UnitUserList empty = new(null) { Holder = CreationHolder() };
                    selector.Setup(troop, empty.Units);
                }
                else
                {
                    selector.Setup(troop, troop.OwnerUnits);
                }
            }

            info.SetUnitSelector(selector);
            // { Original omits Selector.Release here. }
            StrategicMap.Current.OnActivateAbilityStore.Raise(info);
            _ = info.Release();
            return true;
        }
        else
        {
            return false;
        }
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
        if (!defender.GetImmunityTypes().Contains(DamageType.None))
        {
            int result = defender.GetHits() - defender.GetHitPoints();
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
            sbyte hits = defender.GetHits();
            sbyte hitPoints = defender.GetHitPoints();
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
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            int hitPoints = GetHealHitPoints(defender);
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
        return result
            ? source is CombatUnit && target is CombatUnit targetUnit && Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(targetUnit.Troop.GetUnitType()) && (target.GetHits() != target.GetHitPoints() || target.GetAbilityEnabled(96) || target.GetAbilityEnabled(53) || target.GetAbilityEnabled(108))
            : result;
    }

    // AoWAb.PAS:3685
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result = base.CanTouchUnit(source, target);
        if (result)
        {
            UnitType unitType = target.GetUnitType();
            return Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && (target.GetHits() != target.GetHitPoints() || target.GetAbilityEnabled(96) || target.GetAbilityEnabled(53) || target.GetAbilityEnabled(108));
        }

        return result;
    }

    // AoWAb.PAS:3628
    public void SetEnabled(AbilityOwner owner, bool enabled)
    {
        HealingAbilityData data = GetData(owner);
        if (data is null)
        {
            data = new HealingAbilityData(null) { Holder = CreationHolder() };
            owner.AddAbilityData(data);
        }

        data.Enabled = enabled;
    }

    // AoWAb.PAS:3675
    public int GetHealHitPoints(CombatObject target)
    {
        int result = target.GetHits() - target.GetHitPoints();
        return result > 5 ? 5 : result;
    }

    // AoWAb.PAS:3680. The ranged event already drew its random value.
    public override CombatAction CreateRangedCombatAction(CombatUnit source, CombatUnit target) =>
        CreateHealingCombatAction(source, target: target);

    // Draws the random value the touch event used to draw, then builds the action.
    public override CombatAction CreateTouchMoveCombatAction(CombatUnit source, CombatUnit target)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        return CreateHealingCombatAction(source, target: target);
    }

    public HealingCombatAction CreateHealingCombatAction(CombatObject source, CombatObject target)
    {
        HealingCombatAction result = new(null) { Holder = CreationHolder() };
        result.Initialize(source.Id, target.Id);
        return result;
    }

    // AoWAb.PAS:3760
    public bool HealUnit(Troop troop)
    {
        bool result = false;
        if (troop.GetHits() != troop.GetHitPoints())
        {
            int hitPoints = troop.GetHits() - troop.GetHitPoints();
            if (hitPoints > 5)
            {
                hitPoints = 5;
            }

            troop.SetHitPoints((sbyte)(troop.GetHitPoints() + (sbyte)hitPoints));
            result = true;
        }

        if (troop.GetAbilityEnabled(96))
        {
            _ = troop.RemoveAbility(96);
            result = true;
        }

        if (troop.GetAbilityEnabled(53))
        {
            _ = troop.RemoveAbility(53);
            result = true;
        }

        if (troop.GetAbilityEnabled(108))
        {
            _ = troop.RemoveAbility(108);
            result = true;
        }

        troop.Changed();
        return result;
    }

    // AoWAb.PAS:3659
    public override EngineClass AbilityDataClass() => s_healingDataClass;

    public HealingAbilityData GetData(AbilityOwner owner) => owner.GetAbilityData<HealingAbilityData>(AbilityId);

    private static readonly EngineClass s_healingDataClass = EngineClass.Of<HealingAbilityData>();
    private static readonly string s_healingText = "Healing";

}
