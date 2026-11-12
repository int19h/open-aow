// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Graphics;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Movement;

public partial class MoveControlSettings : EngineObject
{

    // MoveT.PAS:73
    public MoveControlSettings(ECustomNode owner) : base(owner)
    {
        _refCount = 1;
        ProcessedMapFields = new DelphiList();
        Path = new MovePath();
        SubMoves = new SubMoveList(null);
        Units = new UnitUserList(null);
        Reset();
    }

    private int _refCount;

    public int MovePointsLimit { get; set; }
    public SubMoveList SubMoves { get; }
    public int SubMoveIndex { get; set; }
    public DelphiList ProcessedMapFields { get; }

    private int[] _rowOffsets = [];
    private int[][] _levelData = [];
    public Int32Array8 EvenOffsets;
    public Int32Array8 OddOffsets;
    private bool _movePointsClear;

    public MoveControlSettingsStatus Status { get; set; }
    public Set<MoveModeElement> Mode { get; set; }
    public MovePath Path { get; }
    public sbyte Player { get; set; }

    private int _width;
    private int _height;
    private int _levelCount;

    public int SourceX { get; private set; }
    public int SourceY { get; private set; }
    public int SourceL { get; private set; }
    public int DestinationX { get; private set; }
    public int DestinationY { get; private set; }
    public int DestinationL { get; private set; }
    public int MovePointsMax { get; set; }
    public UnitHexSprite ArmyHexSprite { get; set; }
    public int Selection { get; set; }
    public ArmyHexSprite TransporterArmy { get; set; }
    public UnitUserList Units { get; }

    // AddUnitsEx
    public SByteArray16By16 MovePointTable;

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
        for (int I = 0, last = _levelCount - 1; I <= last; I++)
        {
            Array.Resize(ref _levelData[I], 0);
        }

        Array.Resize(ref _rowOffsets, 0);
        Array.Resize(ref _levelData, 0);
        _width = 0;
        _height = 0;
        _levelCount = 0;
    }

    // MoveT.PAS:120
    public void ResetState()
    {
        Status = MoveControlSettingsStatus.Idle;
    }

    // MoveT.PAS:125
    public void Reset()
    {
        Path.Clear();
        SubMoves.Clear();
        ProcessedMapFields.Clear();
        MovePointsMax = 4095;
        FillBytes(Bytes.Of(ref MovePointTable, 256), -1);
        for (int I = 0, last = _levelCount - 1; I <= last; I++)
        {
            if (_levelData[I] is not null)
            {
                FillInteger(MemoryMarshal.AsBytes(_levelData[I].AsSpan(0, _width * _height * 4 / 4)), _width * _height, 16777215);
            }
        }

        Mode = Set<MoveModeElement>.Of();
        Player = -1;
        SourceX = -1;
        SourceY = -1;
        SourceL = -1;
        DestinationX = -1;
        DestinationY = -1;
        DestinationL = -1;
        Status = MoveControlSettingsStatus.Idle;
        Units.Clear();
    }

    // MoveT.PAS:148
    public void SetSourcePosition(int X, int Y, int L)
    {
        SourceX = X;
        SourceY = Y;
        SourceL = L;
    }

    // MoveT.PAS:155
    public void SetDestinationPosition(int X, int Y, int L)
    {
        DestinationX = X;
        DestinationY = Y;
        DestinationL = L;
    }

    // MoveT.PAS:162
    public void SetupArmy(ArmyHexSprite army, byte selection)
    {
        ArmyHexSprite = army;
        Selection = selection;
        Units.AddUnits(army.Army, selection);
        SourceX = army.HexX;
        SourceY = army.HexY;
        SourceL = army.HexLevel;
        Player = army.Army.Player;
        army.Army.CreateMovePointTable(selection, Bytes.Of(ref MovePointTable));
        if (AoWHSMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human)
        {
            Mode = Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
        }
    }

    // MoveT.PAS:175
    public void SetupLevel(int level)
    {
        if (_levelData[level] is null)
        {
            Array.Resize(ref _levelData[level], _width * _height);
            FillInteger(MemoryMarshal.AsBytes(_levelData[level].AsSpan(0, _width * _height * 4 / 4)), _width * _height, 16777215);
        }
    }

    // MoveT.PAS:184
    public void SetupMovePointData(int width, int height, int levelCount)
    {
        int rowSize;
        int offset;
        if (width != _width || height != _height || levelCount != _levelCount)
        {
            Clear();
            _width = width;
            _height = height;
            _levelCount = levelCount;
            _movePointsClear = true;
            offset = 0;
            rowSize = width * 4;
            Array.Resize(ref _rowOffsets, height);
            for (int I = 0; I <= height - 1; I++)
            {
                _rowOffsets[I] = offset;
                offset += rowSize;
            }

            Array.Resize(ref _levelData, levelCount);
            Array.Clear(_levelData);
            for (int I = 1; I <= 6; I++)
            {
                EvenOffsets[I] = HexGeometry.s_hexNumberToXYTable[0][I].X * 4 + HexGeometry.s_hexNumberToXYTable[0][I].Y * (width * 4);
                OddOffsets[I] = HexGeometry.s_hexNumberToXYTable[1][I].X * 4 + HexGeometry.s_hexNumberToXYTable[1][I].Y * (width * 4);
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
            for (int I = 0, last = _levelCount - 1; I <= last; I++)
            {
                if (_levelData[I] is not null)
                {
                    FillInteger(MemoryMarshal.AsBytes(_levelData[I].AsSpan(0, _width * _height * 4 / 4)), _width * _height, 16777215);
                }
            }

            _movePointsClear = true;
        }
    }

    // MoveT.PAS:238
    public int[] GetMovePointLevelData(int level) => _levelData[level];

    internal static void FillInteger(Span<byte> destination, int count, int value)
    {
        Blit.FillInteger(destination, count, value);
    }

    // intf_P5_01.inc:63

    // intf_P5_01.inc:63

}

public enum MoveControlSettingsStatus : byte
{
    Idle = 0, Processing = 1, Processed = 2, Failed = 3
}
