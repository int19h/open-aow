// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// status, settings, cycles, army table, transporter table, multiple gaps, mode, search state,
// path settings
public partial class AIMoveControl : EngineObject
{
    // AIGroup.pas:3911
    public AIMoveControl(ECustomNode owner) : base(owner)
    {
        FPathSettings = new MoveControlSettings(null);
        Clear();
    }

    public AIMoveControlStatus FStatus;
    public AIMoveControlSettings FSettings;
    public int FProcessCycles;
    public SByteArray16By16 FMovePointTable;
    public SByteArray16By16 FTransportMovePointTable;
    public bool FMultipleTransportGaps;
    public Mode FMode;
    public int FState;
    public MoveControlSettings FPathSettings;

    // AIGroup.pas:3918
    public override void Dispose()
    {
        Clear();
        FPathSettings.Release();
        base.Dispose();
    }

    // AIGroup.pas:3925
    public override void Clear()
    {
        AIMoveControlSettings Settings;
        Settings = FSettings;
        if (Settings != null)
        {
            Settings.Release();
            FSettings = null;
        }

        FStatus = AIMoveControlStatus.Idle;
        FProcessCycles = 0;
        FPathSettings.Reset();
        FPathSettings.FMode = FPathSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.AI);
    }

    // AIGroup.pas:3941
    public void ProcessingDone(AIMoveControlStatus Status)
    {
        if (FSettings != null)
        {
            FStatus = Status;
            FSettings.FStatus = Status;
            if (FSettings.FOnDone != null)
            {
                FSettings.FOnDone(this);
            }
        }
    }

    // AIGroup.pas:3951
    public void ProcessCanMove()
    {
        int Points;
        switch (FState)
        {
            case 0:
                FPathSettings.FMovePointsMax = FSettings.FQuickMovePointsMax;
                Points = AoWHSMap.Current.FMoveControl.CanMove(FPathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                FSettings.FMovePoints = Points;
                if (Points != -1)
                {
                    ProcessingDone(AIMoveControlStatus.Done);
                }
                else
                {
                    FState += 1;
                    if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 6)
                    {
                        ProcessCanMove();
                    }
                }

                break;
            case 1:
                FPathSettings.FMovePointsMax = FSettings.FMovePointsMax;
                var canMove = AoWHSMap.Current.FMoveControl.CanMove(FPathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Exact));
                FSettings.FMovePoints = canMove;
                FSettings.FTransportPath.Assign(FPathSettings.FPath);
                ProcessingDone(AIMoveControlStatus.Done);
                break;
        }
    }

    // AIGroup.pas:3978
    public void SelectCombinedTransporterArmyMpT()
    {
        sbyte Points;
        for (int Terrain = 0; Terrain <= 15; Terrain++)
        {
            for (int Overlay = -1; Overlay <= 11; Overlay++)
            {
                Points = FMovePointTable[Terrain][Overlay - -1];
                if (Points == -1)
                {
                    Points = FTransportMovePointTable[Terrain][Overlay - -1];
                    if (Points != -1)
                    {
                        Points = 8;
                    }
                }

                FPathSettings.FMovePointTable[Terrain][Overlay - -1] = Points;
            }
        }
    }

    // AIGroup.pas:3993
    public bool CalculateTransportGaps(MovePath Path, SByteArray16By16 Table)
    {
        bool Result;
        int I;
        int FirstGap;
        int LastGap;
        int Gaps;
        bool InGap;
        AoWMapField MapF;
        Gaps = 0;
        FirstGap = -1;
        LastGap = -1;
        I = Path.Count - 1;
        InGap = false;
        while (I > 1)
        {
            I -= 1;
            var item = Path[I];
            var pathItem = Path[I];
            var item2 = Path[I];
            MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(item2.X, pathItem.Y, item.L);
            if (Table[MapF.TerrainType][MapF.OverlayType - -1] == -1)
            {
                if (!InGap)
                {
                    if (FirstGap == -1)
                    {
                        FirstGap = I;
                    }

                    Gaps += 1;
                    InGap = true;
                }
            }
            else
            {
                if (InGap)
                {
                    LastGap = I + 1;
                    InGap = false;
                }
            }
        }

        if (InGap)
        {
            LastGap = 1;
        }

        if (Gaps != 0)
        {
            var item = Path[FirstGap + 1];
            FSettings.FBoardingPoint = item;
            var pathItem = Path[FirstGap];
            FSettings.FTransportSource = pathItem;
            var item2 = Path[LastGap - 1];
            FSettings.FArrivalPoint = item2;
            var item3 = Path[LastGap];
            FSettings.FTransportDestination = item3;
            Result = true;
            FMultipleTransportGaps = Gaps > 1;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:4037
    public void ProcessTransporterCanMove()
    {
        int Points;
        MapCoordinate Location = default;
        switch (FState)
        {
            case 0:
                FPathSettings.FMovePointTable = FMovePointTable;
                FPathSettings.FMovePointsMax = FSettings.FQuickMovePointsMax;
                Points = AoWHSMap.Current.FMoveControl.CanMove(FPathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                FSettings.FMovePoints = Points;
                if (Points == -1)
                {
                    FState += 1;
                    if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 6)
                    {
                        ProcessTransporterCanMove();
                    }
                }
                else
                {
                    ProcessingDone(AIMoveControlStatus.Done);
                }

                break;
            case 1:
                SelectCombinedTransporterArmyMpT();
                FPathSettings.FMovePointsMax = FSettings.FMovePointsMax;
                FPathSettings.FMode = FSettings.FMoveMode + Set<MoveModeElement>.Of(MoveModeElement.AvoidArmies);
                AoWHSMap.Current.FMoveControl.ProcessMove(FPathSettings);
                if (FPathSettings.FPath.Count != 0)
                {
                    var item = FPathSettings.FPath[0];
                    var fPathItem = FPathSettings.FPath[0];
                    var item2 = FPathSettings.FPath[0];
                    var movePoints = FPathSettings.GetMovePoints(item2.X, fPathItem.Y, item.L);
                    FSettings.FMovePoints = movePoints;
                    FSettings.FTransportPath.Assign(FPathSettings.FPath);
                    if (CalculateTransportGaps(FPathSettings.FPath, FMovePointTable))
                    {
                        FState = 2;
                        FPathSettings.FMovePointTable = FTransportMovePointTable;
                        FSettings.FTransporter.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
                        FPathSettings.SetSourcePos(Location.X, Location.Y, Location.L);
                        ref var location = ref FSettings.FTransportSource;
                        FPathSettings.SetDestinationPos(location.X, location.Y, location.L);

                        if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 1)
                        {
                            ProcessTransporterCanMove();
                        }
                    }
                    else
                    {
                        ProcessingDone(AIMoveControlStatus.Done);
                    }
                }
                else
                {
                    ProcessingDone(AIMoveControlStatus.Done);
                }

                break;
            case 2:
                FPathSettings.FMovePointsMax = FSettings.FQuickMovePointsMax;
                Points = AoWHSMap.Current.FMoveControl.CanMove(FPathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                if (Points == -1)
                {
                    FState = 3;
                    if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 6)
                    {
                        ProcessTransporterCanMove();
                    }
                }
                else
                {
                    FSettings.FMovePoints += Points;
                    if (FMultipleTransportGaps)
                    {
                        // { Original repeats the first gap source before continuing. }
                        ref var location = ref FSettings.FTransportSource;
                        FPathSettings.SetDestinationPos(location.X, location.Y, location.L);

                        FMultipleTransportGaps = false;
                        if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 1)
                        {
                            ProcessTransporterCanMove();
                        }
                    }
                    else
                    {
                        ProcessingDone(AIMoveControlStatus.Done);
                    }
                }

                break;
            case 3:
                FPathSettings.FMovePointTable = FTransportMovePointTable;
                FPathSettings.FMovePointsMax = FSettings.FMovePointsMax;
                Points = AoWHSMap.Current.FMoveControl.CanMove(FPathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Exact));
                if (Points == -1)
                {
                    FSettings.FMovePoints = -1;
                    ProcessingDone(AIMoveControlStatus.Done);
                }
                else
                {
                    FSettings.FMovePoints += Points;
                    if (FMultipleTransportGaps)
                    {
                        ref var location = ref FSettings.FTransportDestination;
                        FPathSettings.SetDestinationPos(location.X, location.Y, location.L);

                        FMultipleTransportGaps = false;
                        FState = 2;
                        if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 1)
                        {
                            ProcessTransporterCanMove();
                        }
                    }
                    else
                    {
                        ProcessingDone(AIMoveControlStatus.Done);
                    }
                }

                break;
        }
    }

    // AIGroup.pas:4128
    public void Initialize(AIMoveControlSettings Settings)
    {
        AIMoveControlSettings OldSettings;
        Troop Transporter;
        OldSettings = FSettings;
        if (OldSettings != null)
        {
            OldSettings.Release();
        }

        FSettings = Settings;
        if (FSettings != null)
        {
            FSettings.AddRef();
            FSettings.FMovePoints = -1;
            FSettings.FTransportSource.X = -1;
            FSettings.FTransportDestination.X = -1;
            FSettings.FBoardingPoint.X = -1;
            FSettings.FArrivalPoint.X = -1;
            FSettings.FStatus = AIMoveControlStatus.Processing;
            FMovePointTable = FSettings.FMovePointTable;
            Transporter = FSettings.FTransporter;
            if (Transporter != null)
            {
                var moveTypesToInt = MovementTables.MoveTypesToInt(Transporter.GetMoveTypes());
                FTransportMovePointTable = MovementTables.MovePointTables[moveTypesToInt];
            }
        }
    }

    // AIGroup.pas:4152
    public bool CanMove(AIMoveControlSettings Settings)
    {
        bool Result;
        Reset();
        Initialize(Settings);
        FStatus = AIMoveControlStatus.Processing;
        FPathSettings.Reset();
        FPathSettings.SetSourcePos(this.FSettings.FSourceX, this.FSettings.FSourceY, this.FSettings.FSourceL);

        FPathSettings.FPlayer = FSettings.FPlayer;
        FPathSettings.SetDestinationPos(FSettings.FDestinationX, FSettings.FDestinationY, FSettings.FDestinationL);
        FPathSettings.FMovePointsMax = FSettings.FMovePointsMax;
        FPathSettings.FArmyHS = FSettings.FArmyHS;
        FPathSettings.FSelection = FSettings.FSelection;
        FPathSettings.FMode = FSettings.FMoveMode;
        if (FSettings.FTransporter == null)
        {
            FMode = Mode.CanMove;
            FPathSettings.FMovePointTable = FMovePointTable;
            if (FSettings.FSourceL == FSettings.FDestinationL)
            {
                FState = 0;
                if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 1)
                {
                    ProcessCanMove();
                }
            }
            else
            {
                FState = 1;
            }

            Result = true;
        }
        else
        {
            FMode = Mode.TransporterCanMove;
            var armyHS = FSettings.FTransporter.Owner is HeroList ? null : FSettings.FTransporter.ArmyOwner.GetArmyHS();
            FPathSettings.FTransporterArmy = armyHS;
            if (FSettings.FSourceL == FSettings.FDestinationL)
            {
                FState = 0;
                if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 1)
                {
                    ProcessTransporterCanMove();
                }
            }
            else
            {
                FState = 1;
            }

            Result = true;
        }

        return Result;
    }

    // AIGroup.pas:4191
    public void Cancel()
    {
        if (FStatus == AIMoveControlStatus.Processing)
        {
            ProcessingDone(AIMoveControlStatus.Canceled);
        }
    }

    // AIGroup.pas:4196
    public void Reset()
    {
        Cancel();
        Clear();
    }

    // AIGroup.pas:4202
    public bool Process()
    {
        if (FStatus == AIMoveControlStatus.Processing)
        {
            FProcessCycles += 1;
            switch (FMode)
            {
                case Mode.CanMove:
                    ProcessCanMove();
                    break;
                case Mode.TransporterCanMove:
                    ProcessTransporterCanMove();
                    break;
            }
        }

        return FStatus == AIMoveControlStatus.Processing;
    }

    public enum Mode : byte
    {
        CanMove = 0, TransporterCanMove = 1
    }

    // intf_P5_06.inc:180

}
