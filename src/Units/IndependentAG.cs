// Translated from IndependentAG.pas.
using System.Runtime.CompilerServices;
using Aow.Rtl;
using Aow.Units.AGCPlugins;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.IndependentAG;

public partial class TGuardAG : TAIGroup, IAIGroupClass
{
    // IndependentAG.pas:144
    public new const int ClassID = 136275;

    // IndependentAG.pas:137
    public TGuardAG(TECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:150
    public override EClass GroupControlClass() => EClass.Of<TGuardAGC>();

    // IndependentAG.pas:147
    public static new TGroupBehavior Behavior => TGroupBehavior.gbGuard;

}

public partial class TGuardAGC : TAIGroupControl
{
    public TGuardAGC(TECustomNode owner) : base(owner)
    { }

}

// guard location
public partial class TGuardAreaAG : TAIGroup, IAIGroupClass
{
    // IndependentAG.pas:161
    public new const int ClassID = 136273;

    public TXYL FGuardLocation;

    // IndependentAG.pas:170
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FGuardLocation.XYL, 50);
    }

    // IndependentAG.pas:153
    public TGuardAreaAG(TECustomNode owner) : base(owner)
    {
        FGuardLocation.XYL = 16777215;
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:167
    public override EClass GroupControlClass() => EClass.Of<TGuardAreaAGC>();

    // IndependentAG.pas:164
    public static new TGroupBehavior Behavior => TGroupBehavior.gbGuardArea;

}

public partial class TGuardAreaAGC : TAIGroupControl
{
    public TGuardAreaAGC(TECustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:189
    public override void MoveExecuterDone(TObject Sender)
    {
        switch (FState)
        {
            case 1:
                if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesOutOfMovePoints)
                {
                    Done();
                }
                else
                {
                    if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesInvalidated)
                    {
                        Done();
                    }
                }

                break;
            case 2:
                if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesFailed)
                {
                    ((TGuardAreaAG)FGroup).FGuardLocation.XYL = 16777215;
                    Done();
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:176
    public override void TargetFoundEvent(TAIGroupTargetFinder Sender, TAIGroupControlTarget Target, ref bool SearchDone)
    {
        SetupCP(Target);
        // { Original also accepts crDefeat when the subsequent efficiency tests pass. }
        var execute = FCombatPredictor.Execute();
        if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    // IndependentAG.pas:205
    public override void ProcessStates()
    {
        TAoWMapField MapF;
        TAIGroupTargetFinder Finder;
        TMoveControlSettings Settings;
        switch (FState)
        {
            case 0:
                if (((TGuardAreaAG)FGroup).FGuardLocation.X == -1)
                {
                    ((TGuardAreaAG)FGroup).FGuardLocation = FLocation;
                }

                Finder = new TAIGroupTargetFinder(null);
                Finder.Setup(this);
                {
                    Finder.FSelector.FSourceLocation = ((TGuardAreaAG)FGroup).FGuardLocation;
                    Finder.FSelector.FTargetInfo.Mode = TAITargetMode.tmGuardArea;
                }

                var moves = FArmy.Moves(FSelection);
                Finder.FSelector.FMovePointsMax = moves;
                ActivatePlugin(Finder);
                Finder.Release();
                FState += 1;
                break;
            case 1:
                FTargets.Validate();
                if (FTargets.Count != 0)
                {
                    if (FArmy.Moves(FSelection) != FArmy.MovePoints(FSelection) || ((TGuardAreaAG)FGroup).FGuardLocation.XYL != FLocation.XYL)
                    {
                        Settings = new TMoveControlSettings(null);
                        Settings.FPlayer = FArmy.FPlayer;
                        Settings.FMode = Settings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmAI);
                        Settings.SetSourcePos(FArmy.FX, FArmy.FY, FArmy.FL);
                        Settings.FMovePointsMax = FArmy.MovePoints(FSelection);
                        Settings.FArmyHS = FArmyHS;
                        Settings.FSelection = FSelection;
                        Settings.FMovePointTable = FMovePointTable;
                        AoWHSMap.FMoveControl.ProcessMove(Settings);
                        for (int I = FTargets.Count - 1; I >= 0; I--)
                        {
                            if (Settings.GetMovePointsXYL(FTargets.GetTargets(I).FLocation.XYL) == 16777215)
                            {
                                FTargets.Delete(I);
                            }
                        }

                        Settings.Release();
                    }

                    if (FTargets.Count != 0)
                    {
                        FTargets.SortOnPriority();
                        SetupME();
                        var targetLocation = FTargets.GetTargets(0);
                        var target = FTargets.GetTargets(0);
                        ExecuteME(FTargets.GetTargets(0).FLocation.X, target.FLocation.Y, targetLocation.FLocation.L);
                        FTargets.Delete(0);
                    }
                    else
                    {
                        FState = 2;
                    }
                }
                else
                {
                    FState = 2;
                }

                break;
            case 2:
                if (((TGuardAreaAG)FGroup).FGuardLocation.XYL != FLocation.XYL)
                {
                    MapF = (TAoWMapField)AoWHSMap.MapContainer.GetFieldXYL(((TGuardAreaAG)FGroup).FGuardLocation.XYL);
                    if (MapF.GetPlayer() == -1 || MapF.GetPlayer() == FGroup.FPlayer)
                    {
                        SetupME();
                        var guardAreaAG = (TGuardAreaAG)FGroup;
                        ExecuteME(guardAreaAG.FGuardLocation.X, guardAreaAG.FGuardLocation.Y, guardAreaAG.FGuardLocation.L);
                    }
                    else
                    {
                        Done();
                    }
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:280
    public override void ProcessStatesLooped()
    {
        ProcessStates();
    }
}

// from, to, previous heading, strength
public partial class TPatrolAG : TAIGroup, IAIGroupClass
{
    // IndependentAG.pas:295
    public new const int ClassID = 136274;

    public TXYL FPatrolFrom;
    public TXYL FPatrolTo;
    public sbyte FPreviousHeading;
    public int FPatrolStrength;

    // IndependentAG.pas:304
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FPatrolFrom.XYL, 40);
        Stream.rwInteger(ref FPatrolTo.XYL, 41);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPreviousHeading), 42);
        Stream.rwInteger(ref FPatrolStrength, 43);
    }

    // IndependentAG.pas:285
    public TPatrolAG(TECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
        FPatrolFrom.XYL = 16777215;
        FPatrolTo.XYL = 16777215;
        FPreviousHeading = 0;
    }

    // IndependentAG.pas:301
    public override EClass GroupControlClass() => EClass.Of<TPatrolAGC>();

    // IndependentAG.pas:298
    public static new TGroupBehavior Behavior => TGroupBehavior.gbPatrol;

}

public partial class TPatrolAGC : TAIGroupControl
{
    // Heading 0 is the centre, 1 to 6 the hex sides (fixed-arrays.md, named constant).
    public const int HexDirectionSlotCount = 7;

    public TPatrolAGC(TECustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:415
    public override void MoveExecuterDone(TObject Sender)
    {
        switch (FMoveExecuter.FStatus)
        {
            case TAIMoveExecuterStatus.mesDone:
                Done();
                break;
            case TAIMoveExecuterStatus.mesFailed:
                if (FState == 7)
                {
                    ((TPatrolAG)FGroup).FPatrolTo.XYL = 16777215;
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:426
    public override void TargetFoundEvent(TAIGroupTargetFinder Sender, TAIGroupControlTarget Target, ref bool SearchDone)
    {
        TAoWMapField MapF;
        MapF = (TAoWMapField)AoWHSMap.MapContainer.GetFieldXYL(Target.FLocation.XYL);
        if (MapF.GetPlayer() != FGroup.FPlayer)
        {
            SetupCP(Target);
            var execute = FCombatPredictor.Execute();
            if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(execute))
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
        TAIGroupDefenseControl Defense;
        TAIRecruitmentGroupFinder Recruitment;
        TAIGroupRecruitControl Recruit;
        TAIGroupTargetFinder Finder;
        TMoveControlSettings Settings;
        switch (FState)
        {
            case 0:
                if (((TPatrolAG)FGroup).FPatrolStrength == 0)
                {
                    ((TPatrolAG)FGroup).FPatrolStrength = FStrengthMax;
                }

                Defense = new TAIGroupDefenseControl(null);
                Defense.FDefendMode = TGroupDefendMode.gdmWait;
                ActivatePlugin(Defense);
                Defense.Release();
                FState = 1;
                break;
            case 1:
                if (((TPatrolAG)FGroup).FPatrolTo.X == -1 || ((TPatrolAG)FGroup).FPatrolTo.L == FLocation.L && dHXtoRad(((TPatrolAG)FGroup).FPatrolTo.X, ((TPatrolAG)FGroup).FPatrolTo.Y, FLocation.X, FLocation.Y) <= 2)
                {
                    ResetPatrolToLocation();
                }

                FState += 1;
                break;
            case 2:
                if (FStrengthMax < ((TPatrolAG)FGroup).FPatrolStrength)
                {
                    FState += 1;
                    Recruitment = new TAIRecruitmentGroupFinder(null);
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
                if (FRecruitmentGroups.Count != 0 && FStrengthMax < ((TPatrolAG)FGroup).FPatrolStrength)
                {
                    Recruit = new TAIGroupRecruitControl(null);
                    Recruit.Reset();
                    Recruit.FRecruitStrength = ((TPatrolAG)FGroup).FPatrolStrength - FStrengthMax;
                    Recruit.FMode = TAIGroupRecruitControlMode.rcmAutoRecruit;
                    ActivatePlugin(Recruit);
                    Recruit.Release();
                }

                FState += 1;
                break;
            case 4:
                Finder = new TAIGroupTargetFinder(null);
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
                if (AoWHSMap.Random(3) != 0 && ((TPatrolAG)FGroup).FPatrolTo.X != -1)
                {
                    FState += 1;
                    SetupME();
                    ExecuteME(((TPatrolAG)FGroup).FPatrolTo.X, ((TPatrolAG)FGroup).FPatrolTo.Y, ((TPatrolAG)FGroup).FPatrolTo.L);
                }
                else
                {
                    Done();
                }

                break;
            case 7:
                Settings = new TMoveControlSettings(null);
                Settings.FPlayer = FGroup.FPlayer;
                Settings.FMode = Settings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmAI);
                Settings.SetSourcePos(FLocation.X, FLocation.Y, FLocation.L);
                Settings.SetDestinationPos(((TPatrolAG)FGroup).FPatrolFrom.X, ((TPatrolAG)FGroup).FPatrolFrom.Y, ((TPatrolAG)FGroup).FPatrolFrom.L);
                Settings.FMovePointsMax = 48;
                Settings.FMode = Settings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmIgnoreArmies);
                Settings.FArmyHS = FArmyHS;
                Settings.FSelection = FSelection;
                Settings.FMovePointTable = FMovePointTable;
                AoWHSMap.FMoveControl.ProcessMove(Settings);
                if (Settings.FPath.Count == 0)
                {
                    ((TPatrolAG)FGroup).FPatrolFrom.XYL = 16777215;
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
        TDoubleItemList<TAoWMapField> NewList = default;
        TPatrolAG Group = default;
        TMoveControlSettings Settings = default;
        TList Fields = default;
        var Lists = new TDoubleItemList<TAoWMapField>[HexDirectionSlotCount];
        TMapFieldMsg Msg = default;
        int J = default;
        int Heading = default;
        int LastHeading = default;
        TAoWMapField MapF = default;
        TByteList Choices = default;
        TDoubleItemList<TAoWMapField> SelectedList = default;
        Group = (TPatrolAG)FGroup;
        if (Group.FPatrolFrom.X == -1)
        {
            Group.FPatrolFrom = FLocation;
        }

        Settings = new TMoveControlSettings(null);
        Settings.FPlayer = Group.FPlayer;
        Settings.FMode = Settings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmAI, TMoveModeElement.mmIgnoreArmies);
        Settings.SetSourcePos(Group.FPatrolFrom.X, Group.FPatrolFrom.Y, Group.FPatrolFrom.L);
        Settings.FMovePointsMax = 48;
        Settings.FMode = Settings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmLogProcessedMapFields);
        Settings.FArmyHS = FArmyHS;
        Settings.FSelection = FSelection;
        Settings.FMovePointTable = FMovePointTable;
        AoWHSMap.FMoveControl.ProcessMove(Settings);
        Fields = Settings.FProcessedMapFields;
        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            var doubleItemList = new TDoubleItemList<TAoWMapField>();
            Lists[I] = doubleItemList;
        }

        X = Group.FPatrolFrom.X;
        Y = Group.FPatrolFrom.Y;
        for (int I = Fields.Count - 1; I >= 0; I--)
        {
            // { Original samples every fourth processed field. }
            if ((I & 3) == 3)
            {
                MapF = (TAoWMapField)Fields.List[I];
                Heading = dHXtoHT(X, Y, MapF.Xhx, MapF.Yhx);
                var movePoints = Settings.GetMovePoints(MapF.Xhx, MapF.Yhx, MapF.Lhx);
                Lists[Heading].AddAB(MapF, movePoints);
            }
        }

        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            Lists[I].Sort(IndependentAGUnit.ComparePatrolFields);
        }

        Msg.MsgID = 4384;
        Msg.Param[0] = FArmyHS;
        for (int I = 1; I < HexDirectionSlotCount; I++)
        {
            NewList = new TDoubleItemList<TAoWMapField>();
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

            Lists[I].Free();
            Lists[I] = NewList;
        }

        MapF = (TAoWMapField)AoWHSMap.MapContainer.GetFieldXYL(Group.FPatrolFrom.XYL);
        if (MapF.GetPlayer() == -1)
        {
            Lists[0].AddAB(MapF, 0);
        }

        if (Group.FPatrolTo.X != -1)
        {
            LastHeading = dHXtoHT(Group.FPatrolFrom.X, Group.FPatrolFrom.Y, Group.FPatrolTo.X, Group.FPatrolTo.Y);
        }
        else
        {
            LastHeading = 0;
        }

        PreviousHeading = Group.FPreviousHeading;
        Choices = new TByteList();
        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            if (Lists[I].Count != 0 && LastHeading != I && I != PreviousHeading)
            {
                if (PreviousHeading == 0 || IndependentAGUnit.PatrolHeadingDistance(I, LastHeading) > 1 || IndependentAGUnit.PatrolHeadingDistance(I, PreviousHeading) > 1)
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
            Heading = Choices[AoWHSMap.Random(Choices.Count)];
            SelectedList = Lists[Heading];
            var random = AoWHSMap.Random(SelectedList.Count);
            MapF = SelectedList.List[random].A;
            var xyl = MapF.GetXYL();
            Group.FPatrolTo.XYL = xyl;
            Result = true;
        }
        else
        {
            Result = false;
        }

        for (int I = 0; I < HexDirectionSlotCount; I++)
        {
            Lists[I].Free();
        }

        Choices.Free();
        Settings.Release();
        return Result;
    }
}

public partial class TRefugeAG : TAIGroup, IAIGroupClass
{
    // IndependentAG.pas:551
    public new const int ClassID = 136289;

    // IndependentAG.pas:544
    public TRefugeAG(TECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:557
    public override EClass GroupControlClass() => EClass.Of<TRefugeAGC>();

    // IndependentAG.pas:554
    public static new TGroupBehavior Behavior => TGroupBehavior.gbRefuge;

}

public partial class TRefugeAGC : TAIGroupControl
{
    public TRefugeAGC(TECustomNode owner) : base(owner)
    { }

}

public partial class TRaidAG : TAIGroup, IAIGroupClass
{
    // IndependentAG.pas:567
    public new const int ClassID = 136293;

    // IndependentAG.pas:560
    public TRaidAG(TECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:573
    public override EClass GroupControlClass() => EClass.Of<TRaidAGC>();

    // IndependentAG.pas:570
    public static new TGroupBehavior Behavior => TGroupBehavior.gbRaid;

}

public partial class TRaidAGC : TAIGroupControl
{
    public TRaidAGC(TECustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:589
    public override void MoveExecuterDone(TObject Sender)
    {
        switch (FState)
        {
            case 1:
                if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesOutOfMovePoints)
                {
                    Done();
                }
                else
                {
                    if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesInvalidated)
                    {
                        Done();
                    }
                }

                break;
        }
    }

    // IndependentAG.pas:576
    public override void TargetFoundEvent(TAIGroupTargetFinder Sender, TAIGroupControlTarget Target, ref bool SearchDone)
    {
        SetupCP(Target);
        // { Original also accepts crDefeat when the efficiency test passes. }
        var execute = FCombatPredictor.Execute();
        if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency / 10 + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    public override void ProcessStates()
    {
        switch (FState)
        {
            case 0:
                var finder = new TAIGroupTargetFinder(null!);
                finder.Setup(this);
                finder.FSelector.FMovePointsMax = 100;
                finder.FSelector.FTargetInfo.Mode = TAITargetMode.tmRaid;
                ActivatePlugin(finder);
                finder.Release();
                FState++;
                break;
            case 1:
                FTargets.Validate();
                if (FTargets.Count != 0)
                {
                    FTargets.SortOnPriority();
                    SetupME();
                    var target = FTargets.GetTargets(0);
                    ExecuteME(target.FLocation.X, target.FLocation.Y, target.FLocation.L);
                    FTargets.Delete(0);
                    FState++;
                }
                else Done();
                break;
            case 2:
                var raze = new TAIGroupRazeControl(null!);
                raze.Setup(this);
                // loads the set byte 04 from in the original
                raze.FMode = TSet<TAIGroupRazeControlModeElement>.Of(TAIGroupRazeControlModeElement.rcmAlways);
                ActivatePlugin(raze);
                raze.Release();
                FState = 1;
                break;
        }
    }
}

public partial class TScoutAG : TAIGroup, IAIGroupClass
{
    // IndependentAG.pas:650
    public new const int ClassID = 136277;

    // IndependentAG.pas:643
    public TScoutAG(TECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // IndependentAG.pas:656
    public override EClass GroupControlClass() => EClass.Of<TScoutAGC>();

    // IndependentAG.pas:653
    public static new TGroupBehavior Behavior => TGroupBehavior.gbScout;

}

public partial class TScoutAGC : TAIGroupControl
{
    public TScoutAGC(TECustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:671
    public override void MoveExecuterDone(TObject Sender)
    {
        switch (FState)
        {
            case 1:
                if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesOutOfMovePoints)
                {
                    Done();
                }
                else
                {
                    if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesInvalidated)
                    {
                        Done();
                    }
                }

                break;
        }
    }

    // IndependentAG.pas:659
    public override void TargetFoundEvent(TAIGroupTargetFinder Sender, TAIGroupControlTarget Target, ref bool SearchDone)
    {
        SetupCP(Target);
        var execute = FCombatPredictor.Execute();
        if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crNoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 50)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    // IndependentAG.pas:680
    public override void ProcessStates()
    {
        TAIGroupTargetFinder Finder;
        TAIGroupWanderControl Wander;
        switch (FState)
        {
            case 0:
                bool condition = FGroup.FPlayer != 0;
                if (!condition)
                {
                    var independentAIMode = AoWHSMap.GetIndependentAIMode();
                    condition = AoWHSMap.Random(3) < (int)independentAIMode;
                }

                if (condition)
                {
                    Finder = new TAIGroupTargetFinder(null);
                    Finder.Setup(this);
                    var movePointsMin = FGroup.GetMovePointsMin();
                    Finder.FSelector.FMovePointsMax = movePointsMin;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                }
                else
                {
                    Done();
                }

                break;
            case 1:
                FTargets.Validate();
                if (FTargets.Count != 0)
                {
                    FTargets.SortOnPriority();
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
            case 2:
                Wander = new TAIGroupWanderControl(null);
                Wander.Setup(this);
                Wander.FMovePointsMax = 125;
                ActivatePlugin(Wander);
                Wander.Release();
                FState += 1;
                break;
            case 3:
                Done();
                break;
        }
    }
}

public partial class TRegistrator : TObject
{
    // IndependentAG.pas:136
    public static void MapLoaded(THSMap Sender)
    {
        if (Sender == AoWHSMap)
        {
            AoWHSMap.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<TGuardAG>(), AIGroupClass.Of<TGuardAreaAG>(), AIGroupClass.Of<TPatrolAG>(), AIGroupClass.Of<TRefugeAG>(), AIGroupClass.Of<TRaidAG>(), AIGroupClass.Of<TScoutAG>()]);
        }
    }

}

public static partial class IndependentAGUnit
{
    // IndependentAG.pas:313
    public static int ComparePatrolFields(in TDoubleItem<TAoWMapField> A, in TDoubleItem<TAoWMapField> B) => B.B - A.B;

    // IndependentAG.pas:318
    public static int PatrolHeadingDistance(int A, int B)
    {
        int Result = Math.Abs(B - A);
        if (Result > 3)
        {
            Result = 6 - Result;
        }

        return Result;
    }

    public static void Initialize()
    {
        if (ValidateAoWEngine())
        {
            AoWEngine.RegisterEClasses([EClass.Of<TGuardAG>(), EClass.Of<TGuardAreaAG>(), EClass.Of<TPatrolAG>(), EClass.Of<TRefugeAG>(), EClass.Of<TRaidAG>(), EClass.Of<TScoutAG>()]);
            AoWEngine.OnHSMLoaded += TRegistrator.MapLoaded;
        }
    }

    public static void Finalize()
    {
        if (AoWEngine != null)
        {
            AoWEngine.OnHSMLoaded -= TRegistrator.MapLoaded;
        }
    }
}
