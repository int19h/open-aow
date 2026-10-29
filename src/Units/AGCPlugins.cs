// Translated from AGCPlugins.pas.
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
using Aow.Units.City;
using static Aow.Units.City.City;
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
    // { TAIGroupRazeControl: size $2C, VMT 557D70AC; fields $1C control, $20 mode,
//     $21 damage types, $24 structure factor, $28 city factor. }
    public partial class TAIGroupRazeControl : Aow.Units.AoWE.TAIGroupControlPlugin
    {
        public Aow.Units.AoWE.TAIGroupControl FControl;
        // { $1C }
        public TSet<Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement> FMode;
        // { $20 }
        public TSet<Aow.Units.AoWE.TDamageType> FDamageTypes;
        // { $21 }
        public byte FReserved23;
        public float FStructureStrengthFactor;
        // { $24 }
        public float FCityStrengthFactor;
        // AGCPlugins.pas:101
        // { $28 }
        public override bool Activate(Aow.Units.AoWE.TAIGroupControl Control)
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
            Aow.Units.AoWE.TStructure Structure = default;
            Aow.Units.AoWE.TBattleFieldInfo Info = default;
            if (FState == 0)
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(Unsafe.As<Aow.Units.HSEngine.TXYL, int>(ref FControl.FLocation));
                var call__2 = call__1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                Structure = (Aow.Units.AoWE.TStructure)call__2;
                if ((Structure != null) && Structure.GetRazeable())
                {
                    Aow.Units.AoWE.AoWE.SetupBattleFieldInfo(ref Info, unchecked((sbyte)FControl.FGroup.FPlayer), unchecked((sbyte)FControl.FGroup.FIndependentRelation), TSet<Aow.Units.AoWE.TBattleFieldInfoRequestsElement>.Of(Aow.Units.AoWE.TBattleFieldInfoRequestsElement.bfrIgnoreIndependents), FDamageTypes);
                    Aow.Units.AoWE.AoWE.AoWHSMap.RetrieveBattleFieldInfo(FControl.FLocation.X, FControl.FLocation.Y, FControl.FLocation.L, 15, ref Info);
                    if (Structure is Aow.Units.City.TCity)
                    {
                        Result = ValidateRazeCity((Aow.Units.City.TCity)Structure, Info);
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
            FDamageTypes = Aow.Units.AGCPlugins.AGCPlugins.AllDamageTypes - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall);
            FMode = TSet<Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement>.Of(Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement.rcmCannotDefend);
            FCityStrengthFactor = 2;
            FStructureStrengthFactor = 2;
        }

        // AGCPlugins.pas:98
        public bool Setup(Aow.Units.AoWE.TAIGroupControl Control)
        {
            bool Result = default;
            Reset();
            Result = true;
            return Result;
        }

        // AGCPlugins.pas:108
        public void ExecuteRaze(Aow.Units.AoWE.TStructure Structure)
        {
            Structure.Raze(unchecked((sbyte)FControl.FGroup.FPlayer));
        }

        // AGCPlugins.pas:111
        public bool ValidateRazeCity(Aow.Units.City.TCity City, in Aow.Units.AoWE.TBattleFieldInfo Info)
        {
            bool Result = default;
            var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(City.FRace);
            var call__4 = call__3.GetPlayerRelation(unchecked((sbyte)FControl.FGroup.FPlayer));
            switch (call__4)
            {
                case Aow.Units.AoWE.TStatus.sHostile:
                case Aow.Units.AoWE.TStatus.sWary:
                    Result = FMode.Contains(Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement.rcmAlways);
                    if (FMode.Contains(Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement.rcmOnMigrate))
                    {
                        Result = true;
                    }

                    if (FMode.Contains(Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement.rcmCannotDefend))
                    {
                        Result = Result || (Aow.Rtl.Rtl.Round(((double)Info.FriendlyStrength) * FCityStrengthFactor) < Info.Strength);
                    }

                    break;
                default:
                    Result = FMode.Contains(Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement.rcmAlways);
                    break;
            }

            return Result;
        }

        // AGCPlugins.pas:125
        public bool ValidateRazeStructure(Aow.Units.AoWE.TStructure Structure, in Aow.Units.AoWE.TBattleFieldInfo Info)
        {
            bool Result = default;
            Result = false;
            if (FMode.Contains(Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement.rcmAlways))
            {
                Result = true;
            }

            if (FMode.Contains(Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement.rcmCannotDefend) && (Aow.Rtl.Rtl.Round(((double)Info.FriendlyStrength) * FStructureStrengthFactor) < Info.Strength))
            {
                Result = true;
            }

            return Result;
        }

        // intf_P5_07.inc:218
        public static new Aow.Units.AGCPlugins.TAIGroupRazeControl Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AGCPlugins.TAIGroupRazeControl o = new Aow.Units.AGCPlugins.TAIGroupRazeControl();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TAIGroupDefendTargetSelector: size $3C, VMT 557D71EC; $1C control,
//     $20 selector, $24 targets, $28 mode, $2C path step, $30 radius, $34 damage, $38 factor. }
    public partial class TAIGroupDefendTargetSelector : Aow.Units.AoWE.TAIGroupControlPlugin
    {
        public Aow.Units.AoWE.TAIGroupControl FControl;
        // { $1C }
        public Aow.Units.AoWE.TAIMoveTargetSelector FSelector;
        // { $20 }
        public Aow.Units.AoWE.TAIGroupControlTargetList FTargets;
        // { $24 }
        public TSet<Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement> FMode;
        // { $28 }
        public Aow.Units.FixedArray_byte_3 FReserved29;
        public int FPathStep;
        // { $2C }
        public int FBattleFieldRadius;
        // { $30 }
        public TSet<Aow.Units.AoWE.TDamageType> FDamageTypes;
        // { $34 }
        public Aow.Units.FixedArray_byte_2 FReserved36;
        public float FMovePointsFactor;
        // AGCPlugins.pas:164
        // { $38 }
        public override void Destroy()
        {
            FSelector.Free();
            FTargets.Free();
            base.Destroy();
        }

        // AGCPlugins.pas:157
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__5 = new Aow.Units.AoWE.TAIMoveTargetSelector(null);
            FSelector = call__5;
            var call__6 = new Aow.Units.AoWE.TAIGroupControlTargetList(null);
            FTargets = call__6;
        }

        // AGCPlugins.pas:245
        public override bool Activate(Aow.Units.AoWE.TAIGroupControl Control)
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
            // { Unmatched: tried typed control and scoped move-executer receivers. }
            switch (FState)
            {
                case 0:
                    if (FSelector.FStatus == Aow.Units.AoWE.TAIMoveTargetSelectorStatus.atsSearching)
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
                        FControl.FGroup.SetupTarget(null, Aow.Units.AoWE.TAIGroupTargetStatus.gtsIdle, 0);
                        FControl.SetupME();
                        var call__9 = FTargets.GetTargets(0);
                        if (Unsafe.As<Aow.Units.HSEngine.TXYL, int>(ref call__9.FLocation) == Unsafe.As<Aow.Units.HSEngine.TXYL, int>(ref FControl.FLocation))
                        {
                            FControl.FMoveExecuter.FMode = FControl.FMoveExecuter.FMode - TSet<Aow.Units.AoWE.TAIMoveExecuterModeElement>.Of(Aow.Units.AoWE.TAIMoveExecuterModeElement.memAdjacent);
                        }

                        var call__10 = FTargets.GetTargets(0);
                        var call__11 = FTargets.GetTargets(0);
                        var call__12 = FTargets.GetTargets(0);
                        FControl.ExecuteME(unchecked((sbyte)call__12.FLocation.X), unchecked((sbyte)call__11.FLocation.Y), unchecked((sbyte)call__10.FLocation.L));
                        FState = 2;
                    }
                    else
                    {
                        FControl.SetupME();
                        var aiMoveExecuter = FControl.FMoveExecuter;
                        aiMoveExecuter.FMode = aiMoveExecuter.FMode - TSet<Aow.Units.AoWE.TAIMoveExecuterModeElement>.Of(Aow.Units.AoWE.TAIMoveExecuterModeElement.memAdjacent);

                        var aiGroupControl = FControl;
                        aiGroupControl.ExecuteME(unchecked((sbyte)aiGroupControl.FLocation.X), unchecked((sbyte)aiGroupControl.FLocation.Y), unchecked((sbyte)aiGroupControl.FLocation.L));

                        FControl.FGroup.SetupTarget(null, Aow.Units.AoWE.TAIGroupTargetStatus.gtsIdle, 0);
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
            this.FSelector.FTargetInfo.Mode = Aow.Units.AoWE.TAITargetMode.tmDefend;

            FPathStep = 7;
            FDamageTypes = Aow.Units.AGCPlugins.AGCPlugins.AllDamageTypes - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall);
            FBattleFieldRadius = 10;
            FMovePointsFactor = -1;
            FMode = TSet<Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement>.Of();
        }

        // AGCPlugins.pas:188
        public int GetMovepathBattleFieldStrength(Aow.Units.AoWE.TMovepath Path)
        {
            int Result = default;
            int I = default;
            Aow.Units.AoWE.TBattleFieldInfo Info = default;
            if ((Path != null) && (FPathStep < Path.Count))
            {
                Result = 0;
                I = FPathStep;
                Aow.Units.AoWE.AoWE.SetupBattleFieldInfo(ref Info, unchecked((sbyte)FControl.FGroup.FPlayer), unchecked((sbyte)FControl.FGroup.FIndependentRelation), TSet<Aow.Units.AoWE.TBattleFieldInfoRequestsElement>.Of(), FDamageTypes);
                Info.FriendlyRelations = TSet<byte>.WithBounds(0, 7);
                Info.Requests = Info.Requests + TSet<Aow.Units.AoWE.TBattleFieldInfoRequestsElement>.Of(Aow.Units.AoWE.TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents);
                while (I < Path.Count)
                {
                    Info.Strength = 0;
                    var call__16 = Path[I];
                    var call__17 = Path[I];
                    var call__18 = Path[I];
                    Aow.Units.AoWE.AoWE.AoWHSMap.RetrieveBattleFieldInfo(call__18.X, call__17.Y, call__16.L, FBattleFieldRadius, ref Info);
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
        public void TargetFoundEvent(Aow.Units.AoWE.TMoveTarget Target, ref bool SearchDone)
        {
            Aow.Units.AoWE.TAIGroupControlTarget NewTarget = default;
            NewTarget = new Aow.Units.AoWE.TAIGroupControlTarget(null);
            NewTarget.FInfo = this.FSelector.FTargetInfo;

            NewTarget.FData = Target.FData;
            NewTarget.FLocation = Target.FLocation;
            NewTarget.FMovePoints = Target.FMovePoints;
            NewTarget.FData.Priority += Aow.Rtl.Rtl.Round(((double)Target.FMovePoints) * FMovePointsFactor);
            if (FMode.Contains(Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS))
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
        public bool Setup(Aow.Units.AoWE.TAIGroupControl Control)
        {
            bool Result = default;
            Reset();
            FSelector.FSourceLocation = Control.FLocation;
            FSelector.FMovePointTable = Control.FMovePointTable;
            FSelector.FTargetInfo.Player = unchecked((sbyte)Control.FGroup.FPlayer);
            FSelector.FTargetInfo.IndependentRelation = unchecked((sbyte)Control.FGroup.FIndependentRelation);
            var call__21 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Control.FGroup.FPlayer);
            switch (call__21.FPlayerType)
            {
                case Aow.Units.AoWE.TPlayerType.ptCPU2:
                    FMode = FMode + TSet<Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement>.Of(Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                    break;
                case Aow.Units.AoWE.TPlayerType.ptCPU3:
                    FMode = FMode + TSet<Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement>.Of(Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                    break;
                case Aow.Units.AoWE.TPlayerType.ptCPU4:
                    FMode = FMode + TSet<Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement>.Of(Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                    break;
                case Aow.Units.AoWE.TPlayerType.ptCPU5:
                    FMode = FMode + TSet<Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement>.Of(Aow.Units.AGCPlugins.TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                    break;
            }

            Result = true;
            return Result;
        }

        // AGCPlugins.pas:53
        public static new Aow.Units.AGCPlugins.TAIGroupDefendTargetSelector Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AGCPlugins.TAIGroupDefendTargetSelector o = new Aow.Units.AGCPlugins.TAIGroupDefendTargetSelector();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TAIGroupWanderControl: size $2C, VMT 557D7334; $1C mode, $20 move limit,
//     $24 move settings, $28 control. }
    public partial class TAIGroupWanderControl : Aow.Units.AoWE.TAIGroupControlPlugin
    {
        public TSet<Aow.Units.AGCPlugins.TAIGroupWanderModeElement> FMode;
        // { $1C }
        public Aow.Units.FixedArray_byte_3 FReserved1D;
        public int FMovePointsMax;
        // { $20 }
        public Aow.Units.AoWE.TMoveControlSettings FMoveSettings;
        // { $24 }
        public Aow.Units.AoWE.TAIGroupControl FControl;
        // AGCPlugins.pas:303
        // { $28 }
        public override void Destroy()
        {
            FMoveSettings.Release();
            base.Destroy();
        }

        // AGCPlugins.pas:297
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__22 = new Aow.Units.AoWE.TMoveControlSettings(null);
            FMoveSettings = call__22;
        }

        // AGCPlugins.pas:319
        public override bool Activate(Aow.Units.AoWE.TAIGroupControl Control)
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
            Aow.Units.AoWE.TAIGroupControl Control = default;
            Aow.Units.AoWE.TMoveControlSettings Settings = default;
            Aow.Units.Engine.TDoubleItemList Fields = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            int I = default;
            int TargetWeight = default;
            Aow.Units.HSEngine.TXYL Location = default;
            switch (FState)
            {
                case 0:
                    Control = FControl;
                    FMoveSettings.Reset();
                {
                    this.FMoveSettings.FPlayer = unchecked((sbyte)Control.FGroup.FPlayer);
                    this.FMoveSettings.FMode = this.FMoveSettings.FMode + TSet<Aow.Units.AoWE.TMoveModeElement>.Of(Aow.Units.AoWE.TMoveModeElement.mmAI);
                    this.FMoveSettings.SetSourcePos(Control.FLocation.X, Control.FLocation.Y, Control.FLocation.L);
                }

                {
                    var moveControlSettings = FMoveSettings;
                    moveControlSettings.FMovePointsMax = this.FMovePointsMax;
                    moveControlSettings.FArmyHS = Control.FArmyHS;
                    moveControlSettings.FSelection = Control.FSelection;
                    moveControlSettings.FMovePointTable = Control.FMovePointTable;
                    moveControlSettings.FMode = moveControlSettings.FMode + TSet<Aow.Units.AoWE.TMoveModeElement>.Of(Aow.Units.AoWE.TMoveModeElement.mmLogProcessedMapFields);
                    Aow.Units.AoWE.AoWE.AoWHSMap.FMoveControl.ProcessMove(FMoveSettings);
                }

                {
                    var group = Control.FGroup;
                    // { Original reloads the current control. }
                    bool bool__26 = (group.FTargetMode == 1) && (this.FControl.FGroup.FTargetXYL.X > 0);
                    if (bool__26)
                    {
                        var call__27 = FMoveSettings.GetMovePoints(group.FTargetXYL.X, group.FTargetXYL.Y, group.FTargetXYL.L);
                        bool__26 = (call__27 != 16777215);
                    }

                    if (bool__26 && (Unsafe.As<Aow.Units.HSEngine.TXYL, int>(ref this.FControl.FGroup.FTargetXYL) != Unsafe.As<Aow.Units.HSEngine.TXYL, int>(ref Control.FLocation)))
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
                    Fields = new Aow.Units.Engine.TDoubleItemList();
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
                                    Field = (Aow.Units.AoWE.TAoWMapField)call__29;
                                    var call__30 = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                    if (call__30 != 16777215)
                                    {
                                        var call__31 = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                        Fields.AddAB(Aow.Rtl.Rtl.ObjectToInt(Field), call__31);
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
                                Field = (Aow.Units.AoWE.TAoWMapField)call__33;
                                var call__34 = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                if (call__34 != 16777215)
                                {
                                    var call__35 = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                    Fields.AddAB(Aow.Rtl.Rtl.ObjectToInt(Field), call__35);
                                }
                            }
                        }
                    }

                    if (Fields.Count > 0)
                    {
                        Fields.Sort(Aow.Units.AGCPlugins.AGCPlugins.CompareWanderFields);
                        Field = null;
                        I = 0;
                        while ((Fields.Count != 0) && (Field == null))
                        {
                            var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Aow.Rtl.Rtl.Round(((double)Fields.Count) * 0.1) + 1);
                            I = call__36;
                            var call__37 = Fields[I];
                            Field = Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAoWMapField>((int)call__37.A);
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
                            FControl.FGroup.SetupTargetEx(Location, TargetWeight, Aow.Units.AoWE.TAIGroupTargetStatus.gtsExecute, 1, -1, 0, 0);
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
                    aiGroupControl.ExecuteME(unchecked((sbyte)aiGroupControl.FGroup.FTargetXYL.X), unchecked((sbyte)aiGroupControl.FGroup.FTargetXYL.Y), unchecked((sbyte)aiGroupControl.FGroup.FTargetXYL.L));
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
            FMode = TSet<Aow.Units.AGCPlugins.TAIGroupWanderModeElement>.Of();
            FMovePointsMax = 75;
            FMoveSettings.Reset();
        }

        // AGCPlugins.pas:316
        public bool Setup(Aow.Units.AoWE.TAIGroupControl Control)
        {
            bool Result = default;
            Reset();
            Result = true;
            return Result;
        }

        // AGCPlugins.pas:72
        public static new Aow.Units.AGCPlugins.TAIGroupWanderControl Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AGCPlugins.TAIGroupWanderControl o = new Aow.Units.AGCPlugins.TAIGroupWanderControl();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class AGCPlugins
    {
        public static TSet<Aow.Units.AoWE.TDamageType> AllDamageTypes = TSet<Aow.Units.AoWE.TDamageType>.FromMask(1023UL);
        // AGCPlugins.pas:326
        public static int CompareWanderFields(in Aow.Units.Engine.TDoubleItem Item1, in Aow.Units.Engine.TDoubleItem Item2)
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
