// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class BreathAbility : RangedAttackAbility
{
    // AoWAb.PAS:4852
    public BreathAbility(ECustomNode owner) : base(owner)
    {
        FRangeRA = CombatRange.Short;
        FAttackRepeatRA = 1;
        FAttackRA = 7;
        FDamageRA = 5;
    }

    // AoWAb.PAS:4894
    public override bool fcExecuteCombatCommand(CombatObject Source, FastBattleCommand Command)
    {
        Combat.Battle Combat;
        DelphiList Targets;
        CombatObject Target;
        CombatObject Other;
        int Count;
        RangedAttackCombatAction Action;
        bool Result = base.fcExecuteCombatCommand(Source, Command);
        if (Result)
        {
            Combat = Source.FCombat;
            Targets = new DelphiList();
            Target = Command.FTarget;
            for (int I = Combat.FSides[(int)Target.GetSide()].GetCount() - 1; I >= 0; I--)
            {
                var side = Target.GetSide();
                Other = Combat.FSides[(int)side].GetObjects(I);
                bool condition = Other != Target && Other.GetEnabled() && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)Target.FPosition) >> 4) && Other.fcRoundDistance() <= Combat.GetRoundDistance();
                if (condition)
                {
                    var immunityTypes = Other.GetImmunityTypes();
                    condition = FDamageTypesRA.Value - immunityTypes != Set<DamageType>.Of();
                }

                if (condition)
                {
                    Targets.Add(Other);
                }
            }

            Count = AoWHSMap.Current.Random(2) + 1;
            while (Count != 0 && Targets.Count != 0)
            {
                Other = (CombatObject)Targets[AoWHSMap.Current.Random(Targets.Count)];
                Action = CreateRangedAttackCA(Source, Other, 2);
                Source.FCombat.ExecuteCombatAction(Action);
                Action.Release();
                Targets.Remove(Other);
                Count -= 1;
            }

            Targets?.Dispose();
        }

        return Result;
    }

    // AoWAb.PAS:4860
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        var attackRepeatRA = GetAttackRepeatRA(Owner);
        var damageRA = GetDamageRA(Owner);
        Strength = AbilityRules.P3LimitedDamage(GetAttackRA(Owner), damageRA, attackRepeatRA);
        Strength = Strength * 2;
        Types = GetDamageTypesRA(Owner);
    }

    // AoWAb.PAS:4866
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        int Count;
        Combat.Battle Combat;
        CombatObject Target;
        base.fcGetDamageValueEx(Attacker, Defender, WallSituation, ref DamageValues);
        if (DamageValues.SourceDamage != 0)
        {
            Combat = Attacker.FCombat;
            Count = 0;
            for (int I = Combat.FSides[(int)Defender.GetSide()].GetCount() - 1; I >= 0; I--)
            {
                var side = Defender.GetSide();
                Target = Combat.FSides[(int)side].GetObjects(I);
                bool condition = Target != Defender && Target.GetEnabled() && (int)(((uint)Target.FPosition) >> 4) == (int)(((uint)Defender.FPosition) >> 4) && Target.fcRoundDistance() <= Combat.GetRoundDistance();
                if (condition)
                {
                    var immunityTypes = Target.GetImmunityTypes();
                    condition = FDamageTypesRA.Value - immunityTypes != Set<DamageType>.Of();
                }

                if (condition)
                {
                    Count += 1;
                }
            }

            if (Count != 0)
            {
                if (Count > 3)
                {
                    Count = 3;
                }

                DamageValues.SourceDamage += Round((double)(DamageValues.SourceDamage * Count) / 3);
            }
        }
    }

    // AoWAb.PAS:4930
    public override void tcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, ref AbilityDamageValues DamageValues)
    {
        base.fcGetDamageValueEx(Attacker, Defender, WallSituation.None, ref DamageValues);
    }
}
