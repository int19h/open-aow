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

public partial class DispelMagicAbility : TouchAbility
{
    // AoWAb.PAS:4040
    public DispelMagicAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 60;
        AbilityName = AbilityRules.TranslateResourceString(GameText.DispelMagicText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem, AbilitySelectionType.Editor);
        AbilityType = AbilityType.Enhancement;
    }

    // AoWAb.PAS:4129
    public override string GetName(AbilityOwner owner)
    {
        string result = GetLevelName(GetLevel(owner));
        if (owner != null && !GetEnabled(owner))
        {
            result = result + " (" + LowerCase(AbilityRules.TranslateResourceString(GameText.UsedText)) + ")";
        }

        return result;
    }

    // AoWAb.PAS:4096
    public override int GetLevel(AbilityOwner owner)
    {
        int result;
        DispelMagicAbilityData data;
        if (owner != null)
        {
            if (owner.GetAbilitySetAt(60))
            {
                data = (DispelMagicAbilityData)owner.GetAbilityData(AbilityId);
                if (data != null)
                {
                    result = data.Level;
                }
                else
                {
                    result = 1;
                }
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

    // AoWAb.PAS:4064
    public override bool GetEnabled(AbilityOwner owner)
    {
        DispelMagicAbilityData data;
        bool result = owner.GetAbilitySetAt(AbilityId);
        if (result)
        {
            data = (DispelMagicAbilityData)owner.GetAbilityData(AbilityId);
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

    // AoWAb.PAS:4262
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        DispelMagicAbilityCombatAction action;
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        action = new DispelMagicAbilityCombatAction(null);
        action.Generate(source, target, GetDispelMana(((CombatUnit)source).Troop));
        source.Combat.ExecuteCombatAction(action);
        action.Release();
        return true;
    }

    // AoWAb.PAS:4087
    public override void NewTurn(AbilityOwner owner, sbyte player)
    {
        if (owner is Troop && ((Troop)owner).Player == player)
        {
            SetEnabled(owner, true);
        }
    }

    // AoWAb.PAS:4311
    public override bool Activate(AbilityOwner owner, ref string reason)
    {
        bool result;
        UnitSelectionActivation info;
        DispelMagicAbilityUnitSelector selector;
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

            selector = new DispelMagicAbilityUnitSelector();
            var dispelMagic = AbilityRules.TranslateResourceString(GameText.DispelMagicText);
            selector.Caption = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.SelectUnitFormat), [dispelMagic]);
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
            selector.Release();
            StrategicMap.Current.OnActivateAbilityStore.Raise(info);
            info.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:4139
    public override bool CanExpand(AbilityOwner owner)
    {
        if ((owner.GetAbilitySelectionTypes() & SelectionTypes) == Set<AbilitySelectionType>.Of() || GetLevel(owner) >= 3)
        {
            return false;
        }

        return true;
    }

    // AoWAb.PAS:4135
    public override int ExpandCost(AbilityOwner owner) => 5;

    // AoWAb.PAS:4125
    public override string ExpandName(AbilityOwner owner) => GetLevelName(GetLevel(owner) + 1);

    // AoWAb.PAS:4152
    public override bool Expand(AbilityOwner owner)
    {
        DispelMagicAbilityData data;
        bool result = false;
        data = (DispelMagicAbilityData)owner.GetAbilityData(AbilityId);
        if (data == null)
        {
            data = new DispelMagicAbilityData(null);
            data.Level = 1;
            data.Enabled = true;
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            result = true;
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
        DispelMagicAbilityData data;
        bool result = true;
        data = (DispelMagicAbilityData)owner.GetAbilityData(AbilityId);
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

    // AoWAb.PAS:4209
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        // { Original evaluates dispelling with the healing hit-point formula. }
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

    // AoWAb.PAS:4221
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        // { Original evaluates dispelling with the healing hit-point formula. }
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
        if (result)
        {
            bool condition = source is CombatUnit && target is CombatUnit;
            if (condition)
            {
                var player = source.GetPlayer();
                condition = ((CombatUnit)target).Troop.CanDispelEnchantment(player);
            }

            result = condition;
        }

        return result;
    }

    // AoWAb.PAS:4197
    public override bool CanTouchUnit(Troop source, Troop target) => base.CanTouchUnit(source, target) && target.CanDispelEnchantment(source.Player);

    // AoWAb.PAS:4052
    public void SetEnabled(AbilityOwner owner, bool enabled)
    {
        DispelMagicAbilityData data;
        data = (DispelMagicAbilityData)owner.GetAbilityData(AbilityId);
        if (data == null)
        {
            data = new DispelMagicAbilityData(null);
            owner.AddAbilityData(data);
        }

        data.Enabled = enabled;
    }

    // AoWAb.PAS:4083
    public int GetDispelMana(AbilityOwner owner) => GetLevel(owner) * 10 + 20;

    // AoWAb.PAS:4116
    public string GetLevelName(int level)
    {
        string result = "";
        switch (level)
        {
            case 0:
                result = AbilityRules.TranslateResourceString(GameText.DispelMagicText);
                break;
            case 1:
                result = AbilityRules.TranslateResourceString(GameText.DispelMagicText) + " I";
                break;
            case 2:
                result = AbilityRules.TranslateResourceString(GameText.DispelMagicText) + " II";
                break;
            case 3:
                result = AbilityRules.TranslateResourceString(GameText.DispelMagicText) + " III";
                break;
        }

        return result;
    }

    // AoWAb.PAS:4301
    public DispelMagicAbilityCombatAction CreateCombatAction(CombatUnit source, CombatObject target)
    {
        bool successful;
        DispelMagicAbilityCombatAction result = new DispelMagicAbilityCombatAction(null);
        result.Generate(source, target, GetDispelMana(source.Troop));
        if (source.GetSide() == target.GetSide())
        {
            successful = true;
        }
        else
        {
            successful = CombatTouchRole(source, target);
        }

        if (!successful)
        {
            result.EnchantmentIds.Clear();
        }

        return result;
    }

    // AoWAb.PAS:4092
    public override EngineClass AbilityDataClass() => s_dispelMagicDataClass;

    private static readonly EngineClass s_dispelMagicDataClass = EngineClass.Of<DispelMagicAbilityData>();

}
