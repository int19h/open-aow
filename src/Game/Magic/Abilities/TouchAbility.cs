// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class TouchAbility : AdjustableAbility
{
    // AoWAb.PAS:1843
    public TouchAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = FSelectionTypes + Set<AbilitySelectionType>.Of(AbilitySelectionType.UseItem);
        FAbilityType = AbilityType.Combat;
    }

    // AoWAb.PAS:1875
    public override CombatMode GetCombatMode(AbilityOwner Owner) => CombatMode.Touch;

    // AoWAb.PAS:1850
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:1962
    public override void fcPrefetchCombatCommands(FastBattleCommandList Commands)
    {
        CombatObject Target;
        FastBattleCommand Command;
        AbilityDamageValues Values = default;
        for (int I = 0, last = Commands.FTargetSide.GetCount() - 1; I <= last; I++)
        {
            Target = Commands.FTargetSide.GetObjects(I);
            if (Target.GetEnabled() && CanTouch(Commands.FSource, Target) && fcValidRoundDistance(Commands.FSource, Target))
            {
                fcGetDamageValueEx(Commands.FSource, Target, WallSituation.None, ref Values);
                if (Values.SourceDamage != 0)
                {
                    Command = new FastBattleCommand(FAbilityID);
                    Command.FTarget = Commands.FTargetSide.GetObjects(I);
                    Command.FSourceDV = Values.SourceDamage;
                    Command.FTargetDV = Values.TargetDamage;
                    Command.FExpectedDV = AbilityRules.DVtoDEV(Values.SourceDamage, Values.TargetDamage, Commands.FSource, Target);
                    if (Values.TargetDamage == 0)
                    {
                        Command.FMoveCost = 1;
                    }
                    else
                    {
                        Command.FMoveCost = 2;
                    }

                    Commands.Add(Command);
                }
            }
        }
    }

    // AoWAb.PAS:1860
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        sbyte Attack;
        int Level;
        Level = GetLevel(Owner);
        Attack = GetTouchAttack(Level);
        Info.Attack = Attack;
        if (Attack < 0)
        {
            Info.Attack = 0;
        }

        Info.Damage = -1;
        Info.DamageTypes = Set<DamageType>.Of();
        Info.RepeatCount = 1;
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:1855
    public virtual sbyte GetTouchAttack(int Level) => -10;

    // AoWAb.PAS:1892
    public virtual bool CombatTouchRole(CombatUnit Source, CombatObject Target)
    {
        int Attack;
        bool Result = HitChance.HitRole(Source.GetAttack() - Target.GetDefense());
        Attack = GetTouchAttack(GetLevel(Source.FUnit));
        if (Result && Attack != -10)
        {
            Result = Result && HitChance.HitRole(Attack - Target.GetResistance());
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:1904
    public virtual float CombatTouchRoleProbability(CombatUnit Source, CombatObject Target)
    {
        int Attack;
        float Result = HitChance.HitRoleProbability(Source.GetAttack() - Target.GetDefense());
        Attack = GetTouchAttack(GetLevel(Source.FUnit));
        if (Attack != -10)
        {
            Result = (float)((double)Result * HitChance.HitRoleProbability(Attack - Target.GetResistance()));
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:1880
    public virtual float TouchRoleProbability(Troop Source, Troop Target)
    {
        int Attack;
        float Result = HitChance.HitRoleProbability(Source.GetAttack() - Target.GetDefense());
        Attack = GetTouchAttack(GetLevel(Source));
        if (Attack != -10)
        {
            Result = (float)((double)Result * HitChance.HitRoleProbability(Attack - Target.GetResistance()));
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:1916
    public virtual bool CanTouch(CombatObject Source, CombatObject Target)
    {
        bool Result;
        if (Source.GetSide() == CombatSideIndex.Defender)
        {
            if (Source.fcBehindWall() && Source.fcWallInBetween(Target.FPosition))
            {
                Result = Target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            if (Target.fcBehindWall() && Source.fcWallInBetween(Target.FPosition) && !Source.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        if (Source.GetSide() != Target.GetSide())
        {
            Result = Result && (!Target.GetAbilityEnabled(1) || Source.GetAbilityEnabled(1));
        }

        return Result;
    }

    // AoWAb.PAS:1937
    public virtual bool CanTouchUnit(Troop Source, Troop Target)
    {
        if (Target.GetAbilityEnabled(1) && !Source.GetAbilityEnabled(1))
        {
            return false;
        }

        return true;
    }

    // AoWAb.PAS:1947
    public void fcValidateMoveOutsideWall(CombatObject Source, CombatObject Target)
    {
        MoveWallCombatAction Action;
        if (Source.fcBehindWall() && Source.fcWallInBetween(Target.FPosition) && !Target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
        {
            Action = new MoveWallCombatAction(null);
            Action.F_MoveWallCA_0C = Source.FID;
            Action.F_MoveWallCA_0D = 0;
            Source.FCombat.ExecuteCombatAction(Action);
            Action.Release();
        }
    }
}
