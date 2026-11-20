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

public partial class DispelMagicAbility : TouchAbility
{
    // AoWAb.PAS:4040
    public DispelMagicAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 60;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem, AbilitySelectionType.Editor);
        AbilityType = AbilityType.Enhancement;
    }

    // AoWAb.PAS:4129
    public override string GetName(AbilityOwner owner)
    {
        string result = GetLevelName(GetLevel(owner));
        return owner is not null && !GetEnabled(owner)
            ? $"{result} ({LowerCase(AbilityRules.TranslateResourceString(GetEngine(), GameText.UsedText))})"
            : result;
    }

    // AoWAb.PAS:4096
    public override int GetLevel(AbilityOwner owner)
    {
        if (owner is not null)
        {
            if (owner.GetAbilitySetAt(60))
            {
                DispelMagicAbilityData data = GetData(owner);
                return data is not null ? data.Level : 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    // AoWAb.PAS:4064
    public override bool GetEnabled(AbilityOwner owner)
    {
        bool result = owner.GetAbilitySetAt(AbilityId);
        if (result)
        {
            DispelMagicAbilityData data = GetData(owner);
            return data is null || data.Enabled;
        }

        return result;
    }

    // AoWAb.PAS:4148
    public override int GetSkillPoints(AbilityOwner owner)
    {
        int result = GetLevel(owner);
        return result * 5;
    }

    // AoWAb.PAS:4048
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action, AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:4275
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

    // AoWAb.PAS:4262
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        if (source is CombatUnit sourceUnit)
        {
            DispelMagicAbilityCombatAction action = new(null) { Holder = CreationHolder() };
            action.Generate(source, target, GetDispelMana(sourceUnit.Troop));
            source.Combat.ExecuteCombatAction(action);
            _ = action.Release();
        }

        return true;
    }

    // AoWAb.PAS:4087
    public override void NewTurn(AbilityOwner owner, sbyte player)
    {
        if (owner is Troop troop && troop.Player == player)
        {
            SetEnabled(owner, true);
        }
    }

    // AoWAb.PAS:4311
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

            DispelMagicAbilityUnitSelector selector = new(this);
            string dispelMagic = AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicText);
            selector.Caption = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.SelectUnitFormat), [dispelMagic]);
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
            _ = selector.Release();
            StrategicMap.Current.OnActivateAbilityStore.Raise(info);
            _ = info.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // AoWAb.PAS:4139
    public override bool CanExpand(AbilityOwner owner) => (owner.GetAbilitySelectionTypes() & SelectionTypes) != Set<AbilitySelectionType>.Of() && GetLevel(owner) < 3;

    // AoWAb.PAS:4135
    public override int ExpandCost(AbilityOwner owner) => 5;

    // AoWAb.PAS:4125
    public override string ExpandName(AbilityOwner owner) => GetLevelName(GetLevel(owner) + 1);

    // AoWAb.PAS:4152
    public override bool Expand(AbilityOwner owner)
    {
        bool result = false;
        DispelMagicAbilityData data = GetData(owner);
        if (data is null)
        {
            data = new DispelMagicAbilityData(null)
            {
                Holder = CreationHolder(),
                Level = 1,
                Enabled = true
            };
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            return true;
        }
        else
        {
            if (data.Level != 3)
            {
                result = true;
                data.Level = (byte)(data.Level + 1);
                // { Original skips level one when upgrading data created by SetEnabled. }
                if (data.Level == 1)
                {
                    data.Level = 2;
                }
            }
        }

        return result;
    }

    // AoWAb.PAS:4175
    public override bool Remove(AbilityOwner owner)
    {
        DispelMagicAbilityData data = GetData(owner);
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
            return false;
        }

        return true;
    }

    // AoWAb.PAS:4209
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        // { Original evaluates dispelling with the healing hit-point formula. }
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

    // AoWAb.PAS:4221
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        // { Original evaluates dispelling with the healing hit-point formula. }
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

    // AoWAb.PAS:4242
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            // { Original assigns zero benefit, so its prefetch loop emits no commands. }
            damageValues.SourceCost = 0;
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

    // AoWAb.PAS:4078
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        base.GetCombatInfo(owner, ref info);
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend, CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:4202
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = base.CanTouch(source, target);
        return result
            ? source is CombatUnit && target is CombatUnit targetUnit && targetUnit.Troop.CanDispelEnchantment(source.GetPlayer())
            : result;
    }

    // AoWAb.PAS:4197
    public override bool CanTouchUnit(Troop source, Troop target) => base.CanTouchUnit(source, target) && target.CanDispelEnchantment(source.Player);

    // AoWAb.PAS:4052
    public void SetEnabled(AbilityOwner owner, bool enabled)
    {
        DispelMagicAbilityData data = GetData(owner);
        if (data is null)
        {
            data = new DispelMagicAbilityData(null) { Holder = CreationHolder() };
            owner.AddAbilityData(data);
        }

        data.Enabled = enabled;
    }

    // AoWAb.PAS:4083
    public int GetDispelMana(AbilityOwner owner) => (GetLevel(owner) * 10) + 20;

    // AoWAb.PAS:4116
    public string GetLevelName(int level)
    {
        return level switch
        {
            0 => AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicText),
            1 => $"{AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicText)} I",
            2 => $"{AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicText)} II",
            3 => $"{AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicText)} III",
            _ => "",
        };
    }

    // AoWAb.PAS:4301. The ranged event already drew its random value.
    public override CombatAction CreateRangedCombatAction(CombatUnit source, CombatUnit target) =>
        CreateCombatAction(source, target: target);

    // Draws the random value the touch event used to draw, then builds the action.
    public override CombatAction CreateTouchMoveCombatAction(CombatUnit source, CombatUnit target)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        return CreateCombatAction(source, target: target);
    }

    public DispelMagicAbilityCombatAction CreateCombatAction(CombatUnit source, CombatObject target)
    {
        DispelMagicAbilityCombatAction result = new(null) { Holder = CreationHolder() };
        result.Generate(source, target, GetDispelMana(source.Troop));
        bool successful = source.GetSide() == target.GetSide() || CombatTouchRole(source, target);

        if (!successful)
        {
            result.EnchantmentIds.Clear();
        }

        return result;
    }

    // AoWAb.PAS:4092
    public override EngineClass AbilityDataClass() => s_dispelMagicDataClass;

    public DispelMagicAbilityData GetData(AbilityOwner owner) => owner.GetAbilityData<DispelMagicAbilityData>(AbilityId);

    private static readonly EngineClass s_dispelMagicDataClass = EngineClass.Of<DispelMagicAbilityData>();

}
