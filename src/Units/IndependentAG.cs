// Translated from IndependentAG.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Combat;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups
{

    public partial class GuardAG : AIGroup, IAIGroupClass
    {
        // IndependentAG.pas:144
        public new const int ClassID = 136275;

        // IndependentAG.pas:137
        public GuardAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // IndependentAG.pas:150
        public override EClass GroupControlClass() => EClass.Of<GuardAGC>();

        // IndependentAG.pas:147
        public static new GroupBehavior Behavior => GroupBehavior.Guard;

    }

    public partial class GuardAGC : AIGroupControl
    {
        public GuardAGC(ECustomNode owner) : base(owner)
        { }

    }

    // guard location
    public partial class GuardAreaAG : AIGroup, IAIGroupClass
    {
        // IndependentAG.pas:161
        public new const int ClassID = 136273;

        public XYL FGuardLocation;

        // IndependentAG.pas:170
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FGuardLocation.Packed, 50);
        }

        // IndependentAG.pas:153
        public GuardAreaAG(ECustomNode owner) : base(owner)
        {
            FGuardLocation.Packed = 16777215;
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // IndependentAG.pas:167
        public override EClass GroupControlClass() => EClass.Of<GuardAreaAGC>();

        // IndependentAG.pas:164
        public static new GroupBehavior Behavior => GroupBehavior.GuardArea;

    }

    public partial class GuardAreaAGC : AIGroupControl
    {
        public GuardAreaAGC(ECustomNode owner) : base(owner)
        { }

        // IndependentAG.pas:189
        public override void MoveExecuterDone(object Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == AIMoveExecuter.Status.OutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Invalidated)
                        {
                            Done();
                        }
                    }

                    break;
                case 2:
                    if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Failed)
                    {
                        ((GuardAreaAG)FGroup).FGuardLocation.Packed = 16777215;
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
        public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            // { Original also accepts crDefeat when the subsequent efficiency tests pass. }
            var execute = FCombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
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
            AoWMapField MapF;
            AIGroupTargetFinder Finder;
            MoveControlSettings Settings;
            switch (FState)
            {
                case 0:
                    if (((GuardAreaAG)FGroup).FGuardLocation.X == -1)
                    {
                        ((GuardAreaAG)FGroup).FGuardLocation = FLocation;
                    }

                    Finder = new AIGroupTargetFinder(null);
                    Finder.Setup(this);
                    {
                        Finder.FSelector.FSourceLocation = ((GuardAreaAG)FGroup).FGuardLocation;
                        Finder.FSelector.FTargetInfo.Mode = AITargetMode.GuardArea;
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
                        if (FArmy.Moves(FSelection) != FArmy.MovePoints(FSelection) || ((GuardAreaAG)FGroup).FGuardLocation.Packed != FLocation.Packed)
                        {
                            Settings = new MoveControlSettings(null);
                            Settings.FPlayer = FArmy.FPlayer;
                            Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                            Settings.SetSourcePos(FArmy.FX, FArmy.FY, FArmy.FL);
                            Settings.FMovePointsMax = FArmy.MovePoints(FSelection);
                            Settings.FArmyHS = FArmyHS;
                            Settings.FSelection = FSelection;
                            Settings.FMovePointTable = FMovePointTable;
                            AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
                            for (int I = FTargets.Count - 1; I >= 0; I--)
                            {
                                if (Settings.GetMovePointsXYL(FTargets.GetTargets(I).FLocation.Packed) == 16777215)
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
                    if (((GuardAreaAG)FGroup).FGuardLocation.Packed != FLocation.Packed)
                    {
                        MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(((GuardAreaAG)FGroup).FGuardLocation.Packed);
                        if (MapF.GetPlayer() == -1 || MapF.GetPlayer() == FGroup.FPlayer)
                        {
                            SetupME();
                            var guardAreaAG = (GuardAreaAG)FGroup;
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
    public partial class PatrolAG : AIGroup, IAIGroupClass
    {
        // IndependentAG.pas:295
        public new const int ClassID = 136274;

        public XYL FPatrolFrom;
        public XYL FPatrolTo;
        public sbyte FPreviousHeading;
        public int FPatrolStrength;

        // IndependentAG.pas:304
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FPatrolFrom.Packed, 40);
            Stream.rwInteger(ref FPatrolTo.Packed, 41);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPreviousHeading), 42);
            Stream.rwInteger(ref FPatrolStrength, 43);
        }

        // IndependentAG.pas:285
        public PatrolAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
            FPatrolFrom.Packed = 16777215;
            FPatrolTo.Packed = 16777215;
            FPreviousHeading = 0;
        }

        // IndependentAG.pas:301
        public override EClass GroupControlClass() => EClass.Of<PatrolAGC>();

        // IndependentAG.pas:298
        public static new GroupBehavior Behavior => GroupBehavior.Patrol;

    }

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

    public partial class RefugeAG : AIGroup, IAIGroupClass
    {
        // IndependentAG.pas:551
        public new const int ClassID = 136289;

        // IndependentAG.pas:544
        public RefugeAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // IndependentAG.pas:557
        public override EClass GroupControlClass() => EClass.Of<RefugeAGC>();

        // IndependentAG.pas:554
        public static new GroupBehavior Behavior => GroupBehavior.Refuge;

    }

    public partial class RefugeAGC : AIGroupControl
    {
        public RefugeAGC(ECustomNode owner) : base(owner)
        { }

    }

    public partial class RaidAG : AIGroup, IAIGroupClass
    {
        // IndependentAG.pas:567
        public new const int ClassID = 136293;

        // IndependentAG.pas:560
        public RaidAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // IndependentAG.pas:573
        public override EClass GroupControlClass() => EClass.Of<RaidAGC>();

        // IndependentAG.pas:570
        public static new GroupBehavior Behavior => GroupBehavior.Raid;

    }

    public partial class RaidAGC : AIGroupControl
    {
        public RaidAGC(ECustomNode owner) : base(owner)
        { }

        // IndependentAG.pas:589
        public override void MoveExecuterDone(object Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == AIMoveExecuter.Status.OutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Invalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // IndependentAG.pas:576
        public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            // { Original also accepts crDefeat when the efficiency test passes. }
            var execute = FCombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
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
                    var finder = new AIGroupTargetFinder(null!);
                    finder.Setup(this);
                    finder.FSelector.FMovePointsMax = 100;
                    finder.FSelector.FTargetInfo.Mode = AITargetMode.Raid;
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
                    var raze = new AIGroupRazeControl(null!);
                    raze.Setup(this);
                    // loads the set byte 04 from in the original
                    raze.FMode = Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.Always);
                    ActivatePlugin(raze);
                    raze.Release();
                    FState = 1;
                    break;
            }
        }
    }

    public partial class ScoutAG : AIGroup, IAIGroupClass
    {
        // IndependentAG.pas:650
        public new const int ClassID = 136277;

        // IndependentAG.pas:643
        public ScoutAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // IndependentAG.pas:656
        public override EClass GroupControlClass() => EClass.Of<ScoutAGC>();

        // IndependentAG.pas:653
        public static new GroupBehavior Behavior => GroupBehavior.Scout;

    }

    public partial class ScoutAGC : AIGroupControl
    {
        public ScoutAGC(ECustomNode owner) : base(owner)
        { }

        // IndependentAG.pas:671
        public override void MoveExecuterDone(object Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == AIMoveExecuter.Status.OutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Invalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // IndependentAG.pas:659
        public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var execute = FCombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(execute))
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
            AIGroupTargetFinder Finder;
            AIGroupWanderControl Wander;
            switch (FState)
            {
                case 0:
                    bool condition = FGroup.FPlayer != 0;
                    if (!condition)
                    {
                        var independentAIMode = AoWHSMap.Current.GetIndependentAIMode();
                        condition = AoWHSMap.Current.Random(3) < (int)independentAIMode;
                    }

                    if (condition)
                    {
                        Finder = new AIGroupTargetFinder(null);
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
                    Wander = new AIGroupWanderControl(null);
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
}

namespace Aow.Game.Ai.Groups
{

    [LegacyObject]
    public partial class IndependentRegistrator
    {
        // IndependentAG.pas:136
        public static void MapLoaded(HSMap Sender)
        {
            if (Sender == AoWHSMap.Current)
            {
                AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<GuardAG>(), AIGroupClass.Of<GuardAreaAG>(), AIGroupClass.Of<PatrolAG>(), AIGroupClass.Of<RefugeAG>(), AIGroupClass.Of<RaidAG>(), AIGroupClass.Of<ScoutAG>()]);
            }
        }

        public IndependentRegistrator()
        {
            CaptureAddress.Assign(this);
        }

        internal static void SubscribeMapLoaded()
        {
            if (Magic.Abilities.GameTables.ValidateAoWEngine())
                Magic.Abilities.GameTables.AoWEngine.OnHSMLoaded += MapLoaded;
        }

        internal static void UnsubscribeMapLoaded()
        {
            if (Magic.Abilities.GameTables.AoWEngine != null)
                Magic.Abilities.GameTables.AoWEngine.OnHSMLoaded -= MapLoaded;
        }
    }
}
