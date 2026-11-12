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
        PathSettings = new MoveControlSettings(null);
        Clear();
    }

    public AIMoveControlStatus Status;
    public AIMoveControlSettings Settings;
    public int ProcessCycles;
    public SByteArray16By16 MovePointTable;
    public SByteArray16By16 TransportMovePointTable;
    public bool MultipleTransportGaps;
    public AIMoveControlMode Mode;
    public int State;
    public MoveControlSettings PathSettings;

    // AIGroup.pas:3918
    public override void Dispose()
    {
        Clear();
        PathSettings.Release();
        base.Dispose();
    }

    // AIGroup.pas:3925
    public override void Clear()
    {
        AIMoveControlSettings settings;
        settings = Settings;
        if (settings != null)
        {
            settings.Release();
            Settings = null;
        }

        Status = AIMoveControlStatus.Idle;
        ProcessCycles = 0;
        PathSettings.Reset();
        PathSettings.Mode = PathSettings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
    }

    // AIGroup.pas:3941
    public void ProcessingDone(AIMoveControlStatus status)
    {
        if (Settings != null)
        {
            Status = status;
            Settings.Status = status;
            if (Settings.OnDone != null)
            {
                Settings.OnDone(this);
            }
        }
    }

    // AIGroup.pas:3951
    public void ProcessCanMove()
    {
        int points;
        switch (State)
        {
            case 0:
                PathSettings.MovePointsMax = Settings.QuickMovePointsMax;
                points = AoWHSMap.Current.MoveControl.CanMove(PathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                Settings.MovePoints = points;
                if (points != -1)
                {
                    ProcessingDone(AIMoveControlStatus.Done);
                }
                else
                {
                    State += 1;
                    if (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 6)
                    {
                        ProcessCanMove();
                    }
                }

                break;
            case 1:
                PathSettings.MovePointsMax = Settings.MovePointsMax;
                var canMove = AoWHSMap.Current.MoveControl.CanMove(PathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Exact));
                Settings.MovePoints = canMove;
                Settings.TransportPath.Assign(PathSettings.Path);
                ProcessingDone(AIMoveControlStatus.Done);
                break;
        }
    }

    // AIGroup.pas:3978
    public void SelectCombinedTransporterArmyMpT()
    {
        sbyte points;
        for (int terrain = 0; terrain <= 15; terrain++)
        {
            for (int overlay = -1; overlay <= 11; overlay++)
            {
                points = MovePointTable[terrain][overlay - -1];
                if (points == -1)
                {
                    points = TransportMovePointTable[terrain][overlay - -1];
                    if (points != -1)
                    {
                        points = 8;
                    }
                }

                PathSettings.MovePointTable[terrain][overlay - -1] = points;
            }
        }
    }

    // AIGroup.pas:3993
    public bool CalculateTransportGaps(MovePath path, SByteArray16By16 table)
    {
        bool result;
        int I;
        int firstGap;
        int lastGap;
        int gaps;
        bool inGap;
        AoWMapField mapField;
        gaps = 0;
        firstGap = -1;
        lastGap = -1;
        I = path.Count - 1;
        inGap = false;
        while (I > 1)
        {
            I -= 1;
            var item = path[I];
            var pathItem = path[I];
            var item2 = path[I];
            mapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(item2.X, pathItem.Y, item.MapLevel);
            if (table[mapField.TerrainType][mapField.OverlayType - -1] == -1)
            {
                if (!inGap)
                {
                    if (firstGap == -1)
                    {
                        firstGap = I;
                    }

                    gaps += 1;
                    inGap = true;
                }
            }
            else
            {
                if (inGap)
                {
                    lastGap = I + 1;
                    inGap = false;
                }
            }
        }

        if (inGap)
        {
            lastGap = 1;
        }

        if (gaps != 0)
        {
            var item = path[firstGap + 1];
            Settings.BoardingPoint = item;
            var pathItem = path[firstGap];
            Settings.TransportSource = pathItem;
            var item2 = path[lastGap - 1];
            Settings.ArrivalPoint = item2;
            var item3 = path[lastGap];
            Settings.TransportDestination = item3;
            result = true;
            MultipleTransportGaps = gaps > 1;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:4037
    public void ProcessTransporterCanMove()
    {
        int points;
        MapCoordinate Location = default;
        switch (State)
        {
            case 0:
                PathSettings.MovePointTable = MovePointTable;
                PathSettings.MovePointsMax = Settings.QuickMovePointsMax;
                points = AoWHSMap.Current.MoveControl.CanMove(PathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                Settings.MovePoints = points;
                if (points == -1)
                {
                    State += 1;
                    if (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 6)
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
                PathSettings.MovePointsMax = Settings.MovePointsMax;
                PathSettings.Mode = Settings.MoveMode + Set<MoveModeElement>.Of(MoveModeElement.AvoidArmies);
                AoWHSMap.Current.MoveControl.ProcessMove(PathSettings);
                if (PathSettings.Path.Count != 0)
                {
                    var item = PathSettings.Path[0];
                    var fPathItem = PathSettings.Path[0];
                    var item2 = PathSettings.Path[0];
                    var movePoints = PathSettings.GetMovePoints(item2.X, fPathItem.Y, item.MapLevel);
                    Settings.MovePoints = movePoints;
                    Settings.TransportPath.Assign(PathSettings.Path);
                    if (CalculateTransportGaps(PathSettings.Path, MovePointTable))
                    {
                        State = 2;
                        PathSettings.MovePointTable = TransportMovePointTable;
                        Settings.Transporter.GetLocation(ref Location.X, ref Location.Y, ref Location.MapLevel);
                        PathSettings.SetSourcePosition(Location.X, Location.Y, Location.MapLevel);
                        ref var location = ref Settings.TransportSource;
                        PathSettings.SetDestinationPosition(location.X, location.Y, location.MapLevel);

                        if (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
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
                PathSettings.MovePointsMax = Settings.QuickMovePointsMax;
                points = AoWHSMap.Current.MoveControl.CanMove(PathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                if (points == -1)
                {
                    State = 3;
                    if (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 6)
                    {
                        ProcessTransporterCanMove();
                    }
                }
                else
                {
                    Settings.MovePoints += points;
                    if (MultipleTransportGaps)
                    {
                        // { Original repeats the first gap source before continuing. }
                        ref var location = ref Settings.TransportSource;
                        PathSettings.SetDestinationPosition(location.X, location.Y, location.MapLevel);

                        MultipleTransportGaps = false;
                        if (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
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
                PathSettings.MovePointTable = TransportMovePointTable;
                PathSettings.MovePointsMax = Settings.MovePointsMax;
                points = AoWHSMap.Current.MoveControl.CanMove(PathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Exact));
                if (points == -1)
                {
                    Settings.MovePoints = -1;
                    ProcessingDone(AIMoveControlStatus.Done);
                }
                else
                {
                    Settings.MovePoints += points;
                    if (MultipleTransportGaps)
                    {
                        ref var location = ref Settings.TransportDestination;
                        PathSettings.SetDestinationPosition(location.X, location.Y, location.MapLevel);

                        MultipleTransportGaps = false;
                        State = 2;
                        if (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
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
    public void Initialize(AIMoveControlSettings settings)
    {
        AIMoveControlSettings oldSettings;
        Troop transporter;
        oldSettings = Settings;
        if (oldSettings != null)
        {
            oldSettings.Release();
        }

        Settings = settings;
        if (Settings != null)
        {
            Settings.AddRef();
            Settings.MovePoints = -1;
            Settings.TransportSource.X = -1;
            Settings.TransportDestination.X = -1;
            Settings.BoardingPoint.X = -1;
            Settings.ArrivalPoint.X = -1;
            Settings.Status = AIMoveControlStatus.Processing;
            MovePointTable = Settings.MovePointTable;
            transporter = Settings.Transporter;
            if (transporter != null)
            {
                var moveTypesToInt32 = MovementTables.MoveTypesToInt32(transporter.GetMoveTypes());
                TransportMovePointTable = MovementTables.MovePointTables[moveTypesToInt32];
            }
        }
    }

    // AIGroup.pas:4152
    public bool CanMove(AIMoveControlSettings settings)
    {
        bool result;
        Reset();
        Initialize(settings);
        Status = AIMoveControlStatus.Processing;
        PathSettings.Reset();
        PathSettings.SetSourcePosition(this.Settings.SourceX, this.Settings.SourceY, this.Settings.SourceL);

        PathSettings.Player = Settings.Player;
        PathSettings.SetDestinationPosition(Settings.DestinationX, Settings.DestinationY, Settings.DestinationL);
        PathSettings.MovePointsMax = Settings.MovePointsMax;
        PathSettings.ArmyHexSprite = Settings.ArmyHexSprite;
        PathSettings.Selection = Settings.Selection;
        PathSettings.Mode = Settings.MoveMode;
        if (Settings.Transporter == null)
        {
            Mode = AIMoveControlMode.CanMove;
            PathSettings.MovePointTable = MovePointTable;
            if (Settings.SourceL == Settings.DestinationL)
            {
                State = 0;
                if (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
                {
                    ProcessCanMove();
                }
            }
            else
            {
                State = 1;
            }

            result = true;
        }
        else
        {
            Mode = AIMoveControlMode.TransporterCanMove;
            var armyHexSprite = Settings.Transporter.Owner is HeroList ? null : Settings.Transporter.ArmyOwner.GetArmyHexSprite();
            PathSettings.TransporterArmy = armyHexSprite;
            if (Settings.SourceL == Settings.DestinationL)
            {
                State = 0;
                if (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
                {
                    ProcessTransporterCanMove();
                }
            }
            else
            {
                State = 1;
            }

            result = true;
        }

        return result;
    }

    // AIGroup.pas:4191
    public void Cancel()
    {
        if (Status == AIMoveControlStatus.Processing)
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
        if (Status == AIMoveControlStatus.Processing)
        {
            ProcessCycles += 1;
            switch (Mode)
            {
                case AIMoveControlMode.CanMove:
                    ProcessCanMove();
                    break;
                case AIMoveControlMode.TransporterCanMove:
                    ProcessTransporterCanMove();
                    break;
            }
        }

        return Status == AIMoveControlStatus.Processing;
    }

    // intf_P5_06.inc:180

}

public enum AIMoveControlMode : byte
{
    CanMove = 0, TransporterCanMove = 1
}
