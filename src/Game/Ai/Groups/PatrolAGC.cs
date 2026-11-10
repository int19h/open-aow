// Translated from IndependentAG.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

public partial class PatrolAGC : AIGroupControl
{
    // Heading 0 is the centre, 1 to 6 the hex sides (fixed-arrays.md, named constant).
    public const int HexDirectionSlotCount = 7;

    public PatrolAGC(ECustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:415
    public override void MoveExecuterDone(object Sender)
    {
        switch (FMoveExecuter.FStatus)
        {
            case AIMoveExecuter.Status.Done:
                Done();
                break;
            case AIMoveExecuter.Status.Failed:
                if (FState == 7)
                {
                    ((PatrolAG)FGroup).FPatrolTo.Packed = 16777215;
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:426
    public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
    {
        AoWMapField MapF;
        MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Target.FLocation.Packed);
        if (MapF.GetPlayer() != FGroup.FPlayer)
        {
            SetupCP(Target);
            var execute = FCombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
            {
                if (FCombatPredictor.GetSuperiority() > 80)
                {
                    if (FCombatPredictor.GetEfficiency() > 50)
                    {
                        var efficiency = FCombatPredictor.GetEfficiency();
                        Target.FData.Priority = efficiency + Target.FData.Priority * 100;
                        base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    }
                }
            }
        }
    }

    // IndependentAG.pas:444
    public override void ProcessStates()
    {
        AIGroupDefenseControl Defense;
        AIRecruitmentGroupFinder Recruitment;
        AIGroupRecruitControl Recruit;
        AIGroupTargetFinder Finder;
        MoveControlSettings Settings;
        switch (FState)
        {
            case 0:
                if (((PatrolAG)FGroup).FPatrolStrength == 0)
                {
                    ((PatrolAG)FGroup).FPatrolStrength = FStrengthMax;
                }

                Defense = new AIGroupDefenseControl(null);
                Defense.FDefendMode = AIGroupDefenseControl.GroupDefendMode.Wait;
                ActivatePlugin(Defense);
                Defense.Release();
                FState = 1;
                break;
            case 1:
                if (((PatrolAG)FGroup).FPatrolTo.X == -1 || ((PatrolAG)FGroup).FPatrolTo.L == FLocation.L && HexGeometry.dHXtoRad(((PatrolAG)FGroup).FPatrolTo.X, ((PatrolAG)FGroup).FPatrolTo.Y, FLocation.X, FLocation.Y) <= 2)
                {
                    ResetPatrolToLocation();
                }

                FState += 1;
                break;
            case 2:
                if (FStrengthMax < ((PatrolAG)FGroup).FPatrolStrength)
                {
                    FState += 1;
                    Recruitment = new AIRecruitmentGroupFinder(null);
                    Recruitment.Setup(this);
                    ActivatePlugin(Recruitment);
                    Recruitment.Release();
                }
                else
                {
                    FState = 4;
                }

                break;
            case 3:
                if (FRecruitmentGroups.Count != 0 && FStrengthMax < ((PatrolAG)FGroup).FPatrolStrength)
                {
                    Recruit = new AIGroupRecruitControl(null);
                    Recruit.Reset();
                    Recruit.FRecruitStrength = ((PatrolAG)FGroup).FPatrolStrength - FStrengthMax;
                    Recruit.FMode = AIGroupRecruitControl.Mode.AutoRecruit;
                    ActivatePlugin(Recruit);
                    Recruit.Release();
                }

                FState += 1;
                break;
            case 4:
                Finder = new AIGroupTargetFinder(null);
                Finder.Setup(this);
                var moves = FArmy.Moves(FSelection);
                Finder.FSelector.FMovePointsMax = moves;
                ActivatePlugin(Finder);
                Finder.Release();
                FState += 1;
                break;
            case 5:
                FTargets.Validate();
                if (FTargets.Count != 0)
                {
                    SetupME();
                    var targetLocation = FTargets.GetTargets(0);
                    var target = FTargets.GetTargets(0);
                    ExecuteME(FTargets.GetTargets(0).FLocation.X, target.FLocation.Y, targetLocation.FLocation.L);
                    FTargets.Delete(0);
                }
                else
                {
                    FState += 1;
                }

                break;
            case 6:
                if (AoWHSMap.Current.Random(3) != 0 && ((PatrolAG)FGroup).FPatrolTo.X != -1)
                {
                    FState += 1;
                    SetupME();
                    ExecuteME(((PatrolAG)FGroup).FPatrolTo.X, ((PatrolAG)FGroup).FPatrolTo.Y, ((PatrolAG)FGroup).FPatrolTo.L);
                }
                else
                {
                    Done();
                }

                break;
            case 7:
                Settings = new MoveControlSettings(null);
                Settings.FPlayer = FGroup.FPlayer;
                Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                Settings.SetSourcePos(FLocation.X, FLocation.Y, FLocation.L);
                Settings.SetDestinationPos(((PatrolAG)FGroup).FPatrolFrom.X, ((PatrolAG)FGroup).FPatrolFrom.Y, ((PatrolAG)FGroup).FPatrolFrom.L);
                Settings.FMovePointsMax = 48;
                Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.IgnoreArmies);
                Settings.FArmyHS = FArmyHS;
                Settings.FSelection = FSelection;
                Settings.FMovePointTable = FMovePointTable;
                AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
                if (Settings.FPath.Count == 0)
                {
                    ((PatrolAG)FGroup).FPatrolFrom.Packed = 16777215;
                }

                Settings.Release();
                Done();
                break;
        }
    }

    // IndependentAG.pas:324
    public bool ResetPatrolToLocation()
    {
        bool Result = default;
        int PreviousHeading = default;
        int X = default;
        int Y = default;
        DoubleItemList<AoWMapField> NewList = default;
        PatrolAG Group = default;
        MoveControlSettings Settings = default;
        DelphiList Fields = default;
        var Lists = new DoubleItemList<AoWMapField>[HexDirectionSlotCount];
        MapFieldMsg Msg = default;
        int J = default;
        int Heading = default;
        int LastHeading = default;
        AoWMapField MapF = default;
        ByteList Choices = default;
        DoubleItemList<AoWMapField> SelectedList = default;
        Group = (PatrolAG)FGroup;
        if (Group.FPatrolFrom.X == -1)
        {
            Group.FPatrolFrom = FLocation;
        }

        Settings = new MoveControlSettings(null);
        Settings.FPlayer = Group.FPlayer;
        Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.AI, MoveModeElement.IgnoreArmies);
        Settings.SetSourcePos(Group.FPatrolFrom.X, Group.FPatrolFrom.Y, Group.FPatrolFrom.L);
        Settings.FMovePointsMax = 48;
        Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
        Settings.FArmyHS = FArmyHS;
        Settings.FSelection = FSelection;
        Settings.FMovePointTable = FMovePointTable;
        AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
        Fields = Settings.FProcessedMapFields;
        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            var doubleItemList = new DoubleItemList<AoWMapField>();
            Lists[I] = doubleItemList;
        }

        X = Group.FPatrolFrom.X;
        Y = Group.FPatrolFrom.Y;
        for (int I = Fields.Count - 1; I >= 0; I--)
        {
            // { Original samples every fourth processed field. }
            if ((I & 3) == 3)
            {
                MapF = (AoWMapField)Fields.List[I];
                Heading = HexGeometry.dHXtoHT(X, Y, MapF.Xhx, MapF.Yhx);
                var movePoints = Settings.GetMovePoints(MapF.Xhx, MapF.Yhx, MapF.Lhx);
                Lists[Heading].AddAB(MapF, movePoints);
            }
        }

        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            Lists[I].Sort(ComparePatrolFields);
        }

        Msg.MsgID = 4384;
        Msg.Param[0] = FArmyHS;
        for (int I = 1; I < HexDirectionSlotCount; I++)
        {
            NewList = new DoubleItemList<AoWMapField>();
            J = 0;
            while (NewList.Count != 30 && J != Lists[I].Count)
            {
                MapF = Lists[I].List[J].A;
                if (MapF.GetPlayer() == -1)
                {
                    Msg.Result = 0;
                    MapF.SendMapFieldMsg(ref Msg);
                    if (Msg.Result.Int == 0)
                    {
                        var item = Lists[I][J];
                        NewList.Add(item);
                    }
                }

                J += 1;
            }

            Lists[I]?.Dispose();
            Lists[I] = NewList;
        }

        MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Group.FPatrolFrom.Packed);
        if (MapF.GetPlayer() == -1)
        {
            Lists[0].AddAB(MapF, 0);
        }

        if (Group.FPatrolTo.X != -1)
        {
            LastHeading = HexGeometry.dHXtoHT(Group.FPatrolFrom.X, Group.FPatrolFrom.Y, Group.FPatrolTo.X, Group.FPatrolTo.Y);
        }
        else
        {
            LastHeading = 0;
        }

        PreviousHeading = Group.FPreviousHeading;
        Choices = new ByteList();
        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            if (Lists[I].Count != 0 && LastHeading != I && I != PreviousHeading)
            {
                if (PreviousHeading == 0 || PatrolHeadingDistance(I, LastHeading) > 1 || PatrolHeadingDistance(I, PreviousHeading) > 1)
                {
                    Choices.Add((byte)I);
                }
            }
        }

        if (Choices.Count == 0)
        {
            for (int I = 0; I < HexDirectionSlotCount; I++)
            {
                if (Lists[I].Count != 0 && LastHeading != I)
                {
                    Choices.Add((byte)I);
                }
            }
        }

        if (Choices.Count != 0)
        {
            Group.FPreviousHeading = (sbyte)LastHeading;
            Heading = Choices[AoWHSMap.Current.Random(Choices.Count)];
            SelectedList = Lists[Heading];
            var random = AoWHSMap.Current.Random(SelectedList.Count);
            MapF = SelectedList.List[random].A;
            var xyl = MapF.GetXYL();
            Group.FPatrolTo.Packed = xyl;
            Result = true;
        }
        else
        {
            Result = false;
        }

        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            Lists[I]?.Dispose();
        }

        Choices?.Dispose();
        Settings.Release();
        return Result;
    }

    private static int ComparePatrolFields(in DoubleItem<AoWMapField> A, in DoubleItem<AoWMapField> B)
    {
        return B.B - A.B;
    }

    private static int PatrolHeadingDistance(int A, int B)
    {
        int num = Math.Abs(B - A);
        if (num > 3)
        {
            num = 6 - num;
        }
        return num;
    }
}
