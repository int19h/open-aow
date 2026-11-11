// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// completion, control, groups, mode, remaining strength, remaining strengths, path factor,
// target, strength, damage types, strengths, required superiority
public partial class AIGroupCooperationControl : AIGroupTask
{
    // AIGroup.pas:2839
    public AIGroupCooperationControl(ECustomNode owner) : base(owner)
    {
        FGroups = new AIRecruitmentGroupList(null);
    }

    public bool FComplete;
    public AIGroupControl FControl;
    public AIRecruitmentGroupList FGroups;
    public CooperationMode FMode;
    public int FRemainingStrength;
    public Int32Array10 FRemainingStrengths;
    public float FPathStrengthFactor;
    public AIGroupControlTarget FTarget;
    public int FCooperateStrength;
    public Set<DamageType> FDamageTypes;
    public Int32Array10 FCooperateStrengths;
    public int FSuperiority;

    // AIGroup.pas:2845
    public override void Dispose()
    {
        FGroups?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:2919
    public override bool GetBusy() => FState != -1;

    // AIGroup.pas:2932
    public override bool Activate(AIGroupControl Control)
    {
        FControl = Control;
        FRemainingStrength = FCooperateStrength;
        UpdateCompletion();
        return true;
    }

    // AIGroup.pas:3017
    public override bool Process()
    {
        int I;
        switch (FState)
        {
            case 0:
                AoWHSMap.Current.FAIExecuter.UseProcessTime(500);
                FControl.FRecruitmentGroups.Validate(FControl);
                I = 0;
                while (!FComplete && I != FControl.FRecruitmentGroups.Count)
                {
                    if (ValidCooperationGroup(FControl.FRecruitmentGroups.GetRecruitmentGroups(I)))
                    {
                        AddCooperationGroup(FControl.FRecruitmentGroups.GetRecruitmentGroups(I));
                    }

                    I += 1;
                }

                if ((FGroups.Count != 0 && FMode == CooperationMode.AutoCooperate) || (FMode == CooperationMode.CompleteCooperateOnly && FComplete))
                {
                    ExecuteCooperation();
                }

                Done();
                break;
        }

        return false;
    }

    // AIGroup.pas:2924
    public virtual bool Setup(AIGroupControl Control, AIGroupControlTarget Target)
    {
        Reset();
        FTarget = Target;
        return true;
    }

    // AIGroup.pas:2851
    public int GetCooperateMoveDays()
    {
        int Moves;
        int Days;
        int Result = -1;
        // { Original bug: the first cooperation group is skipped. }
        for (int I = 1, last = FGroups.Count - 1; I <= last; I++)
        {
            Moves = FGroups.GetRecruitmentGroups(I).FGroup.GetMovesMin();
            if (Moves > 0)
            {
                Days = (FGroups.GetRecruitmentGroups(I).FMovePoints - 1) / Moves;
                if (Result < Days)
                {
                    Result = Days;
                }
            }
        }

        return Result;
    }

    // AIGroup.pas:2868
    public int GetCooperatedStrength() => FCooperateStrength - FRemainingStrength;

    // AIGroup.pas:2873
    public int GetCooperateStrengthEx(DamageType DamageType) => FCooperateStrengths[(int)DamageType];

    // AIGroup.pas:2878
    public void SetCooperateStrengthEx(DamageType DamageType, int Value)
    {
        FCooperateStrengths[(int)DamageType] = Value;
    }

    // AIGroup.pas:2883
    public void UpdateCompletion()
    {
        DamageType D;
        FComplete = FRemainingStrength <= 0;
        if (FComplete)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                if (FRemainingStrengths[(int)D] > 0)
                {
                    FComplete = false;
                    return;
                }
            }

            if (FSuperiority != -1)
            {
                FControl.FCombatPredictor.Execute();
                if (FControl.FCombatPredictor.GetSuperiority() < FSuperiority)
                {
                    FComplete = false;
                }
            }
        }
    }

    // AIGroup.pas:2905
    public void Reset()
    {
        FGroups.Clear();
        FComplete = false;
        FMode = CooperationMode.None;
        FTarget = null;
        FCooperateStrength = 0;
        FDamageTypes = AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall);
        FCooperateStrengths = default;
        FSuperiority = 120;
        FPathStrengthFactor = (float)0.8;
        FState = 0;
    }

    // AIGroup.pas:2940
    public void AddCooperationGroup(AIRecruitmentGroup Group)
    {
        Troop AUnit;
        DamageType D;
        FGroups.AddChild(Group);
        for (int I = Group.FGroup.Count - 1; I >= 0; I--)
        {
            AUnit = Group.FGroup.GetUnit(I);
            var strengthMax = AUnit.GetStrengthMax(FDamageTypes);
            FRemainingStrength -= strengthMax;
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                var strength = AUnit.GetStrength(D);
                FRemainingStrengths[(int)D] -= strength;
            }

            if (FSuperiority != -1)
            {
                FControl.FCombatPredictor.AddUnit(AUnit);
            }
        }

        UpdateCompletion();
    }

    // AIGroup.pas:2958
    public bool ValidCooperationGroup(AIRecruitmentGroup Group)
    {
        bool Result;
        int Strength1;
        int Strength2;
        int Strength;
        int[] Location__cell = new int[1];
        ref int Location = ref Location__cell[0];
        int Advantage1;
        int Advantage2;
        int Moves;
        int OtherMoves;
        int Threshold;
        AIGroup Group1;
        AIGroup Group2;
        if (Group.FGroup.FTransporterID == -1 && ((Unsafe.As<MapCoordinate, int>(ref Group.FGroup.FTargetXYL) == Unsafe.As<MapCoordinate, int>(ref FTarget.FLocation) && Group.FGroup.FTargetType == AIGroupTargetStatus.Setup) || Set<AIGroupTargetStatus>.Of(AIGroupTargetStatus.None, AIGroupTargetStatus.Idle).Contains(Group.FGroup.FTargetType)) && Group.FGroup.CooperateWithGroup(FControl.FGroup, FTarget))
        {
            if (Group.FPathStrength != 0)
            {
                Location = Group.FGroup.GetLoc();
                Strength1 = Group.FGroup.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                Group1 = Group.FGroup;
                Advantage1 = -GetBattleFieldStrengthAdvantage(Location, Group1.FPlayer, Group1.FIndependentRelation, Group1, 1, 10, AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));

                Location = FControl.FGroup.GetLoc();
                Strength2 = FControl.FGroup.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                Group2 = FControl.FGroup;
                Advantage2 = -GetBattleFieldStrengthAdvantage(Location, Group2.FPlayer, Group2.FIndependentRelation, Group2, 1, 10, AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));

                if (Strength1 < Strength2)
                {
                    Strength = Strength1;
                }
                else
                {
                    Strength = Strength2;
                }

                // { Original bug: compares the first advantage with itself. }
                if (Advantage1 > Advantage1 + 0)
                {
                }
                else
                {
                    Advantage1 = Advantage2;
                }

                Threshold = Round((double)Group.FPathStrength * FPathStrengthFactor);
                if (Strength < Threshold && Strength >= Advantage1)
                {
                    Moves = Group.FGroup.GetMovePointsMin();
                    OtherMoves = FControl.FGroup.GetMovePointsMin();
                    if (OtherMoves < Moves)
                    {
                        Moves = OtherMoves;
                    }

                    Result = Moves >= Group.FMovePoints;
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:2998
    public void ExecuteCooperation()
    {
        AIGroup Group;
        Troop Leader;
        for (int I = FGroups.Count - 1; I >= 0; I--)
        {
            Group = FGroups.GetRecruitmentGroups(I).FGroup;
            if (Group.FTargetType == AIGroupTargetStatus.Setup && FTarget.FMovePoints > Group.FTargetMovePoints)
            {
                Leader = Group.FLeader;
            }
            else
            {
                Leader = FControl.FGroup.FLeader;
            }

            for (int J = Group.FProductions.Count - 1; J >= 0; J--)
            {
                Group.FProductions.ChangeGroup(J, FControl.FGroup);
            }

            FControl.FGroup.AddUnits(Group);
            FControl.FGroup.SetLeader(Leader);
        }
    }

    private static int GetBattleFieldStrengthAdvantage(int Location, sbyte Player, sbyte Relation, UnitList Units, int FriendlyRadius, int EnemyRadius, Set<DamageType> DamageTypes)
    {
        BattleFieldInfo Info = default(BattleFieldInfo);
        BattleFieldInfo.SetupBattleFieldInfo(ref Info, Player, Relation, Set<BattleFieldInfoRequestsElement>.Of(Array.Empty<BattleFieldInfoRequestsElement>()), DamageTypes);
        Info.EnemyRelations = Set<byte>.WithBounds(0, 7);
        Info.Units = Units;
        Info.Requests += Set<BattleFieldInfoRequestsElement>.Of(new BattleFieldInfoRequestsElement[1] { BattleFieldInfoRequestsElement.IgnorePassiveIndependents });
        AoWHSMap.Current.RetrieveBattleFieldInfo(Unsafe.As<int, MapCoordinate>(ref Location).X, Unsafe.As<int, MapCoordinate>(ref Location).Y, Unsafe.As<int, MapCoordinate>(ref Location).L, FriendlyRadius, ref Info);
        int friendlyStrength = Info.FriendlyStrength;
        BattleFieldInfo.SetupBattleFieldInfo(ref Info, Player, Relation, Set<BattleFieldInfoRequestsElement>.Of(Array.Empty<BattleFieldInfoRequestsElement>()), DamageTypes);
        Info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
        Info.Units = Units;
        Info.Requests += Set<BattleFieldInfoRequestsElement>.Of(new BattleFieldInfoRequestsElement[1] { BattleFieldInfoRequestsElement.IgnorePassiveIndependents });
        AoWHSMap.Current.RetrieveBattleFieldInfo(Unsafe.As<int, MapCoordinate>(ref Location).X, Unsafe.As<int, MapCoordinate>(ref Location).Y, Unsafe.As<int, MapCoordinate>(ref Location).L, EnemyRadius, ref Info);
        int strength = Info.Strength;
        return friendlyStrength - strength;
    }

    public enum CooperationMode : byte
    {
        None = 0, AutoCooperate = 1, CompleteCooperateOnly = 2
    }

    // intf_P5_07.inc:431

    // intf_P5_07.inc:431

}
