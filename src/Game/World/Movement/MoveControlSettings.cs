#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Movement;

public partial class MoveControlSettings : EngineObject
{

    // MoveT.PAS:73
    public MoveControlSettings(CustomNode? owner) : base(owner)
    {
        _refCount = 1;
        ProcessedMapFields = [];
        Path = [];
        SubMoves = new SubMoveList(null) { Holder = CreationHolder() };
        Units = new UnitUserList(null) { Holder = CreationHolder() };
        Reset();
    }

    private int _refCount;

    public int MovePointsLimit { get; set; }
    public SubMoveList SubMoves { get; }
    public int SubMoveIndex { get; set; }
    public DelphiList<MapField> ProcessedMapFields { get; }

    private int[] _rowOffsets = [];
    private int[][] _levelData = [];

    public int[] EvenOffsets { get; } = new int[8];
    public int[] OddOffsets { get; } = new int[8];

    private bool _movePointsClear;

    public MoveControlSettingsStatus Status { get; set; }
    public MoveModes Mode { get; set; }
    public MovePath Path { get; }
    public sbyte Player { get; set; }

    private int _width;
    private int _height;
    private int _levelCount;

    public MapCoordinate Source { get; private set; }
    public MapCoordinate Destination { get; private set; }
    public int MovePointsMax { get; set; }
    public UnitHexSprite? ArmyHexSprite { get; set; }
    public int Selection { get; set; }
    public ArmyHexSprite? TransporterArmy { get; set; }
    public UnitUserList Units { get; }

    // AddUnitsEx
    public MovePointTable MovePointTable { get; internal set; }

    // MoveT.PAS:84
    public override void Dispose()
    {
        Clear();
        ProcessedMapFields?.Dispose();
        SubMoves?.Dispose();
        Path?.Dispose();
        Units?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:107
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // MoveT.PAS:113
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (_refCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // MoveT.PAS:94
    public override void Clear()
    {
        Path.Clear();
        ProcessedMapFields.Clear();
        for (int i = 0, last = _levelCount - 1; i <= last; i++)
        {
            Array.Resize(ref _levelData[i], newSize: 0);
        }

        Array.Resize(ref _rowOffsets, newSize: 0);
        Array.Resize(ref _levelData, newSize: 0);
        _width = 0;
        _height = 0;
        _levelCount = 0;
    }

    // MoveT.PAS:120
    public void ResetState() => Status = MoveControlSettingsStatus.Idle;

    // MoveT.PAS:125
    public void Reset()
    {
        Path.Clear();
        SubMoves.Clear();
        ProcessedMapFields.Clear();
        MovePointsMax = 4_095;
        MovePointTable movePointTable = MovePointTable;
        movePointTable.Fill(-1);
        MovePointTable = movePointTable;
        for (int i = 0, last = _levelCount - 1; i <= last; i++)
        {
            _levelData[i]?.AsSpan(0, length: _width * _height).Fill(16_777_215);
        }

        Mode = MoveModes.None;
        Player = -1;
        Source = new MapCoordinate(-1, -1, -1);
        Destination = new MapCoordinate(-1, -1, -1);
        Status = MoveControlSettingsStatus.Idle;
        Units.Clear();
    }

    // MoveT.PAS:148
    public void SetSourcePosition(MapCoordinate source) => Source = source;

    public void SetSourcePosition(int x, int y, int level)
        => SetSourcePosition(new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)level));

    // MoveT.PAS:155
    public void SetDestinationPosition(MapCoordinate destination) => Destination = destination;

    public void SetDestinationPosition(int x, int y, int level)
        => SetDestinationPosition(new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)level));

    // MoveT.PAS:162
    public void SetupArmy(ArmyHexSprite? army, int selection)
    {
        ArmyHexSprite = army;
        Selection = selection;
        Units.AddUnits(army.Army.Units, selection);
        Source = new MapCoordinate(army.HexX, army.HexY, army.HexLevel);
        Player = army.Army.Player;
        _ = MovePointTable;
        MovePointTable movePointTable = army.Army.CreateMovePointTable(selection);
        MovePointTable = movePointTable;
        if (StrategicMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human)
        {
            Mode |= MoveModes.AI;
        }
    }

    // MoveT.PAS:175
    public void SetupLevel(int level)
    {
        if (_levelData[level] is null)
        {
            Array.Resize(ref _levelData[level], newSize: _width * _height);
            _levelData[level].AsSpan(0, length: _width * _height).Fill(16_777_215);
        }
    }

    // MoveT.PAS:184
    public void SetupMovePointData(int width, int height, int levelCount)
    {
        if (width != _width || height != _height || levelCount != _levelCount)
        {
            Clear();
            _width = width;
            _height = height;
            _levelCount = levelCount;
            _movePointsClear = true;
            int offset = 0;
            int rowSize = width * 4;
            Array.Resize(ref _rowOffsets, newSize: height);
            for (int i = 0; i <= height - 1; i++)
            {
                _rowOffsets[i] = offset;
                offset += rowSize;
            }

            Array.Resize(ref _levelData, newSize: levelCount);
            Array.Clear(_levelData);
            for (int i = 1; i <= 6; i++)
            {
                EvenOffsets[i] = (HexGeometry.HexNumberToXYTable[0][i].X * 4) + (HexGeometry.HexNumberToXYTable[0][i].Y * width * 4);
                OddOffsets[i] = (HexGeometry.HexNumberToXYTable[1][i].X * 4) + (HexGeometry.HexNumberToXYTable[1][i].Y * width * 4);
            }
        }
        else
        {
            ClearMovePoints();
        }
    }

    // MoveT.PAS:214
    public void ClearMovePoints()
    {
        if (!_movePointsClear)
        {
            for (int i = 0, last = _levelCount - 1; i <= last; i++)
            {
                _levelData[i]?.AsSpan(0, length: _width * _height).Fill(16_777_215);
            }

            _movePointsClear = true;
        }
    }

    // MoveT.PAS:238
    public int[] GetMovePointLevelData(int level) => _levelData[level];
}

public enum MoveControlSettingsStatus : byte
{
    Idle = 0, Processing = 1, Processed = 2, Failed = 3
}
