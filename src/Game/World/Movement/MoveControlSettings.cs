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
        RefCount = 1;
        ProcessedMapFields = new DelphiList();
        Path = new MovePath();
        SubMoves = new SubMoveList(null);
        Units = new UnitUserList(null);
        Reset();
    }

    public int RefCount;
    public int MovePointsLimit;
    public SubMoveList SubMoves;
    public int SubMoveIndex;
    public DelphiList ProcessedMapFields;
    public int[] RowOffsets = [];
    public int[][] LevelData = [];
    public Int32Array8 EvenOffsets;
    public Int32Array8 OddOffsets;
    public bool MovePointsClear;
    public MoveControlSettingsStatus Status;
    public Set<MoveModeElement> Mode;
    public MovePath Path;
    public sbyte Player;
    public int Width;
    public int Height;
    public int LevelCount;
    public int SourceX;
    public int SourceY;
    public int SourceL;
    public int DestinationX;
    public int DestinationY;
    public int DestinationL;
    public int MovePointsMax;
    public UnitHexSprite ArmyHexSprite;
    public int Selection;
    public ArmyHexSprite TransporterArmy;
    public UnitUserList Units;
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
        RefCount += 1;
        return RefCount;
    }

    // MoveT.PAS:113
    public override int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (RefCount == 0)
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
        for (int I = 0, last = LevelCount - 1; I <= last; I++)
        {
            Array.Resize(ref LevelData[I], 0);
        }

        Array.Resize(ref RowOffsets, 0);
        Array.Resize(ref LevelData, 0);
        Width = 0;
        Height = 0;
        LevelCount = 0;
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
        for (int I = 0, last = LevelCount - 1; I <= last; I++)
        {
            if (LevelData[I] is not null)
            {
                FillInteger(MemoryMarshal.AsBytes(LevelData[I].AsSpan(0, Width * Height * 4 / 4)), Width * Height, 16777215);
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
        if (LevelData[level] is null)
        {
            Array.Resize(ref LevelData[level], Width * Height);
            FillInteger(MemoryMarshal.AsBytes(LevelData[level].AsSpan(0, Width * Height * 4 / 4)), Width * Height, 16777215);
        }
    }

    // MoveT.PAS:184
    public void SetupMovePointData(int width, int height, int levelCount)
    {
        int rowSize;
        int offset;
        if (width != Width || height != Height || levelCount != LevelCount)
        {
            Clear();
            Width = width;
            Height = height;
            LevelCount = levelCount;
            MovePointsClear = true;
            offset = 0;
            rowSize = width * 4;
            Array.Resize(ref RowOffsets, height);
            for (int I = 0; I <= height - 1; I++)
            {
                RowOffsets[I] = offset;
                offset += rowSize;
            }

            Array.Resize(ref LevelData, levelCount);
            Array.Clear(LevelData);
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
        if (!MovePointsClear)
        {
            for (int I = 0, last = LevelCount - 1; I <= last; I++)
            {
                if (LevelData[I] is not null)
                {
                    FillInteger(MemoryMarshal.AsBytes(LevelData[I].AsSpan(0, Width * Height * 4 / 4)), Width * Height, 16777215);
                }
            }

            MovePointsClear = true;
        }
    }

    // MoveT.PAS:238
    public int[] GetMovePointLevelData(int level) => LevelData[level];

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
