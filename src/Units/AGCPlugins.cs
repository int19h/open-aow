// Translated from AGCPlugins.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.City;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AGCPlugins
{
    public enum TAIGroupRazeControlModeElement : byte
    {
        rcmCannotDefend = 0,
        rcmOnMigrate = 1,
        rcmAlways = 2
    }

    public enum TAIGroupDefendTargetSelectorModeElement : byte
    {
        dtmCalculateMovepathBFS = 0
    }

    public enum TAIGroupWanderModeElement : byte
    {
        wmTransportersAllowed = 0
    }

    // { ot=1 }
    // fields control, mode, damage types, structure factor, city factor
    public partial class TAIGroupRazeControl : TAIGroupControlPlugin
    {
        public TAIGroupRazeControl(TECustomNode owner) : base(owner)
        { }

        public TAIGroupControl FControl;
        public TSet<TAIGroupRazeControlModeElement> FMode;
        public TSet<TDamageType> FDamageTypes;
        public float FStructureStrengthFactor;
        public float FCityStrengthFactor;
        // AGCPlugins.pas:101
        public override bool Activate(TAIGroupControl Control)
        {
            bool Result = default;
            FState = 0;
            FControl = Control;
            Result = true;
            return Result;
        }

        // AGCPlugins.pas:133
        public override bool Process()
        {
            bool Result = default;
            TStructure Structure = default;
            TBattleFieldInfo Info = default;
            if (FState == 0)
            {
                var call__1 = AoWHSMap.MapContainer.GetFieldXYL(Unsafe.As<TXYL, int>(ref FControl.FLocation));
                var call__2 = call__1.FindNoneTransparentHS(typeof(TStructure));
                Structure = (TStructure)call__2;
                if (Structure != null && Structure.GetRazeable())
                {
                    SetupBattleFieldInfo(ref Info, FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnoreIndependents), FDamageTypes);
                    AoWHSMap.RetrieveBattleFieldInfo(FControl.FLocation.X, FControl.FLocation.Y, FControl.FLocation.L, 15, ref Info);
                    if (Structure is TCity)
                    {
                        Result = ValidateRazeCity((TCity)Structure, Info);
                    }
                    else
                    {
                        Result = ValidateRazeStructure(Structure, Info);
                    }

                    if (Result)
                    {
                        ExecuteRaze(Structure);
                    }
                }

                Done();
            }
            else
            {
                Done();
            }

            Result = false;
            return Result;
        }

        // AGCPlugins.pas:90
        public void Reset()
        {
            FDamageTypes = AGCPlugins.AllDamageTypes - TSet<TDamageType>.Of(TDamageType.dtWall);
            FMode = TSet<TAIGroupRazeControlModeElement>.Of(TAIGroupRazeControlModeElement.rcmCannotDefend);
            FCityStrengthFactor = 2;
            FStructureStrengthFactor = 2;
        }

        // AGCPlugins.pas:98
        public bool Setup(TAIGroupControl Control)
        {
            bool Result = default;
            Reset();
            Result = true;
            return Result;
        }

        // AGCPlugins.pas:108
        public void ExecuteRaze(TStructure Structure)
        {
            Structure.Raze(FControl.FGroup.FPlayer);
        }

        // AGCPlugins.pas:111
        public bool ValidateRazeCity(TCity City, in TBattleFieldInfo Info)
        {
            bool Result = default;
            var call__3 = AoWHSMap.FRaces.GetRace(City.FRace);
            var call__4 = call__3.GetPlayerRelation(FControl.FGroup.FPlayer);
            switch (call__4)
            {
                case TStatus.sHostile:
                case TStatus.sWary:
                    Result = FMode.Contains(TAIGroupRazeControlModeElement.rcmAlways);
                    if (FMode.Contains(TAIGroupRazeControlModeElement.rcmOnMigrate))
                    {
                        Result = true;
                    }

                    if (FMode.Contains(TAIGroupRazeControlModeElement.rcmCannotDefend))
                    {
                        Result = Result || Round((double)Info.FriendlyStrength * FCityStrengthFactor) < Info.Strength;
                    }

                    break;
                default:
                    Result = FMode.Contains(TAIGroupRazeControlModeElement.rcmAlways);
                    break;
            }

            return Result;
        }

        // AGCPlugins.pas:125
        public bool ValidateRazeStructure(TStructure Structure, in TBattleFieldInfo Info)
        {
            bool Result = default;
            Result = false;
            if (FMode.Contains(TAIGroupRazeControlModeElement.rcmAlways))
            {
                Result = true;
            }

            if (FMode.Contains(TAIGroupRazeControlModeElement.rcmCannotDefend) && Round((double)Info.FriendlyStrength * FStructureStrengthFactor) < Info.Strength)
            {
                Result = true;
            }

            return Result;
        }

        // intf_P5_07.inc:218

    }

    // control, selector, targets, mode, path step, radius, damage, factor
    public partial class TAIGroupDefendTargetSelector : TAIGroupControlPlugin
    {
        public TAIGroupControl FControl;
        public TAIMoveTargetSelector FSelector;
        public TAIGroupControlTargetList FTargets;
        public TSet<TAIGroupDefendTargetSelectorModeElement> FMode;
        public int FPathStep;
        public int FBattleFieldRadius;
        public TSet<TDamageType> FDamageTypes;
        public float FMovePointsFactor;
        // AGCPlugins.pas:164
        public override void Destroy()
        {
            FSelector.Free();
            FTargets.Free();
            base.Destroy();
        }

        // AGCPlugins.pas:157
        public TAIGroupDefendTargetSelector(TECustomNode owner) : base(owner)
        {
            var call__5 = new TAIMoveTargetSelector(null);
            FSelector = call__5;
            var call__6 = new TAIGroupControlTargetList(null);
            FTargets = call__6;
        }

        // AGCPlugins.pas:245
        public override bool Activate(TAIGroupControl Control)
        {
            bool Result = default;
            FControl = Control;
            FSelector.Search(Control);
            FState = 0;
            Result = true;
            return Result;
        }

        // AGCPlugins.pas:253
        public override bool Process()
        {
            bool Result = default;
            switch (FState)
            {
                case 0:
                    if (FSelector.FStatus == TAIMoveTargetSelectorStatus.atsSearching)
                    {
                        var call__7 = FSelector.Process();
                        Result = call__7;
                    }
                    else
                    {
                        Result = false;
                    }

                    if (!Result)
                    {
                        FTargets.SortOnPriority();
                        FState += 1;
                    }

                    break;
                case 1:
                    var call__8 = FTargets.Count;
                    if (call__8 != 0)
                    {
                        FControl.FGroup.SetupTarget(null, TAIGroupTargetStatus.gtsIdle, 0);
                        FControl.SetupME();
                        var call__9 = FTargets.GetTargets(0);
                        if (Unsafe.As<TXYL, int>(ref call__9.FLocation) == Unsafe.As<TXYL, int>(ref FControl.FLocation))
                        {
                            FControl.FMoveExecuter.FMode = FControl.FMoveExecuter.FMode - TSet<TAIMoveExecuterModeElement>.Of(TAIMoveExecuterModeElement.memAdjacent);
                        }

                        var call__10 = FTargets.GetTargets(0);
                        var call__11 = FTargets.GetTargets(0);
                        var call__12 = FTargets.GetTargets(0);
                        FControl.ExecuteME(call__12.FLocation.X, call__11.FLocation.Y, call__10.FLocation.L);
                        FState = 2;
                    }
                    else
                    {
                        FControl.SetupME();
                        var aiMoveExecuter = FControl.FMoveExecuter;
                        aiMoveExecuter.FMode = aiMoveExecuter.FMode - TSet<TAIMoveExecuterModeElement>.Of(TAIMoveExecuterModeElement.memAdjacent);

                        var aiGroupControl = FControl;
                        aiGroupControl.ExecuteME(aiGroupControl.FLocation.X, aiGroupControl.FLocation.Y, aiGroupControl.FLocation.L);

                        FControl.FGroup.SetupTarget(null, TAIGroupTargetStatus.gtsIdle, 0);
                        Done();
                    }

                    Result = false;
                    break;
                case 2:
                    Done();
                    Result = false;
                    break;
                default:
                    Result = false;
                    break;
            }

            return Result;
        }

        // AGCPlugins.pas:171
        public void Reset()
        {
            FSelector.Reset();
            this.FSelector.FOnTargetFound = TargetFoundEvent;
            this.FSelector.FMovePointsMax = 150;
            this.FSelector.FMovePointsMin = 0;
            this.FSelector.FTargetInfo.Mode = TAITargetMode.tmDefend;

            FPathStep = 7;
            FDamageTypes = AGCPlugins.AllDamageTypes - TSet<TDamageType>.Of(TDamageType.dtWall);
            FBattleFieldRadius = 10;
            FMovePointsFactor = -1;
            FMode = TSet<TAIGroupDefendTargetSelectorModeElement>.Of();
        }

        // AGCPlugins.pas:188
        public int GetMovepathBattleFieldStrength(TMovepath Path)
        {
            int Result = default;
            int I = default;
            TBattleFieldInfo Info = default;
            if (Path != null && FPathStep < Path.Count)
            {
                Result = 0;
                I = FPathStep;
                SetupBattleFieldInfo(ref Info, FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, TSet<TBattleFieldInfoRequestsElement>.Of(), FDamageTypes);
                Info.FriendlyRelations = TSet<byte>.WithBounds(0, 7);
                Info.Requests = Info.Requests + TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents);
                while (I < Path.Count)
                {
                    Info.Strength = 0;
                    var call__16 = Path[I];
                    var call__17 = Path[I];
                    var call__18 = Path[I];
                    AoWHSMap.RetrieveBattleFieldInfo(call__18.X, call__17.Y, call__16.L, FBattleFieldRadius, ref Info);
                    if (Result < Info.Strength)
                    {
                        Result = Info.Strength;
                    }

                    I += FPathStep;
                }
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // AGCPlugins.pas:213
        public void TargetFoundEvent(TMoveTarget Target, ref bool SearchDone)
        {
            TAIGroupControlTarget NewTarget = default;
            NewTarget = new TAIGroupControlTarget(null);
            NewTarget.FInfo = this.FSelector.FTargetInfo;

            NewTarget.FData = Target.FData;
            NewTarget.FLocation = Target.FLocation;
            NewTarget.FMovePoints = Target.FMovePoints;
            NewTarget.FData.Priority += Round((double)Target.FMovePoints * FMovePointsFactor);
            if (FMode.Contains(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS))
            {
                var call__20 = GetMovepathBattleFieldStrength(Target.FMovepath);
                NewTarget.FPathStrength = call__20;
            }

            if (Target.FTransporterID != -1)
            {
                NewTarget.FTransporterID = Target.FTransporterID;
            }

            if (NewTarget.FPathStrength < FControl.FStrengthMax)
            {
                FTargets.Add(NewTarget);
            }

            NewTarget.Release();
        }

        // AGCPlugins.pas:229
        public bool Setup(TAIGroupControl Control)
        {
            bool Result = default;
            Reset();
            FSelector.FSourceLocation = Control.FLocation;
            FSelector.FMovePointTable = Control.FMovePointTable;
            FSelector.FTargetInfo.Player = Control.FGroup.FPlayer;
            FSelector.FTargetInfo.IndependentRelation = Control.FGroup.FIndependentRelation;
            var call__21 = AoWHSMap.FPlayers.GetPlayers(Control.FGroup.FPlayer);
            switch (call__21.FPlayerType)
            {
                case TPlayerType.ptCPU2:
                    FMode = FMode + TSet<TAIGroupDefendTargetSelectorModeElement>.Of(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                    break;
                case TPlayerType.ptCPU3:
                    FMode = FMode + TSet<TAIGroupDefendTargetSelectorModeElement>.Of(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                    break;
                case TPlayerType.ptCPU4:
                    FMode = FMode + TSet<TAIGroupDefendTargetSelectorModeElement>.Of(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                    break;
                case TPlayerType.ptCPU5:
                    FMode = FMode + TSet<TAIGroupDefendTargetSelectorModeElement>.Of(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                    break;
            }

            Result = true;
            return Result;
        }

    }

    // mode, move limit, move settings, control
    public partial class TAIGroupWanderControl : TAIGroupControlPlugin
    {
        public TSet<TAIGroupWanderModeElement> FMode;
        public int FMovePointsMax;
        public TMoveControlSettings FMoveSettings;
        public TAIGroupControl FControl;
        // AGCPlugins.pas:303
        public override void Destroy()
        {
            FMoveSettings.Release();
            base.Destroy();
        }

        // AGCPlugins.pas:297
        public TAIGroupWanderControl(TECustomNode owner) : base(owner)
        {
            var call__22 = new TMoveControlSettings(null);
            FMoveSettings = call__22;
        }

        // AGCPlugins.pas:319
        public override bool Activate(TAIGroupControl Control)
        {
            bool Result = default;
            FControl = Control;
            FState = 0;
            Result = true;
            return Result;
        }

        // AGCPlugins.pas:329
        public override bool Process()
        {
            bool Result = default;
            TAIGroupControl Control = default;
            TMoveControlSettings Settings = default;
            TDoubleItemList Fields = default;
            TAoWMapField Field = default;
            int I = default;
            int TargetWeight = default;
            TXYL Location = default;
            switch (FState)
            {
                case 0:
                    Control = FControl;
                    FMoveSettings.Reset();
                {
                    this.FMoveSettings.FPlayer = Control.FGroup.FPlayer;
                    this.FMoveSettings.FMode = this.FMoveSettings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmAI);
                    this.FMoveSettings.SetSourcePos(Control.FLocation.X, Control.FLocation.Y, Control.FLocation.L);
                }

                {
                    var moveControlSettings = FMoveSettings;
                    moveControlSettings.FMovePointsMax = this.FMovePointsMax;
                    moveControlSettings.FArmyHS = Control.FArmyHS;
                    moveControlSettings.FSelection = Control.FSelection;
                    moveControlSettings.FMovePointTable = Control.FMovePointTable;
                    moveControlSettings.FMode = moveControlSettings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmLogProcessedMapFields);
                        AoWHSMap.FMoveControl.ProcessMove(FMoveSettings);
                }

                {
                    var group = Control.FGroup;
                    // { Original reloads the current control. }
                    bool bool__26 = group.FTargetMode == 1 && this.FControl.FGroup.FTargetXYL.X > 0;
                    if (bool__26)
                    {
                        var call__27 = FMoveSettings.GetMovePoints(group.FTargetXYL.X, group.FTargetXYL.Y, group.FTargetXYL.L);
                        bool__26 = call__27 != 16777215;
                    }

                    if (bool__26 && Unsafe.As<TXYL, int>(ref this.FControl.FGroup.FTargetXYL) != Unsafe.As<TXYL, int>(ref Control.FLocation))
                    {
                        FState = 2;
                    }
                    else
                    {
                        FState = 1;
                    }
                }

                    Result = false;
                    break;
                case 1:
                    Settings = FMoveSettings;
                    Fields = new TDoubleItemList();
                    if (Settings.FProcessedMapFields.Count > 40)
                    {
                        {
                            I = Settings.FProcessedMapFields.Count - 1;
                            byte end__28 = 0;
                            for (; I >= end__28; I -= 1)
                            {
                                if ((I & 3) == 3)
                                {
                                    var call__29 = Settings.FProcessedMapFields[I];
                                    Field = (TAoWMapField)call__29;
                                    var call__30 = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                    if (call__30 != 16777215)
                                    {
                                        var call__31 = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                        Fields.AddAB(ObjectToInt(Field), call__31);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        {
                            I = Settings.FProcessedMapFields.Count - 1;
                            byte end__32 = 0;
                            for (; I >= end__32; I -= 1)
                            {
                                var call__33 = Settings.FProcessedMapFields[I];
                                Field = (TAoWMapField)call__33;
                                var call__34 = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                if (call__34 != 16777215)
                                {
                                    var call__35 = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                    Fields.AddAB(ObjectToInt(Field), call__35);
                                }
                            }
                        }
                    }

                    if (Fields.Count > 0)
                    {
                        Fields.Sort(AGCPlugins.CompareWanderFields);
                        Field = null;
                        I = 0;
                        while (Fields.Count != 0 && Field == null)
                        {
                            var call__36 = AoWHSMap.Random(Round((double)Fields.Count * 0.1) + 1);
                            I = call__36;
                            var call__37 = Fields[I];
                            Field = ReferenceFromId<TAoWMapField>(call__37.A);
                            if (Field.GetPlayer() != -1)
                            {
                                Fields.Delete(I);
                                Field = null;
                            }
                        }

                        if (Field != null)
                        {
                            // { Original reads the list item before GetXYL. }
                            var call__38 = Fields[I];
                            TargetWeight = call__38.B;
                            var call__39 = Field.GetXYL();
                            Location.XYL = call__39;
                            FControl.FGroup.SetupTargetEx(Location, TargetWeight, TAIGroupTargetStatus.gtsExecute, 1, -1, 0, 0);
                            FState += 1;
                        }
                        else
                        {
                            Done();
                        }

                        Fields.Free();
                    }
                    else
                    {
                        // { Original leaks Fields when no reachable field was collected. }
                        Done();
                    }

                    Result = false;
                    break;
                case 2:
                {
                    var aiGroupControl = FControl;
                    aiGroupControl.SetupME();
                    aiGroupControl.ExecuteME(aiGroupControl.FGroup.FTargetXYL.X, aiGroupControl.FGroup.FTargetXYL.Y, aiGroupControl.FGroup.FTargetXYL.L);
                }

                    FState += 1;
                    Result = false;
                    break;
                case 3:
                    Done();
                    Result = false;
                    break;
                default:
                    Result = false;
                    break;
            }

            return Result;
        }

        // AGCPlugins.pas:309
        public void Reset()
        {
            FMode = TSet<TAIGroupWanderModeElement>.Of();
            FMovePointsMax = 75;
            FMoveSettings.Reset();
        }

        // AGCPlugins.pas:316
        public bool Setup(TAIGroupControl Control)
        {
            bool Result = default;
            Reset();
            Result = true;
            return Result;
        }

    }

    public static partial class AGCPlugins
    {
        public static TSet<TDamageType> AllDamageTypes = TSet<TDamageType>.FromMask(1023UL);
        // AGCPlugins.pas:326
        public static int CompareWanderFields(in TDoubleItem Item1, in TDoubleItem Item2)
        {
            int Result = default;
            Result = Item2.B - Item1.B;
            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
