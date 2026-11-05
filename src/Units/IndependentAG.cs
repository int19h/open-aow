// Translated from IndependentAG.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AGCPlugins;

namespace Aow.Units.IndependentAG
{
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TGuardAGC>();
            return Result;
        }

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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TGuardAreaAGC>();
            return Result;
        }

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
        public override void TargetFoundEvent(TAIGroupControlPlugin Sender, TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            // { Original also accepts crDefeat when the subsequent efficiency tests pass. }
            var call__1 = FCombatPredictor.Execute();
            if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(call__1))
            {
                var call__2 = FCombatPredictor.GetEfficiency();
                if (call__2 > 0)
                {
                    var call__3 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__3 + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }

        // IndependentAG.pas:205
        public override void ProcessStates()
        {
            int I = default;
            TAoWMapField MapF = default;
            TAIGroupTargetFinder Finder = default;
            TMoveControlSettings Settings = default;
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

                    var call__5 = FArmy.Moves(FSelection);
                    Finder.FSelector.FMovePointsMax = call__5;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Validate();
                    var call__6 = FTargets.Count;
                    if (call__6 != 0)
                    {
                        var call__7 = FArmy.Moves(FSelection);
                        var call__8 = FArmy.MovePoints(FSelection);
                        if (call__7 != call__8 || ((TGuardAreaAG)FGroup).FGuardLocation.XYL != FLocation.XYL)
                        {
                            Settings = new TMoveControlSettings(null);
                            Settings.FPlayer = FArmy.FPlayer;
                            Settings.FMode = Settings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmAI);
                            Settings.SetSourcePos(FArmy.FX, FArmy.FY, FArmy.FL);
                            var call__9 = FArmy.MovePoints(FSelection);
                            Settings.FMovePointsMax = call__9;
                            Settings.FArmyHS = FArmyHS;
                            Settings.FSelection = FSelection;
                            Settings.FMovePointTable = FMovePointTable;
                            AoWHSMap.FMoveControl.ProcessMove(Settings);
                            {
                                I = FTargets.Count - 1;
                                byte end__10 = 0;
                                for (; I >= end__10; I -= 1)
                                {
                                    var call__11 = FTargets.GetTargets(I);
                                    var call__12 = Settings.GetMovePointsXYL(call__11.FLocation.XYL);
                                    if (call__12 == 16777215)
                                    {
                                        FTargets.Delete(I);
                                    }
                                }
                            }

                            Settings.Release();
                        }

                        var call__13 = FTargets.Count;
                        if (call__13 != 0)
                        {
                            FTargets.SortOnPriority();
                            SetupME();
                            var call__14 = FTargets.GetTargets(0);
                            var call__15 = FTargets.GetTargets(0);
                            var call__16 = FTargets.GetTargets(0);
                            ExecuteME(call__16.FLocation.X, call__15.FLocation.Y, call__14.FLocation.L);
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
                        var call__17 = AoWHSMap.MapContainer.GetFieldXYL(((TGuardAreaAG)FGroup).FGuardLocation.XYL);
                        MapF = (TAoWMapField)call__17;
                        var call__18 = MapF.GetPlayer();
                        bool bool__19 = call__18 == -1;
                        if (!bool__19)
                        {
                            var call__20 = MapF.GetPlayer();
                            bool__19 = call__20 == FGroup.FPlayer;
                        }

                        if (bool__19)
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TPatrolAGC>();
            return Result;
        }

        // IndependentAG.pas:298
        public static new TGroupBehavior Behavior => TGroupBehavior.gbPatrol;

    }

    public partial class TPatrolAGC : TAIGroupControl
    {
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
        public override void TargetFoundEvent(TAIGroupControlPlugin Sender, TAIGroupControlTarget Target, ref bool SearchDone)
        {
            TAoWMapField MapF = default;
            var call__22 = AoWHSMap.MapContainer.GetFieldXYL(Target.FLocation.XYL);
            MapF = (TAoWMapField)call__22;
            var call__23 = MapF.GetPlayer();
            if (call__23 != FGroup.FPlayer)
            {
                SetupCP(Target);
                var call__24 = FCombatPredictor.Execute();
                if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(call__24))
                {
                    var call__25 = FCombatPredictor.GetSuperiority();
                    if (call__25 > 80)
                    {
                        var call__26 = FCombatPredictor.GetEfficiency();
                        if (call__26 > 50)
                        {
                            var call__27 = FCombatPredictor.GetEfficiency();
                            Target.FData.Priority = call__27 + Target.FData.Priority * 100;
                            base.TargetFoundEvent(Sender, Target, ref SearchDone);
                        }
                    }
                }
            }
        }

        // IndependentAG.pas:444
        public override void ProcessStates()
        {
            TAIGroupDefenseControl Defense = default;
            TAIRecruitmentGroupFinder Recruitment = default;
            TAIGroupRecruitControl Recruit = default;
            TAIGroupTargetFinder Finder = default;
            TMoveControlSettings Settings = default;
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
                    bool bool__28 = ((TPatrolAG)FGroup).FPatrolTo.X == -1;
                    if (!bool__28)
                    {
                        bool bool__29 = ((TPatrolAG)FGroup).FPatrolTo.L == FLocation.L;
                        if (bool__29)
                        {
                            var call__30 = dHXtoRad(((TPatrolAG)FGroup).FPatrolTo.X, ((TPatrolAG)FGroup).FPatrolTo.Y, FLocation.X, FLocation.Y);
                            bool__29 = call__30 <= 2;
                        }

                        bool__28 = bool__29;
                    }

                    if (bool__28)
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
                    var call__31 = FRecruitmentGroups.Count;
                    if (call__31 != 0 && FStrengthMax < ((TPatrolAG)FGroup).FPatrolStrength)
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
                    var call__32 = FArmy.Moves(FSelection);
                    Finder.FSelector.FMovePointsMax = call__32;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 5:
                    FTargets.Validate();
                    var call__33 = FTargets.Count;
                    if (call__33 != 0)
                    {
                        SetupME();
                        var call__34 = FTargets.GetTargets(0);
                        var call__35 = FTargets.GetTargets(0);
                        var call__36 = FTargets.GetTargets(0);
                        ExecuteME(call__36.FLocation.X, call__35.FLocation.Y, call__34.FLocation.L);
                        FTargets.Delete(0);
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 6:
                    var call__37 = AoWHSMap.Random(3);
                    if (call__37 != 0 && ((TPatrolAG)FGroup).FPatrolTo.X != -1)
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
            TDoubleItemList NewList = default;
            TPatrolAG Group = default;
            TMoveControlSettings Settings = default;
            TList Fields = default;
            FixedArray_Aow_Units_Engine_TDoubleItemList_7 Lists = default;
            TMapFieldMsg Msg = default;
            TDoubleItem Item = default;
            int I = default;
            int J = default;
            int Heading = default;
            int LastHeading = default;
            TAoWMapField MapF = default;
            TByteList Choices = default;
            TDoubleItemList SelectedList = default;
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
            {
                I = 0;
                byte end__38 = 6;
                for (; I <= end__38; I += 1)
                {
                    var call__39 = new TDoubleItemList();
                    Lists[I] = call__39;
                }
            }

            X = Group.FPatrolFrom.X;
            Y = Group.FPatrolFrom.Y;
            {
                I = Fields.Count - 1;
                byte end__40 = 0;
                for (; I >= end__40; I -= 1)
                {
                    // { Original samples every fourth processed field. }
                    if ((I & 3) == 3)
                    {
                        MapF = (TAoWMapField)Fields.List[I];
                        var call__41 = dHXtoHT(X, Y, MapF.Xhx, MapF.Yhx);
                        Heading = call__41;
                        var call__42 = Settings.GetMovePoints(MapF.Xhx, MapF.Yhx, MapF.Lhx);
                        Lists[Heading].AddAB(ObjectToInt(MapF), call__42);
                    }
                }
            }

            {
                I = 0;
                byte end__43 = 6;
                for (; I <= end__43; I += 1)
                {
                    Lists[I].Sort(IndependentAG.ComparePatrolFields);
                }
            }

            Msg.MsgID = 4384;
            Msg.Param[0] = FArmyHS;
            {
                I = 1;
                byte end__44 = 6;
                for (; I <= end__44; I += 1)
                {
                    NewList = new TDoubleItemList();
                    J = 0;
                    while (NewList.Count != 30 && J != Lists[I].Count)
                    {
                        MapF = ReferenceFromId<TAoWMapField>(Lists[I].List[J].A);
                        if (MapF.GetPlayer() == -1)
                        {
                            Msg.Result = 0;
                            MapF.SendMapFieldMsg(ref Msg);
                            if (Msg.Result.Value == 0)
                            {
                                var call__45 = Lists[I][J];
                                NewList.Add(call__45);
                            }
                        }

                        J += 1;
                    }

                    Lists[I].Free();
                    Lists[I] = NewList;
                }
            }

            var call__46 = AoWHSMap.MapContainer.GetFieldXYL(Group.FPatrolFrom.XYL);
            MapF = (TAoWMapField)call__46;
            if (MapF.GetPlayer() == -1)
            {
                Lists[0].AddAB(ObjectToInt(MapF), 0);
            }

            if (Group.FPatrolTo.X != -1)
            {
                var call__47 = dHXtoHT(Group.FPatrolFrom.X, Group.FPatrolFrom.Y, Group.FPatrolTo.X, Group.FPatrolTo.Y);
                LastHeading = call__47;
            }
            else
            {
                LastHeading = 0;
            }

            PreviousHeading = Group.FPreviousHeading;
            Choices = new TByteList();
            {
                I = 0;
                byte end__48 = 6;
                for (; I <= end__48; I += 1)
                {
                    if (Lists[I].Count != 0 && LastHeading != I && I != PreviousHeading)
                    {
                        bool bool__49 = PreviousHeading == 0;
                        if (!bool__49)
                        {
                            var call__50 = IndependentAG.PatrolHeadingDistance(I, LastHeading);
                            bool__49 = call__50 > 1;
                        }

                        bool bool__51 = bool__49;
                        if (!bool__51)
                        {
                            var call__52 = IndependentAG.PatrolHeadingDistance(I, PreviousHeading);
                            bool__51 = call__52 > 1;
                        }

                        if (bool__51)
                        {
                            Choices.Add((byte)I);
                        }
                    }
                }
            }

            if (Choices.Count == 0)
            {
                {
                    I = 0;
                    byte end__53 = 6;
                    for (; I <= end__53; I += 1)
                    {
                        if (Lists[I].Count != 0 && LastHeading != I)
                        {
                            Choices.Add((byte)I);
                        }
                    }
                }
            }

            if (Choices.Count != 0)
            {
                Group.FPreviousHeading = (sbyte)LastHeading;
                var call__54 = AoWHSMap.Random(Choices.Count);
                var call__55 = Choices[call__54];
                Heading = call__55;
                SelectedList = Lists[Heading];
                var call__56 = AoWHSMap.Random(SelectedList.Count);
                MapF = ReferenceFromId<TAoWMapField>(SelectedList.List[call__56].A);
                var call__57 = MapF.GetXYL();
                Group.FPatrolTo.XYL = call__57;
                Result = true;
            }
            else
            {
                Result = false;
            }

            {
                I = 0;
                byte end__58 = 6;
                for (; I <= end__58; I += 1)
                {
                    Lists[I].Free();
                }
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TRefugeAGC>();
            return Result;
        }

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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TRaidAGC>();
            return Result;
        }

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
        public override void TargetFoundEvent(TAIGroupControlPlugin Sender, TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            // { Original also accepts crDefeat when the efficiency test passes. }
            var call__59 = FCombatPredictor.Execute();
            if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(call__59))
            {
                var call__60 = FCombatPredictor.GetEfficiency();
                if (call__60 > 0)
                {
                    var call__61 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__61 / 10 + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TScoutAGC>();
            return Result;
        }

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
        public override void TargetFoundEvent(TAIGroupControlPlugin Sender, TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var call__62 = FCombatPredictor.Execute();
            if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crNoDefenders).Contains(call__62))
            {
                var call__63 = FCombatPredictor.GetEfficiency();
                if (call__63 > 50)
                {
                    var call__64 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__64 + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }

        // IndependentAG.pas:680
        public override void ProcessStates()
        {
            TAIGroupTargetFinder Finder = default;
            TAIGroupWanderControl Wander = default;
            switch (FState)
            {
                case 0:
                    bool bool__65 = FGroup.FPlayer != 0;
                    if (!bool__65)
                    {
                        var call__66 = AoWHSMap.GetIndependentAIMode();
                        var call__67 = AoWHSMap.Random(3);
                        bool__65 = call__67 < (int)call__66;
                    }

                    if (bool__65)
                    {
                        Finder = new TAIGroupTargetFinder(null);
                        Finder.Setup(this);
                        var call__68 = FGroup.GetMovePointsMin();
                        Finder.FSelector.FMovePointsMax = call__68;
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
                    var call__69 = FTargets.Count;
                    if (call__69 != 0)
                    {
                        FTargets.SortOnPriority();
                        SetupME();
                        var call__70 = FTargets.GetTargets(0);
                        var call__71 = FTargets.GetTargets(0);
                        var call__72 = FTargets.GetTargets(0);
                        ExecuteME(call__72.FLocation.X, call__71.FLocation.Y, call__70.FLocation.L);
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
        public static void MapLoaded(TObject Sender)
        {
            if (Sender == AoWHSMap)
            {
                AoWHSMap.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<TGuardAG>(), AIGroupClass.Of<TGuardAreaAG>(), AIGroupClass.Of<TPatrolAG>(), AIGroupClass.Of<TRefugeAG>(), AIGroupClass.Of<TRaidAG>(), AIGroupClass.Of<TScoutAG>()]);
            }
        }

    }

    public static partial class IndependentAG
    {
        // IndependentAG.pas:313
        public static int ComparePatrolFields(in TDoubleItem A, in TDoubleItem B)
        {
            int Result = default;
            Result = B.B - A.B;
            return Result;
        }

        // IndependentAG.pas:318
        public static int PatrolHeadingDistance(int A, int B)
        {
            int Result = default;
            Result = Math.Abs(B - A);
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
                AoWEngine.OnHSMLoaded.Add(TRegistrator.MapLoaded);
            }
        }

        public static void Finalize()
        {
            if (AoWEngine != null)
            {
                AoWEngine.OnHSMLoaded.Remove(TRegistrator.MapLoaded);
            }
        }
    }
}
