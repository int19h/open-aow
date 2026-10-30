// Translated from IndependentAG.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AGCPlugins;
using static Aow.Units.AGCPlugins.AGCPlugins;

namespace Aow.Units.IndependentAG
{
    // { TGuardAG (instance size 80, VMT 557D7E5C) }
    public partial class TGuardAG : Aow.Units.AoWE.TAIGroup, Aow.Units.AoWE.IAIGroupClass
    {
        // IndependentAG.pas:144
        public new const int ClassID = 136275;

        // IndependentAG.pas:137
        public TGuardAG(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // IndependentAG.pas:150
        public override Aow.Units.Engine.EClass GroupControlClass()
        {
            Aow.Units.Engine.EClass Result = default;
            Result = Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TGuardAGC>();
            return Result;
        }

        // IndependentAG.pas:147
        public static new Aow.Units.AoWE.TGroupBehavior Behavior => Aow.Units.AoWE.TGroupBehavior.gbGuard;

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // IndependentAG.pas:144
            public override int ClassID() => Aow.Units.IndependentAG.TGuardAG.ClassID;

            // IndependentAG.pas:29
            public new Aow.Units.IndependentAG.TGuardAG Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IndependentAG.TGuardAG)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IndependentAG.TGuardAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TGuardAGC (instance size 368, VMT 557D7FB0) }
    public partial class TGuardAGC : Aow.Units.AoWE.TAIGroupControl
    {
        public TGuardAGC(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

    }

    // { TGuardAreaAG (instance size 84, VMT 557D80A8): $50 guard location. }
    public partial class TGuardAreaAG : Aow.Units.AoWE.TAIGroup, Aow.Units.AoWE.IAIGroupClass
    {
        // IndependentAG.pas:161
        public new const int ClassID = 136273;

        public Aow.Units.HSEngine.TXYL FGuardLocation;
        // IndependentAG.pas:170
        // { $50 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FGuardLocation.XYL, 50);
        }

        // IndependentAG.pas:153
        public TGuardAreaAG(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FGuardLocation.XYL = 16777215;
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // IndependentAG.pas:167
        public override Aow.Units.Engine.EClass GroupControlClass()
        {
            Aow.Units.Engine.EClass Result = default;
            Result = Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TGuardAreaAGC>();
            return Result;
        }

        // IndependentAG.pas:164
        public static new Aow.Units.AoWE.TGroupBehavior Behavior => Aow.Units.AoWE.TGroupBehavior.gbGuardArea;

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // IndependentAG.pas:161
            public override int ClassID() => Aow.Units.IndependentAG.TGuardAreaAG.ClassID;

            // IndependentAG.pas:45
            public new Aow.Units.IndependentAG.TGuardAreaAG Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IndependentAG.TGuardAreaAG)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IndependentAG.TGuardAreaAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TGuardAreaAGC (instance size 368, VMT 557D8204) }
    public partial class TGuardAreaAGC : Aow.Units.AoWE.TAIGroupControl
    {
        public TGuardAreaAGC(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // IndependentAG.pas:189
        public override void MoveExecuterDone(Aow.Rtl.TObject Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesOutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesInvalidated)
                        {
                            Done();
                        }
                    }

                    break;
                case 2:
                    if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesFailed)
                    {
                        ((Aow.Units.IndependentAG.TGuardAreaAG)FGroup).FGuardLocation.XYL = 16777215;
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
        public override void TargetFoundEvent(Aow.Units.AoWE.TAIGroupControlPlugin Sender, Aow.Units.AoWE.TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            // { Original also accepts crDefeat when the subsequent efficiency tests pass. }
            var call__1 = FCombatPredictor.Execute();
            if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crDefeat, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(call__1))
            {
                var call__2 = FCombatPredictor.GetEfficiency();
                if (call__2 > 0)
                {
                    var call__3 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__3 + ((Target.FData.Priority * 100) - (Target.FMovePoints * 5));
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }

        // IndependentAG.pas:205
        public override void ProcessStates()
        {
            int I = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TMoveControlSettings Settings = default;
            switch (FState)
            {
                case 0:
                    if (((Aow.Units.IndependentAG.TGuardAreaAG)FGroup).FGuardLocation.X == -1)
                    {
                        ((Aow.Units.IndependentAG.TGuardAreaAG)FGroup).FGuardLocation = FLocation;
                    }

                    Finder = new Aow.Units.AoWE.TAIGroupTargetFinder(null);
                    Finder.Setup(this);
                {
                    Finder.FSelector.FSourceLocation = ((Aow.Units.IndependentAG.TGuardAreaAG)FGroup).FGuardLocation;
                    Finder.FSelector.FTargetInfo.Mode = Aow.Units.AoWE.TAITargetMode.tmGuardArea;
                }

                    var call__5 = FArmy.Moves(unchecked((byte)FSelection));
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
                        var call__7 = FArmy.Moves(unchecked((byte)FSelection));
                        var call__8 = FArmy.MovePoints(unchecked((byte)FSelection));
                        if ((call__7 != call__8) || (((Aow.Units.IndependentAG.TGuardAreaAG)FGroup).FGuardLocation.XYL != FLocation.XYL))
                        {
                            Settings = new Aow.Units.AoWE.TMoveControlSettings(null);
                            Settings.FPlayer = unchecked((sbyte)FArmy.FPlayer);
                            Settings.FMode = Settings.FMode + TSet<Aow.Units.AoWE.TMoveModeElement>.Of(Aow.Units.AoWE.TMoveModeElement.mmAI);
                            Settings.SetSourcePos(FArmy.FX, FArmy.FY, FArmy.FL);
                            var call__9 = FArmy.MovePoints(unchecked((byte)FSelection));
                            Settings.FMovePointsMax = call__9;
                            Settings.FArmyHS = FArmyHS;
                            Settings.FSelection = FSelection;
                            Settings.FMovePointTable = FMovePointTable;
                            Aow.Units.AoWE.AoWE.AoWHSMap.FMoveControl.ProcessMove(Settings);
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
                            ExecuteME(unchecked((sbyte)call__16.FLocation.X), unchecked((sbyte)call__15.FLocation.Y), unchecked((sbyte)call__14.FLocation.L));
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
                    if (((Aow.Units.IndependentAG.TGuardAreaAG)FGroup).FGuardLocation.XYL != FLocation.XYL)
                    {
                        var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(((Aow.Units.IndependentAG.TGuardAreaAG)FGroup).FGuardLocation.XYL);
                        MapF = (Aow.Units.AoWE.TAoWMapField)call__17;
                        var call__18 = MapF.GetPlayer();
                        bool bool__19 = (call__18 == -1);
                        if (!bool__19)
                        {
                            var call__20 = MapF.GetPlayer();
                            bool__19 = (call__20 == FGroup.FPlayer);
                        }

                        if (bool__19)
                        {
                            SetupME();
                            var guardAreaAG = (Aow.Units.IndependentAG.TGuardAreaAG)FGroup;
                            ExecuteME(unchecked((sbyte)guardAreaAG.FGuardLocation.X), unchecked((sbyte)guardAreaAG.FGuardLocation.Y), unchecked((sbyte)guardAreaAG.FGuardLocation.L));
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

    // { TPatrolAG (instance size 96, VMT 557D8304): $50 from, $54 to, $58 previous heading, $5C strength. }
    public partial class TPatrolAG : Aow.Units.AoWE.TAIGroup, Aow.Units.AoWE.IAIGroupClass
    {
        // IndependentAG.pas:295
        public new const int ClassID = 136274;

        public Aow.Units.HSEngine.TXYL FPatrolFrom;
        // { $50 }
        public Aow.Units.HSEngine.TXYL FPatrolTo;
        // { $54 }
        public sbyte FPreviousHeading;
        // { $58 }
        public Aow.Units.FixedArray_byte_3 F_PatrolAG_59;
        public int FPatrolStrength;
        // IndependentAG.pas:304
        // { $5C }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FPatrolFrom.XYL, 40);
            Stream.rwInteger(ref FPatrolTo.XYL, 41);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPreviousHeading), 42);
            Stream.rwInteger(ref FPatrolStrength, 43);
        }

        // IndependentAG.pas:285
        public TPatrolAG(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
            FPatrolFrom.XYL = 16777215;
            FPatrolTo.XYL = 16777215;
            FPreviousHeading = 0;
        }

        // IndependentAG.pas:301
        public override Aow.Units.Engine.EClass GroupControlClass()
        {
            Aow.Units.Engine.EClass Result = default;
            Result = Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TPatrolAGC>();
            return Result;
        }

        // IndependentAG.pas:298
        public static new Aow.Units.AoWE.TGroupBehavior Behavior => Aow.Units.AoWE.TGroupBehavior.gbPatrol;

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // IndependentAG.pas:295
            public override int ClassID() => Aow.Units.IndependentAG.TPatrolAG.ClassID;

            // IndependentAG.pas:69
            public new Aow.Units.IndependentAG.TPatrolAG Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IndependentAG.TPatrolAG)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IndependentAG.TPatrolAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TPatrolAGC (instance size 368, VMT 557D845C) }
    public partial class TPatrolAGC : Aow.Units.AoWE.TAIGroupControl
    {
        public TPatrolAGC(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // IndependentAG.pas:415
        public override void MoveExecuterDone(Aow.Rtl.TObject Sender)
        {
            switch (FMoveExecuter.FStatus)
            {
                case Aow.Units.AoWE.TAIMoveExecuterStatus.mesDone:
                    Done();
                    break;
                case Aow.Units.AoWE.TAIMoveExecuterStatus.mesFailed:
                    if (FState == 7)
                    {
                        ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.XYL = 16777215;
                    }
                    else
                    {
                        Done();
                    }

                    break;
            }
        }

        // IndependentAG.pas:426
        public override void TargetFoundEvent(Aow.Units.AoWE.TAIGroupControlPlugin Sender, Aow.Units.AoWE.TAIGroupControlTarget Target, ref bool SearchDone)
        {
            Aow.Units.AoWE.TAoWMapField MapF = default;
            var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(Target.FLocation.XYL);
            MapF = (Aow.Units.AoWE.TAoWMapField)call__22;
            var call__23 = MapF.GetPlayer();
            if (call__23 != FGroup.FPlayer)
            {
                SetupCP(Target);
                var call__24 = FCombatPredictor.Execute();
                if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crDefeat, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(call__24))
                {
                    var call__25 = FCombatPredictor.GetSuperiority();
                    if (call__25 > 80)
                    {
                        var call__26 = FCombatPredictor.GetEfficiency();
                        if (call__26 > 50)
                        {
                            var call__27 = FCombatPredictor.GetEfficiency();
                            Target.FData.Priority = call__27 + (Target.FData.Priority * 100);
                            base.TargetFoundEvent(Sender, Target, ref SearchDone);
                        }
                    }
                }
            }
        }

        // IndependentAG.pas:444
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupDefenseControl Defense = default;
            Aow.Units.AoWE.TAIRecruitmentGroupFinder Recruitment = default;
            Aow.Units.AoWE.TAIGroupRecruitControl Recruit = default;
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TMoveControlSettings Settings = default;
            switch (FState)
            {
                case 0:
                    if (((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolStrength == 0)
                    {
                        ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolStrength = FStrengthMax;
                    }

                    Defense = new Aow.Units.AoWE.TAIGroupDefenseControl(null);
                    Defense.FDefendMode = Aow.Units.AoWE.TGroupDefendMode.gdmWait;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState = 1;
                    break;
                case 1:
                    bool bool__28 = (((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.X == -1);
                    if (!bool__28)
                    {
                        bool bool__29 = (((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.L == FLocation.L);
                        if (bool__29)
                        {
                            var call__30 = Aow.Units.HSEngine.HSEngine.dHXtoRad(((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.X, ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.Y, FLocation.X, FLocation.Y);
                            bool__29 = (call__30 <= 2);
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
                    if (FStrengthMax < ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolStrength)
                    {
                        FState += 1;
                        Recruitment = new Aow.Units.AoWE.TAIRecruitmentGroupFinder(null);
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
                    if ((call__31 != 0) && (FStrengthMax < ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolStrength))
                    {
                        Recruit = new Aow.Units.AoWE.TAIGroupRecruitControl(null);
                        Recruit.Reset();
                        Recruit.FRecruitStrength = ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolStrength - FStrengthMax;
                        Recruit.FMode = Aow.Units.AoWE.TAIGroupRecruitControlMode.rcmAutoRecruit;
                        ActivatePlugin(Recruit);
                        Recruit.Release();
                    }

                    FState += 1;
                    break;
                case 4:
                    Finder = new Aow.Units.AoWE.TAIGroupTargetFinder(null);
                    Finder.Setup(this);
                    var call__32 = FArmy.Moves(unchecked((byte)FSelection));
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
                        ExecuteME(unchecked((sbyte)call__36.FLocation.X), unchecked((sbyte)call__35.FLocation.Y), unchecked((sbyte)call__34.FLocation.L));
                        FTargets.Delete(0);
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 6:
                    var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                    if ((call__37 != 0) && (((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.X != -1))
                    {
                        FState += 1;
                        SetupME();
                        ExecuteME(unchecked((sbyte)((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.X), unchecked((sbyte)((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.Y), unchecked((sbyte)((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolTo.L));
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 7:
                    Settings = new Aow.Units.AoWE.TMoveControlSettings(null);
                    Settings.FPlayer = unchecked((sbyte)FGroup.FPlayer);
                    Settings.FMode = Settings.FMode + TSet<Aow.Units.AoWE.TMoveModeElement>.Of(Aow.Units.AoWE.TMoveModeElement.mmAI);
                    Settings.SetSourcePos(FLocation.X, FLocation.Y, FLocation.L);
                    Settings.SetDestinationPos(((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolFrom.X, ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolFrom.Y, ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolFrom.L);
                    Settings.FMovePointsMax = 48;
                    Settings.FMode = Settings.FMode + TSet<Aow.Units.AoWE.TMoveModeElement>.Of(Aow.Units.AoWE.TMoveModeElement.mmIgnoreArmies);
                    Settings.FArmyHS = FArmyHS;
                    Settings.FSelection = FSelection;
                    Settings.FMovePointTable = FMovePointTable;
                    Aow.Units.AoWE.AoWE.AoWHSMap.FMoveControl.ProcessMove(Settings);
                    if (Settings.FPath.Count == 0)
                    {
                        ((Aow.Units.IndependentAG.TPatrolAG)FGroup).FPatrolFrom.XYL = 16777215;
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
            Aow.Units.Engine.TDoubleItemList NewList = default;
            Aow.Units.IndependentAG.TPatrolAG Group = default;
            Aow.Units.AoWE.TMoveControlSettings Settings = default;
            Aow.Rtl.TList Fields = default;
            Aow.Units.FixedArray_Aow_Units_Engine_TDoubleItemList_7 Lists = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            Aow.Units.Engine.TDoubleItem Item = default;
            int I = default;
            int J = default;
            int Heading = default;
            int LastHeading = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            Aow.Units.Engine.TByteList Choices = default;
            Aow.Units.Engine.TDoubleItemList SelectedList = default;
            Group = (Aow.Units.IndependentAG.TPatrolAG)FGroup;
            if (Group.FPatrolFrom.X == -1)
            {
                Group.FPatrolFrom = FLocation;
            }

            Settings = new Aow.Units.AoWE.TMoveControlSettings(null);
            Settings.FPlayer = unchecked((sbyte)Group.FPlayer);
            Settings.FMode = Settings.FMode + TSet<Aow.Units.AoWE.TMoveModeElement>.Of(Aow.Units.AoWE.TMoveModeElement.mmAI, Aow.Units.AoWE.TMoveModeElement.mmIgnoreArmies);
            Settings.SetSourcePos(Group.FPatrolFrom.X, Group.FPatrolFrom.Y, Group.FPatrolFrom.L);
            Settings.FMovePointsMax = 48;
            Settings.FMode = Settings.FMode + TSet<Aow.Units.AoWE.TMoveModeElement>.Of(Aow.Units.AoWE.TMoveModeElement.mmLogProcessedMapFields);
            Settings.FArmyHS = FArmyHS;
            Settings.FSelection = FSelection;
            Settings.FMovePointTable = FMovePointTable;
            Aow.Units.AoWE.AoWE.AoWHSMap.FMoveControl.ProcessMove(Settings);
            Fields = Settings.FProcessedMapFields;
            {
                I = 0;
                byte end__38 = 6;
                for (; I <= end__38; I += 1)
                {
                    var call__39 = new Aow.Units.Engine.TDoubleItemList();
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
                        MapF = (Aow.Units.AoWE.TAoWMapField)Fields.List[I];
                        var call__41 = Aow.Units.HSEngine.HSEngine.dHXtoHT(X, Y, MapF.Xhx, MapF.Yhx);
                        Heading = call__41;
                        var call__42 = Settings.GetMovePoints(MapF.Xhx, MapF.Yhx, MapF.Lhx);
                        Lists[Heading].AddAB(Aow.Rtl.Rtl.ObjectToInt(MapF), call__42);
                    }
                }
            }

            {
                I = 0;
                byte end__43 = 6;
                for (; I <= end__43; I += 1)
                {
                    Lists[I].Sort(Aow.Units.IndependentAG.IndependentAG.ComparePatrolFields);
                }
            }

            Msg.MsgID = 4384;
            Msg.Param[0] = FArmyHS;
            {
                I = 1;
                byte end__44 = 6;
                for (; I <= end__44; I += 1)
                {
                    NewList = new Aow.Units.Engine.TDoubleItemList();
                    J = 0;
                    while ((NewList.Count != 30) && (J != Lists[I].Count))
                    {
                        MapF = Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAoWMapField>((int)Lists[I].List[J].A);
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

            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(Group.FPatrolFrom.XYL);
            MapF = (Aow.Units.AoWE.TAoWMapField)call__46;
            if (MapF.GetPlayer() == -1)
            {
                Lists[0].AddAB(Aow.Rtl.Rtl.ObjectToInt(MapF), 0);
            }

            if (Group.FPatrolTo.X != -1)
            {
                var call__47 = Aow.Units.HSEngine.HSEngine.dHXtoHT(Group.FPatrolFrom.X, Group.FPatrolFrom.Y, Group.FPatrolTo.X, Group.FPatrolTo.Y);
                LastHeading = call__47;
            }
            else
            {
                LastHeading = 0;
            }

            PreviousHeading = Group.FPreviousHeading;
            Choices = new Aow.Units.Engine.TByteList();
            {
                I = 0;
                byte end__48 = 6;
                for (; I <= end__48; I += 1)
                {
                    if (((Lists[I].Count != 0) && (LastHeading != I)) && (I != PreviousHeading))
                    {
                        bool bool__49 = (PreviousHeading == 0);
                        if (!bool__49)
                        {
                            var call__50 = Aow.Units.IndependentAG.IndependentAG.PatrolHeadingDistance(I, LastHeading);
                            bool__49 = (call__50 > 1);
                        }

                        bool bool__51 = bool__49;
                        if (!bool__51)
                        {
                            var call__52 = Aow.Units.IndependentAG.IndependentAG.PatrolHeadingDistance(I, PreviousHeading);
                            bool__51 = (call__52 > 1);
                        }

                        if (bool__51)
                        {
                            Choices.Add(unchecked((byte)I));
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
                        if ((Lists[I].Count != 0) && (LastHeading != I))
                        {
                            Choices.Add(unchecked((byte)I));
                        }
                    }
                }
            }

            if (Choices.Count != 0)
            {
                Group.FPreviousHeading = unchecked((sbyte)LastHeading);
                var call__54 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Choices.Count);
                var call__55 = Choices[call__54];
                Heading = call__55;
                SelectedList = Lists[Heading];
                var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(SelectedList.Count);
                MapF = Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAoWMapField>((int)SelectedList.List[call__56].A);
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

    // { TRefugeAG (instance size 80, VMT 557D8554) }
    public partial class TRefugeAG : Aow.Units.AoWE.TAIGroup, Aow.Units.AoWE.IAIGroupClass
    {
        // IndependentAG.pas:551
        public new const int ClassID = 136289;

        // IndependentAG.pas:544
        public TRefugeAG(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // IndependentAG.pas:557
        public override Aow.Units.Engine.EClass GroupControlClass()
        {
            Aow.Units.Engine.EClass Result = default;
            Result = Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TRefugeAGC>();
            return Result;
        }

        // IndependentAG.pas:554
        public static new Aow.Units.AoWE.TGroupBehavior Behavior => Aow.Units.AoWE.TGroupBehavior.gbRefuge;

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // IndependentAG.pas:551
            public override int ClassID() => Aow.Units.IndependentAG.TRefugeAG.ClassID;

            // IndependentAG.pas:87
            public new Aow.Units.IndependentAG.TRefugeAG Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IndependentAG.TRefugeAG)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IndependentAG.TRefugeAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TRefugeAGC (instance size 368, VMT 557D86AC) }
    public partial class TRefugeAGC : Aow.Units.AoWE.TAIGroupControl
    {
        public TRefugeAGC(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

    }

    // { TRaidAG (instance size 80, VMT 557D87A4) }
    public partial class TRaidAG : Aow.Units.AoWE.TAIGroup, Aow.Units.AoWE.IAIGroupClass
    {
        // IndependentAG.pas:567
        public new const int ClassID = 136293;

        // IndependentAG.pas:560
        public TRaidAG(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // IndependentAG.pas:573
        public override Aow.Units.Engine.EClass GroupControlClass()
        {
            Aow.Units.Engine.EClass Result = default;
            Result = Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TRaidAGC>();
            return Result;
        }

        // IndependentAG.pas:570
        public static new Aow.Units.AoWE.TGroupBehavior Behavior => Aow.Units.AoWE.TGroupBehavior.gbRaid;

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // IndependentAG.pas:567
            public override int ClassID() => Aow.Units.IndependentAG.TRaidAG.ClassID;

            // IndependentAG.pas:101
            public new Aow.Units.IndependentAG.TRaidAG Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IndependentAG.TRaidAG)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IndependentAG.TRaidAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TRaidAGC (instance size 368, VMT 557D88F4) }
    public partial class TRaidAGC : Aow.Units.AoWE.TAIGroupControl
    {
        public TRaidAGC(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // IndependentAG.pas:589
        public override void MoveExecuterDone(Aow.Rtl.TObject Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesOutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesInvalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // IndependentAG.pas:576
        public override void TargetFoundEvent(Aow.Units.AoWE.TAIGroupControlPlugin Sender, Aow.Units.AoWE.TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            // { Original also accepts crDefeat when the efficiency test passes. }
            var call__59 = FCombatPredictor.Execute();
            if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crDefeat, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(call__59))
            {
                var call__60 = FCombatPredictor.GetEfficiency();
                if (call__60 > 0)
                {
                    var call__61 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = (call__61 / 10) + ((Target.FData.Priority * 100) - (Target.FMovePoints * 5));
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }
    }

    // { TScoutAG (instance size 80, VMT 557D89E8) }
    public partial class TScoutAG : Aow.Units.AoWE.TAIGroup, Aow.Units.AoWE.IAIGroupClass
    {
        // IndependentAG.pas:650
        public new const int ClassID = 136277;

        // IndependentAG.pas:643
        public TScoutAG(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // IndependentAG.pas:656
        public override Aow.Units.Engine.EClass GroupControlClass()
        {
            Aow.Units.Engine.EClass Result = default;
            Result = Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TScoutAGC>();
            return Result;
        }

        // IndependentAG.pas:653
        public static new Aow.Units.AoWE.TGroupBehavior Behavior => Aow.Units.AoWE.TGroupBehavior.gbScout;

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // IndependentAG.pas:650
            public override int ClassID() => Aow.Units.IndependentAG.TScoutAG.ClassID;

            // IndependentAG.pas:118
            public new Aow.Units.IndependentAG.TScoutAG Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IndependentAG.TScoutAG)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IndependentAG.TScoutAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TScoutAGC (instance size 368, VMT 557D8B3C) }
    public partial class TScoutAGC : Aow.Units.AoWE.TAIGroupControl
    {
        public TScoutAGC(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // IndependentAG.pas:671
        public override void MoveExecuterDone(Aow.Rtl.TObject Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesOutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesInvalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // IndependentAG.pas:659
        public override void TargetFoundEvent(Aow.Units.AoWE.TAIGroupControlPlugin Sender, Aow.Units.AoWE.TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var call__62 = FCombatPredictor.Execute();
            if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(call__62))
            {
                var call__63 = FCombatPredictor.GetEfficiency();
                if (call__63 > 50)
                {
                    var call__64 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__64 + ((Target.FData.Priority * 100) - (Target.FMovePoints * 5));
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }

        // IndependentAG.pas:680
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AGCPlugins.TAIGroupWanderControl Wander = default;
            switch (FState)
            {
                case 0:
                    bool bool__65 = (FGroup.FPlayer != 0);
                    if (!bool__65)
                    {
                        var call__66 = Aow.Units.AoWE.AoWE.AoWHSMap.GetIndependentAIMode();
                        var call__67 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                        bool__65 = (call__67 < ((int)call__66));
                    }

                    if (bool__65)
                    {
                        Finder = new Aow.Units.AoWE.TAIGroupTargetFinder(null);
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
                        ExecuteME(unchecked((sbyte)call__72.FLocation.X), unchecked((sbyte)call__71.FLocation.Y), unchecked((sbyte)call__70.FLocation.L));
                        FTargets.Delete(0);
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 2:
                    Wander = new Aow.Units.AGCPlugins.TAIGroupWanderControl(null);
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

    public partial class TRegistrator : Aow.Rtl.TObject
    {
        // IndependentAG.pas:136
        public static void MapLoaded(Aow.Rtl.TObject Sender)
        {
            if (Sender == Aow.Units.AoWE.AoWE.AoWHSMap)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.RegisterAIGroupClasses([Aow.Units.AoWE.AIGroupClass.Of<Aow.Units.IndependentAG.TGuardAG>(), Aow.Units.AoWE.AIGroupClass.Of<Aow.Units.IndependentAG.TGuardAreaAG>(), Aow.Units.AoWE.AIGroupClass.Of<Aow.Units.IndependentAG.TPatrolAG>(), Aow.Units.AoWE.AIGroupClass.Of<Aow.Units.IndependentAG.TRefugeAG>(), Aow.Units.AoWE.AIGroupClass.Of<Aow.Units.IndependentAG.TRaidAG>(), Aow.Units.AoWE.AIGroupClass.Of<Aow.Units.IndependentAG.TScoutAG>()]);
            }
        }

    }

    public static partial class IndependentAG
    {
        // IndependentAG.pas:313
        public static int ComparePatrolFields(in Aow.Units.Engine.TDoubleItem A, in Aow.Units.Engine.TDoubleItem B)
        {
            int Result = default;
            Result = B.B - A.B;
            return Result;
        }

        // IndependentAG.pas:318
        public static int PatrolHeadingDistance(int A, int B)
        {
            int Result = default;
            Result = System.Math.Abs(B - A);
            if (Result > 3)
            {
                Result = 6 - Result;
            }

            return Result;
        }

        public static void Initialize()
        {
            if (Aow.Units.AoWE.AoWE.ValidateAoWEngine())
            {
                Aow.Units.AoWE.AoWE.AoWEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TGuardAG>(), Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TGuardAreaAG>(), Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TPatrolAG>(), Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TRefugeAG>(), Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TRaidAG>(), Aow.Units.Engine.EClass.Of<Aow.Units.IndependentAG.TScoutAG>()]);
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Add(Aow.Units.IndependentAG.TRegistrator.MapLoaded);
            }
        }

        public static void Finalize()
        {
            if (Aow.Units.AoWE.AoWE.AoWEngine != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Remove(Aow.Units.IndependentAG.TRegistrator.MapLoaded);
            }
        }
    }
}
